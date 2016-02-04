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
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
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

    class API
    {

        private static double _TOTALSIZE = 0, MAXSIZE = 0;

        public static string bilibiliDownloadURL(string str)
        {
            str = str.Substring(str.IndexOf("<url><![CDATA[") + "<url><![CDATA[".Length);
            str = str.Substring(0, str.IndexOf("]]></url>"));
            return str;
        }

        public static string decryptHook(string cid)
        {
            String appkey = "85eb6835b0a1034e"+"Thanks anonymous Account ^______^+".Substring(0, 0);
            String secretkey = "2ad42749773c441109bdc0191257a664";
            MD5 md5 = MD5.Create();
            byte[] source = Encoding.UTF8.GetBytes("appkey=" + appkey + "&cid=" + cid + secretkey);//將字串轉為Byte[]
            byte[] crypto = md5.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);
            result = "http://interface.bilibili.com/playurl?appkey=" + appkey + "&cid=" + cid + "&sign=" + result;
            return result;
        }

        public static void DownloadFunc(String url, String downloadPath,ProgressBar processbarSmall,Label labelSmall)
        {

            _TOTALSIZE = 0;

            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                System.IO.Stream dataStream = httpResponse.GetResponseStream();
                MAXSIZE = (int)httpResponse.ContentLength;
                Form1 obj = new Form1();
                processbarSmall.Maximum = (int)MAXSIZE;
                Console.WriteLine("總大小:" + httpResponse.ContentLength);

                byte[] buffer = new byte[8192];

                FileStream fs = new FileStream(downloadPath,
                FileMode.Create, FileAccess.Write);
                int size = 0;
                do
                {
                    size = dataStream.Read(buffer, 0, buffer.Length);
                    _TOTALSIZE += size;
                    processbarSmall.Value = (int)_TOTALSIZE;
                    labelSmall.Text = _TOTALSIZE + "/" + httpResponse.ContentLength;
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
    }

}
