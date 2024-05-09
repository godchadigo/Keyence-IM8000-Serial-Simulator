using Parjet_TcpServer.Model;
using System.Diagnostics;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Parjet_TcpServer
{
    internal class Program
    {
        private static string otMsg = "ST\tB0\r\nSE\t6C0N000127\t12.0.2\tF5\r\nDA\t2024/04/30 15:47:57\t55\r\nMS\t2011575\t17\r\nLO\tA4\r\nSC\t0001\t69\r\nCH\t94\r\nHT\tNECESSARY\t5B\r\nHL\t---\t2D\r\nAD\t1\t\t\t外觀\t27\r\nIT\t1\t6.411\tmm\tA\t6.350\t0.200\t-0.200\tOK\tD7\r\nIT\t2\t54.132\tmm\tD\t54.600\t0.300\t-0.300\tNG\t3C\r\nIT\t3\t67.616\tmm\tB-3\t67.580\t0.070\t-0.070\tOK\tBE\r\nIT\t4\t67.594\tmm\tB-2\t67.580\t0.070\t-0.070\tOK\tC3\r\nIT\t5\t67.588\tmm\tB-1\t67.580\t0.070\t-0.070\tOK\tC6\r\nIT\t6\t0.011\tmm\tC\t0.000\t0.130\t0.000\tOK\t99\r\nEN\t9C\r\n";
        private static string otMsg0 = "ST\tB0\r\nSE\t6C0N000127\t12.0.2\tF5\r\nDA\t2024/04/30 15:47:57\t55\r\nMS\t2011575\t17\r\nLO\tA4\r\nSC\t000ARY\t5B\r\nHL\t---\t2D\r\nAD\t1\t\t\t外觀\t27\r\nIT\t1\t6.411\tmm\tA\t6.350\t0.200\t-0.200\tOK\tD7\r\nIT\t2\t54.132\tmm\tD\t54.600\t0.300\t-0.300\tNG\t3C\r\nIT\t3\t67.616\tmm\tB-3\t67.580\t0.070\t-0.070\tOK\tBE\r\nITOK\tC3\r\nItOK\tC6\r\nIT\t6\t0.011\tmm\tC\t0.000\t0.130\t0.000\tOK\t99\r\nEN\t9C\r\n";
        private static string otMsg1 = "ST\tB0\r\nSE\t6C0N000127\t12.0.2\tF5\r\nDA\t2024/04/30 15:47:57\t55\r\nMS\t2011575\t17\r\nLO\tA4\r\nSC\t0001\t69\r\nCH\t94\r\nHT\tNECESSARY\t5B\r\nHL\t---\t2D\r\nAD\t1\t\t\t外觀\t27\r\nIT\t1\t6.411\tmm\tA\t6.350\t0.200\t-0.200\tOK\tD7\r\nIT\t2\t54.132\tmm\tD\t54.600\t0.300\t-0.300\tNG\t3C\r\nIT\t3\t67.616\tmm\tB-3\t67.580\t0.070\t-0.070\tOK\tBE\r\nIT\t4\t67.594\tmm\tB-2\t67.580\t0.070\t-0.070\tOK\tC3\r\nIT\t5\t67.588\tmm\tB-1\t67.580\t0.070\t-0.070\tOK\tC6\r\nIT\t6\t0.011\tmm\tC\t0.000\t0.130\t0.000\tOK\t99\r\nIT\t7\t0.511\tmm\tD\t0.000\t0.170\t0.000\tNG\t99\r\nEN\t9C";
        private static CustomTimer CT;
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            CT = new CustomTimer(1000 , Timeout.Infinite);
            CT.CallFinish += CT_CallFinish;
            Start();
            Console.ReadLine();
        }

        private static void CT_CallFinish(object? sender, EventArgs e)
        {
            msFlag = false;
        }

        private static bool msFlag = false;
        static void Start()
        {
            var service = new TcpService();
            service.Connecting = (client, e) => { return EasyTask.CompletedTask; };//有客戶端正在連接
            service.Connected = (client, e) => { return EasyTask.CompletedTask; };//有客戶端成功連接
            service.Disconnecting = (client, e) => { return EasyTask.CompletedTask; };//有客戶端正在斷開連接，只有當主動斷開時才有效。
            service.Disconnected = (client, e) => { return EasyTask.CompletedTask; };//有客戶端斷開連接
            service.Received = (client, e) =>
            {
                //從客戶端收到信息
                var mes = Encoding.ASCII.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);//注意：數據長度是byteBlock.Len
                client.Logger.Info($"已從{client.Id}接收到信息：{mes}");

                var sp = mes.Split(' ');
                if (sp[0] == "MS")
                {
                    Console.WriteLine("##################MS##################\r\n");
                    client.Send("CP 0 03");
                    //msFlag = !msFlag;
                }

                if (sp[0] == "EX")
                {
                    //msFlag = !msFlag;
                    CT.Start();
                    msFlag = true;
                }

                if (mes == "OT C3\r\n")
                {
                    Console.WriteLine("##################OT##################\r\n");
                    client.Send(FakeOT().ToString());
                }

                if (mes == "SA B4\r\n")
                {
                    if (msFlag)
                    {
                        client.Send("SA 10 3");
                    }
                    else
                    {
                        client.Send("SA 0 3");
                    }
                   
                }

                return EasyTask.CompletedTask;
            };

            service.Setup(new TouchSocketConfig()//載入配置
                .SetListenIPHosts("tcp://0.0.0.0:5000", 55055)//同時監聽兩個地址
                .ConfigureContainer(a =>//容器的配置順序應該在最前面
                {
                    a.AddConsoleLogger();//添加一個控制台日志注入（注意：在maui中控制台日志不可用）
                })
                .ConfigurePlugins(a =>
                {
                    //a.Add();//此處可以添加插件
                }));

            service.Start();//啟動
        }
        static OTModel FakeOT()
        {
            string dt = "DA\t" + DateTime.Now.ToString("yyyy/MM/dd    HH:mm:ss");
            List<ITModel> itModelList = new List<ITModel>();
            itModelList.Add(new ITModel() {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.000\t",
                上限公差 = "0.005\t",
                下限公差 = "0.005\t",
                判定 = "OK\t",
                總和檢查碼 = "FF\r\n",
            });

            itModelList.Add(new ITModel()
            {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.450\t",
                上限公差 = "0.305\t",
                下限公差 = "0.205\t",
                判定 = "NG\t",
                總和檢查碼 = "FF\r\n",
            });


            itModelList.Add(new ITModel()
            {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.450\t",
                上限公差 = "0.305\t",
                下限公差 = "0.205\t",
                判定 = "OK\t",
                總和檢查碼 = "FF\r\n",
            });
            itModelList.Add(new ITModel()
            {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.450\t",
                上限公差 = "0.305\t",
                下限公差 = "0.205\t",
                判定 = "OK\t",
                總和檢查碼 = "FF\r\n",
            });
            itModelList.Add(new ITModel()
            {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.450\t",
                上限公差 = "0.305\t",
                下限公差 = "0.205\t",
                判定 = "OK\t",
                總和檢查碼 = "FF\r\n",
            });
            itModelList.Add(new ITModel()
            {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.450\t",
                上限公差 = "0.305\t",
                下限公差 = "0.205\t",
                判定 = "OK\t",
                總和檢查碼 = "FF\r\n",
            });
            itModelList.Add(new ITModel()
            {
                命令 = "IT\t",
                序號 = "1\t",
                測量值 = "1.123\t",
                單位 = "mm\t",
                量測項目名稱 = "A\t",
                設計值 = "1.450\t",
                上限公差 = "0.305\t",
                下限公差 = "0.205\t",
                判定 = "OK\t",
                總和檢查碼 = "FF\r\n",
            });


            OTModel model = new OTModel();
            model.DA = dt + "  " + CheckSum(dt.ToCharArray()).ToString("X");
            model.ITList = itModelList;
            return model;
        }

        static int CheckSum(char[] data)
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
    }

    public class CustomTimer
    {
        private Timer timer;
        private int dueTime;
        private int period;
        public event EventHandler CallFinish;

        public CustomTimer(int dueTime, int period)
        {
            this.dueTime = dueTime;
            this.period = period;
        }

        public void Start()
        {
            if (timer == null)
            {
                timer = new Timer(TimerCallback, null, dueTime, period);
            }
            else
            {
                timer.Change(dueTime, period);
            }
        }

        private void TimerCallback(Object o)
        {
            Console.WriteLine($"{DateTime.Now}: 計時器觸發！");
            CallFinish?.Invoke(this , EventArgs.Empty);
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }
    }
}
