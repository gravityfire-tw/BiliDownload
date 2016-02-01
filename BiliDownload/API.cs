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

namespace WindowsFormsApplication1
{
    class API
    {
     



        public static string bilibiliDownloadURL(string str)
        {
            str = str.Substring(str.IndexOf("<url><![CDATA[") + "<url><![CDATA[".Length);
            str = str.Substring(0, str.IndexOf("]]></url>"));
            return str;
        }

        public static string decryptHook(string cid)
        {
            String appkey = "85eb6835b0a1034e";//+ "Thanks anonymous Account ^______^+".Substring(0, 0);
            String secretkey = "2ad42749773c441109bdc0191257a664";
            MD5 md5 = MD5.Create();
            byte[] source = Encoding.UTF8.GetBytes("appkey=" + appkey + "&cid=" + cid + secretkey);//將字串轉為Byte[]
            byte[] crypto = md5.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);
            result = "http://interface.bilibili.com/playurl?appkey=" + appkey + "&cid=" + cid + "&sign=" + result;
            return result;
        }
       
    }

}
