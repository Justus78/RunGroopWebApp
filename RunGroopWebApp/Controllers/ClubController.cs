using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using System.Reflection.Metadata.Ecma335;

/////////////////////////////////////////////////////////////
/// This Repository uses the IClubInterface and is injected /
/// into the project with dependency inkection in the       /
/// program.cs file.                                        / 
/////////////////////////////////////////////////////////////
namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository ClubRepository; // bring in the interface here and constructor
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            ClubRepository = clubRepository; // add the reposity for club
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        } // end constructor
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await ClubRepository.GetAll();
            return View(clubs);
        } // end index action

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await ClubRepository.GetByIdAsync(id);
            return View(club);
        } // end Get detail action

        public IActionResult Create() 
        {
            // get the user using an extension
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId
            };
            return View(createClubViewModel);
        } // end create action

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };
                ClubRepository.Add(club);
                return RedirectToAction("Index", "Club");
            } else
            {
                ModelState.AddModelError("", "Photo upload failed.");
            }
            return View(clubVM);
        } // end post action Create

        public async Task<IActionResult> Edit(int id)
        {
            var club = await ClubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        } // end edit action

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await ClubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    // delete the current photo from cloudinary for the club
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch {
                    // if error, return to the edit view with the clubVM
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address
                };

                ClubRepository.Update(club);

                return RedirectToAction("Index");
            } else
            {
                return  View(clubVM);
            }             
        } // end edit post action


    } // end controller
} // end namespace
