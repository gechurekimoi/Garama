using Microsoft.Datasync.Client;
using System;
namespace Garama.Models
{
    public class Budget : DatasyncClientData
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public int Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

