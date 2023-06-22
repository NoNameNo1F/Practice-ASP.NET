Yêu cầu 2 middleware

dotnet add package System.Data.SqlClient
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Console


dotnet add package Microsoft.AspNetCore.Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.AspNetCore.Authentication
dotnet add package Microsoft.AspNetCore.Http.Abstractions
dotnet add package Microsoft.AspNetCore.Authentication.Cookies
dotnet add package Microsoft.AspNetCore.Authentication.Facebook
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
dotnet add package Microsoft.AspNetCore.Authentication.oAuth
dotnet add package Microsoft.AspNetCore.Authentication.OpenIDConnect
dotnet add package Microsoft.AspNetCore.Authentication.Twitter
    
app.UseAuthentication();
app.UseAuthorization();

đổi tên bảng khi tạo thêm bảng trong mssql:
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            foreach ( var entityType in modelBuilder.Model.GetEntityTypes()){
                var tableName = entityType.GetTableName();
                if(tableName.StartsWith("AspNet")){
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }

        // Truy cập IdentityOptions
services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

});

/Identity/Account/Login
/Identity/Account/Manage

- Các trang Identity này được cài đặt trong Areas/Identity/Pages
- Để navbar biết dc user đã đăng nhập hay chưa thì dùng 2 lớp sau:
    + SignInManager<AppUser> s;
    + UserManager<AppUser> u;
        - Được khai trong _LoginParital.cshtml 
        inject 2 dịch vụ này @using Microsoft.AspNetCore.Identity;
                                @using razorweb.models;
                                                        
                        @inject SignInManager<AppUser> SignInManager
                        @inject UserManager<AppUser> UserManager

- 1 vấn đề nữa: trong các Controller , PageModel kể cả View đều có 1 Properties là User
    #this.User {ClaimsPrincipal } // lấy thông tin đăng nhập của USer, được thiết lập trong mỗi truy vấn do 2 cái middleware Authen/Authori DO ĐỐ có thể
    dùng 2 cái DỊCH VỤ để KIỂM TRA ĐĂNG NHẬP OR KHÔNG

- Sau khi đăng ký mà ko xác thực email thì đéo cho vô , 
- Để đăng ký dịch vụ xác thực email khi đăng ký thì 
                + EmailSender
        add 2package:
                dotnet add package MailKit
                dotnet add package MimeKit
    Xây dựng nó trong thư mục Services
    //Khoan copy cái cấu hình này vào appsettings cái
        {

            // các mục khác của config

            "MailSettings": {
                "Mail": "gmail của bạn",
                "DisplayName": "Tên Hiện Thị (ví dụ XUANTHULAB)",
                "Password": "passowrd ở đây",
                "Host": "smtp.gmail.com",
                "Port": 587
            }
            }
    - Services/SendMailService nhớ import
    - Sau khi có dịch vụ SendMailService phải đăng ký nó dưới tên IEmailSender thì Identity mới sử dụng
    - rồi thêm cái này vào Program
    Singleton tạo 1 lần dùng mãi mãi ? dùng suốt quá trình hoạt động
            builder.Services.AddOptions();
            var mailSetting = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailSetting);
            builder.Services.AddSingleton<IEmailSender, SendMailService>();
            *trong hệ thống có dịch vụ tên IEmailSender khi dịch vụ này được lấy ra thì nó là đối tượng SendMailService*
            builderServices.AddTransient : cái này thì mỗi khi truy vấn nó sẽ tạo 1 dịch vụ send emailservice 

    - Để tùy biến theo custom: 
        *dotnet aspnet-codegenerator identity -dc razorweb.models.MyBlogContext*

    *LƯU Ý*
    - Đối với lỗi CS0121: 
    /////////////////////////////////////////////////////////////////////////////////////////////////
    //error CS0121: The call is ambiguous between the following methods or properties: 'Microsoft.AspNetCore.Identity.IdentityBuilderExtensions. 
    //AddDefaultTokenProviders(Microsoft.AspNetCore.Identity.IdentityBuilder)' and 'Microsoft.AspNetCore.Identity.IdentityBuilderExtensions. 
    //AddDefaultTokenProviders(Microsoft.AspNetCore.Identity.IdentityBuilder)'
    //at Microsoft.VisualStudio.Web.CodeGeneration.ActionInvoker.<BuildCommandLine>b__6_0()
    //at Microsoft.Extensions.CommandLineUtils.CommandLineApplication.Execute(String[] args)
    //at Microsoft.VisualStudio.Web.CodeGeneration.ActionInvoker.Execute(String[] args)
    //at Microsoft.VisualStudio.Web.CodeGeneration.CodeGenCommand.Execute(String[] args)
    //RunTime 00:00:08.54
    /////////////////////////////////////////////////////////////////////////////////////////////////
            thì commentLine tại dòng r redo lại.