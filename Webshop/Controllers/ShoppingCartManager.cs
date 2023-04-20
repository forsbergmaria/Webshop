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

        public List<Item> GetCartItems()
        {
            var httpContext = _ca.HttpContext;

            var shoppingCart = httpContext.Session.GetObjectFromJson<List<int>>("ShoppingCart") ?? new List<int>();

            var existingItems = itemRepository.GetAllItems().Where(id => shoppingCart.Contains(id.ItemId)).ToList();

            return existingItems;
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

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            await _cm.AddToCart(id);

            var existingCookie = Request.Cookies["ShoppingCart"];

            if(existingCookie != null)
            {
                CookieOptions opt = new CookieOptions();
                opt.Expires = DateTime.Now.AddDays(3);
                Response.Cookies.Append("ShoppingCart", existingCookie, opt);
            }

            return RedirectToAction("Index");
        }

    }

}
