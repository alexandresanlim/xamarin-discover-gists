using DiscoverGists.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscoverGists.Services.Interfaces
{
    public interface IGitHubService
    {
        Task<List<Gist>> GetGistList(int page);
    }
}
