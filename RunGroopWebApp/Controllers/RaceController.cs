using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository RaceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _contextAccessor;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor contextAccessor)
        {
            RaceRepository = raceRepository;
            _photoService = photoService;
            _contextAccessor = contextAccessor;
        } // end constructor
        public async Task<IActionResult> Index()
        {
            var races = await RaceRepository.GetAll();
            return View(races);
        }  // end index action

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await RaceRepository.GetByIdAsync(id);
            return View(race);
        } // end Get detail action

        public IActionResult Create()
        {
            // get the user id using the extension
            var curUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var createUserViewModel = new CreateRaceViewModel
            {
                AppUserId = curUserId
            };
            return View(createUserViewModel);
        } // end create action

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceVM.AppUserId,
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                RaceRepository.Add(race);
                return RedirectToAction("Index", "Club");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed.");
            }
            return View(raceVM);
        } // end post action Create

        public async Task<IActionResult> Edit(int id)
        {
            var race = await RaceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");

            var clubVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(clubVM);
        } // end edit action

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVM);
            }

            var userRace = await RaceRepository.GetByIdAsyncNoTracking(id);

            if (userRace != null)
            {
                try
                {
                    // delete the current photo from cloudinary for the club
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch
                {
                    // if error, return to the edit view with the clubVM
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address
                };

                RaceRepository.Update(race);

                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }
        } // end edit post action
    } // end controller
}// end namespace

