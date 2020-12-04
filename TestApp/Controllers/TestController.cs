using System.Threading.Tasks;
using uBeac.Web.Api.Controllers;

namespace TestApp.Controllers
{
    public class TestController : BaseController
    {
        [Get]
        public async Task<string> Hello() 
        {
            return await Task.FromResult("hello world!");
        }
    }
}
