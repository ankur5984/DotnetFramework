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
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            IManager mgr = new MySqlDbManager();
            if (mgr.AuthenticateUser(username,password))
            {
                return RedirectToAction("index", "products");

            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            User newUser = new User();
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            IManager mgr = new MySqlDbManager();
            if (ModelState.IsValid)
            {
                if(mgr.RegisterUser(model))
                {
                    return RedirectToAction("login", "accounts");
                }
                
            }
            return View();
            
        }
    }
}