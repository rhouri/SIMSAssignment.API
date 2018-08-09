using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace SIMSAssignment.API.Domain
{
    public class SIMSRole 
        {
        [Key]
        public string Id
            {
            get;set;
            }
        }
    }
