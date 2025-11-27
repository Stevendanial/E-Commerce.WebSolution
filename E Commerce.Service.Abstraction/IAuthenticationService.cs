using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Abstraction
{
    public  interface IAuthenticationService
    {
        //login
        // email, password=>token ,displayName, email
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);

        //register
        // email, password,userName,displayName, phoneno=>token ,displayName, email

        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}
