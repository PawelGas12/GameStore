using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ChangePasswordModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Podaj aktualne hasło.")]
            [DataType(DataType.Password)]
            [Display(Name = "Aktualne hasło")]
            public string OldPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "Podaj nowe hasło.")]
            [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nowe hasło")]
            public string NewPassword { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź nowe hasło")]
            [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Nie udało się wczytać danych użytkownika.");

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
                return RedirectToPage("./Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Nie udało się wczytać danych użytkownika.");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Hasło zostało zmienione.";
            return RedirectToPage();
        }
    }
}
