using System;
namespace Garama.Domain.Entities
{
    public class Income
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncomeDate { get; set; }
        public DateTime DateAdded { get; set; }

    }
}

