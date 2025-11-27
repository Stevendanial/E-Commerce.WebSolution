using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataIntializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataIntializer> logger;

        public IdentityDataIntializer(UserManager<ApplicationUser>userManager,RoleManager<IdentityRole> roleManager
                        ,ILogger<IdentityDataIntializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.logger = logger;
        }


        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(
                        new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(
                         new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {

                    var user01 = new ApplicationUser()
                    {
                        DisplayName = "Steven danial",
                        UserName = "StevenDanial",
                        Email = "Stevendanial@gmail.com",
                        PhoneNumber = "01223456789"
                    };
                    var user02 = new ApplicationUser()
                    {
                        DisplayName = "Ahmed Amin",
                        UserName = "AhmedAmin",
                        Email = "AhmedAmin@gmail.com",
                        PhoneNumber = "01201267789"
                    };


                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "Admin");
                    await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Error while Seeding Identity DataBase :Message ={ex}");

            }
            
        }
    }
}
