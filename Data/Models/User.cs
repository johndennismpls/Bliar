using System;
using System.Collections.Generic;
using System.Text;

namespace Bliar.Data.Models
{ 
    public class User
    {
        public Guid UserId { get; set; }
        public string UserIdentityName { get; set; }
        public string DisplayName { get; set; }
    }
}
