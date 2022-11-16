using Microsoft.AspNetCore.Datasync.EFCore;
using System;
namespace Garama.Domain.Entities
{
    public class User : EntityTableData
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public int Pin { get; set; }
        public DateTime DateAdded { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int AuthMethod { get; set; }
        public DateTime? LastAuthDate { get; set; }
        public string AuthMethodImmutableIdSent { get; set; }
    }
}

