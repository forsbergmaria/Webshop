using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Web.Mvc;
using Models.ViewModels;

namespace DataAccess.Data.Services
{
    public class OrderService
    {
        OrderRepository _orderRepository { get { return new OrderRepository(); } }

        public List<Order> FilterOrdersByStatus(int shippingStatusId)
        {
            var status = _orderRepository.GetShippingStatus(shippingStatusId);
            var orders = _orderRepository.GetAllOrders()
                .Where(o => o.ShippingStatusId == status.StatusId).ToList();

            return orders;
        }

        public List<SelectListItem> PopulateStatusList()
        {
            var allStatuses = _orderRepository.GetAllShippingStatuses();

        List<SelectListItem> statuses = allStatuses.ConvertAll(s =>
        {
            return new SelectListItem()
            {
                Text = s.Name.ToString(),
                Value = s.StatusId.ToString(),
                Selected = false
            };
        });

            return statuses;
        }

        public List<OrderViewModel> ConvertOrdersToOrderViewModel(List<Order> orders)
        {
            List<OrderViewModel> model = new List<OrderViewModel>();

            foreach (Order order in orders)
            {
                model.Add(new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    CustomerName = order.CustomerName,
                    CustomerAddress = order.CustomerAddress,
                    CustomerAddress2 = order.CustomerAddress2,
                    CustomerZipCode = order.CustomerZipCode,
                    CustomerCity = order.CustomerCity,
                    ShippingMethod = _orderRepository.GetShippingMethodName(order.ShippingMethodId),
                    ShippingStatus = _orderRepository.GetShippingStatusName(order.ShippingStatusId)
                });
            }

            return model;
        }
    }
}
