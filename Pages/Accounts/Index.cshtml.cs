﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly PRN211_Project.Models.Prn211ProjectContext _context;

        private IHttpContextAccessor _httpContext;
        private GetSession session = new GetSession();
        public IndexModel(PRN211_Project.Models.Prn211ProjectContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IList<Account> Account { get; set; } = default!;

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
                    var accs = _context.Accounts.Where(a => a.AccountId != 1 &&
                                                (Search == null || a.Email.ToLower().Contains(Search.ToLower()) ||
                                                                   a.UserName.ToLower().Contains(Search.ToLower())))
                                               .ToList();

                    TotalPages = (int)Math.Ceiling((double)accs.Count() / PageSize);

                    CurrentPage = Math.Max(1, Math.Min(PageIndex, TotalPages));

                    Account = accs.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                    ViewData["Search"] = Search;
                    return Page();
                }
            }
            else
                return Redirect("/Index");

        }
    }
}
