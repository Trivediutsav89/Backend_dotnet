using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Entities.General
{
    [Table("Categories")]
    public class Category : Base<int>
    {
        [Required, StringLength(maximumLength: 50, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(maximumLength: 200)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
