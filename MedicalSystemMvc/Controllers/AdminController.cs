using Microsoft.AspNetCore.Mvc;
using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Services.Interfaces;
using System.Threading.Tasks;
using MedicalSystemMvc.Models;
using Microsoft.EntityFrameworkCore;
using MedicalSystemClassLibrary.Data;
using Microsoft.Extensions.Logging;
using MedicalSystemClassLibrary.Utilities;
using System.Linq;

namespace MedicalSystemMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly MedicalSystemDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService, MedicalSystemDbContext context, ILogger<AdminController> logger)
        {
            _userService = userService;
            _context = context;
            _logger = logger;
        }

        // List all users
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users.OrderBy(u => u.UserId).ToList());
        }

        // Show user details
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Create()
        {
            ViewData["HideNavbar"] = true;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Create(UserViewModel userVm)
        {
            var trimmedUsername = userVm.Username.Trim();

            if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
            {
                ModelState.AddModelError("", "This username already exists");
                return View();
            }

            return RedirectToAction("ConfirmCreation", userVm);
        }

        public ActionResult ConfirmCreation(UserViewModel userVm)
        {
            return View(userVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CompleteCreation(UserViewModel userVm)
        {
            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(userVm.Password, b64salt);

            var user = new User
            {
                Username = userVm.Username,
                PwdHash = b64hash,
                PwdSalt = b64salt,
                FirstName = userVm.FirstName,
                LastName = userVm.LastName,
                Email = userVm.Email,
                Phone = userVm.Phone,
                RoleId = 1
            };

            _context.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Registration successful! You can now log in.";

            return RedirectToAction("Index");
        }

        // Edit user
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _userService.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // Delete user
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(model.NewPassword, b64salt);

                user.PwdSalt = b64salt;
                user.PwdHash = b64hash;

                await _userService.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Promote(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Load the role entity
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 2);
            if (role == null)
            {
                return NotFound();
            }

            // Update the user's role
            user.RoleId = role.RoleId;
            user.Role = role;

            await _userService.UpdateUserAsync(user);

            return RedirectToAction(nameof(Index));
        }

    }
}
