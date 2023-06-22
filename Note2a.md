Trong PageMode thì các action của nó sẽ return về Partial, ViewComponent
Còn đối với MVVM , MVC model thì Controller sẽ return PartialView


Pages\Shared\Component\ProductBox\Default.cshtml
Pages\Shared\Component\ProductBox\ProductBox.cs

public IActionResult OnGet(){
    return ViewComponent("ProductBox");
    //PageModel.ViewComponent(string componentName, object args);
}

MessagePage: ViewComponent | Lớp này dùng để hiển thị 1 hộp thông báo , có thể xuất hiện trong 1 khoản bnh giây ví dụ

Views/Shared/Components/MessagePage/MessagePage.cs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NoName
{
    [ViewComponent]
    public class MessagePage : ViewComponent
    {
        public const string COMPONENTNAME = "MessagePage";
        // Dữ liệu nội dung trang thông báo
        public class Message {
            public string title {set; get;} = "Thông báo";     // Tiêu đề của Box hiện thị
            public string htmlcontent {set; get;} = "";         // Nội dung HTML hiện thị
            public string urlredirect {set; get;} = "/";        // Url chuyển hướng đến
            public int secondwait {set; get;} = 3;              // Sau secondwait giây thì chuyển
        }
        public MessagePage() {}
        public IViewComponentResult Invoke(Message message) {
            // Thiết lập Header của HTTP Respone - chuyển hướng về trang đích
            this.HttpContext.Response.Headers.Add("REFRESH",$"{message.secondwait};URL={message.urlredirect}");
            return  View(message);
        }
    }
}


public IActionResult OnPost(){
    var username = this.Request.Form("username");
    var password = this.Request.Form("password");
    var payload = new Payload();
    payload.htmlcontent = "Login/ registration successful";
    payload.secondwait = 2;
    payload.urlredirect = Url.Page("/"); <= đây là Homepage , còn cái default kia là Landing page.

}