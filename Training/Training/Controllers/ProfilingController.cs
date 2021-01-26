using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Training.Context;
using Training.Models;

namespace Training.Controllers
{
    public class ProfilingController : Controller
    {
        private readonly MyContext myContext;

        public ProfilingController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public IActionResult Index()
        {
            var profiling = myContext.Profilings.ToList();
            return View();
        }
        public IActionResult Create()
        {
            List<Education> educations = new List<Education>();

            educations = (from education in myContext.Educations
                          select education).ToList();
            educations.Insert(0, new Education { Id = 0, Degree = "Select" });

            ViewBag.ListofEducations = educations;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Profiling profiling, Account account)
        {
            myContext.Add(profiling);
            myContext.Add(account);
            var result = myContext.SaveChanges();
            if (result > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(int? Id)
        {

            List<Education> educations = new List<Education>();

            educations = (from education in myContext.Educations
                          select education).ToList();
            educations.Insert(0, new Education { Id = 0, Degree = "Select" });

            ViewBag.ListofEducations = educations;

            if (Id == null)
            {
                return View();
            }
            var result = myContext.Educations.Find(Id);
            if (result == null)
            {
                return View();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(int? Id, Profiling profiling)
        {
            if (Id == null)
            {
                return View();
            }
            var get = myContext.Profilings.Find(Id);
            if (get != null)
            {
                get.Education_Id = profiling.Education_Id;
                myContext.Entry(get).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            return View();
        }

        public IActionResult Delete(int Id)
        {
            var get = myContext.Educations.Find(Id);
            if (get != null)
            {
                myContext.Educations.Remove(get);
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    return Json(result);
                }
            }
            return Json(0);
        }
    }
}
