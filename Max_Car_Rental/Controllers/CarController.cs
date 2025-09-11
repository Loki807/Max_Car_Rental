using Max_Car_Rental.Data;
using Max_Car_Rental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Max_Car_Rental.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarController(ApplicationDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cars = _dbcontext.Cars.ToList();
                 
            return View(cars);
        }

        [HttpGet]
        

        // GET: Car/Create
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();  
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car car, IFormFile? CarImageFile)
        {
            if (ModelState.IsValid)
            {
                // check for file upload
                if (CarImageFile != null && CarImageFile.Length > 0)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    var uploadPath = Path.Combine(webRootPath, "images", "car");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(CarImageFile.FileName);
                    string filePath = Path.Combine(uploadPath, newFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        CarImageFile.CopyTo(fileStream);
                    }

                    // Save path in DB
                    car.ImageUrl = "/images/car/" + newFileName;
                }
                else if (!string.IsNullOrEmpty(car.ImageUrl))
                {
                    // if user typed/pasted a URL, keep it
                    car.ImageUrl = car.ImageUrl;
                }

                _dbcontext.Cars.Add(car);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_dbcontext.Brands, "Id", "BrandName", car.BrandId);
            return View(car);
        }
        [HttpGet]
        public IActionResult Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var car = _dbcontext.Cars
                .Include(c => c.Brand)
                .FirstOrDefault(c => c.CarId == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var car = _dbcontext.Cars.FirstOrDefault(c => c.CarId == id);
            if (car == null)
                return NotFound();

            ViewData["BrandId"] = new SelectList(_dbcontext.Brands, "Id", "BrandName", car.BrandId);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    var uploadPath = Path.Combine(webRootPath, "images", "car");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string newFileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    string fileName = newFileName + extension;

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                   car.ImageUrl = "/images/car/" + fileName;
                }

                _dbcontext.Cars.Update(car);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_dbcontext.Brands, "Id", "BrandName", car.BrandId);
            return View(car);
        }

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var car = _dbcontext.Cars
                .Include(c => c.Brand)
                .FirstOrDefault(c => c.CarId == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var car = _dbcontext.Cars.FirstOrDefault(c => c.CarId == id);
            if (car == null)
                return NotFound();

            _dbcontext.Cars.Remove(car);
            _dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
