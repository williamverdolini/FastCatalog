using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Web.Areas.Mongo.Workers;
using Web.Infrastructure.Common;
using Web.Models.Search;
using Web.Models.Views;

namespace Web.Areas.Mongo.Controllers
{
    public class MongoCatalogController : ApiController
    {
        private readonly ICatalogWorker worker;
        private readonly IMappingEngine mapper;

        public MongoCatalogController(ICatalogWorker worker, IMappingEngine mapper)
        {
            Contract.Requires<ArgumentNullException>(worker != null, "worker");
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            this.worker = worker;
            this.mapper = mapper;
        }

        // http://localhost:52646/Mongo/api/Catalog/Search
        [HttpPost]
        public async Task<IHttpActionResult> Search(SearchInputViewModel searchInput)
        {
            try
            {
                SearchInput input = mapper.Map<SearchInputViewModel, SearchInput>(searchInput);
                return Ok(await worker.Search(input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
