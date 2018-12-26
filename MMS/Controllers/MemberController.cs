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
    public class MemberController : Controller
    {
        private MMSContext db = new MMSContext();

        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            var data = GetAllMembers();
            return View(data);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            var designationList = db.Members.ToList();
            return designationList;
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Member member = new Member();
            if (id != 0)
            {
                member = db.Members.Find(id);
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Member member)
        {
            string message = "";
            try
            {
                
                if (member.MemberId == 0)
                {
                    member.CreatedBy = 1;
                    member.CreatedDate = DateTime.Now;
                    member.UpdatedBy = 1;
                    member.UpdatedDate = DateTime.Now;
                    db.Members.Add(member);
                    db.SaveChanges();
                    message = "Added Successfully";
                }
                else
                {
                    member.UpdatedBy = 1;
                    member.UpdatedDate = DateTime.Now;
                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "Updated Succesfully";
                }

                return Json(
                    new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllMembers()),
                        message = message
                    }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json(
                    new
                    {
                        success = false,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllMembers()),
                        message = e.Message
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Member member = db.Members.FirstOrDefault(x => x.MemberId == id);
                db.Members.Remove(member);
                db.SaveChanges();

                return Json(
                    new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllMembers()),
                        message = "Deleted Successfully"
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}