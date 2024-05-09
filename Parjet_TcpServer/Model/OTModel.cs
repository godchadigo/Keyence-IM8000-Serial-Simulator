using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parjet_TcpServer.Model
{
    public class OTModel
    {
        public string ST { get; set; } = "ST\tB0";
        public string SE { get; set; } = "SE\t6C0N000127\t12.0.2\tF5";
        public string DA { get; set; }
        public string MS { get; set; } = "MS\t2011575\t17";
        public string LO { get; set; } = "LO\tA4";
        public string SC { get; set; } = "SC\t0001\t69";
        public string CH { get; set; } = "CH\t94";
        public string HT { get; set; } = "HT\tNECESSARY\t5B";
        public string HL { get; set; } = "HL\t---\t2D";
        public string AD { get; set; } = "AD\t1\t\t\t外觀\t27";
        public List<ITModel> ITList { get; set; }
        public string EN { get; set; } = "EN\t9C";

        public override string ToString()
        {
            int cnt = 1;
            string str = string.Empty;
            str += ST + "\r\n";
            str += SE + "\r\n";
            str += DA + "\r\n";
            str += MS + "\r\n";
            str += LO + "\r\n";
            str += SC + "\r\n";
            str += CH + "\r\n";
            str += HT + "\r\n";
            str += HL + "\r\n";
            str += AD + "\r\n";
            if (ITList != null)
            {
                foreach (var item in ITList)
                {
                    item.序號 = cnt++ + "\t";
                    str += item.ToString();
                }
            }            
            str += EN + "\r\n";

            return str;
        }
    }
}
