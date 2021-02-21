using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            return View();
        }


        public async Task<IActionResult> PositionXY(string x, string y)
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            var dx = float.Parse(x, nfi);
            var dy = float.Parse(y, nfi);
            LiveControlHub.Program.ObjectController.MoveObjectAddPosition(dx, dy, 0, "Cube");
            return Ok();
        }
    }
}
