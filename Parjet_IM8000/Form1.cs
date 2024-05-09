using System.Diagnostics;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Parjet_IM8000
{
    public partial class Form1 : Form
    {
        private TcpClient tcpClient = new TcpClient();
        private Result result = new Result();
        private bool latch = false;
        public Form1()
        {
            InitializeComponent();
        }
        public void Connect()
        {

            tcpClient.Connecting = (client, e) => { return EasyTask.CompletedTask; };//即將連接到服務器，此時已經創建socket，但是還未建立tcp
            tcpClient.Connected = (client, e) =>
            {
                this.Invoke(new Action(delegate
                {
                    button1.BackColor = Color.Green;
                    richTextBox2.AppendText("Connected!" + "\r\n");
                }));
                return EasyTask.CompletedTask;
            };//成功連接到服務器
            tcpClient.Disconnecting = (client, e) => { return EasyTask.CompletedTask; };//即將從服務器斷開連接。此處僅主動斷開才有效。
            tcpClient.Disconnected = (client, e) =>
            {
                this.Invoke(new Action(delegate
                {
                    button1.BackColor = Color.White;
                }));

                return EasyTask.CompletedTask;
            };//從服務器斷開連接，當連接不成功時不會觸發。
            tcpClient.Received = (client, e) =>
            {
                //從服務器收到信息。但是一般byteBlock和requestInfo會根據適配器呈現不同的值。
                var mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
                tcpClient.Logger.Info($"客戶端接收到信息：{mes}");
                Debug.WriteLine(mes);
                this.Invoke(new Action(delegate
                {
                    richTextBox2.AppendText(mes + "\r\n");
                }));

                var _sp = mes.Split("\r\n")[0];
                if (_sp == "ST\tB0")
                {
                    //Debug.WriteLine("OT");
                    this.Invoke(new Action(delegate
                    {
                        analyzeOT(mes);
                    }));
                    
                }


                return EasyTask.CompletedTask;
            };

            var ip = ip_tbox.Text;
            var _port = int.TryParse(port_tbox.Text, out int port);

            //載入配置
            tcpClient.Setup(new TouchSocketConfig()
                .SetRemoteIPHost($"{ip}:{port}")
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();//添加一個日志注入
                }));

            result = tcpClient.TryConnect();//調用連接，當連接不成功時，會拋出異常。

            if (result.IsSuccess())
            {
                Console.WriteLine("客戶端連接成功!");
            }
            else
            {
                Console.WriteLine("客戶端連接失敗!");
            }
        }
        public void Disconnect()
        {
            tcpClient.Close();
        }
        public void Tcp_Send(List<byte> data)
        {
            if (result.IsSuccess())
            {
                tcpClient.Send(data.ToArray(), 0, data.ToArray().Length);
            }
        }
        public void Tcp_Send(byte[] data)
        {
            if (result.IsSuccess())
            {
                tcpClient.Send(data, 0, data.ToArray().Length);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (result.IsSuccess())
            {

                Disconnect();
                result = new Result();
            }
            else
            {

                Connect();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var _data = richTextBox1.Text;

            if (radioButton1.Checked)
            {
                var _chk = CheckSum(_data.ToCharArray());
                _data += " " + _chk.ToString("X");
            }
            var data = Encoding.UTF8.GetBytes(_data).ToList();

            data.Add(0x0D);
            data.Add(0x0A);
            Tcp_Send(data);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<byte> data = new List<byte>();
            data.Add(0x53); //S
            data.Add(0x54); //T
            data.Add(0x20); //SPACE
            data.Add(0x43); //C
            data.Add(0x37); //7
            data.Add(0x0D); //CR
            data.Add(0x0A); //LF
            Tcp_Send(data);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var chararray = richTextBox1.Text.ToCharArray();
            var _a = CheckSum(chararray);
            richTextBox2.AppendText("加總Asc:" + _a.ToString("X") + "，加總Dec:"  + _a + "\r\n");
        }
        private int CheckSum(char[] data)
        {
            int acc = 0;
            foreach (var _char in data)
            {
                acc = acc + _char;
            }
            acc = acc + 32;//space
            Debug.WriteLine("完整校驗碼:" + acc);
            var _a = acc % 256;
            return _a;
        }
        private static string otMsg = "ST\tB0\r\nSE\t6C0N000127\t12.0.2\tF5\r\nDA\t2024/04/30 15:47:57\t55\r\nMS\t2011575\t17\r\nLO\tA4\r\nSC\t0001\t69\r\nCH\t94\r\nHT\tNECESSARY\t5B\r\nHL\t---\t2D\r\nAD\t1\t\t\t外觀\t27\r\nIT\t1\t6.411\tmm\tA\t6.350\t0.200\t-0.200\tOK\tD7\r\nIT\t2\t54.132\tmm\tD\t54.600\t0.300\t-0.300\tNG\t3C\r\nIT\t3\t67.616\tmm\tB-3\t67.580\t0.070\t-0.070\tOK\tBE\r\nIT\t4\t67.594\tmm\tB-2\t67.580\t0.070\t-0.070\tOK\tC3\r\nIT\t5\t67.588\tmm\tB-1\t67.580\t0.070\t-0.070\tOK\tC6\r\nIT\t6\t0.011\tmm\tC\t0.000\t0.130\t0.000\tOK\t99\r\nEN\t9C";
        private void button5_Click(object sender, EventArgs e)
        {
            analyzeOT(otMsg);
        }
        private void analyzeOT(string _otMsg)
        {
            dataGridView1.Rows.Clear();
            var rows = _otMsg.Split("\r\n");
            if (rows.Length > 10)
            {
                var itLen = rows.Length - 10 - 2;
                for (int i = 0; i < itLen; i++)
                {
                    Debug.WriteLine(rows[i + 10]);
                    var _sp = rows[i + 10].Split('\t');

                    var 命令 = _sp[0];
                    var 序號 = _sp[1];
                    var 測量值 = _sp[2];
                    var 單位 = _sp[3];
                    var 量測項目名稱 = _sp[4];
                    var 設計值 = _sp[5];
                    var 上限公差 = _sp[6];
                    var 下限公差 = _sp[7];
                    var 判定 = _sp[8];
                    var 總和檢查碼 = _sp[9];
                    dataGridView1.Rows.Add(命令, 序號, 測量值, 單位, 量測項目名稱, 設計值, 上限公差, 下限公差, 判定, 總和檢查碼);
                }
            }
        }
        private void GridInit()
        {            
            dataGridView1.Columns.Add("命令", "命令");
            dataGridView1.Columns.Add("序號", "序號");
            dataGridView1.Columns.Add("測量值", "測量值");
            dataGridView1.Columns.Add("單位", "單位");
            dataGridView1.Columns.Add("量測項目名稱", "量測項目名稱");
            dataGridView1.Columns.Add("設計值", "設計值");
            dataGridView1.Columns.Add("上限公差", "上限公差");
            dataGridView1.Columns.Add("下限公差", "下限公差");
            dataGridView1.Columns.Add("判定", "判定");
            dataGridView1.Columns.Add("總和檢查碼", "總和檢查碼");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GridInit();
        }
    }
}
