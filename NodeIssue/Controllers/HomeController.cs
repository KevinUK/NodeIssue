using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodeIssue.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;

namespace NodeIssue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            try
            {
                string javascriptModule = @"
 module.exports = (callback, x, y) => {  // Module must export a function that takes a callback as its first parameter
     var result = x + y; // Your javascript logic
     callback(null /* If an error occurred, provide an error object or message */, result); // Call the callback when you're done.
 }";

                // Invoke javascript
                int result = await StaticNodeJSService.InvokeFromStringAsync<int>(javascriptModule, args: new object[] { 3, 5 });
                ViewBag.Result = result;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "serverside node");
                throw;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
