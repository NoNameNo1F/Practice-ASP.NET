/// In ra thông tin các Header của HTTP Response
public static void ShowHeaders(HttpHeaders headers)
{
    Console.WriteLine("CÁC HEADER:");
    foreach (var header in headers)
    {
        foreach (var value in header.Value)
        {
            Console.WriteLine($"{header.Key,25} : {value}");
        }
     }
    Console.WriteLine();
 }

 public User user {set; get; }

 [FromQuery] => var header_name = this.Request.Query["id"];
 [FromRoute] => var header_name = this.Request.RouteValues["id"];
 [FromForm] => var header_name = this.Request.Form["id"];
 [FromHeaders] => var header_name = this.Request.Headers["id"];
 [FromBody] => var header_name = this.Request.Query["id"];

if (! string.IsNullOrEmpty(header_name)){

}



/// từ api chỉ định đọc từ params public void OnGet([FromRoute]int? id );
 BindProperty nó sẽ tìm các nguồn gửi đến , đọc và gán vào, chỉ thực hiện Binding với POST method và 
 action để post gửi đến là asp-page 

 thay vì sử dụng các html element <input name="UserID,...">  thì viết thẻ input và 
 sự dụng phần tử input cho phần tử trong model
 <label asp-for="@Model.UserID"></label>
<input asp-for="@Model.UserID"/>
<input asp-for="@Model.UserName"/>
<input asp-for="@Model.Password"/>
<input asp-for="@Model.Email"/>
