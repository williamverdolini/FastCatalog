using System.Threading.Tasks;
using Web.Models.Search;

namespace Web.Areas.Mongo.Workers
{
    public interface ICatalogWorker
    {
        Task<SearchResult> Search(SearchInput input);
    }
}
