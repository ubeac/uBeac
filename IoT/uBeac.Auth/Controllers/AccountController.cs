using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Auth.Models;
using uBeac.Web.Api.Controllers;

namespace uBeac.Auth.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Post]
        public async Task Register(CancellationToken cancellationToken = default)
        {
            var roles = new List<Role>
            {
                new Role{ Name="Registered" },
                new Role{ Name="All" },
                new Role{ Name="Admin" },
            };

            roles.ForEach(async x => await _roleManager.CreateAsync(x));

            var user = new User
            {
                Email = "ali@momentaj.com",
                UserName = "ali",
                PhoneNumber = "321321321321"
            };

            var result = await _userManager.CreateAsync(user, "123");
            //if (!result.Succeeded)
            //    return;

           var x222= await _userManager.AddToRoleAsync(user, "Registered");
        }
    }

}
