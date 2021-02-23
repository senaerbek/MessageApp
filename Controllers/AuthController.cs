using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR.Models;

namespace SignalR.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;
        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }


        public IActionResult FacebookLogin(string ReturnUrl)
        {
            string redirectUrl = Url.Action("FacebookResponse", "Auth", new { ReturnUrl = ReturnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);

        }

        public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
        {

            ExternalLoginInfo loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("Login", "Auth");

            else
            {
                Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
                if (loginResult.Succeeded)
                    return Redirect(returnUrl);
                else
                {
                    User user = new User
                    {
                        Email = loginInfo.Principal.FindFirst(ClaimTypes.Email).Value,
                        UserName = loginInfo.Principal.FindFirst(ClaimTypes.Email).Value
                    };
                    IdentityResult createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        IdentityResult addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);

                        if (addLoginResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, true);
                            return Redirect(returnUrl);
                        }
                    }

                }
            }
            return Redirect(returnUrl);
        }


        [HttpPost]
        public async Task<IActionResult> Login(string uName, string password)
        {
            var user = await _userManager.FindByNameAsync(uName);
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string uName, string password)
        {
            var user = new User
            {
                UserName = uName
            };

            var AddUser = await _userManager.CreateAsync(user, password);
            if (AddUser.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Register", "Auth");
        }

        public async Task<IActionResult> LogOut(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}