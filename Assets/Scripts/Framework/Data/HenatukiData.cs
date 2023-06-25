using PreGeppou.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class HenatukiData {
        private static readonly long serialVersionUID = 1L;
        public Item itemHenatukiMei = new Item();
        public Item itemHenatukiSou = new Item();
        public Item itemHenatukikVA = new Item();
        public Item itemHenatukikVA_Tani = new Item("kVA");
        public Item itemHenatukiDenatu1 = new Item();
        public Item itemHenatukiDenatu = new Item();
        public Item itemHenatukiDenatuRS = new Item();
        public Item itemHenatukiDenatuST = new Item();
        public Item itemHenatukiDenatuTR = new Item();
        public Item itemHenatukiDenryuR = new Item();
        public Item itemHenatukiDenryuS = new Item();
        public Item itemHenatukiDenryuT = new Item();
        public Item itemHenatukiDenryuS_Tani = new Item("S");
        public Item itemHenatukiDenatu_Tani = new Item("/");
        public Item itemHenatukiOndo = new Item();
        public Item itemHenatukiIg1 = new Item();
        public Item itemHenatukiIg = new Item();
        public Item itemHenatukiTeikaku = new Item();
        public bool boolIka = false;
        public int? image;
        public int? textColor;
        public int? bgColor;
        public HenatukiData(Item itemHenatukiMei, int? image, int? textColor, int? bgColor) {
            this.itemHenatukiMei = itemHenatukiMei;
            this.itemHenatukiSou = new Item();
            this.itemHenatukikVA = new Item();
            this.itemHenatukikVA_Tani = new Item();
            this.itemHenatukiDenatu1 = new Item();
            this.itemHenatukiDenatu = new Item();
            this.itemHenatukiDenatuRS = new Item();
            this.itemHenatukiDenatuST = new Item();
            this.itemHenatukiDenatuTR = new Item();
            this.itemHenatukiDenryuR = new Item();
            this.itemHenatukiDenryuS = new Item();
            this.itemHenatukiDenryuT = new Item();
            this.itemHenatukiDenryuS_Tani = new Item("S");
            this.itemHenatukiDenatu_Tani = new Item("/");
            this.itemHenatukiOndo = new Item();
            this.itemHenatukiIg1 = new Item();
            this.itemHenatukiIg = new Item();
            this.itemHenatukiTeikaku = new Item();
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
        }
        public Item getTextHenatukiMei() {
            if (itemHenatukiMei == null) {
                itemHenatukiMei = new Item("");
            }
            return itemHenatukiMei;
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
        public static void nullCheck(HenatukiData henatukiData) {
            if (henatukiData.itemHenatukiMei == null)
                henatukiData.itemHenatukiMei = new Item();
            if (henatukiData.itemHenatukiSou == null)
                henatukiData.itemHenatukiSou = new Item();
            if (henatukiData.itemHenatukikVA == null)
                henatukiData.itemHenatukikVA = new Item();
            if (henatukiData.itemHenatukikVA_Tani == null)
                henatukiData.itemHenatukikVA_Tani = new Item("kVA");
            if (henatukiData.itemHenatukiDenatu1 == null)
                henatukiData.itemHenatukiDenatu1 = new Item();
            if (henatukiData.itemHenatukiDenatu == null)
                henatukiData.itemHenatukiDenatu = new Item();
            if (henatukiData.itemHenatukiDenatuRS == null)
                henatukiData.itemHenatukiDenatuRS = new Item();
            if (henatukiData.itemHenatukiDenatuST == null)
                henatukiData.itemHenatukiDenatuST = new Item();
            if (henatukiData.itemHenatukiDenatuTR == null)
                henatukiData.itemHenatukiDenatuTR = new Item();
            if (henatukiData.itemHenatukiDenryuR == null)
                henatukiData.itemHenatukiDenryuR = new Item();
            if (henatukiData.itemHenatukiDenryuS == null)
                henatukiData.itemHenatukiDenryuS = new Item();
            if (henatukiData.itemHenatukiDenryuT == null)
                henatukiData.itemHenatukiDenryuT = new Item();
            if (henatukiData.itemHenatukiDenryuS_Tani == null)
                henatukiData.itemHenatukiDenryuS_Tani = new Item("S");
            if (henatukiData.itemHenatukiOndo == null)
                henatukiData.itemHenatukiOndo = new Item();
            if (henatukiData.itemHenatukiIg1 == null)
                henatukiData.itemHenatukiIg1 = new Item();
            if (henatukiData.itemHenatukiIg == null)
                henatukiData.itemHenatukiIg = new Item();
            if (henatukiData.itemHenatukiTeikaku == null)
                henatukiData.itemHenatukiTeikaku = new Item();
        }
    }
}
