using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveControlHub.Areas.SmartController.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TouchPosition()
        {
            //LiveControlHub.Program.ObjectController.MoveObjectAddPosition(1, 0, 0, "Cube");
            return View();
        }
    }
}
