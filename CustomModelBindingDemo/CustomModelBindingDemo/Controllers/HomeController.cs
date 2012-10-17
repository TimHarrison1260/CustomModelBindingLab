using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomModelBindingDemo.Models;
using CustomModelBindingDemo.ViewModels;
using CustomModelBindingDemo.Infrastructure;

namespace CustomModelBindingDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();   
        }

        [HttpGet]
        public ActionResult SimpleForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SimpleForm(Member member)
        {
            return View("ShowMember", member);
        }

        [HttpGet]
        public ActionResult SimpleFormVM()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SimpleFormVM(MemberViewModel mvm)
        {
            Member member = mvm.Member;
            return View("ShowMember", member);
        }

        [HttpGet]
        public ActionResult ComplexObjectForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ComplexObjectForm(User user)
        {
            //string str = Request["roles"];
            return View("ShowUser", user);
        }

        [HttpGet]
        public ActionResult DateForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DateForm(Card card)
        {
            return View("ShowCard", card);
        }

        public ActionResult CreateCart()
        {
            ShoppingCart cart = new ShoppingCart { Items = new List<string>{ "Stratocaster", "Telecaster", "Jazz Bass" } };
            Session["cart"] = cart;
            return View();
        }

        public ActionResult CheckOut(ShoppingCart cart)
        {
            //ShoppingCart cart = (ShoppingCart)Session["cart"];
            return View(cart);
        }


        public ActionResult About()
        {
            return View();
        }
    }
}
