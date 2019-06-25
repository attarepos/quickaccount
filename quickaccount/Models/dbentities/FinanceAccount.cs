using System;
using System.Collections.Generic;

namespace quickaccount.Models.dbentities
{
    public partial class FinanceAccount
    {
        public FinanceAccount()
        {
            FinanceTransaction = new HashSet<FinanceTransaction>();
            InverseFkParent = new HashSet<FinanceAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FinanceAccountType { get; set; }
        public int? FkParentId { get; set; }

        public FinanceAccount FkParent { get; set; }
        public ICollection<FinanceTransaction> FinanceTransaction { get; set; }
        public ICollection<FinanceAccount> InverseFkParent { get; set; }
    }
}
