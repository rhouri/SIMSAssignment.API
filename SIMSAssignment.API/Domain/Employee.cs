using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSAssignment.API.Domain
    {
    public class Employee
        {
        [Key]
        public int Id
            {
            get; set;
            }
        public string PictureUrl
            {
            get; set;
            }
        public string FirstName
            {
            get; set;
            }
        public string MiddleName
            {
            get; set;
            }
        public string LastName
            {
            get; set;
            }
        public string Department
            {
            get; set;
            }
        public string Email
            {
            get; set;
            }
        public string Phone
            {
            get; set;
            }

        public string DriverLicense //Visible only to Manager, Sub Manager.
            {
            get;set;
            }

        public string SSN
            {
            get; set;
            }               // Visible to the Manager only.
        }
    }
