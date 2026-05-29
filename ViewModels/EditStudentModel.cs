using System.ComponentModel.DataAnnotations;

namespace ApprenticeshipManagement.ViewModels;

public class EditStudentModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Apprentice ID")]
    [StringLength(20)]
    public string ApprenticeId { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Full Name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Trade / Field")]
    [StringLength(100)]
    public string TradeField { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [Display(Name = "Phone")]
    [StringLength(15)]
    public string MobileNumber { get; set; } = string.Empty;

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
}
