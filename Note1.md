/*
    Razor page (.cshtml) = htlm + c#
    Engine Razor => bien dich cshtml => Response
    - @page
    - Xuất giá trị tên biến: - @param, @(biểu thức), @phương thức
    -
    @{
        <HTML> ???</HTML>
    }

    Những trang Razor page lưu ở thư mục mặc định là Pages
    Nếu lưu ở thư mục khác: ví dụ "Content"
        Services.AddRazorPages() trả về  đối tượng MVCBuilder
        ta có thể thiết lập thêm bằng AddRazorPagesOptions(options => {
            options.RootDirectory = "/Pages"; <= default
            options.RootDirectory = "/Content";
        })

    Rewrite URL FirstPage URL => trang-thu-nhat
    1.
        options.Conventions.AddPageRoute("/FirstPage","trang-thu-nhat");
    2.
        hoặc chúng ta có thể set tại chỉ định page của từng cshtml
        @page "/trang-thu-nhat/"
        @page "/trang-thu-nhat/{n:int?}" : n có thể có hoặc không.

    tai dịch vụ 
    nó sẽ tạo ra endpoint
    DichVu/dv1 DichVu/dv2



    TagHelper => HTML
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelper
    <a  asp-page="Dichvu1">Dichvu 1</a>

    cấu hình lowercase URL
    builder.Services.Configure<RouteOptions>(routeOptions => {
        routeOptions.LowercaseUrls = true;
    });




    trangchu thi se co layout
    con register va login , se set layout = null;
*/

