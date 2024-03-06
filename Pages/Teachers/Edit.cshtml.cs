using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Teachers
{
    public class EditModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        public Account Account;
        public DateTime MaxDate = DateTime.Now;
        public DateTime MinDate = Convert.ToDateTime("1932-01-01");
        public EditModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = default!;
        public string Message;
        public bool Status = false;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin") && !account.Role.ToLower().Equals("teacher"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    if (account.Role.ToLower().Equals("admin"))
                    {
                        Account = account;

                        if (id == null || _context.Teachers == null)
                        {
                            return NotFound();
                        }

                        var teacher = await _context.Teachers.Include(t => t.Account).FirstOrDefaultAsync(m => m.TeacherId == id);
                        if (teacher == null)
                        {
                            return NotFound();
                        }
                        Teacher = teacher;
                        ViewData["Email"] = _context.Accounts.Where(a => a.AccountId != 1)
                                                             .Select(a => new SelectListItem { Value = a.AccountId.ToString(), Text = a.Email })
                                                             .ToList();
                    }
                    else
                    {
                        if (id == null || _context.Teachers == null)
                        {
                            return NotFound();
                        }

                        var teacher = await _context.Teachers.Include(t => t.Account).FirstOrDefaultAsync(m => m.TeacherId == id);
                        if (teacher == null)
                        {
                            return NotFound();
                        }
                        Teacher = teacher;
                        Account = teacher.Account;
                        ViewData["Email"] = _context.Accounts.Where(a => a.AccountId == account.AccountId)
                                                             .Select(a => new SelectListItem { Value = a.AccountId.ToString(), Text = a.Email })
                                                             .ToList();
                    }
                    return Page();
                }
            }
            return Redirect("/Index");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Teacher).State = EntityState.Modified;


            try
            {
                _context.Update(Teacher);
                _context.SaveChanges();
                Message = "Successfull!";
                Status = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(Teacher.TeacherId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.TeacherId == id)).GetValueOrDefault();
        }
    }
}
