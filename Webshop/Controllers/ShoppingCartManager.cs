using Azure;
using Azure.Core;
using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using System.Linq;

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
            //Retrieves value from "ShoppingCart"
            var shoppingCartCookie = _ca.HttpContext.Request.Cookies["ShoppingCart"];

            //Instatiates a list of ShoppingCartItem
            List<ShoppingCartItem> scItems = new List<ShoppingCartItem>();

            if (shoppingCartCookie != null)
            {
                //Splits the cookieValues by commas as it's structured (id, size, quantity)
                var cookieValues = shoppingCartCookie.Split(',');
                for (int i = 0; i < cookieValues.Length; i += 3)
                {
                    int id = int.Parse(cookieValues[i]);
                    string size = cookieValues[i + 1];
                    int quantity = int.Parse(cookieValues[i + 2]);

                    //Adds the values to a ShoppingCartItem
                    ShoppingCartItem cartItem = new ShoppingCartItem
                    {
                        Id = id,
                        Size = size,
                        Quantity = quantity,
                    };

                    //Adds cartItem to the scItems list
                    scItems.Add(cartItem);
                }
            }

            //Creates a dictionary by the amount of items per size
            var quantityPerSize = scItems.GroupBy(item => item.Id)
            .ToDictionary(
             group => group.Key,
             group => group
            .GroupBy(item => item.Size)
            .ToDictionary(subGroup => subGroup.Key, subGroup => subGroup.Sum(item => item.Quantity))
             );

            //Create a list of items to display
            List<Item> items = new List<Item>();
            foreach (var item in scItems)
            {
                items.Add(itemRepository.GetItem(item.Id));
            }

            //Instantiate the ShoppingCart to be returned
            ShoppingCart cart = new ShoppingCart
            {
                Items = items,
                ItemSize = quantityPerSize
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

        public IActionResult Add(int id, string size, int quantity)
        {
            //Retrieves the value from the cookie "ShoppingCart"
            var existingCookie = Request.Cookies["ShoppingCart"];
            string newCookieValue;

            //If the cookie exists, append new values
            if (existingCookie != null)
            {
                if(quantity == 0)
                {
                    newCookieValue = $"{existingCookie},{id}, 0, 1";
                }
                else
                {
                    newCookieValue = $"{existingCookie},{id}, {size}, {quantity}";
                }       
            }
            //Else create a new cookie
            else
            {
                if(quantity == 0)
                {
                    newCookieValue = $"{id}, 0, 1";
                }
                else
                {
                    newCookieValue = $"{id}, {size}, {quantity}";
                }
            }

            //Cookie options
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            //Adds or updates "ShoppingCart" with the newCookieValue and cookieOptions
            Response.Cookies.Append("ShoppingCart", newCookieValue, cookieOptions);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id, int quantity, string size)
        {
            var existingCookie = Request.Cookies["ShoppingCart"];
            string newCookieValue = null;
            // Create a new list to hold the updated items
            //var updatedItems = new List<string>();
            List<ShoppingCartItem> scItem = new List<ShoppingCartItem>();
            List<ShoppingCartItem> newItems = new List<ShoppingCartItem>();

            if (existingCookie != null)
            {
                // Split the existing cookie value into individual items
                var items = existingCookie.Split(',');
                //List<string> itemIds = new List<string>();
                for (int i = 0; i < items.Length; i += 3)
                {
                    string itemId = int.Parse(items[i]).ToString();
                    string itemSize = items[i + 1];
                    int existingQuantity = int.Parse(items[i + 2]);

                    ShoppingCartItem sci = new ShoppingCartItem
                    {
                        Id = int.Parse(itemId),
                        Size = itemSize,
                        Quantity = existingQuantity
                    };

                    scItem.Add(sci);
                }

                

                // Iterate over the items and add all but the one with the specified Id & the ones with Id but different size
                // Add back the items of specified id according to Quantity
                foreach (var item in scItem)
                {
                    // Returning a list of the Ids NOT deleted back to the index view
                    if (item.Id != id)
                    {
                        newItems.Add(item);
                    }

                    if(item.Id == id && item.Size != size)
                    {
                        newItems.Add(item);
                    }

                    if (item.Id == id && item.Size == size && quantity > 1)
                    {
                        newItems.Add(item);
                        item.Quantity--;
                        quantity--;
                    }
                }

            }

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            foreach (var item in newItems)
            {
                // Combine the updated items into a new cookie value
                newCookieValue += $"{item.Id}, {item.Size}, {item.Quantity},";
                
            }

            // Add new values to cookie
            if(newCookieValue != null) {
                newCookieValue = newCookieValue.Remove(newCookieValue.Length - 1, 1);
                Response.Cookies.Append("ShoppingCart", newCookieValue, cookieOptions); 
            }
            // Remove cookie if there's no values
            else
            {
                Response.Cookies.Delete("ShoppingCart", cookieOptions);
            }

            return RedirectToAction("Index");
        }

    }

}
