using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace UdpSocket
{
    public partial class Form1 : Form
    {
        Thread t1 = null;
        Process current_pro = null;
        bool used = true;

        public Form1()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Add("Timeline", 800, HorizontalAlignment.Center);
        }

        private void udpserverStart()
        {
            UdpClient srv = null;
            try
            {
                srv = new UdpClient(5582);

                IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);

                while (used)
                {
                    byte[] dgram = srv.Receive(ref clientEP);
                    listView1.Items.Add(Encoding.Default.GetString(dgram));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                srv.Close();
            }
        }

        private void hookStart()
        {
            ProcessStartInfo proInfo = new ProcessStartInfo();
            proInfo.FileName = "python.exe";
            //proInfo.FileName = @"cmd";
            proInfo.Arguments = String.Format(" {0} {1}", @"C:\Temp\Temp3\scanning_tool_windows\Dragon_Fist\Dragon_Fist\bin\x64\Release\Modules\hook\output.py", "com.Dragonfist.Where");
            proInfo.WorkingDirectory = @"C:\Temp\Temp3\scanning_tool_windows\Dragon_Fist\Dragon_Fist\bin\x64\Release\Modules\hook";
            proInfo.CreateNoWindow = true;
            proInfo.UseShellExecute = false;
            proInfo.RedirectStandardInput = true;
            proInfo.RedirectStandardOutput = true;

            current_pro = new Process();
            current_pro.StartInfo = proInfo;
            current_pro.Start();
            current_pro.Exited += (sender, e) =>
            {
                MessageBox.Show("Hook Process exited!");
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Thread(udpserverStart);
            t1.Start();
            hookStart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            used = false;
            current_pro.Kill();
            t1.Join();
        }
    }
}
