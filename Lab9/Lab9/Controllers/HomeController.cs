using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Util;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Lab9.Models;
using Lab9.RabbitMQ;

namespace Lab9.Controllers
{
    public class HomeController : Controller
    {
        Sender sender = new Sender();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Tariff tariff)
        {
            sender.Send(tariff);

            return View("Index");
        }
    }
}