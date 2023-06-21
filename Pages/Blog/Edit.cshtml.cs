using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using razorweb.models;

namespace asprazor04.Pages_Blog
{
    public class EditModel : PageModel
    {
        private readonly razorweb.models.MyBlogContext _context;

        public EditModel(razorweb.models.MyBlogContext context)
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

            var article =  await _context.articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return Content("Không tìm thấy bài viết"); 
            }
            Article = article;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return Content("Không tìm thấy bài viết"); 
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
          return (_context.articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
