using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models.Extensions;

namespace Models
{
    public class SessionCart : Cart
    {
        public const string CartSessionKey = "_Cart";

        [JsonIgnore] public ISession Session { get; set; }


        public override void AddItem(Item item, int quantity)
        {
            base.AddItem(item, quantity);
            Session.SetJson(CartSessionKey, this);
        }


        public override void Clear()
        {
            base.Clear();
            Session.Remove(CartSessionKey);
        }


        public static Cart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var cart = session?.GetJson<SessionCart>(CartSessionKey) ?? new SessionCart();

            cart.Session = session;
            return cart;
        }


        public override void RemoveItem(Item item, int quantity)
        {
            base.RemoveItem(item, quantity);
            Session.SetJson(CartSessionKey, this);
        }


        public override void Update()
        {
            Session.SetJson(CartSessionKey, this);
        }
    }
}
