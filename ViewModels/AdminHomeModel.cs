namespace ApprenticeshipManagement.ViewModels;

public class AdminHomeModel
{
    public string AdminName { get; set; } = string.Empty;
    public int TotalApprentices { get; set; }
    public int ActiveApprentices { get; set; }
    public int InactiveApprentices { get; set; }
    public string? SearchQuery { get; set; }
    public List<StudentRowModel> Apprentices { get; set; } = [];
}

public class StudentRowModel
{
    public int Id { get; set; }
    public string ApprenticeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string TradeField { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
