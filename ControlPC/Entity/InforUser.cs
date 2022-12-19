using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPC.Entity
{
    class InforUser
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public string Domain { get; set; }

        public InforUser(string user, string pass, string domain)
        {
            User = user;
            Pass = pass;
            Domain = domain;
        }

        private static InforUser admv = new InforUser("admv458574", "8574&Hly", "cvn.canon.co.jp");
        private static InforUser ad1 = new InforUser("administrator", "D3ckTop$p", "");
        private static InforUser ad2 = new InforUser("administrator", "Abq19&25pc", "");

        public static List<InforUser> AllInfo = new List<InforUser>() { admv, ad1, ad2 };
    }
}
