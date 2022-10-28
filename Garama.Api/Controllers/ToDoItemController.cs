using Garama.Domain.Entities;
using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Garama.Api.Controllers
{
    [Route("tables/[controller]")]
    public class ToDoItemController : TableController<ToDoItem>
    {
        public ToDoItemController(GaramaDbContext context, ILogger<ToDoItemController> logger)
        {
            Repository = new EntityTableRepository<ToDoItem>(context);
            Logger = logger;
        }

    }
}
