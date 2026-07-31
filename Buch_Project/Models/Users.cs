
using System;
using System.Collections.Generic;
using System.Text;

namespace Buch_Project.Models
{
    internal class Users
    {
        public int id { get; set; }
        protected string login { get; set; }= string.Empty;
        protected string password { get; set; }= string.Empty;
        protected string email { get; set; }=string.Empty;

        protected string phone { get; set; }= string.Empty;

        public Users() { }

    }
}
