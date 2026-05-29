using ApprenticeshipManagement.Data;
using ApprenticeshipManagement.Helpers;
using ApprenticeshipManagement.Models;
using ApprenticeshipManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipManagement.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly InternshipDb _db;

    public AdminController(InternshipDb db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search)
    {
        var searchTerm = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

        var query = _db.Apprentices.AsNoTracking();

        if (searchTerm != null)
        {
            var term = searchTerm.ToLowerInvariant();
            query = query.Where(a =>
                a.FullName.ToLower().Contains(term) ||
                a.ApprenticeId.ToLower().Contains(term) ||
                a.Department.ToLower().Contains(term) ||
                a.Email.ToLower().Contains(term) ||
                a.MobileNumber.ToLower().Contains(term));
        }

        var list = await query
            .OrderBy(a => a.ApprenticeId)
            .Select(a => new StudentRowModel
            {
                Id = a.Id,
                ApprenticeId = a.ApprenticeId,
                FullName = a.FullName,
                TradeField = a.Department,
                Email = a.Email,
                Phone = a.MobileNumber,
                IsActive = a.IsActive
            })
            .ToListAsync();

        var model = new AdminHomeModel
        {
            AdminName = User.Identity?.Name ?? "Administrator",
            TotalApprentices = await _db.Apprentices.CountAsync(),
            ActiveApprentices = await _db.Apprentices.CountAsync(a => a.IsActive),
            InactiveApprentices = await _db.Apprentices.CountAsync(a => !a.IsActive),
            SearchQuery = searchTerm,
            Apprentices = list
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var apprentice = await _db.Apprentices.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (apprentice == null)
            return NotFound();

        ViewBag.Apprentice = apprentice;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var apprentice = await _db.Apprentices.FindAsync(id);
        if (apprentice == null)
            return NotFound();

        return View(new EditStudentModel
        {
            Id = apprentice.Id,
            ApprenticeId = apprentice.ApprenticeId,
            FullName = apprentice.FullName,
            TradeField = apprentice.Department,
            Email = apprentice.Email,
            MobileNumber = apprentice.MobileNumber,
            IsActive = apprentice.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditStudentModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var apprentice = await _db.Apprentices.FindAsync(model.Id);
        if (apprentice == null)
            return NotFound();

        var email = model.Email.Trim().ToLowerInvariant();
        var apprenticeId = model.ApprenticeId.Trim();

        if (await _db.Apprentices.AnyAsync(a => a.Email == email && a.Id != model.Id))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already used.");
            return View(model);
        }

        if (await _db.Apprentices.AnyAsync(a => a.ApprenticeId == apprenticeId && a.Id != model.Id))
        {
            ModelState.AddModelError(nameof(model.ApprenticeId), "This apprentice ID is already used.");
            return View(model);
        }

        apprentice.ApprenticeId = apprenticeId;
        apprentice.FullName = model.FullName.Trim();
        apprentice.Department = model.TradeField.Trim();
        apprentice.Email = email;
        apprentice.MobileNumber = model.MobileNumber.Trim();
        apprentice.IsActive = model.IsActive;

        await _db.SaveChangesAsync();
        TempData["Success"] = "Apprentice updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var apprentice = await _db.Apprentices.FindAsync(id);
        if (apprentice == null)
            return NotFound();

        apprentice.IsActive = !apprentice.IsActive;
        await _db.SaveChangesAsync();

        TempData["Success"] = apprentice.IsActive
            ? $"{apprentice.FullName} activated."
            : $"{apprentice.FullName} deactivated.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View(new StudentFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(StudentFormModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        var studentId = model.StudentId.Trim();

        if (await _db.Apprentices.AnyAsync(a => a.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
            return View(model);
        }

        if (await _db.Apprentices.AnyAsync(a => a.ApprenticeId == studentId))
        {
            ModelState.AddModelError(nameof(model.StudentId), "This student ID is already in use.");
            return View(model);
        }

        if (await _db.Admins.AnyAsync(a => a.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already used by an admin account.");
            return View(model);
        }

        _db.Apprentices.Add(new Apprentice
        {
            FullName = model.FullName.Trim(),
            Email = email,
            ApprenticeId = studentId,
            Department = model.TradeField.Trim(),
            MobileNumber = model.MobileNumber.Trim(),
            ApprenticeshipPasswordHash = AuthHelper.HashPassword(Guid.NewGuid().ToString("N")),
            CreatedAt = DateTime.UtcNow,
            IsActive = model.IsActive
        });

        await _db.SaveChangesAsync();
        var statusNote = model.IsActive ? "added successfully." : "added as inactive.";
        TempData["Success"] = $"{model.FullName.Trim()} {statusNote}";
        return RedirectToAction(nameof(Index));
    }
}
