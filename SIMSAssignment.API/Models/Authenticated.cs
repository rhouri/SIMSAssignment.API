using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSAssignment.API.Models
{
        public class AuthenticatedModel
            {
            public string Token
                {
                get; set;
                }
            public DateTime Expiration
                {
                get; set;
                }
            public string RefreshToken
                {
                get; set;
                }
            public DateTime RefreshTokenExpiration
                {
                get; set;
                }
            }


}
