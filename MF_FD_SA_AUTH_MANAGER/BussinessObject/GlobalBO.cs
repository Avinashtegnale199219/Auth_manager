using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.BussinessObject
{
    public static class GlobalBO
    {
        public static string CreatedBy { get; set; }
        public static string CreatedByUName { get; set; }
        public static long SessionID { get; set; }
        public static string AppCode { get; set; }
        public static string CPCode { get; set; }
        public static string CPName { get; set; }
        public static string CreatedIP { get; set; }
        public static string FormCode { get; set; }
        public static string Source { get; set; }
        public static string Version { get; set; }
        public static string CalledURL { get; set; }

    }
}
