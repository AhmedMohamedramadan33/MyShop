using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Entities.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            var res = _unitOfWork.GetRepository<Product>().GetAll(IncludeWord: "Category").Select(x => new GetProduct
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                CategoryName = x.Category.Name
            }).ToList();
            return Json(new { data = res });
        }


        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Product product, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string rootpath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string FileName = Guid.NewGuid().ToString();
                        var Upload = Path.Combine(rootpath, @"img\products");
                        var extenstion = Path.GetExtension(file.FileName);
                        using (var filestream = new FileStream(Path.Combine(Upload, FileName + extenstion), FileMode.Create))
                        {
                            file.CopyTo(filestream);

                        }
                        product.Image = @"\img\products\" + FileName + extenstion;
                    }
                    _unitOfWork.GetRepository<Product>().Insert(product);
                    if (_unitOfWork.SaveChanges() > 0)
                    {
                        return RedirectToAction("GetAllProduct");
                    }
                    ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().GetAll(), "Id", "Name");
                    return View(product);
                }
                ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().GetAll(), "Id", "Name");
                return View(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var res = _unitOfWork.GetRepository<Product>().GetById(id);
            if (res == null) return NotFound();
            ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().GetAll(), "Id", "Name");
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Product product, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string rootpath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string FileName = Guid.NewGuid().ToString();
                        var Upload = Path.Combine(rootpath, @"img\products");
                        var extenstion = Path.GetExtension(file.FileName);
                        if (product.Image != null)
                        {
                            var oldpath = Path.Combine(rootpath, product.Image.TrimStart('\\'));
                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }

                        using (var filestream = new FileStream(Path.Combine(Upload, FileName + extenstion), FileMode.Create))
                        {
                            file.CopyTo(filestream);

                        }
                        product.Image = @"\img\products\" + FileName + extenstion;
                    }
                    _unitOfWork.GetRepository<Product>().Update(product);
                    if (_unitOfWork.SaveChanges() > 0)
                    {
                        return RedirectToAction("GetAllProduct");
                    }
                    ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().GetAll(), "Id", "Name");
                    return View(product);
                }
                ViewBag.Categories = new SelectList(_unitOfWork.GetRepository<Category>().GetAll(), "Id", "Name");

                return View(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var res = _unitOfWork.GetRepository<Product>().GetById(id);
            if (res == null) return Json(new { success = false, message = "Error While Deleting" });
            _unitOfWork.GetRepository<Product>().Delete(res);
            if (_unitOfWork.SaveChanges() > 0)
            {
                return Json(new { success = true, message = "Deleted Successfully" }); ;
            }
            return Json(new { success = false, message = "Error While Deleting" });
        }
    }
}
