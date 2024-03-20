using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PRN211_Project.Pages.Shared.ViewComponents
{
    public class PaginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string pageUrl,int totalPage, int currentPage, int pageIndex, int displayRange,
                                            string filter, string viewDate, string search)
        {
            var model = new PaginationViewModel
            {
                PageUrl = pageUrl,
                TotalPage = totalPage,
                CurrentPage = currentPage,
                PageIndex = pageIndex,
                DisplayRange = displayRange,
                Filter = filter,
                ViewDate = viewDate,
                Search = search
            };

            return View("/Pages/Shared/Component/Pagination/Pagination.cshtml", model);
        }
    }
    public class PaginationViewModel
    {
        public string PageUrl { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageIndex { get; set; }
        public int DisplayRange { get; set; }
        public string Filter { get; set; }
        public string ViewDate { get; set; }
        public string Search { get; set; }
    }
}
