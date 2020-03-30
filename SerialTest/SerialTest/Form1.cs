using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialTest
{
    public partial class Form1 : Form
    {
        Thread t;
        bool setup = false;
        int sleep;
        DateTime comeco;
        int mss;
        int best = int.MaxValue;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            SelectPortDialog d = new SelectPortDialog(ports);
            d.ShowDialog();
            serialPort1.PortName = ports[d.index];
            serialPort1.Open();
            d.Dispose();
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        { 
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if(indata == "1")
            {
                StopCounter();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            label5.Visible = false;
            Random r = new Random();
            sleep = r.Next(1, 5000);
            comeco = DateTime.Now;
            timerSleep.Enabled = true;
            setup = true;

        }
 
        
        private void StopCounter()
        {
            timerReac.Enabled = false;
            if (panel1.Visible)
            {
                label4.Invoke((Action)(() => { label4.Text = $"Tempo: {mss} ms"; button1.Visible = true; panel1.Visible = false; }));
                if (mss < best)
                {
                    label2.Invoke((Action)(() => { label2.Text = $"Melhor tempo: {mss} ms"; }));
                    best = mss;
                }    
            }
            else
            {
                label5.Invoke((Action)(() => { label5.Visible = true; timerSleep.Enabled = false; button1.Visible = true; }));
            }
            mss = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          label4.Text = $"Tempo: {(DateTime.Now - comeco).Milliseconds + (DateTime.Now - comeco).Seconds * 1000} ms";
            mss = (DateTime.Now - comeco).Milliseconds + (DateTime.Now - comeco).Seconds * 1000;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void timerSleep_Tick(object sender, EventArgs e)
        {
            
            if (mss > sleep)
            {
                mss = 0;
                panel1.Visible = true;
                timerReac.Enabled = true;
                timerSleep.Enabled = false;
                comeco = DateTime.Now;
            }
            else
            {
                mss = (DateTime.Now - comeco).Milliseconds + (DateTime.Now - comeco).Seconds*1000;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            string[] ports = SerialPort.GetPortNames();
            SelectPortDialog d = new SelectPortDialog(ports);
            d.ShowDialog();
            serialPort1.PortName = ports[d.index];
            serialPort1.Open();
            d.Dispose();
        }
    }
}
