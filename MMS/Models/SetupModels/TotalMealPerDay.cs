using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MMS.Models.SetupModels
{
    public class TotalMealPerDay
    {
        public int TotalMealPerDayId { get; set; }
        [Required]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        [Required]
        public int MealNo { get; set; }
        [Required]
        public DateTime MealDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Member Member { get; set; }
        [NotMapped]
        public List<Member> Members { get; set; }
    }
}