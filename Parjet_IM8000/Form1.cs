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

            tcpClient.Connecting = (client, e) => { return EasyTask.CompletedTask; };//�Y�N�s����A�Ⱦ��A���ɤw�g�Ы�socket�A���O�٥��إ�tcp
            tcpClient.Connected = (client, e) =>
            {
                this.Invoke(new Action(delegate
                {
                    button1.BackColor = Color.Green;
                    richTextBox2.AppendText("Connected!" + "\r\n");
                }));
                return EasyTask.CompletedTask;
            };//���\�s����A�Ⱦ�
            tcpClient.Disconnecting = (client, e) => { return EasyTask.CompletedTask; };//�Y�N�q�A�Ⱦ��_�}�s���C���B�ȥD���_�}�~���ġC
            tcpClient.Disconnected = (client, e) =>
            {
                this.Invoke(new Action(delegate
                {
                    button1.BackColor = Color.White;
                }));

                return EasyTask.CompletedTask;
            };//�q�A�Ⱦ��_�}�s���A��s�������\�ɤ��|Ĳ�o�C
            tcpClient.Received = (client, e) =>
            {
                //�q�A�Ⱦ�����H���C���O�@��byteBlock�MrequestInfo�|�ھھA�t���e�{���P���ȡC
                var mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
                tcpClient.Logger.Info($"�Ȥ�ݱ�����H���G{mes}");
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

            //���J�t�m
            tcpClient.Setup(new TouchSocketConfig()
                .SetRemoteIPHost($"{ip}:{port}")
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();//�K�[�@�Ӥ�Ӫ`�J
                }));

            result = tcpClient.TryConnect();//�եγs���A��s�������\�ɡA�|�ߥX���`�C

            if (result.IsSuccess())
            {
                Console.WriteLine("�Ȥ�ݳs�����\!");
            }
            else
            {
                Console.WriteLine("�Ȥ�ݳs������!");
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
            richTextBox2.AppendText("�[�`Asc:" + _a.ToString("X") + "�A�[�`Dec:"  + _a + "\r\n");
        }
        private int CheckSum(char[] data)
        {
            int acc = 0;
            foreach (var _char in data)
            {
                acc = acc + _char;
            }
            acc = acc + 32;//space
            Debug.WriteLine("�������X:" + acc);
            var _a = acc % 256;
            return _a;
        }
        private static string otMsg = "ST\tB0\r\nSE\t6C0N000127\t12.0.2\tF5\r\nDA\t2024/04/30 15:47:57\t55\r\nMS\t2011575\t17\r\nLO\tA4\r\nSC\t0001\t69\r\nCH\t94\r\nHT\tNECESSARY\t5B\r\nHL\t---\t2D\r\nAD\t1\t\t\t�~�[\t27\r\nIT\t1\t6.411\tmm\tA\t6.350\t0.200\t-0.200\tOK\tD7\r\nIT\t2\t54.132\tmm\tD\t54.600\t0.300\t-0.300\tNG\t3C\r\nIT\t3\t67.616\tmm\tB-3\t67.580\t0.070\t-0.070\tOK\tBE\r\nIT\t4\t67.594\tmm\tB-2\t67.580\t0.070\t-0.070\tOK\tC3\r\nIT\t5\t67.588\tmm\tB-1\t67.580\t0.070\t-0.070\tOK\tC6\r\nIT\t6\t0.011\tmm\tC\t0.000\t0.130\t0.000\tOK\t99\r\nEN\t9C";
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

                    var �R�O = _sp[0];
                    var �Ǹ� = _sp[1];
                    var ���q�� = _sp[2];
                    var ��� = _sp[3];
                    var �q�����ئW�� = _sp[4];
                    var �]�p�� = _sp[5];
                    var �W�����t = _sp[6];
                    var �U�����t = _sp[7];
                    var �P�w = _sp[8];
                    var �`�M�ˬd�X = _sp[9];
                    dataGridView1.Rows.Add(�R�O, �Ǹ�, ���q��, ���, �q�����ئW��, �]�p��, �W�����t, �U�����t, �P�w, �`�M�ˬd�X);
                }
            }
        }
        private void GridInit()
        {            
            dataGridView1.Columns.Add("�R�O", "�R�O");
            dataGridView1.Columns.Add("�Ǹ�", "�Ǹ�");
            dataGridView1.Columns.Add("���q��", "���q��");
            dataGridView1.Columns.Add("���", "���");
            dataGridView1.Columns.Add("�q�����ئW��", "�q�����ئW��");
            dataGridView1.Columns.Add("�]�p��", "�]�p��");
            dataGridView1.Columns.Add("�W�����t", "�W�����t");
            dataGridView1.Columns.Add("�U�����t", "�U�����t");
            dataGridView1.Columns.Add("�P�w", "�P�w");
            dataGridView1.Columns.Add("�`�M�ˬd�X", "�`�M�ˬd�X");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GridInit();
        }
    }
}
