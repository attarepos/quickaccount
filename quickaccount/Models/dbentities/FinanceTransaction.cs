using System;
using System.Collections.Generic;

namespace quickaccount.Models.dbentities
{
    public partial class FinanceTransaction
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string Name { get; set; }
        public double? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? DateTime { get; set; }
        public int? ChildOf { get; set; }
        public string UserType { get; set; }
        public string FkAspnetusersId { get; set; }
        public string PaymentMethod { get; set; }
        public string ReferenceNumber { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string OtherDetail { get; set; }
        public string OherDetails2 { get; set; }
        public int? UserId { get; set; }
        public int? FkFinanceAccountId { get; set; }

        public AspNetUsers FkAspnetusers { get; set; }
        public FinanceAccount FkFinanceAccount { get; set; }
    }
}
