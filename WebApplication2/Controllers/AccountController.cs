using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
		{
			if (User.Identity.IsAuthenticated)
				return BadRequest();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (User.Identity.IsAuthenticated)
				return BadRequest();
			if (!ModelState.IsValid)
					return View();
			AppUser newUser = new()
			{
				Fullname = registerViewModel.FullName,
				Email = registerViewModel.Email,
				UserName = registerViewModel.UserName,
				IsActive = true,
			};

			var identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);
			if (!identityResult.Succeeded)
			{
				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View();
			}

			return RedirectToAction(nameof(Login));

		}

		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
				return BadRequest();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (User.Identity.IsAuthenticated)
				return BadRequest();


			if (!ModelState.IsValid)
				return View();
			var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
			if(user is null)
			{
				ModelState.AddModelError("", "Username or Password invalid");
				return View();
			}

		       var signInResult=await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
			if (signInResult.IsLockedOut)
			{
				ModelState.AddModelError("", "Your account is blocked temporary");
					return View();
			}
			if (!signInResult.Succeeded)
			{
				ModelState.AddModelError("", "Username or Password invalid");
				return View();
			}


			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> Logout()
		{
			if (User.Identity.IsAuthenticated)
			{
		    	await _signInManager.SignOutAsync();

			     return RedirectToAction(nameof(Login));
			}
			return BadRequest();
		}

	}
}
