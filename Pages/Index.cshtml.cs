using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razorweb.models;

namespace asprazor04.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly MyBlogContext myBlogContext;
    public IndexModel(ILogger<IndexModel> logger, MyBlogContext _myContext)
    {
        _logger = logger;
        myBlogContext = _myContext;
    }

    public void OnGet()
    {
        var posts = (from a in myBlogContext.articles
                    orderby a.Created descending
                    select a).ToList();
        ViewData["posts"] = posts;
    }
}
