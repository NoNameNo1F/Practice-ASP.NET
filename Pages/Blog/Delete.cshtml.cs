using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace asprazor04.Pages_Blog
{
    public class DeleteModel : PageModel
    {
        private readonly razorweb.models.MyBlogContext _context;

        public DeleteModel(razorweb.models.MyBlogContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.articles == null)
            {
                return Content("Không tìm thấy bài viết");
            }

            var article = await _context.articles.FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return Content("Không tìm thấy bài viết");
            }
            else 
            {
                Article = article;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.articles == null)
            {
                return Content("Không tìm thấy bài viết");
            }
            var article = await _context.articles.FindAsync(id);

            if (article != null)
            {
                Article = article;
                _context.articles.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
