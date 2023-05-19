using System.ComponentModel.DataAnnotations;

namespace Feast.Foundation.Server.Test.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
