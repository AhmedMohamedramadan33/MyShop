using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult GetCategory()
        {
            try
            {
                var res = _unitOfWork.GetRepository<Category>().GetAll();
                return View(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.GetRepository<Category>().Insert(category);
                    if (_unitOfWork.SaveChanges() > 0)
                    {
                        TempData["Create"] = "Item Created Successfully";
                        return RedirectToAction("GetCategory");
                    };
                    return View(category);
                }
                return View(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [ActionName("DeleteCategory")]
        public IActionResult Confirmdelete(int id)
        {
            try
            {
                var res = _unitOfWork.GetRepository<Category>().GetById(id);
                if (res == null) return NotFound();
                return View(res);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var res = _unitOfWork.GetRepository<Category>().GetById(id);
                if (res == null) return NotFound();
                _unitOfWork.GetRepository<Category>().Delete(res);
                if (_unitOfWork.SaveChanges() > 0)
                {
                    TempData["Delete"] = "Item Deleted Successfully";

                    return RedirectToAction("GetCategory");
                }
                return View(res);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            try
            {
                var res = _unitOfWork.GetRepository<Category>().GetById(id);
                if (res == null) return NotFound();
                return View(res);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult EditCategory(int id, Category category)
        {
            try
            {
                var res = _unitOfWork.GetRepository<Category>().GetById(id);
                if (res == null) return NotFound();
                if (res == null) return NotFound();
                res.Name = category.Name;
                res.Description = category.Description;
                _unitOfWork.GetRepository<Category>().Update(res);
                if (_unitOfWork.SaveChanges() > 0)
                {
                    TempData["Edit"] = "Item Updated Successfully";

                    return RedirectToAction("GetCategory");
                }
                return View(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
