using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using SIMSAssignment.API.Models;

namespace SIMSAssignment.API.Domain
    {
    public class SIMSUser 
        {
        [Key]
        public string UserID
            {
            get;set;
            }

        public string Password
            {
            get;set;
            }

        public string FirstName
            {
            get; set;
            }
        public string LastName
            {
            get; set;
            }
        public string Roles
            {
            get; set;
            }

        public string Claims
            {
            get; set;
            }
        }
    }
