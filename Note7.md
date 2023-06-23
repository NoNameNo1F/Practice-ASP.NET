- Tại login page thì form-login yêu cầu là Email thay cho UserName
var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

var result = await _signInManager.PasswordSignInAsync(Input.UserNameOrEmail, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                // TÌm theo Email
                if(!result.Succeeded){
                    var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);
                    if (user != null){
                        // TÌm theo Email 
                        result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                    }
                }


- Thiết lập Lockout , params cuối mở là true, sau đó cấu tại file Programs

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;
- Có thể enable LockStatuc bằng truy cập vào 1 trong 2 cách:

*UPDATE Users SET LockoutEnabled = 0 WHERE Email = '{email-locked}@gmail.com'*
*UPDATE Users SET Lockout = null WHERE Username = '{user-locked}'*


- Để biết được User đã đăng nhập hay chưa có 2 cách:
    1. User.Identity?.IsAuthenticated
    2. _signInManager.IsSignedIn(User)
    
- Token/Code để Register và Đặt lại password là như nhau

*ĐIỀU CHỈNH CÁC PAGE TRONG IDENTITY*

- Chỉnh sửa form-input , các trường asp-for như là UserName , Password ,...
- Tiếp theo là các method , Post , Get ,...
- chỉnh sửa View Title, page" /url,.."

*RAZOR-PAGE => URL-LINK*

Areas/Identity/Pages/Error  -> error    
Areas/Identity/Pages/Account/AccessDenied   -> access-denied           

Areas/Identity/Pages/Account/ConfirmEmail   -> confirm-email
Areas/Identity/Pages/Account/ConfirmEmailChange     -> confirm-email

Areas/Identity/Pages/Account/ExternalLogin  -> external-login

Areas/Identity/Pages/Account/ForgotPassword -> forgot-password
Areas/Identity/Pages/Account/ForgotPasswordConfirmation -> forgot-password-confirmation

Areas/Identity/Pages/Account/Lockout    ->  lock-out

Areas/Identity/Pages/Account/Login  -> login
Areas/Identity/Pages/Account/LoginWith2Fa   -> login-2fa
Areas/Identity/Pages/Account/LoginWithRecoveryCode  -> login-recovery-code

Areas/Identity/Pages/Account/Logout ->  logout

Areas/Identity/Pages/Account/Register   -> register
Areas/Identity/Pages/Account/RegisterConfirmation   -> register-confirm
Areas/Identity/Pages/Account/ResendEmailConfirmation    -> resend-email-confirmation

Areas/Identity/Pages/Account/ResetPassword  -> reset-password
Areas/Identity/Pages/Account/ResetPasswordConfirmation  -> reset-password-confirmation

//////////////////////////////////////////////////////////////////////////////////////////////////////
//      Areas/Identity/Pages/Account/Manage/ChangePassword -> Account/change-password
//      Areas/Identity/Pages/Account/Manage/DeletePersonalData -> Account/delete-personal-data
//      Areas/Identity/Pages/Account/Manage/Disable2fa  -> Account/disable-2fa
//      Areas/Identity/Pages/Account/Manage/DownloadPersonalData -> Account/download-personal-data
//      Areas/Identity/Pages/Account/Manage/Email -> Account/email
//      Areas/Identity/Pages/Account/Manage/EnableAuthenticator -> Account/enable-authenticator
//      Areas/Identity/Pages/Account/Manage/ExternalLogins -> Account/external-logins
//      Areas/Identity/Pages/Account/Manage/GenerateRecoveryCodes -> Account/generate-recovery-codes
//      Areas/Identity/Pages/Account/Manage/Index -> Account/
//
//      Areas/Identity/Pages/Account/Manage/ExternalLogins -> Account/external-logins
//      Areas/Identity/Pages/Account/Manage/PersonalData -> Account/personal-data
//      Areas/Identity/Pages/Account/Manage/ResetAuthenticator -> Account/reset-authenticator
//
//      Areas/Identity/Pages/Account/Manage/SetPassword -> Account/set-password
//      Areas/Identity/Pages/Account/Manage/ShowRecoveryCodes -> Account/show-recovery-codes
//      Areas/Identity/Pages/Account/Manage/TwoFactor-Authentication -> Account/two-factor-authentication
//////////////////////////////////////////////////////////////////////////////////////////////////////


- identity3: chinh sua Login/ Logout / ResetPassword /Lockout /