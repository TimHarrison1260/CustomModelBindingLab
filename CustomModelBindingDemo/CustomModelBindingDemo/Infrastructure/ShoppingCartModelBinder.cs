using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomModelBindingDemo.Models;

namespace CustomModelBindingDemo.Infrastructure
{
    public class ShoppingCartModelBinder : IModelBinder
    { 
        private const string sessionKey = "cart";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext modelBindingContext)
        {
            ShoppingCart cart = (ShoppingCart)controllerContext.HttpContext.Session[sessionKey];

            if (cart == null)
            {
                cart = new ShoppingCart();
                cart.Items = new List<string>();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }
            return cart;
        }
    }
}