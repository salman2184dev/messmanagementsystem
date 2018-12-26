using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMS.Models;
using MMS.Models.SetupModels;

namespace MMS.Controllers
{
    public class TotalMealPerDayController : Controller
    {
        private MMSContext db = new MMSContext();

        // GET: TotalMealPerDay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            var data = GetAllTotalMealPerDays();
            return View(data);
        }

        public IEnumerable<TotalMealPerDay> GetAllTotalMealPerDays()
        {
            var designationList = db.TotalMealPerDays.Include(a=>a.Member).ToList();
            return designationList;
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            TotalMealPerDay totalMealPerDay = new TotalMealPerDay();
            if (id != 0)
            {
                totalMealPerDay = db.TotalMealPerDays.Find(id);
            }

            totalMealPerDay.Members = db.Members.Where(a=>a.IsActive).ToList();
            return View(totalMealPerDay);
        }

        [HttpPost]
        public ActionResult AddOrEdit(TotalMealPerDay totalMealPerDay)
        {
            string message = "";
            try
            {

                if (totalMealPerDay.TotalMealPerDayId == 0)
                {
                    totalMealPerDay.CreatedBy = 1;
                    totalMealPerDay.CreatedDate = DateTime.Now;
                    totalMealPerDay.UpdatedBy = 1;
                    totalMealPerDay.UpdatedDate = DateTime.Now;
                    db.TotalMealPerDays.Add(totalMealPerDay);
                    db.SaveChanges();
                    message = "Added Successfully";
                }
                else
                {
                    totalMealPerDay.UpdatedBy = 1;
                    totalMealPerDay.UpdatedDate = DateTime.Now;
                    db.Entry(totalMealPerDay).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "Updated Succesfully";
                }

                return Json(
                    new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllTotalMealPerDays()),
                        message = message
                    }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(
                    new
                    {
                        success = false,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllTotalMealPerDays()),
                        message = e.Message
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                TotalMealPerDay totalMealPerDay = db.TotalMealPerDays.FirstOrDefault(x => x.TotalMealPerDayId == id);
                db.TotalMealPerDays.Remove(totalMealPerDay);
                db.SaveChanges();

                return Json(
                    new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllTotalMealPerDays()),
                        message = "Deleted Successfully"
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}