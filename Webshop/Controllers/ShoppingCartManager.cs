using Azure;
using Azure.Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Newtonsoft.Json;

namespace Webshop.Controllers
{
    //SHOPPING CART MANAGER -----------------------------------------------------------------
    public class ShoppingCartManager
    {
        ItemRepository itemRepository { get { return new ItemRepository(); } }

        private readonly IHttpContextAccessor _ca;

        public ShoppingCartManager(IHttpContextAccessor ca)
        {
            _ca = ca;
        }

        public ShoppingCart GetCartItems()
        {
            var shoppingCartCookie = _ca.HttpContext.Request.Cookies["ShoppingCart"];
            var itemIds = shoppingCartCookie?.Split(',').Select(int.Parse) ?? new List<int>();
            List<Item> cartItems = new List<Item>();

            //Gets list of Items from itemIds
            foreach(var item in itemIds)
            {
                var specificItem = itemRepository.GetItem(item);
                cartItems.Add(specificItem);
            }

            //Dictionary to contain a key-value pair of the quantity of a specific type of item (itemId, quantity)
            Dictionary<int,int> itemQuantity = cartItems.GroupBy(x => x.ItemId).ToDictionary(x => x.Key, x => x.Count());

            // calculate the total sum of all order amounts
            decimal totalPrice = cartItems.Sum(o => o.PriceWithoutVAT);

            //Instantiate the ShoppingCart to be returned
            ShoppingCart cart = new ShoppingCart
            {
                Items = cartItems,
                Quantity = cartItems.Count(),
                ItemQuantity = itemQuantity,
                Total = totalPrice
            };

           
            return cart;
        }

        public async Task AddToCart(int id)
        {

            var httpContext = _ca.HttpContext;

            var shoppingCart = httpContext.Session.GetObjectFromJson<List<int>>("ShoppingCart") ?? new List<int>();

            if (!shoppingCart.Contains(id))
            {
                shoppingCart.Add(id);
                _ca.HttpContext.Session.SetObjectAsJson("ShoppingCart", shoppingCart);
            }

            await _ca.HttpContext.Session.CommitAsync();
        }
    }

    //JSON--------------------------------------------------------------------------------------
    public static class SessionExtensions
    {

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    //CONTROLLER------------------------------------------------------------------------------------
    public class ShoppingCartController : Controller

    {
        private readonly ShoppingCartManager _cm;

        public ShoppingCartController(ShoppingCartManager cm)
        {
            _cm = cm;
        }

        public IActionResult Index()
        {
            var cartItems = _cm.GetCartItems();
            return View(cartItems);
        }

        public IActionResult Add(int id)
        {
            var existingCookie = Request.Cookies["ShoppingCart"];
            string newCookieValue;

            if (existingCookie != null)
            {
                newCookieValue = $"{existingCookie},{id}";
            }
            else
            {
                newCookieValue = $"{id}";
            }

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            Response.Cookies.Append("ShoppingCart", newCookieValue, cookieOptions);

            return RedirectToAction("Index");
        }

    }

}
