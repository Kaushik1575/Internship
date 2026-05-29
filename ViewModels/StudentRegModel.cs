using System.ComponentModel.DataAnnotations;

namespace ApprenticeshipManagement.ViewModels;

public class StudentRegModel
{
    [Required(ErrorMessage = "Full name is required")]
    [Display(Name = "Full Name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Apprentice ID is required")]
    [Display(Name = "Apprentice ID")]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    [Display(Name = "Department")]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mobile number is required")]
    [Phone(ErrorMessage = "Enter a valid mobile number")]
    [Display(Name = "Mobile Number")]
    [StringLength(15)]
    public string MobileNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Apprenticeship Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Apprenticeship Password must be at least 6 characters")]
    [Display(Name = "Apprenticeship Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm your Apprenticeship Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Apprenticeship passwords do not match")]
    [Display(Name = "Confirm Apprenticeship Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
