using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopping.Controllers
{

    public class HomeController : Controller
    {
        //Action Methods-->each actio method belong to controller has separate view---.cshtml file(razor file--has presentation logic)
        //Each method will be able to process request
        //Each request is of HTTPRequest because web based app is based on http protocol
        // GET: Home
        public ActionResult Index()
        {//logic for processing http request and passing data
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}