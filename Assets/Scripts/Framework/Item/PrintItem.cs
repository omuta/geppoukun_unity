using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework
{
    internal class PrintItem
    {
        public Item item;
        bool print = true;
        public PrintItem(string str, bool print)
        {
            item = new Item(str);
            this.print = print;
        }

        public PrintItem(Item item, bool print)
        {
            this.item = item;
            this.print = print;
        }

        public bool isPrint()
        {
            return print;
        }
    }
}
