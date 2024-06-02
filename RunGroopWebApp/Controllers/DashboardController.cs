using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _DashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, 
            IHttpContextAccessor httpContextAccessor, IPhotoService photoService) 
        {
            _DashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        } // end constructor

        // add our own mapper for the AppUser
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoresult)
        {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = photoresult.Url.ToString();
            user.City = editVM.City;
            user.State = editVM.State;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _DashboardRepository.GetAllRaces();
            var userClubs = await _DashboardRepository.GetAllClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        } // end action

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _DashboardRepository.GetUserById(curUserId);
            if (user == null) { return View("Error"); }
            var editUserViewModel = new EditUserDashboardViewModel
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        } // end edit user profile action

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit profile");
                return View("EditUserProfile", editVM);
            }// end if 

            var user = await _DashboardRepository.GetUserByIdNoTracking(editVM.Id);

            // if the user has no profile pic yet
            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null) 
            {
                // add the photo to cloudinary
                var photoService = await _photoService.AddPhotoAsync(editVM.Image);

                //Optimistic Comcurrency - "Tracking Error"
                MapUserEdit(user, editVM, photoService);

                _DashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            // when the user already has a profile pic uploaded
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                } catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }

                // add the photo to cloudinary
                var photoService = await _photoService.AddPhotoAsync(editVM.Image);

                //Optimistic Comcurrency - "Tracking Error"
                MapUserEdit(user, editVM, photoService);

                _DashboardRepository.Update(user);
                return RedirectToAction("Index");
            } // end if else

        } // end post action


    } // end controller
} // end namespace
