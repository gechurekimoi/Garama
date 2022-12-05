using System;
using Microsoft.Datasync.Client;

namespace Garama.Models
{
    public class Category : DatasyncClientData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

