using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TenkouData {
        private static readonly long serialVersionUID = 1L;
        public String textTenkou;
        public int image;
        public int textColor;
        public int bgColor;
        public TenkouData(String textTenkou, int image, int textColor, int bgColor) {
            this.textTenkou = textTenkou;
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
        }
        public String getTextHenatukiMei() {
            return textTenkou;
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
