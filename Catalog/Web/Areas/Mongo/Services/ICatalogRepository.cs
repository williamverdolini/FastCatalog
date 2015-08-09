using System.Threading.Tasks;
using Web.Models.Search;

namespace Web.Areas.Mongo.Services
{
    public interface ICatalogRepository
    {
        Task<SearchResult> Search(SearchInput input);
    }
}
