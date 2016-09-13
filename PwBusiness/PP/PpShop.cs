namespace PwBusiness.PP
{
    public class PpShop {
        public string titile { get; set; }
        public string como_num { get; set; }
        public string legend { get; set; }
        public string promotions { get; set; }
        public string adress { get; set; }
        public string mainseller_owner { get; set; }
        public string mainseller_sell { get; set; }


        public string qqnum { get; set; }
        public string sigT { get; set; }    public string sigP { get; set; }


        public string imtakStr {
            set {

                var res = value.Split('\'');
                qqnum = res[1];
                sigT = res[5];
                sigP = res[7];

            }
        }

        public int index { get; set; }

        public bool isonline { get; set; }
    }
}