using System.ComponentModel.DataAnnotations;

namespace InterestAPI.Models
{
    public class InterestDescription
    {
        [Key]
        public int InterestDescriptionId { get; set; }
        public string InterestName { get; set; }
        public string Description { get; set; }
    }
}
