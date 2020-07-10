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
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer { Id = 15, Name = "Ankur Raj", Location = "Jharkhand", Age = 26 });
            customers.Add(new Customer { Id = 16, Name = "Ankit Raj", Location = "Bangalore", Age = 29 });
            customers.Add(new Customer { Id = 17, Name = "Ansh Raj", Location = "Jharkhand", Age = 9 });
            customers.Add(new Customer { Id = 18, Name = "Vivan SHarma", Location = "Jharkhand", Age = 3 });

            this.ViewData["customers"] = customers;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer { Id = 15, Name = "Ankur Raj", Location = "Jharkhand", Age = 26 });
            customers.Add(new Customer { Id = 16, Name = "Ankit Raj", Location = "Bangalore", Age = 29 });
            customers.Add(new Customer { Id = 17, Name = "Ansh Raj", Location = "Jharkhand", Age = 9 });
            customers.Add(new Customer { Id = 18, Name = "Vivan SHarma", Location = "Jharkhand", Age = 3 });
            Customer foundCustomer = customers.Find(customer => (customer.Id == id));
            this.ViewData["customer"] = foundCustomer;
            return View();
        }
    }
}