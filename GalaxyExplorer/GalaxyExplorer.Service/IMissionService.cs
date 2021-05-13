using GalaxyExplorer.DTO;
using System.Threading.Tasks;

namespace GalaxyExplorer.Service
{
    public interface IMissionService
    {
        Task<MissionStartResponse> StartMissionAsync(MissionStartRequest request);
    }
}
