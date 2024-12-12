using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER_BO
{
    public class AuthManagerBO
    {
        public string CP_Code { get; set; }
        public string AppCode { get; set; }
        //public string User_Name { get; set; }
        //public string User_Pwd { get; set; }

        [Required(ErrorMessage = "UsernameRequired")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [MaxLength(100)]
        public string Password { get; set; }

        public string Active { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string API_Name { get; set; }
        public string Start_DateTime { get; set; }
        public string GUID { get; set; }
        public string Auth_Key { get; set; }
    }
}
