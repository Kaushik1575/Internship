using System.ComponentModel.DataAnnotations;

namespace ApprenticeshipManagement.ViewModels;

public class AddStudentViewModel
{
    [Required(ErrorMessage = "Full name is required")]
    [Display(Name = "Full Name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Student ID is required")]
    [Display(Name = "Student ID")]
    [StringLength(20)]
    public string StudentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trade/Field is required")]
    [Display(Name = "Trade / Field")]
    [StringLength(100)]
    public string TradeField { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Enter a valid phone number")]
    [Display(Name = "Phone")]
    [StringLength(15)]
    public string MobileNumber { get; set; } = string.Empty;

    [Display(Name = "Student Status")]
    public bool IsActive { get; set; } = true;
}
