using System.ComponentModel.DataAnnotations;

namespace Max_Car_Rental.Models
{
    public class Car
    {
        [Key]
        public Guid CarId { get; set; }

        [Required, StringLength(100)]
        public string CarName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string CarModel { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool Availability { get; set; } = true;

        // Foreign Key → Brand
        public Guid? BrandId { get; set; }
        public Brand? Brand { get; set; }

    }
}
