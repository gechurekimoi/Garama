using Microsoft.Datasync.Client;
using System;
namespace Garama.Domain.Entities
{
    public class Expense : DatasyncClientData
    {
        public string Description { get; set; }
        public int Amount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

