using System.Security.Claims;
using ApprenticeshipManagement.Data;
using ApprenticeshipManagement.Helpers;
using ApprenticeshipManagement.Models;
using ApprenticeshipManagement.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipManagement.Controllers;

public class AccountController : Controller
{
    private readonly InternshipDb _db;

    public AccountController(InternshipDb db)
    {
        _db = db;
    }

    // ---------- Admin Register ----------

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AdminRegister()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToDashboard();

        return View(new AdminRegModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminRegister(AdminRegModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email.Trim().ToLowerInvariant();

        if (await _db.Admins.AnyAsync(a => a.Email == email))
        {
            ModelState.AddModelError(string.Empty, "This email is already registered as admin.");
            return View(model);
        }

        if (await _db.Apprentices.AnyAsync(a => a.Email == email))
        {
            ModelState.AddModelError(string.Empty, "This email is already used by an apprentice account.");
            return View(model);
        }

        _db.Admins.Add(new Admin
        {
            FullName = model.FullName.Trim(),
            Email = email,
            PasswordHash = AuthHelper.HashPassword(model.Password),
            Department = string.IsNullOrWhiteSpace(model.Department) ? null : model.Department.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        });

        await _db.SaveChangesAsync();

        TempData["Success"] = "Admin registration successful. Please log in.";
        return RedirectToAction(nameof(AdminLogin));
    }

    // ---------- Apprentice Register ----------

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ApprenticeRegister()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToDashboard();

        return View(new StudentRegModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprenticeRegister(StudentRegModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        var apprenticeId = model.EmployeeId.Trim();

        if (await _db.Apprentices.AnyAsync(a => a.Email == email))
        {
            ModelState.AddModelError(string.Empty, "This email is already registered as apprentice.");
            return View(model);
        }

        if (await _db.Apprentices.AnyAsync(a => a.ApprenticeId == apprenticeId))
        {
            ModelState.AddModelError(nameof(model.EmployeeId), "This apprentice ID is already registered.");
            return View(model);
        }

        if (await _db.Admins.AnyAsync(a => a.Email == email))
        {
            ModelState.AddModelError(string.Empty, "This email is already used by an admin account.");
            return View(model);
        }

        _db.Apprentices.Add(new Apprentice
        {
            FullName = model.FullName.Trim(),
            Email = email,
            ApprenticeId = apprenticeId,
            Department = model.Department.Trim(),
            MobileNumber = model.MobileNumber.Trim(),
            ApprenticeshipPasswordHash = AuthHelper.HashPassword(model.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        });

        await _db.SaveChangesAsync();

        TempData["Success"] = "Apprentice registration successful. Please log in.";
        return RedirectToAction(nameof(ApprenticeLogin));
    }

    // ---------- Admin Login ----------

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AdminLogin()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToDashboard();

        return View(new AdminLoginModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminLogin(AdminLoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        var admin = await _db.Admins.FirstOrDefaultAsync(a => a.Email == email);

        if (admin == null || !admin.IsActive || !AuthHelper.VerifyPassword(model.Password, admin.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        await SignInAsync(admin.Id, admin.FullName, admin.Email, UserRole.Admin, model.RememberMe);
        return RedirectToAction("Index", "Admin");
    }

    // ---------- Apprentice Login ----------

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ApprenticeLogin()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToDashboard();

        return View(new ApprenticeAdminLoginModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprenticeLogin(ApprenticeAdminLoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        var apprenticeId = model.ApprenticeId.Trim();

        var apprentice = await _db.Apprentices.FirstOrDefaultAsync(a => a.Email == email);

        if (apprentice == null || !apprentice.IsActive ||
            !string.Equals(apprentice.ApprenticeId, apprenticeId, StringComparison.OrdinalIgnoreCase) ||
            !AuthHelper.VerifyPassword(model.ApprenticeshipPassword, apprentice.ApprenticeshipPasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email, apprentice ID, or Apprenticeship password.");
            return View(model);
        }

        await SignInAsync(apprentice.Id, apprentice.FullName, apprentice.Email, UserRole.Apprentice, model.RememberMe);
        return RedirectToAction("Index", "Apprentice");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInAsync(int id, string fullName, string email, UserRole role, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddHours(8)
            });
    }

    private IActionResult RedirectToDashboard()
    {
        if (User.IsInRole(UserRole.Admin.ToString()))
            return RedirectToAction("Index", "Admin");

        return RedirectToAction("Index", "Apprentice");
    }
}
