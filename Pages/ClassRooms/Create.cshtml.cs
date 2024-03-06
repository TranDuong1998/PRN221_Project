﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.ClassRooms
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
            }else
                return Redirect("/Index");
        }

        [BindProperty]
        public ClassRoom ClassRoom { get; set; } = default!;
        public bool CheckAccount = true;
        public string Message;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.ClassRooms == null || ClassRoom == null)
            {
                return Page();
            }

            if (ClassRoomExists(ClassRoom.ClassName))
            {
                CheckAccount = false;
                Message = "Classroom dose exists!";
                return Page();
            }

            _context.ClassRooms.Add(ClassRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool ClassRoomExists(string name)
        {
            return (_context.ClassRooms?.Any(e => e.ClassName.ToLower().Equals(name))).GetValueOrDefault();
        }
    }
}
