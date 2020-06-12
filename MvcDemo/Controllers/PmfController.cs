using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class PmfController : Controller
    {
        public IActionResult Odjeli()
        {
            return View(new DepartmentsVm(new List<DepartmentVm>()));
        }
    }
}