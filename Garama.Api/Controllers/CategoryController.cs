using Garama.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garama.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : TableController<Category>
    {
        public CategoryController(GaramaDbContext context, ILogger<CategoryController> logger)
        {
            Repository = new EntityTableRepository<Category>(context);
            Logger = logger;
        }

    }
}
