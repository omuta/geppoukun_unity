using PreGeppou.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TenkenKasyoData {
        private static readonly long serialVersionUID = 1L;
        public Item itemTenkenKasyo;
        public int image;
        public int textColor;
        public int bgColor;
        public TenkenKasyoData(Item itemTenkou, int image, int textColor, int bgColor) {
            this.itemTenkenKasyo = itemTenkou;
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
        }
        public Item getTextHenatukiMei() {
            return itemTenkenKasyo;
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
