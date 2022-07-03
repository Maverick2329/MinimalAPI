using System;
using System.Collections.Generic;

namespace IntroMinimalAPI.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
