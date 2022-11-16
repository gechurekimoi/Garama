using System;
using Microsoft.AspNetCore.Datasync.EFCore;

namespace Garama.Domain.Entities
{
    public class Category : EntityTableData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

