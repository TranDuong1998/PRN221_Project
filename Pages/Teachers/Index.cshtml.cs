using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;
        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();

        private Account Account;

        public IndexModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IList<Teacher> Teacher { get; set; } = default!;

        private int PageSize = 10;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; } = default;

        public async Task<IActionResult> OnGetAsync(int pageIndex)
        {
            if (_httpContext.HttpContext!.Session.GetString("Account") != null)
            {
                var account = session.GetObject(_httpContext.HttpContext!.Session, "Account");
                if (!account.Role.ToLower().Equals("admin"))
                {
                    return Redirect("/Index");
                }
                else
                {
                    var teachers = _context.Teachers.Include(t => t.Account)
                                                    .Where(t=> (Search==null || t.FullName.ToLower().Contains(Search.ToLower()) ||
                                                                t.TeachersCode.ToLower().Contains(Search.ToLower()) ||
                                                                t.Phone.Contains(Search) ||
                                                                t.Address.ToLower().Contains(Search.ToLower()) ||
                                                                t.Account.Email.ToLower().Contains(Search.ToLower()))).ToList();

                    TotalPages = (int)Math.Ceiling((double)teachers.Count() / PageSize);

                    CurrentPage = Math.Max(1, Math.Min(PageIndex, TotalPages));

                    Teacher = teachers.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                    ViewData["Search"] = Search;
                    return Page();
                }
            }
            else
                return Redirect("/Index");
        }
    }
}
