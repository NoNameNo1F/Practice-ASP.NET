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
    public class IndexModel : PageModel
    {
        private readonly razorweb.models.MyBlogContext _context;

        public IndexModel(razorweb.models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;
        public const int ITEM_PER_PAGE = 10;
        [BindProperty(SupportsGet = true, Name ="p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public async Task OnGetAsync(string SearchString)
        {
            // if (_context.articles != null)
            // {
            //     Article = await _context.articles.ToListAsync();
            // }
            // Article = await _context.articles.ToListAsync();
           int totalArticle = await _context.articles.CountAsync();
           countPages = (int)Math.Ceiling((double)totalArticle /ITEM_PER_PAGE);
            if(currentPage < 1)
                currentPage = 1;

            var qr = (from a in _context.articles
                    orderby a.Created descending
                    select a)
                    .Skip((currentPage - 1) * 10)
                    .Take(ITEM_PER_PAGE);
            if(!string.IsNullOrEmpty(SearchString)){
                Article = qr.Where(a => a.Title.Contains(SearchString)).ToList();
            }
            else{
                Article = await qr.ToListAsync();
            }
        }
    }
}
