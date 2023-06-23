- identity 4: Thêm 2 Option về ExternalLog: Google and Facebook
- để từ http -> https thì tại file launchSettings đưa https lên trc http

dotnet add package Microsoft.AspNetCore.Authentication.Facebook
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
dotnet add package Microsoft.AspNetCore.Authentication.Twitter

để sử dụng OAuth nào thì thêm vào Programs
services.AddAuthentication()
    .AddMicrosoftAccount(microsoftOptions => { ... })   // thêm provider Microsoft và cấu hình
    .AddGoogle(googleOptions => { ... })                // thêm provider Google và cấu hình
    .AddTwitter(twitterOptions => { ... })              // thêm provider Twitter và cấu hình
    .AddFacebook(facebookOptions => { ... });           // thêm provider Facebook và cấu hình

- các thông tin để truy cập API của Provider gồm : client-id và client-secret: *ĐÂY LÀ CÁC THÔNG TIN NHẠY CẢM*
Google: console.cloud.google.com
- Sau khi đăng ký provider API lên ứng dụng mà ta muốn xác thực thì ta cấu hình tiếp trong file appsettings
{
  // Các cấu hình khác

  "Authentication": {
    "Google": {
      "ClientId": "client-ID",
      "ClientSecret": "client-Secret",
    }
  }
}

- Sau đó Configure Services trong Programs
services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
 
        // Thiết lập ClientID và ClientSecret để truy cập API google
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
        googleOptions.CallbackPath = "/login-google";

    });

- Khi người dùng dùng External thì nó sẽ gọi OnPost lấy các thông tin clientId, clientSecret ,..
- Sau đó chuyển hướng về OnGetCallbackASync

 // Nếu có key ErrorMessage thì tức có lỗi , thì sẽ load lỗi lên form
    this.TempData.ContainsKey("ErrorMessage")
- StatusMessage để hiển thị lỗi , chuỗi của nó là string
<partial name="/Areas/Identity/Pages/Account/_StatusMessage.cshtml" />

  //Input.Email 
    var registeredUser = await _userManager.FindByEmailAsync(Input.Email);
    string externalEmail = null;
    AppUser externalEmailUser = null;
    // tìm xem có Email để lấy ra không.
    if(info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)){
        externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
    }
    //tìm kiếm email theo external email sau do gan' vao externalEmail
    if(externalEmail != null){
        externalEmailUser = await _userManager.FindByEmailAsync(Input.Email);
    }

    if((registeredUser != null) && (externalEmailUser != null)){
        //externalEmail == Input.Email
        if(registeredUser.Id == externalEmailUser.Id){
            // Lien ket tai khoan dang nhap
            var resultLink = await _userManager.AddLoginAsync(registeredUser, info);
            if(resultLink.Succeeded){
                await _signInManager.SignInAsync(registeredUser, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }
        else{
            // 2 email khac nhau
            // registeredUser = externalEmailUser ( externalEmail != Input.Email)
            // info => user1 (email1)
            //      => user2 (email2)

            ModelState.AddModelError(string.Empty , "Không liên kết được tài khoản , hãy sử dụng tài khoản khác!!!");


        }
    }
    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
        returnUrl = returnUrl ?? Url.Content("~/");
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Lỗi lấy thông tin đăng nhập third-party khi xác nhận";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        *Tiếp cái trên*
            // trong bd đã liên kết account khac , nhma email input vao ko ton tai trong db
            if((externalEmailUser != null) && (registeredUser == null)){
                ModelState.AddModelError(string.Empty , "Không hỗ trợ tạo tài khoản mới - có email khác email từ provider !!!");
                return Page();
            }
            // TH : tim email o cac dich vu khac nhung khong co
            if((externalEmail == null) && (externalEmail == Input.Email)){
                // chua co taikhoan -> tao tai khoan, lien ket va dang nhap
                var newUser = new AppUser(){
                    UserName = externalEmail,
                    Email = externalEmail
                };
                // 1.Tao account
                var resultNewUser = await _userManager.CreateAsync(newUser);
                // 2. Dang nhap , xac nhan email , lien ket email
                if(resultNewUser.Succeeded){
                    await _userManager.AddLoginAsync(newUser, info);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    await _userManager.ConfirmEmailAsync(newUser, code);

                    await _signInManager.SignInAsync(newUser, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                else{
                    ModelState.AddModelError(string.Empty , "Không tạo được tài khoản mới, đã tôn tại!!!");
                    return Page();
                }
            }


    - khi mà tài khoản dc tạo thông qua thirdparty thì lúc này nó ko có mật khẩu 
    - Do đó không thể bỏ Dùng Third-Party chỉ khi nào nhập xong mật khẩu
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    - Các phương thức được dùng:

    1.Lấy các dịch vụ xác thực ngoài
        var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync ()).ToList ();

    2. Lấy thông tin đăng nhập từ tài khoản của phiên hiện tại (sau khi được chuyển hướng từ dịch vụ ngoài về ứng dụng)
        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync ();

    3. Đăng nhập tài khoản bằng thông tin từ dịch vụ ngoài
        var result = await _signInManager.ExternalLoginSignInAsync (info.LoginProvider, info.ProviderKey, isPersistent : false, bypassTwoFactor : true);

    4.Tìm User theo thông tin liên kết với dịch vụ ngoài
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

    5. Liên kết User với dịch vụ ngoài
        var rs = await _userManager.AddLoginAsync (user, info);

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    - FACEBOOK- PARTY : /login-facebook
    vào : developers.facebook.com/apps/

    dotnet add package Microsoft.AspNetCore.Authentication.Facebook
    "Facebook": {
      "AppId": "điền ID ứng dụng Facebook",
      "AppSecret": "điền mã số bí mật"
    }

    sau đó config Programs
    services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        // Cấu hình Google từ bài trước ... (bỏ đi nếu không dùng)
    })
    .AddFacebook(facebookOptions => {
        // Đọc cấu hình
        IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
        facebookOptions.AppId = facebookAuthNSection["AppId"];
        facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
        // Thiết lập đường dẫn Facebook chuyển hướng đến
        facebookOptions.CallbackPath = "/login-facebook";
    });

    - Còn lỗi do Facebook 
    Sorry, something went wrong.
    We're working on getting this fixed as soon as we can.

    Go Back