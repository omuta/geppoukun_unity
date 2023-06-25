using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework {
    internal class ItemPrint {
        public Item item;
        bool print = true;
    //	public ItemPrint(Item item, boolean print){
    //		this.item = item;
    //		if(mode == 1){
    //			this.item.text = this.item.text.replace("時", ":").replace("分", "");
    //		}else if(mode == 2){
    //			this.item.text = this.item.text.replace("年", "/").replace("月", "").replace("日", "");
    //		}else if(mode == 3){
    //			this.item.text = Common.formatDecimal(this.item.text);
    //		}
    //		this.print = print;
    //	}

        public ItemPrint(string str, bool print) {
            this.item = new Item(str);
            this.print = print;
        }

        public ItemPrint(Item item, bool print) {
            this.item = item;
            this.print = print;
        }

        public bool isPrint() {
            return print;
        }
    }
}
