using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Орканоид
{
    public class human : Object
    {
        public string fio;
        public int height;
        public human(string fio, int height)
        {
            this.fio = fio;
            this.height = height;
        }
        public override string ToString()
        {
            return fio + " " + height.ToString();
        }
    }
}
