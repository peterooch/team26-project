using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class LoginError
    {
        public string username { get; set; }
        public bool username_error { get; set; }
        public bool password_error { get; set; }
    }
}
