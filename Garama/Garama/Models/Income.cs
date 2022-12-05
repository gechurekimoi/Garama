using Microsoft.Datasync.Client;
using System;
namespace Garama.Domain.Entities
{
    public class Income : DatasyncClientData
    {
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncomeDate { get; set; }
        public DateTime DateAdded { get; set; }

    }
}

