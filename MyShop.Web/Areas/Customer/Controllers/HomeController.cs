using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using System.Security.Claims;
using Utilities;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        public static ShoppingCardVM data = null;
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork, IShoppingCardRepository shoppingCardRepository)
        {
            _unitOfWork = unitOfWork;
            _shoppingCardRepository = shoppingCardRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var res = _unitOfWork.GetRepository<Product>().GetAll();
            return View(res);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }

            var res = new ShoppingCardVM()
            {
                ProductId = id,
                Product = _unitOfWork.GetRepository<Product>().GetFirstOrDefault(x => x.Id == id, "Category"),
                Count = 1
            };
            data = res;
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCardVM shoppingCardVM)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                shoppingCardVM.ApplicationUserId = userId;
                ShoppingCardVM currentcard = _unitOfWork.GetRepository<ShoppingCardVM>().GetFirstOrDefault(x => x.ProductId == shoppingCardVM.ProductId && x.ApplicationUserId == userId);
                if (currentcard == null)
                {
                    _unitOfWork.GetRepository<ShoppingCardVM>().Insert(shoppingCardVM);
                    _unitOfWork.SaveChanges();
                    HttpContext.Session.SetInt32(SD.sessionkey,
                        _unitOfWork.GetRepository<ShoppingCardVM>().GetAll(x => x.ApplicationUserId == userId).ToList().Count());
                    return RedirectToAction("index");
                }
                else
                {
                    _shoppingCardRepository.increasecount(currentcard, shoppingCardVM.Count);
                    _unitOfWork.SaveChanges();
                    return RedirectToAction("index");

                }


                return View(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
