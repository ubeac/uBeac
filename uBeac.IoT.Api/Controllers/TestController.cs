using System.Threading.Tasks;
using uBeac.Web.Api.Controllers;

namespace uBeac.IoT.Api.Controllers
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
