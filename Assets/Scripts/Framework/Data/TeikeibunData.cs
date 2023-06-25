using PreGeppou.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TeikeibunData {
        private static readonly long serialVersionUID = 1L;
        private Item itemTeikeibun;
        private int image;
        private int textColor;
        private int bgColor;
        public TeikeibunData(Item item, int image, int textColor, int bgColor) {
            this.itemTeikeibun = item;
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
        }
        public Item getTeikeibun() {
            if (itemTeikeibun == null) {
                itemTeikeibun = new Item();
            }
            return itemTeikeibun;
        }
        public int getImage() {
            return image;
        }
        public int getTextColor() {
            return textColor;
        }
        public int getBgColor() {
            return bgColor;
        }
    }
}
