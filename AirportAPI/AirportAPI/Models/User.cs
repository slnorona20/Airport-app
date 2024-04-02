using System;
using System.Xml.Linq;

namespace AirportAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
    }
}
