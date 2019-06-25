using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quickaccount.Models;

namespace quickaccount.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Sale")]
        public IActionResult Customer()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Manufacturer()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Rack()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Category()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Vendor()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Product()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ProductEdit(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [Authorize(Roles = "Admin,Sale")]
        public IActionResult Sale()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Product_Batch()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Purchase()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ChartOfAccount()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        public IActionResult AllTransactions()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AllProductTransactions()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        public IActionResult CustomerLedger()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult VendorLedger()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Expences()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AllSale()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AllPurchase()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Sale")]
        public IActionResult SaleReturn()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Sale")]
        public IActionResult PurchaseReturn()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Closing()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Staff()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Sale")]
        public IActionResult AboutSoftware()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Asset()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Income()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Equity()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CombinedEntry()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult NewTheme()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AccountWithBalance()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ProfitDetails()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
