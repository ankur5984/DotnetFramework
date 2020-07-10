using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOL;
using IDAL;
using DAL;

namespace OnlineShopping.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            Cart theCart = this.HttpContext.Session["shoppingcart"] as Cart;
            List<Item> theItem = theCart.Items; 
            return View(theItem);
        }
        public ActionResult AddToCart(Object id)
        {
            int productid = int.Parse(id.ToString());
            Item theItem = new Item { ProductId = productid, Quantity = 0 };
            //Cart addtocart = this.HttpContext.Session["shoppingcart"] as Cart;

            return View(theItem);
        }

        [HttpPost]
        public ActionResult AddToCart(Item item)
        {
            Cart theCart = this.HttpContext.Session["shoppingcart"] as Cart;
            theCart.Items.Add(item);
            return RedirectToAction("index", "shoppingcart");
        }
    }
}