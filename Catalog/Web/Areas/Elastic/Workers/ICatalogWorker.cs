using System.Threading.Tasks;
using Web.Models.Search;

namespace Web.Areas.Elastic.Workers
{
    public interface ICatalogWorker
    {
        Task<SearchResult> Search(SearchInput input);
    }
}
