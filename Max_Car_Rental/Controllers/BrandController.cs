using Max_Car_Rental.Data;
using Max_Car_Rental.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Max_Car_Rental.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {

            _dbcontext = dbContext;
            _webHostEnvironment = webHostEnvironment;


        }
            
            

            [HttpGet]
            public IActionResult Index()
            {
                List<Brand> Brands = _dbcontext.Brands.ToList();
                return View(Brands);
            }
            [HttpGet]
            public IActionResult Create()
            {

                return View();


            }
            [HttpPost]
            
            public IActionResult Create(Brand brand)
            {
                if (ModelState.IsValid)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    if (files.Count > 0)
                    {
                        // Ensure upload folder exists
                        var uploadPath = Path.Combine(webRootPath, "images", "brand");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        // Unique filename
                        string newFileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(files[0].FileName);
                        string fileName = newFileName + extension;

                        // Save file to server
                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        // Save relative path to DB
                        brand.BrandLogo = "/images/brand/" + fileName;
                    }

                    _dbcontext.Brands.Add(brand);
                    _dbcontext.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }

                return View(brand);
            }
            [HttpGet]
            public IActionResult Details(Guid? id)
            {
                if (id == null || id == Guid.Empty)
                {
                    return NotFound();
                }

                var brand = _dbcontext.Brands.FirstOrDefault(b => b.Id == id);

                if (brand == null)
                {
                    return NotFound();
                }

                return View(brand);
            }


        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var brand = _dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.Brands.Update(brand);
                _dbcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }
        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var brand = _dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
                return NotFound();

            return View(brand); // this opens Delete.cshtml
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var brand = _dbcontext.Brands.FirstOrDefault(b => b.Id == id);
            if (brand == null)
                return NotFound();

            _dbcontext.Brands.Remove(brand);
            _dbcontext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}

