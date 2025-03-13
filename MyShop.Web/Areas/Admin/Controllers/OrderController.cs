using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using MyShop.Entities.ViewModels;
using Stripe;
using Utilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        public OrderController(IUnitOfWork unitOfWork, IOrderHeaderRepository orderHeaderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderHeaderRepository = orderHeaderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetData()
        {
            var res = _unitOfWork.GetRepository<OrderHeader>().GetAll(IncludeWord: "ApplicationUser");
            return Json(new { data = res });
        }
        public IActionResult Detail(int id)
        {
            var res = new OrderVM()
            {
                OrderHeader = _unitOfWork.GetRepository<OrderHeader>().GetFirstOrDefault(x => x.Id == id, IncludeWord: "ApplicationUser"),
                OrderDetails = _unitOfWork.GetRepository<OrderDetail>().GetAll(x => x.OrderId == id, IncludeWord: "Product").ToList()
            };
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Startproccess(OrderVM orderVM)
        {
            _orderHeaderRepository.UpdateOrderStatus(orderVM.OrderHeader.Id, SD.Processing, null);
            _unitOfWork.SaveChanges();
            TempData["Edit"] = "order start process Successfully";
            return RedirectToAction("Detail", new { id = orderVM.OrderHeader.Id });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Startshipping(OrderVM orderVM)
        {
            var orderHeader = _unitOfWork.GetRepository<OrderHeader>().GetFirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);
            orderHeader.ShippingDate = DateTime.Now;
            orderHeader.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            orderHeader.OrderStatus = SD.Shipping;
            orderHeader.Carrier = orderVM.OrderHeader.Carrier;
            _unitOfWork.GetRepository<OrderHeader>().Update(orderHeader);
            _unitOfWork.SaveChanges();
            TempData["Edit"] = "order has shipped Successfully";
            return RedirectToAction("Detail", new { id = orderVM.OrderHeader.Id });


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult cancelorder(OrderVM orderVM)
        {
            var orderHeader = _unitOfWork.GetRepository<OrderHeader>().GetFirstOrDefault(x => x.Id == orderVM.OrderHeader.Id);
            if (orderHeader.OrderStatus == SD.Approve)
            {
                var optoin = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId

                };
                var service = new RefundService();
                Refund refund = service.Create(optoin);
                _orderHeaderRepository.UpdateOrderStatus(orderHeader.Id, SD.Cancelled, SD.Refund);
                _unitOfWork.SaveChanges();

            }
            else
            {
                _orderHeaderRepository.UpdateOrderStatus(orderHeader.Id, SD.Cancelled, SD.Cancelled);

            }
            _unitOfWork.SaveChanges();
            TempData["Edit"] = "order has cancelled Successfully";
            return RedirectToAction("Detail", new { id = orderVM.OrderHeader.Id });

        }

    }
}
