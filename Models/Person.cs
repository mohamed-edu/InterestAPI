using System.ComponentModel.DataAnnotations;

namespace LeaveAPI.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
    }
}
