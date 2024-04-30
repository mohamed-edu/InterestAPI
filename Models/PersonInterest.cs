using LeaveAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterestAPI.Models
{
    public class PersonInterest
    {
        [Key]
        public int personInterestId { get; set; }

        [ForeignKey("InterestDescription")]
        public int FKInterestDescriptionId { get; set; }
        public InterestDescription? InterestDescription { get; set; }
        [ForeignKey("Link")]
        public int? LinkId { get; set; }
        public Link? Link { get; set; }
        [ForeignKey("Person")]
        public int FkPersonId { get; set; }
        public Person? Person { get; set; }

    }
}
