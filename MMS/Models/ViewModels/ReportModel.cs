using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models.ViewModels
{
    public class ReportModel
    {
        public int MemberId { get; set; }
        public string MemberName  { get; set; }
        public int? TotalMeals { get; set; }
        public decimal? TotalBazaar { get; set; }
        
        
    }
}