using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Training.Context;
using Training.Models;

namespace Training.Controllers
{
    public class EducationController : Controller
    {

        private readonly MyContext myContext;

        public EducationController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public IActionResult Index()
        {
            var educations = myContext.Educations.ToList();
            return View(educations);
        }

        public IActionResult Create()
        {
            List<University> universities = new List<University>();

            universities = (from university in myContext.Universities
                            select university).Where(x => x.IsAvailable == true).ToList();
            universities.Insert(0, new University { Id = 0, Name = "Select" });

            ViewBag.ListofUniversities = universities;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Education education)
        {
            myContext.Add(education);
            var result = myContext.SaveChanges();
            if (result > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(int? Id)
        {

            List<University> universities = new List<University>();

            universities = (from university in myContext.Universities
                            select university).Where(x => x.IsAvailable == true).ToList();
            universities.Insert(0, new University { Id = 0, Name = "Select" });

            ViewBag.ListofUniversities = universities;

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
        public IActionResult Edit(int? Id, Education education)
        {
            if (Id == null)
            {
                return View();
            }
            var get = myContext.Educations.Find(Id);
            if (get != null)
            {
                get.GPA = education.GPA;
                get.University_Id = education.University_Id;
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