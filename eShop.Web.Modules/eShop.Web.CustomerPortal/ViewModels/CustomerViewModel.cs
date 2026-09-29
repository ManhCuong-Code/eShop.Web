using System.ComponentModel.DataAnnotations;

namespace eShop.Web.CustomerPortal.ViewModels
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? CustomerAddress { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? CustomerCity { get; set; }

        [Required(ErrorMessage = "State/Province is required.")]
        public string? CustomerStateProvince { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string? CustomerCountry { get; set; }
    }
}
