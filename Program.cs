using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


using Album.Mail;
using razorweb.models;

var builder = WebApplication.CreateBuilder(args);


// trước 35
builder.Services.AddOptions();
var mailSetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailSetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>(options =>{
    string? connectString = builder.Configuration.GetConnectionString("MyBlogContext");
    options.UseSqlServer(connectString);
});

// builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MyBlogContext>();

//////////////////////////////////////////////////////////////
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MyBlogContext>()
                .AddDefaultTokenProviders();
////////////////////////////////////////////////////////////////

// builder.Services.AddDefaultIdentity<AppUser>()
//                 .AddEntityFrameworkStores<MyBlogContext>()
//                 .AddDefaultTokenProviders();
//Config thêm Identity Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user 
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;
});

builder.Services.ConfigureApplicationCookie(options =>{
    options.LoginPath ="/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/access-denied";
});

// Config Provider Third Party Authentication
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>{
        // doc thongtin Authentication:google tu appsettings
          var googleAuthSection = builder.Configuration.GetSection("Authentication:Google");

          // Thiet lap Client-ID va Client-Secret
          googleOptions.ClientId = googleAuthSection["ClientId"];
          googleOptions.ClientSecret = googleAuthSection["ClientSecret"];
          // Cau hinh Url Callback cho google default la: signin-google
          googleOptions.CallbackPath = "/login-google";          
    })
    .AddFacebook(facebookOptions => {
        var facebookAuthSection = builder.Configuration.GetSection("Authentication:Facebook");

        facebookOptions.AppId = facebookAuthSection["AppId"];
        facebookOptions.AppSecret = facebookAuthSection["AppSecret"];
        facebookOptions.CallbackPath = "/login-facebook"; 
    })
    // .AddTwitter(twitterOptions => {}) 
    // .AddMicrosoftAccount(microsoftOptions => {}) 
    ;


var app = builder.Build();
app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// IdentityUser identityUser;
// IdentityDbContext identityDbContext;


app.MapRazorPages();

app.Run();
