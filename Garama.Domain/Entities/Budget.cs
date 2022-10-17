using System;
namespace Garama.Domain.Entities
{
    public class Budget
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public int Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

