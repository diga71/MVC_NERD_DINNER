using NerdDinnerRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerdDinnerRazor.Controllers
{
    public class DinnersController : Controller
    {
        DinnerRepository dinnerRepository = new DinnerRepository();
        //
        // GET: /Dinners/

        public ActionResult Index()
        {
            var dinners = dinnerRepository.FindUpcomingDinners().ToList();
            return View("Index", dinners);
        }

        public ActionResult Details(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            if (dinner == null)
                return View("NotFound");
            else
                return View("Details", dinner);
        }

        public ActionResult Edit(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            return View("Edit", dinner);
        }

        //[HttpPost]
        //public ActionResult Edit(Dinner std)
        //{
        //    //write code to update student 

        //    return RedirectToAction("Index");
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            try
            {
                UpdateModel(dinner);
                dinnerRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
            catch(Exception err)
            {
                String msg = err.ToString();
                foreach (var issue in dinner.GetRuleViolations())
                {
                    ModelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
                }
                return View(dinner);
            }
        }

        public ActionResult Create()
        {
            Dinner dn = new Dinner()
            {
                EventDate = DateTime.Now.AddDays(1)
            };
            return View(dn);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Dinner dinner)
        {
            try
            {
                UpdateModel(dinner);
                dinnerRepository.Add(dinner);
                dinnerRepository.Save();
                return RedirectToAction("Details", new { id = dinner.DinnerID });
            }
            catch (Exception err)
            {
                String msg = err.ToString();
                foreach (var issue in dinner.GetRuleViolations())
                {
                    ModelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
                }
                return View(dinner);
            }
        }

    }
}
