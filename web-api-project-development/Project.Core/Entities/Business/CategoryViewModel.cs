using System.ComponentModel.DataAnnotations;

namespace Project.Core.Entities.Business
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryCreateViewModel
    {
        [Required, StringLength(maximumLength: 50, MinimumLength = 2)]
        public string? Name { get; set; }
        [StringLength(maximumLength: 200)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryUpdateViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 50, MinimumLength = 2)]
        public string? Name { get; set; }
        [StringLength(maximumLength: 200)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
