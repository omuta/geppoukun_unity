using PreGeppou.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TenkenKigouData {
        private static long serialVersionUID = 1L;
        public Item itemTenkenKigou;
        public int image;
        public int textColor;
        public int bgColor;
        public TenkenKigouData(Item itemTenkou, int? image, int? textColor, int? bgColor) {
            this.itemTenkenKigou = itemTenkou;
            this.image = (int)image;
            this.textColor = (int)textColor;
            this.bgColor = (int)bgColor;
        }
        public Item getTextHenatukiMei() {
            return itemTenkenKigou;
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
