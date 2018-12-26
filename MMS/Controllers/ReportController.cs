using System;
using System.IO;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MMS.Models;
using MMS.Models.ViewModels;

namespace MMS.Controllers
{
    public class ReportController : Controller
    {
        private MMSContext db= new MMSContext();
        
        // GET: Report
        public ActionResult Index(DateTime? fromDate, DateTime? toDate)
        {           
            Session["FromDate"] = String.Format("{0:dd-MMM-yyyy}", fromDate);
            Session["ToDate"] = String.Format("{0:dd-MMM-yyyy}", toDate);

            var members = db.Bazaars
                .Where(a => a.BazaarDate >= fromDate && a.BazaarDate <= toDate || fromDate == null ||
                            toDate == null ||
                            fromDate.ToString() == "" || toDate.ToString() == "")
                .GroupBy(s => new
                {
                    s.Member.MemberName,
                    s.BazaarDate,
                    Sum = s.Member.Bazaars.Where(a =>
                            a.BazaarDate >= fromDate && a.BazaarDate <= toDate || fromDate == null ||
                            toDate == null ||
                            fromDate.ToString() == "" || toDate.ToString() == "")
                        .Sum(x => (decimal?) x.Amount != null ? x.Amount : 0),
                    TotalMeal = s.Member.TotalMealPerDays
                        .Where(a => a.MealDate >= fromDate && a.MealDate <= toDate || fromDate == null ||
                                    fromDate.ToString() == "" || toDate == null || toDate.ToString() == "")
                        .Sum(y => (int?) y.MealNo != null ? y.MealNo : 0)
                }).Select(x => new ReportModel
                {
                    MemberName = x.Key.MemberName,
                    TotalBazaar = x.Key.Sum,
                    TotalMeals = x.Key.TotalMeal
                }).DistinctBy(b => b.MemberName).ToList();

            //var test = from b in db.Bazaars
            //    join m in db.Members on b.MemberId equals m.MemberId
            //    join t in db.TotalMealPerDays on b.MemberId equals t.MemberId
            //    where (b.BazaarDate >= fromDate && b.BazaarDate <= toDate || fromDate == null || toDate == null ||
            //           fromDate.ToString() == "" || toDate.ToString() == "")
            //    group b by new
            //    {
            //        b.MemberId,
            //        b.BazaarDate,
            //        t.MealDate,
            //        t.MealNo,
            //        m.MemberName
            //    }
            //        into grp select new
            //        {
            //            grp.Key.MemberName,
            //            TotalMeal = grp.Sum(),
            //            TotalBazaar= grp.Sum(s=>s.Amount)
            //        }

            //var members = db.Bazaars.SqlQuery(
            //    "select distinct(b.MemberId),b.BazaarId, b.BazaarDate,m.MemberName," +
            //    " Sum(b.Amount) as Amount, ISNULL(SUM(t.MealNo),0) as MealNo from Bazaars b " +
            //    " inner join TotalMealPerDays t on b.MemberId= t.MemberId " +
            //    " inner join Members m on m.MemberId=b.MemberId " +
            //    " where (b.BazaarDate between @fromDate and @toDate and t.MealDate between @fromDate and @toDate) " +
            //    " group by b.MemberId, b.BazaarDate, m.MemberName, t.MealDate, b.BazaarId ",
            //    new SqlParameter("@fromDate",
            //        String.IsNullOrEmpty(Convert.ToString(fromDate)) ? "" : fromDate),
            //    new SqlParameter("@toDate",
            //        String.IsNullOrEmpty(Convert.ToString(toDate)) ? "" : toDate));
            //var list = members.ToList();


            return View(members);
        }

        public ActionResult ReportPrint(DateTime? fromDate, DateTime? toDate)
        {
            Session["FromDate"] = String.Format("{0:dd-MMM-yyyy}", fromDate);
            Session["ToDate"] = String.Format("{0:dd-MMM-yyyy}", toDate);
            var members = db.Bazaars
                .Where(a => a.BazaarDate >= fromDate && a.BazaarDate <= toDate || fromDate == null || toDate == null ||
                            fromDate.ToString() == "" || toDate.ToString() == "").GroupBy(s => new
                            {
                                s.Member.MemberName,
                                s.BazaarDate,
                                Sum = s.Member.Bazaars.Sum(x => (decimal?)x.Amount != null ? x.Amount : 0),
                                TotalMeal = s.Member.TotalMealPerDays.Sum(y => (int?)y.MealNo != null ? y.MealNo : 0)
                            }).Select(x => new ReportModel
                            {
                                MemberName = x.Key.MemberName,
                                TotalBazaar = x.Key.Sum,
                                TotalMeals = x.Key.TotalMeal
                            }).DistinctBy(b => b.MemberName).ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "ReportCustomer.rpt"));

            rd.SetDataSource(members);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "CustomerList.pdf");
        }
    }
}