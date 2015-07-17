using System;
using System.Threading.Tasks;
using Web.Areas.Elastic.Services;
using Web.Infrastructure.Common;
using Web.Models.Search;

namespace Web.Areas.Elastic.Workers
{
    public class CatalogWorker : ICatalogWorker
    {
        private readonly ICatalogRepository repository;

        public CatalogWorker(ICatalogRepository repository)
        {
            Contract.Requires<ArgumentNullException>(repository != null, "repository");
            this.repository = repository;
        }

        public async Task<SearchResult> Search(SearchInput input)
        {
            return await repository.Search(input);
        }

    }
}