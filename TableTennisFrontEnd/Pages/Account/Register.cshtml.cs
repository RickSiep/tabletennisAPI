using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TableTennisShared.DTO.User;

namespace TableTennisFrontEnd.Pages.Account
{
    public class RegisterModel(IHttpClientFactory factory) : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = factory.CreateClient("api");
            var response = await client.PostAsJsonAsync("auth/register", new RegisterDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Form wasn't filled in correctly");
                return Page();
            }

            TempData["SuccessMessage"] = "Account created successfully, please log in";
            return RedirectToPage("/Account/Login");
        }
    }
}
