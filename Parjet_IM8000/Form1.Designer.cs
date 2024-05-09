namespace Parjet_IM8000
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ip_tbox = new TextBox();
            port_tbox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            richTextBox1 = new RichTextBox();
            button2 = new Button();
            richTextBox2 = new RichTextBox();
            button3 = new Button();
            button4 = new Button();
            radioButton1 = new RadioButton();
            button5 = new Button();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // ip_tbox
            // 
            ip_tbox.Location = new Point(116, 18);
            ip_tbox.Margin = new Padding(5);
            ip_tbox.Name = "ip_tbox";
            ip_tbox.Size = new Size(155, 30);
            ip_tbox.TabIndex = 0;
            ip_tbox.Text = "192.168.0.245";
            // 
            // port_tbox
            // 
            port_tbox.Location = new Point(116, 63);
            port_tbox.Margin = new Padding(5);
            port_tbox.Name = "port_tbox";
            port_tbox.Size = new Size(155, 30);
            port_tbox.TabIndex = 1;
            port_tbox.Text = "55055";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 31);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(30, 23);
            label1.TabIndex = 2;
            label1.Text = "IP:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 75);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(50, 23);
            label2.TabIndex = 2;
            label2.Text = "Port:";
            // 
            // button1
            // 
            button1.Location = new Point(283, 18);
            button1.Margin = new Padding(5);
            button1.Name = "button1";
            button1.Size = new Size(118, 80);
            button1.TabIndex = 3;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(41, 107);
            richTextBox1.Margin = new Padding(5);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(358, 163);
            richTextBox1.TabIndex = 4;
            richTextBox1.Text = "";
            // 
            // button2
            // 
            button2.Location = new Point(283, 282);
            button2.Margin = new Padding(5);
            button2.Name = "button2";
            button2.Size = new Size(118, 80);
            button2.TabIndex = 5;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox2.Location = new Point(41, 371);
            richTextBox2.Margin = new Padding(5);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(2108, 484);
            richTextBox2.TabIndex = 6;
            richTextBox2.Text = "";
            // 
            // button3
            // 
            button3.Location = new Point(41, 282);
            button3.Margin = new Padding(5);
            button3.Name = "button3";
            button3.Size = new Size(118, 80);
            button3.TabIndex = 5;
            button3.Text = "Send";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(203, 282);
            button4.Margin = new Padding(5);
            button4.Name = "button4";
            button4.Size = new Size(57, 80);
            button4.TabIndex = 5;
            button4.Text = "Send";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(407, 108);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(161, 27);
            radioButton1.TabIndex = 7;
            radioButton1.TabStop = true;
            radioButton1.Text = "自動補充校驗碼";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(2092, 13);
            button5.Margin = new Padding(5);
            button5.Name = "button5";
            button5.Size = new Size(57, 80);
            button5.TabIndex = 5;
            button5.Text = "OT解析";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(586, 13);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 32;
            dataGridView1.Size = new Size(1498, 349);
            dataGridView1.TabIndex = 8;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2170, 876);
            Controls.Add(dataGridView1);
            Controls.Add(radioButton1);
            Controls.Add(richTextBox2);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(richTextBox1);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(port_tbox);
            Controls.Add(ip_tbox);
            Margin = new Padding(5);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox ip_tbox;
        private TextBox port_tbox;
        private Label label1;
        private Label label2;
        private Button button1;
        private RichTextBox richTextBox1;
        private Button button2;
        private RichTextBox richTextBox2;
        private Button button3;
        private Button button4;
        private RadioButton radioButton1;
        private Button button5;
        private DataGridView dataGridView1;
    }
}
