using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework {
    internal class ItemPrintYMD {
        Item item;
        bool print = true;
        public ItemPrintYMD(Item item, bool print) {
            this.item = item;
            this.item.text = item.text.Replace("時", ":").Replace("分", "");
            this.print = print;
        }
    }
}
