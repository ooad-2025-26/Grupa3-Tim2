// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SmartHairSalonApp.Models;

namespace SmartHairSalonApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly UserManager<Korisnik> _userManager;
        private readonly IUserStore<Korisnik> _userStore;
        private readonly IUserEmailStore<Korisnik> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Korisnik> userManager,
            IUserStore<Korisnik> userStore,
            SignInManager<Korisnik> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Polje Ime je obavezno.")]
            [Display(Name = "Ime")]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Polje Prezime je obavezno.")]
            [Display(Name = "Prezime")]
            public string Prezime { get; set; }

            [Required(ErrorMessage = "Polje Email je obavezno.")]
            [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Polje Lozinka je obavezno.")]
            [StringLength(100, ErrorMessage = "{0} mora imati najmanje {2} a najviše {1} karaktera.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke se ne podudaraju.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // 🔥 POPRAVLJENO: Dodjeljuje se uloga "korisnik" umjesto "admin" za registraciju klijenata
                    await _userManager.AddToRoleAsync(user, "korisnik");

                    _logger.LogInformation("Korisnik je kreirao novi račun sa šifrom.");

                    // Automatska prijava korisnika nakon uspješne registracije
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["SuccessMessage"] = "Registracija je uspješno završena.";
                    return Redirect("~/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Korisnik CreateUser()
        {
            try
            {
                return new Korisnik
                {
                    Ime = Input.Ime,
                    Prezime = Input.Prezime
                };
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Korisnik)}'. " +
                    $"Ensure that '{nameof(Korisnik)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<Korisnik> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Korisnik>)_userStore;
        }
    }
}