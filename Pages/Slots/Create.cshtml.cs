using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Slots
{
    public class CreateModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public CreateModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IActionResult OnGet()
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                    return Page();
            }
            else
                return Redirect("/Index");
        }

        [BindProperty]
        public TimeSlot TimeSlot { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.TimeSlots == null || TimeSlot == null)
            {
                return Page();
            }

            _context.TimeSlots.Add(TimeSlot);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
