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
        public static double _TOTALSIZE = 0, MAXSIZE = 0;
        private CookieContainer ccc;
        private SpWebClient spwc;
        private List<string> hackListURL = new List<string>();
        private List<Data> There_are_many_ways_to_fame = new List<Data>();
        public Thread MyThread;
        private String folder;
        
        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            folder = Environment.CurrentDirectory + "\\BiliDownload\\";
            this.ccc = new CookieContainer();
            this.spwc = new SpWebClient(ccc);
            this.spwc.Headers.Add("User-Agent", "User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.97 Safari/537.36");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            There_are_many_ways_to_fame.Clear();
            hackListURL.Clear();
            //INIT ProcessBar
            progressSmall.Value = 0;
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

            requestURL = null;

            //Decrypt & ADD 
            foreach (string url in hackListURL)
            {
                string ret = spwc.DownloadString(url, Encoding.UTF8);

                Regex regTitle = new Regex(@"(?<=<title[^>]*>)([^<]*)(?=</title>)");
                string title = regTitle.Match(ret).Groups[0].Value;
                FileName.Text = title;
                Regex regSWF = new Regex("com/play.swf\x22, \x22([^\x22]+)\x22");
                Regex regCid = new Regex(@"=[^\x26]+\x26");
                string ct = regSWF.Match(ret).Groups[0].Value;
                String cid = regCid.Match(ct).Groups[0].Value.Trim('\x26').Trim('\x3d');
                ret = spwc.DownloadString(API.decryptHook(cid),Encoding.UTF8);
                ret = API.bilibiliDownloadURL(ret);
                There_are_many_ways_to_fame.Add(new Data(title, ret));
            }

            //Check MAX URL
            MyThread = new System.Threading.Thread(this.DownloadThread);
            MyThread.Start();



        }

        public void DownloadThread()
        {

            for (int i = 0; i < There_are_many_ways_to_fame.Count;i++ )
            {
                DownloadFunc(There_are_many_ways_to_fame[i].getUrl(), folder + There_are_many_ways_to_fame[i].getName() + ".flv");
                Console.WriteLine(There_are_many_ways_to_fame[i].getUrl());
            }

        }



        public void DownloadFunc(String url, String downloadPath)
        {

            _TOTALSIZE = 0;
            progressSmall.Value = 0;
            
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                System.IO.Stream dataStream = httpResponse.GetResponseStream();
                MAXSIZE = (int)httpResponse.ContentLength;
                progressSmall.Maximum = (int)MAXSIZE;
                Console.WriteLine("總大小:" + httpResponse.ContentLength);
          
                byte[] buffer = new byte[8192];

                FileStream fs = new FileStream(downloadPath,
                FileMode.Create, FileAccess.Write);
                int size = 0;
                do
                {
                    size = dataStream.Read(buffer, 0, buffer.Length);
                    _TOTALSIZE += size;
                    progressSmall.Value = (int)_TOTALSIZE;
                    label2.Text = _TOTALSIZE + "/" + httpResponse.ContentLength;
                    if (size > 0)
                        fs.Write(buffer, 0, size);
                } while (size > 0);
                fs.Close();

                httpResponse.Close();
            }
            catch (Exception e)
            {

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://never-nop.com/");
        }




    }
}
