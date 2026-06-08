using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EmailModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string? Email { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Podaj adres e-mail.")]
            [EmailAddress(ErrorMessage = "Niepoprawny adres e-mail.")]
            [Display(Name = "Nowy e-mail")]
            public string NewEmail { get; set; } = string.Empty;
        }

        private async Task LoadAsync(IdentityUser user)
        {
            Email = await _userManager.GetEmailAsync(user);
            Input = new InputModel { NewEmail = Email ?? string.Empty };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Nie udało się wczytać danych użytkownika.");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Nie udało się wczytać danych użytkownika.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var setEmail = await _userManager.SetEmailAsync(user, Input.NewEmail);
                var setName = await _userManager.SetUserNameAsync(user, Input.NewEmail);
                if (!setEmail.Succeeded || !setName.Succeeded)
                {
                    StatusMessage = "Błąd: nie udało się zmienić adresu e-mail.";
                    return RedirectToPage();
                }
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Adres e-mail został zmieniony.";
                return RedirectToPage();
            }

            StatusMessage = "Twój e-mail jest bez zmian.";
            return RedirectToPage();
        }
    }
}
