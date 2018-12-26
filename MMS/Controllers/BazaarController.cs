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
    public class BazaarController : Controller
    {
        private MMSContext db = new MMSContext();

        // GET: Bazaar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            var data = GetAllBazaars();
            return View(data);
        }

        public IEnumerable<Bazaar> GetAllBazaars()
        {
            var designationList = db.Bazaars.Include(a=>a.Member).ToList();
            return designationList;
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Bazaar bazaar = new Bazaar();
            if (id != 0)
            {
                
                bazaar = db.Bazaars.Find(id);
                
            } 
            bazaar.MemberList = db.Members.Where(a=>a.IsActive).ToList();
            return View(bazaar);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Bazaar bazaar)
        {
            string message = "";
            try
            {

                if (bazaar.BazaarId == 0)
                {
                    bazaar.CreatedBy = 1;
                    bazaar.CreatedDate = DateTime.Now;
                    bazaar.UpdatedBy = 1;
                    bazaar.UpdatedDate = DateTime.Now;
                    db.Bazaars.Add(bazaar);
                    db.SaveChanges();
                    message = "Added Successfully";
                }
                else
                {
                    bazaar.UpdatedBy = 1;
                    bazaar.UpdatedDate = DateTime.Now;
                    db.Entry(bazaar).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "Updated Succesfully";
                }
                return Json(
                    new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllBazaars()),
                        message = message
                    }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(
                    new
                    {
                        success = false,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllBazaars()),
                        message = e.Message
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Bazaar bazaar = db.Bazaars.FirstOrDefault(x => x.BazaarId == id);
                db.Bazaars.Remove(bazaar);
                db.SaveChanges();

                return Json(
                    new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllBazaars()),
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