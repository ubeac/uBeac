using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Web.Api.Controllers;

namespace uBeac.Auth.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [Post]
        public async Task Add(CancellationToken cancellationToken = default)
        {
            var x = new User
            {
                Email = "ap1@momentaj.com",
                UserName = "admin1"
            };

          var x1 =   await _userManager.CreateAsync(x, "admin");
        }
    }

}
