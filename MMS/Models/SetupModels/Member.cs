using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models.SetupModels
{
    public class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual List<Bazaar> Bazaars { get; set; }
        public virtual List<TotalMealPerDay> TotalMealPerDays { get; set; }
    }
}