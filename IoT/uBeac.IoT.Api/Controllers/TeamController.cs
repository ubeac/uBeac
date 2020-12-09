using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using uBeac.IoT.Api.DTOModels;
using uBeac.IoT.Api.Facades;
using uBeac.IoT.Models;
using uBeac.Web.Api.Controllers;

namespace uBeac.IoT.Api.Controllers
{
    public class TeamController : BaseController
    {
        private readonly ITeamFacade _teamFacade;
        private readonly IMapper _mapper;
        public TeamController(ITeamFacade teamFacade, IMapper mapper)
        {
            _teamFacade = teamFacade;
            _mapper = mapper;
        }

        //[Get]
        //public async Task<PaginatedList<Team>> GetAll(CancellationToken cancellationToken = default)
        //{
        //    return await _teamService.GetAll(cancellationToken);
        //}

        [Post]
        public async Task<Team> Add(TeamAddModel model, CancellationToken cancellationToken = default)
        {
            var team = _mapper.Map<Team>(model);
            return await _teamFacade.Add(team, cancellationToken);
        }
    }
}
