using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceBusSender.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ServiceBusSender.Services;
using ServiceBusShared.Models;

namespace ServiceBusSender.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQueueService _queueService;

        public HomeController(ILogger<HomeController> logger, IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("FirstName,LastName")] PersonModel person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            try
            {
                await _queueService.SendMessageAsync(person, "personqueue");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index");

        }

    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
