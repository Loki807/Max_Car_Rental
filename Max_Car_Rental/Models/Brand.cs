using System.ComponentModel.DataAnnotations;

namespace Max_Car_Rental.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }=string.Empty;
        
        public int EstablishedYear { get; set; }
        public string BrandLogo { get;set; }=string.Empty;

    }
}
