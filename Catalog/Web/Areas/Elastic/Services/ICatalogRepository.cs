using System.Threading.Tasks;
using Web.Models.Search;

namespace Web.Areas.Elastic.Services
{
    public interface ICatalogRepository
    {
        Task<SearchResult> Search(SearchInput input);
    }
}
