using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        //My Name is UM ^______^  Welcome to Fork me XDD!!!
        
        private CookieContainer cc;
        private SpWebClient spwc;
        private List<string> hackListURL = new List<string>();
        private List<Data> There_are_many_ways_to_fame = new List<Data>();
        private Thread downloadThread;
        private String folder;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            folder = Environment.CurrentDirectory + "\\BiliDownload\\";
            this.cc = new CookieContainer();
            this.spwc = new SpWebClient(cc);
            this.spwc.Headers.Add("User-Agent", "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.97 Safari/537.36");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (HackList.Lines.Length > 0)
            {
                //Protect Memory Leak
                GC.Collect();
                downloadThread = new Thread(this.go);
                downloadThread.Start();
                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "Downloading...";
            }
            else
            {
                MessageBox.Show("URL IS NULL");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://never-nop.com/");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Close Thread
            try
            {
                downloadThread.Abort();
            }
            catch (Exception ex) {}
        }

        public void go()
        {
            try
            {
                There_are_many_ways_to_fame.Clear();
                hackListURL.Clear();
                //INIT ProcessBar
                progressSmall.Value = 0;
                progressBarAll.Minimum = 0;
                progressBarAll.Value = 0;
                //Get All URL

                string[] requestURL = (!String.IsNullOrEmpty(HackList.Text.Trim())) ? HackList.Lines : null;

                //Filter Error URL
                foreach (string url in requestURL)
                {
                    if (url.Contains("http"))
                    {
                        hackListURL.Add(url);
                    }
                }
                //Protect Memory Leak
                requestURL = null;
                GC.Collect();

                //Decrypt & ADD 
                foreach (string url in hackListURL)
                {
                    string ret = spwc.DownloadString(url, Encoding.UTF8);
                    Regex regTitle = new Regex(@"(?<=<title[^>]*>)([^<]*)(?=</title>)");
                    string title = regTitle.Match(ret).Groups[0].Value;

                    Regex regSWF = new Regex("com/play.swf\x22, \x22([^\x22]+)\x22");
                    Regex regCid = new Regex(@"=[^\x26]+\x26");
                    string ct = regSWF.Match(ret).Groups[0].Value;
                    String cid = regCid.Match(ct).Groups[0].Value.Trim('\x26').Trim('\x3d');
                    ret = spwc.DownloadString(API.decryptHook(cid), Encoding.UTF8);
                    ret = API.bilibiliDownloadURL(ret);
                    There_are_many_ways_to_fame.Add(new Data(title, ret));
                }
                //Check MAX URL
                progressBarAll.Maximum = There_are_many_ways_to_fame.Count;

                //DownLoad Func
                for (int i = 0; i < There_are_many_ways_to_fame.Count; i++)
                {
                    progressSmall.Value = 0;
                    Now.Text = Convert.ToString("Runing:" + (i + 1) + "/" + There_are_many_ways_to_fame.Count);
                    FileName.Text = There_are_many_ways_to_fame[i].getName();
                    API.DownloadFunc(There_are_many_ways_to_fame[i].getUrl(), folder + There_are_many_ways_to_fame[i].getName() + ".flv", progressSmall, label2);
                    ++progressBarAll.Value;
                    Console.WriteLine(There_are_many_ways_to_fame[i].getUrl());
                }
                MessageBox.Show("FINISH ^________^+");
                //Protect Memory Leak
                GC.Collect();
                button1.Enabled = true;
                button1.Text = "StartDownload";

            }
            catch (Exception ex)
            {
                Console.WriteLine("URL IS NULL");
            }
        }



    }
}
