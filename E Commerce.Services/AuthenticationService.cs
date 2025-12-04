using E_Commerce.Domain.Entites.IdentityModule;
using E_Commerce.Service.Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<bool> CheckEmailAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return user != null;
        }

        public async Task<Result<UserDTO>> GeyUserByEmailAsync(string Email)
        {
            var user =await  _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
             return Error.NotFound("user.NotFound",$"No User with Email {Email} was Found ");

            }
            return new UserDTO(user.Email!, user.DisplayName, await CreateTokenAsync(user));
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                Error.InvalidCredentials("user.InvalidCredentials");
            }
            var IsPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!IsPasswordValid)
                 Error.InvalidCredentials("user.InvalidCredentials");
           var token = await CreateTokenAsync(user);
            return new UserDTO(user.Email!, user.DisplayName, token);


        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };
            var IdentityResult = await _userManager.CreateAsync(user, registerDTO.Password);
            
            if (IdentityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(user);
                 return new UserDTO(user.Email, user.DisplayName, Token);
            }
               
            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();

        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            //token [issuer, audience , cliams, expires ,signning credintials]

            var Claims = new List<Claim>()
            {
                 new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                    new Claim(JwtRegisteredClaimNames.Name,user.UserName!)
            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
                Claims.Add(new Claim(ClaimTypes.Role, role));

            var securityKey = Configuration["JWTOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var Cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: Configuration["JWTOptions: Issuer"],
                audience: Configuration["JWTOptions:Audience"], 
                expires: DateTime.UtcNow.AddHours(1),
                claims: Claims,
                signingCredentials: Cred
                );

            return new JwtSecurityTokenHandler().WriteToken(Token); 
        }
    }
}
