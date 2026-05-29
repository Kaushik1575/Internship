using System.ComponentModel.DataAnnotations;

namespace ApprenticeshipManagement.ViewModels;

public class ApprenticeAdminLoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Apprentice ID is required")]
    [Display(Name = "Apprentice ID")]
    [StringLength(20)]
    public string ApprenticeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Apprenticeship Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Apprenticeship Password")]
    public string ApprenticeshipPassword { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}
