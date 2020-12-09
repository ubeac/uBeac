using System.Threading;
using System.Threading.Tasks;
using uBeac.IoT.Models;
using uBeac.Services.Abstractions;

namespace uBeac.IoT.Api.Facades
{
    public interface ITeamFacade
    {
        Task<Team> Add(Team team, CancellationToken cancellationToken = default);
    }

    public class TeamFacade : ITeamFacade
    {
        private readonly IBaseEntityService<Team> _baseEntityService;
        public TeamFacade(IBaseEntityService<Team> baseEntityService)
        {
            _baseEntityService = baseEntityService;
        }
        public async Task<Team> Add(Team team, CancellationToken cancellationToken = default) 
        {
            await _baseEntityService.Add(team, cancellationToken);
            return team;
        }
    }
}
