using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using System.ComponentModel;

namespace RunGroopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ClubContext _context;


        // constructor to bring in Managers and Context class
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ClubContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        } // end action

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) { return View(loginViewModel); }

            // get the user from the database by their email address
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            // if the user is found above
            if (user != null)
            {
                // returns true if the password matches, false if not
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                // if the passwork matches
                if (passwordCheck)
                {
                    // sign in with the user and password
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    } // end if
                } // end if 
                // if password check failed
                TempData["Error"] = "Wrong credentials. Please try again.";
                return View(loginViewModel);
            } // end if 
            
            // if no user if found
            TempData["Error"] = "Wrong credentials. Please try again.";
            return View(loginViewModel);
        } // end post login action

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        } // end action

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) { return View(registerViewModel); }

            // search the AppUser table for a user by the email address
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            // if user is found in the database, that means the email is already in use
            if (user != null)
            {
                TempData["Error"] = "This email is already in use";
                return View(registerViewModel);
            } // end if

            // make a new user
            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };

            // create the new user with the user manager
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            // if new user creation was successfule, add to database with user roles
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Race");
        } // end action

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");
        } // end action for logout


    } // end controller
} // end namespace
