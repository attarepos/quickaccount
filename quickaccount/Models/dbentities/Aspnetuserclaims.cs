using System;
using System.Collections.Generic;

namespace quickaccount.Models.dbentities
{
    public partial class AspNetUserClaims
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }

        public AspNetUsers User { get; set; }
    }
}
