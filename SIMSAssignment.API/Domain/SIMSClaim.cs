using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSAssignment.API.Domain
{
    public class SIMSClaim
    {
        [Key]
        public string Id
            {
            get;set;
            }
    }
}
