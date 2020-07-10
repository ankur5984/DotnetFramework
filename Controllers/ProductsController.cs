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
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            IManager mgr = new MySqlDbManager(); 
            List<Product> products = mgr.GetAllProducts();
            this.ViewBag.products = products;
            return View(ViewBag);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            IManager mgr = new MySqlDbManager();
            Product theFoundProduct = mgr.GetProduct(id);
            this.ViewBag.product = theFoundProduct;
            return View();
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            //we have to get the product firstly then we can change something in it
            IManager mgr = new MySqlDbManager();
            Product theFoundProduct = mgr.GetProduct(id);
            return View(theFoundProduct);
        }

        [HttpPost]
        public ActionResult Update(Product theProduct)
        {
            IManager mgr = new MySqlDbManager();
            if (ModelState.IsValid)
            {
                if(mgr.UpdateProduct(theProduct))

                {
                    return RedirectToAction("index", "products");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            IManager mgr = new MySqlDbManager();
            Product thefoundProduct = mgr.GetProduct(id);
            if(mgr.DeleteProduct(thefoundProduct))
            {
                return RedirectToAction("index", "products");
            }
          
            return View();
        }
        [HttpGet]
        public ActionResult Insert()
        {
            
            Product newProduct = new Product();
            return View();
        }
        [HttpPost]
        public ActionResult Insert(Product newProduct)
        {
            IManager mgr = new MySqlDbManager();
            if (ModelState.IsValid)
            {
                if(mgr.InsertProduct(newProduct))
                {
                    return RedirectToAction("index", "products");
                }
            }
            return View();
        }
    }
}