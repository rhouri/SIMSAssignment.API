using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SIMSAssignment.API.Domain;

namespace SIMSAssignment.API.Models
    {
    public class SIMSDbContext :DbContext
        {
        public SIMSDbContext ( DbContextOptions<SIMSDbContext> options )
            : base(options)
            {
            }

        public DbSet<SIMSUser> Users
            {
            get; set;
            }

        public DbSet<SIMSRole> Roles
            {
            get; set;
            }
        public DbSet<SIMSClaim> Claims
            {
            get;set;
            }

        public DbSet<Employee> Employees
            {
            get; set;
            }

        }
    }