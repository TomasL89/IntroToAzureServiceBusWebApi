using System.ComponentModel.DataAnnotations;

namespace ServiceBusShared.Models
{
    public class PersonModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
