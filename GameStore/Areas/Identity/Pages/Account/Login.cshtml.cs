using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Podaj adres e-mail.")]
            [EmailAddress(ErrorMessage = "Niepoprawny adres e-mail.")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Podaj hasło.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Zapamiętaj mnie")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            ReturnUrl = returnUrl;
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Konto zostało tymczasowo zablokowane.");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Nieprawidłowy e-mail lub hasło.");
            return Page();
        }
    }
}
