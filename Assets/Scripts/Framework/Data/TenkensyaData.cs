using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TenkensyaData {
        private static readonly long serialVersionUID = 1L;
        public string textTenkensyamei;
        public string textTenkensyaJigyousyomei;
        public string textTenkensyaSyozokuDantai1;
        public string textTenkensyaSyozokuDantai2;
        public string textTenkensyaTouroku;
        public string textTenkensyaJyusyo;
        public string textTenkensyaTel;
        public string textTenkensyaFax;
        public string textTenkensyaKeitai;
        public string textTenkensyaMail;
        public string textTenkensyaGinkou;
        public int? image;
        public int? textColor;
        public int? bgColor;
        public TenkensyaData(string textTenkensyamei, int? image, int? textColor, int? bgColor) {
            this.textTenkensyamei = textTenkensyamei;
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
        }
        public string getTextHenatukiMei() {
            return textTenkensyamei;
        }
        public int? getImage() {
            return image;
        }
        public int? getTextColor() {
            return textColor;
        }
        public int? getBgColor() {
            return bgColor;
        }
    }
}
