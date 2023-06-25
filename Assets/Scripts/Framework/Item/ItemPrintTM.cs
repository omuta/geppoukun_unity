using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework {
    internal class ItemPrintTM {
        Item item;
        bool print = false;
        public ItemPrintTM(Item item, bool print) {
            try {
                this.item = item.clone();
                this.item.text = item.text.Replace("時", ":").Replace("分", "");
                if (this.item.text.IndexOf(":") != -1) {
                    string[] HM = this.item.text.Split(":");
                    this.item.text = string.Format("%d:%02d", int.Parse(HM[0]), int.Parse(HM[1]));
                }
                this.print = print;
            } catch (Exception e) {
                //e.printStackTrace();
            }
        }

        public string getTime() {
            return this.item.text;
        }
    }
}
