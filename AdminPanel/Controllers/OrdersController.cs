using Data;
using DataAccess.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;

namespace AdminPanel.Controllers
{
    public class OrdersController : Controller
    {
        OrderRepository _orderRepository { get { return new OrderRepository(); } }
        OrderService _orderService { get { return new OrderService(); } }

        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult Index()
        {
            List<Order> orders = _orderRepository.GetAllOrders();
            List<OrderViewModel> model = new List<OrderViewModel>();

            foreach (Order order in orders)
            {
                string shippingMethod = _orderRepository.GetShippingMethodName(order.ShippingMethodId);
                string status = _orderRepository.GetShippingStatusName(order.ShippingStatusId);

                model.Add(new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    CustomerName = order.CustomerName,
                    ShippingMethod = shippingMethod,
                    ShippingStatus = status
                });
            }

            ViewBag.Statuses = _orderService.PopulateStatusList();
            ViewBag.UnhandledOrders = _orderRepository.GetNumberOfUnhandledOrders();

            return View(model);
        }

        [HttpGet]
        public IActionResult FilterStatus(int status)
        {
            var orders = new List<Order>();
            List<OrderViewModel> model = new List<OrderViewModel>();

            orders = _orderService.FilterOrdersByStatus(status);

            var filteredOrders = _orderService.ConvertOrdersToOrderViewModel(orders);

            ViewBag.Statuses = _orderService.PopulateStatusList();

            return View("Index", filteredOrders);
        }
    }
}
