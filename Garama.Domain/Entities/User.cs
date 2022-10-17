using System;
namespace Garama.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public int Pin { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

