using System.ComponentModel.DataAnnotations;

namespace UniqPac_ERP.ViewModels
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email/Username")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Employee Code")]
        public string? EmployeeCode { get; set; }

        public string? Department { get; set; }
        public string? Designation { get; set; }

        [Required]
        [Display(Name = "Assigned Role")]
        public string Role { get; set; } = null!;
    }
}
