using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parjet_TcpServer.Model
{
    public class ITModel
    {
        public string 命令 {  get; set; }
        public string 序號{ get; set; }
        public string 測量值 { get; set; }
        public string 單位 { get; set; }
        public string 量測項目名稱 { get; set; }
        public string 設計值 { get; set; }
        public string 上限公差 { get; set; }
        public string 下限公差 { get; set; }
        public string 判定 { get; set; }
        public string 總和檢查碼 { get; set; }

        public override string ToString()
        {
            string str = string.Empty;
            str += 命令;
            str += 序號;
            str += 測量值;
            str += 單位;
            str += 量測項目名稱;
            str += 設計值;
            str += 上限公差;
            str += 下限公差;
            str += 判定;
            str += 總和檢查碼;
            return str;
        }
    }
}
