using System;
using System.Collections.Generic;
using System.Linq;
using GadgetsOnline.Models;
using GadgetsOnline.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GadgetsOnline.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderProcessing _orderProcessing;
        public CheckoutController(IOrderProcessing orderProcessing)
        {
            _orderProcessing = orderProcessing;
        }
        //OrderProcessing orderProcessing;
        private IOrderProcessing GetOrderProcess()
        {
            return _orderProcessing;
        }

        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        // GET: Checkout
        [HttpGet]
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> AddressAndPayment(Order order)
        {
            /* Added by CTA: This updated method might require the parameters to be re-organized */
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            try
            {
                order.Username = "Anonymous";
                order.OrderDate = DateTime.Now;
                bool result = GetOrderProcess().ProcessOrder(order, this.HttpContext);
                return RedirectToAction("Complete", new
                {
                id = order.OrderId
                }

                );
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        public ActionResult Complete(int id)
        {
            return View(id);
        }
    }
}