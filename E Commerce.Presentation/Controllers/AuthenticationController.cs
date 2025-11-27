using E_Commerce.Service.Abstraction;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class AuthenticationController:ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        //Login 
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result =await _authenticationService.LoginAsync(loginDTO);
            return HandleResult(Result);
       
        }

        //register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> register(RegisterDTO registerDTO)
        {
            var Result = await _authenticationService.RegisterAsync(registerDTO);
            return HandleResult(Result);

        }
    }
}
