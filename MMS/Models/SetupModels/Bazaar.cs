using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MMS.Models.SetupModels
{
    public class Bazaar
    {
        public int BazaarId { get; set; }
        [Required]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime BazaarDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        [NotMapped]
        public List<Member> MemberList { get; set; } 

        public virtual Member Member { get; set; }
    }
}