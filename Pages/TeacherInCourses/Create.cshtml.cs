﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN211_Project.Models;
using PRN211_Project.Services;

namespace PRN211_Project.Pages.TeacherInCourses
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
                {
                    LoadData();
                    return Page();
                }
            }
            return Redirect("/Index");
        }

        [BindProperty]
        public TeacherDetail TeacherDetail { get; set; } = default!;

        public void LoadData()
        {
            var teachers = _context.Teachers.ToList();
            var courses = _context.Courses.ToList();

            ViewData["CourseId"] = courses.Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.CourseCode }).ToList();
            ViewData["TeacherId"] = teachers.Select(t => new SelectListItem { Value = t.TeacherId.ToString(), Text = t.TeachersCode }).ToList();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            LoadData();
            if (!ModelState.IsValid || _context.TeacherDetails == null || TeacherDetail == null)
            {
                return Page();
            }

            _context.TeacherDetails.Add(TeacherDetail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
