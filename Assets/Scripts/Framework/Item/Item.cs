using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework {
    internal class Item {
        private static readonly long serialVersionUID = 1L;
        public String text;
        public String color;
        public String style;
        public String bgcolor;
        //
        public Item() {
            this.text = "";
            this.color = "";
            this.style = "";
            this.bgcolor = "";
        }

        public Item(String text) {
            this.text = text;
            this.color = "";
            this.style = "";
            this.bgcolor = "";
        }

        public Item(String text, String color, String style, String bgcolor) {
            this.text = text;
            this.color = color;
            this.style = style;
            this.bgcolor = bgcolor;
        }

        public Item(Item item, String color, String style, String bgcolor) {
            if (item != null)
                this.text = item.text;
            this.color = color;
            this.style = style;
            this.bgcolor = bgcolor;
        }

        public Item clone() {
            return (Item)MemberwiseClone();
            //Item cloneItem = null;
            //try {
            //    cloneItem = (Item)super.clone();
            //} catch (CloneNotSupportedException e) {
            //    // 省略
            //}
            //return cloneItem;
        }
    }
}
