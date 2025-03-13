using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Entities.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCardRepository _shoppingCardRepository;
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        public CardController(IUnitOfWork unitOfWork, IShoppingCardRepository shoppingCardRepository, IOrderHeaderRepository orderHeaderRepository)
        {
            _unitOfWork = unitOfWork;
            _shoppingCardRepository = shoppingCardRepository;
            _orderHeaderRepository = orderHeaderRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cardlist = new ShoppingCardViewModel()
            {
                cards = _unitOfWork.GetRepository<ShoppingCardVM>().GetAll(x => x.ApplicationUserId == userId, "Product"),
            };
            foreach (var item in cardlist.cards)
            {
                cardlist.Total += (item.Count * item.Product.Price);
            }
            return View(cardlist);
        }
        public IActionResult Plus(int cardid)
        {
            var shopcard = _shoppingCardRepository.GetFirstOrDefault(x => x.ShoppingCardId == cardid);
            _shoppingCardRepository.increasecount(shopcard, 1);
            if (_unitOfWork.SaveChanges() > 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Minus(int cardid)
        {
            var shopcard = _shoppingCardRepository.GetFirstOrDefault(x => x.ShoppingCardId == cardid);
            if (shopcard.Count <= 1)
            {
                _shoppingCardRepository.Delete(shopcard);
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            else
            {
                _shoppingCardRepository.decreasecount(shopcard, 1);
            }
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Remove(int cardid)
        {
            var shopcard = _shoppingCardRepository.GetFirstOrDefault(x => x.ShoppingCardId == cardid);
            _shoppingCardRepository.Delete(shopcard);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cardlist = new ShoppingCardViewModel()
            {
                cards = _unitOfWork.GetRepository<ShoppingCardVM>().GetAll(x => x.ApplicationUserId == userId, "Product"),
            };
            foreach (var item in cardlist.cards)
            {
                cardlist.Total += (item.Count * item.Product.Price);
            }
            return View(cardlist);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Summary(ShoppingCardViewModel model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // جلب المنتجات من عربة التسوق
            var cartItems = _unitOfWork.GetRepository<ShoppingCardVM>()
                                       .GetAll(x => x.ApplicationUserId == userId, "Product").ToList();

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Summary");
            }

            // إنشاء الطلب الرئيسي
            var orderHeader = new OrderHeader()
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(3), // شحن بعد 3 أيام افتراضيًا
                TotalPrice = cartItems.Sum(x => x.Count * x.Product.Price),
                OrderStatus = "Pending",
                PaymentStatus = "Pending",
                Address = model.Address,  // يمكن تحديثها لاحقًا من الـ View
                phonenumber = model.phonenumber, // يمكن تحديثها لاحقًا من الـ View
            };

            _unitOfWork.GetRepository<OrderHeader>().Insert(orderHeader);
            _unitOfWork.SaveChanges();

            // إضافة تفاصيل الطلب
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = orderHeader.Id,
                    ProductId = item.Product.Id,
                    Count = item.Count,
                    Price = item.Product.Price
                };
                _unitOfWork.GetRepository<OrderDetail>().Insert(orderDetail);
                _unitOfWork.SaveChanges();
            }
            var domain = "https://localhost:7045/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item.Product.Price * 100), // Stripe يأخذ القيم بالسنتات
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count
                }).ToList(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Card/OrderConfirmation?id={orderHeader.Id}",
                CancelUrl = domain + $"Customer/Card/index"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            //orderHeader.PaymentIntentId = session.PaymentIntentId;
            orderHeader.SessionId = session.Id;
            _unitOfWork.SaveChanges();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult OrderConfirmation(int id)
        {
            var orderheader = _unitOfWork.GetRepository<OrderHeader>().GetFirstOrDefault(x => x.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderheader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderheader.PaymentDate = DateTime.Now;
                orderheader.PaymentIntentId = session.PaymentIntentId;

                _orderHeaderRepository.UpdateOrderStatus(id, "Approve", "Approve");
                _unitOfWork.SaveChanges();
            }

            var cartItems = _unitOfWork.GetRepository<ShoppingCardVM>().GetAll(x => x.ApplicationUserId == orderheader.ApplicationUserId).ToList();
            foreach (var item in cartItems)
            {
                _unitOfWork.GetRepository<ShoppingCardVM>().Delete(item);

            }
            _unitOfWork.SaveChanges();
            return View(id);
        }


    }
}
