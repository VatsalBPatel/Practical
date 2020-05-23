using System.ComponentModel.DataAnnotations;

namespace Practical.Models
{
    public class Club
    {
        [Key]
        public char ClubId { get; set; }

        public string Name { get; set; }
    }
}
