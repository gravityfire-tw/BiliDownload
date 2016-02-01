using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Data
    {
        private string name;
        private string url;

        public Data(string n, string u){setName(n).setUrl(u);}
        public string getName() {return name; }
        public string getUrl(){return url;}
        public Data setName(string n){name = n;return this;}
        public Data setUrl(string u){url = u;return this;}
    }
}
