- Để tích hợp quyền truy cập vào 1 nguồn tài nguyên thì 
[Authorize] // using Microsoft.AspNetCore.Authorization;
Attach vào model của page 
- Trong file Program cần cấu hình thêm login , logout và access-denied uri

 builder.Services.ConfigureApplicationCookie(options =>{
        options.LoginPath ="/login/";
        options.LogoutPath = "/logout/";
        options.AccessDeniedPath = "/access-denied";
    });
    
- Trong Programs , để yêu cầu người dùng cần xác thực tài khoản ms có thể (ĐĂNG NHẬP)truy cập tài nguyên
thì cấu hình thêm
        *options.SignIn.RequireConfirmedAccount = true;* default là false
        
Tại dòng : 156 / Register.cshtml.cs
    await _signInManager.SignInAsync(user, isPersistent: false);
    isPersistent = true: dùng để tạo 1 token-login cứng 


Tại file ConfirmEmail
để sau khi xác nhận email thành công thì chuyển hướng đăng nhập vào luôn thì ta cấu hình tiếp
////////////////////////////////////////////////////////////////////////////////////////////////
    private readonly SignInManager<AppUser> _signInManager;
    public ConfirmEmailModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager; // thêm vào signIn để sau khi xác thực đăng mẹ nhập luôn
    } 

    sau đó: 
     // nếu success thì đăng nhập luôn email
        if(result.Succeeded){
            await _signInManager.SignInAsync(user, false);
            return RedirectToPage("/Index");
        }
        else{
            return Content("Lỗi xác thực email!!!");
        }
                
        // return Page();
////////////////////////////////////////////////////////////////////////////////////////////////
