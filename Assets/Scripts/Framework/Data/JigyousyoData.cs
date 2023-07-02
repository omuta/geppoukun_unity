using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    public class JigyousyoData {
        private static readonly long serialVersionUID = 1L;
        public string textTitle;
        public string textSubTitle;
        public string textAddress;
        public string textTantou;
        public string textSibuSiten;
        public string textJyouyouHatudenki;
        public string textHiJyouyouHatudenki;
        public string textTaiyoukouHatudenki;
        public string textOtherData;
        public string textOkyakusamaBangou;
        public string textMeterBangou;
        public string textDencyuBangou;
        public string textDaikousyaTitle;
        public string textDaikousya;
        public int image;
        public int textColor;
        public int bgColor;
        public bool finish;
        public bool[] tukitenken = new bool[12];
        public bool[] nentenken = new bool[12];
        public string textDenryokugaisya;
        public string textKeiyakusyubetu;
        public string textCyouhyouFreeFormatFileName;
        public JigyousyoData(string textTitle, string textSubTitle, string textDenryokugaisya, string textKeiyakusyubetu, int image, int textColor, int bgColor, bool finish) {
            this.textTitle = textTitle;
            this.textSubTitle = textSubTitle;
            if (Common.isEmpty(textDenryokugaisya)) {
                this.textDenryokugaisya = "東京電力";
            } else {
                this.textDenryokugaisya = textDenryokugaisya;
            }
            if (Common.isEmpty(textDenryokugaisya)) {
                this.textKeiyakusyubetu = "業務用電力(契約電力500kW未満)";
            } else {
                this.textKeiyakusyubetu = textKeiyakusyubetu;
            }
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
            this.finish = finish;
            for (int i = 0; i < tukitenken.Length; i++)
                tukitenken[i] = true;
            for (int i = 0; i < nentenken.Length; i++)
                nentenken[i] = false;
        }
        public JigyousyoData(string textTitle, string textSubTitle, string textAddress, string textTantou, string textSibuSiten,
                             string textDenryokugaisya, string textKeiyakusyubetu,
                             string textJyouyouHatudenki, string textHijyouyouHatudenki, string textTaiyoukouHatudenki, string textOtherData,
                             string textOkyakusamaBangou, string textMeterBangou, string textDencyuBangou,
                             string textDaikousyaTitle, string textDaikousya,
                              int image, int textColor, int bgColor,
                              bool finish, bool[] tukitenken, bool[] nentenken) {
            this.textTitle = textTitle;
            this.textSubTitle = textSubTitle;
            this.textAddress = textAddress;
            this.textTantou = textTantou;
            if (Common.isEmpty(textDenryokugaisya)) {
                this.textDenryokugaisya = "東京電力";
            } else {
                this.textDenryokugaisya = textDenryokugaisya;
            }
            this.textKeiyakusyubetu = textKeiyakusyubetu;
            this.textSibuSiten = textSibuSiten;
            this.textJyouyouHatudenki = textJyouyouHatudenki;
            this.textHiJyouyouHatudenki = textHijyouyouHatudenki;
            this.textTaiyoukouHatudenki = textTaiyoukouHatudenki;
            this.textOtherData = textOtherData;
            this.textOkyakusamaBangou = textOkyakusamaBangou;
            this.textMeterBangou = textMeterBangou;
            this.textDencyuBangou = textDencyuBangou;
            if (Common.isEmpty(textDaikousyaTitle)) {
                this.textDaikousyaTitle = "(代行者)";
            } else {
                this.textDaikousyaTitle = textDaikousyaTitle;
            }
            this.textDaikousya = textDaikousya;
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
            this.finish = finish;
            this.tukitenken = tukitenken;
            this.nentenken = nentenken;
        }
        public string getTextTitle() {
            return textTitle;
        }
        public void setTextTitle(string textTitle) {
            this.textTitle = textTitle;
        }
        public string getTextSubTitle() {
            return textSubTitle;
        }
        public void setTextSubTitle(string textSubTitle) {
            this.textSubTitle = textSubTitle;
        }
        public string getTextAddress() {
            return textAddress;
        }
        public void setTextAddress(string textAddress) {
            this.textAddress = textAddress;
        }
        public int getImage() {
            return image;
        }
        public void setImage(int image) {
            this.image = image;
        }
        public int getTextColor() {
            return textColor;
        }
        public void setTextColor(int textColor) {
            this.textColor = textColor;
        }
        public int getBgColor() {
            return bgColor;
        }
        public void setBgColor(int bgColor) {
            this.bgColor = bgColor;
        }
        public bool isFinish(JigyousyoData jigyousyoData) {
            SystemData systemData = new SystemData();
            systemData.listJigyousyo = new List<JigyousyoData>();
            systemData.listJigyousyo.Add(jigyousyoData); // makeTenkenFileNameで読み込ませるためにlistに事業所データをセットする
            systemData.positionJigyousyo = 0;
            systemData = DataManager.readTenken(systemData, null, false);
            return systemData.tenkenData.flgTenkenFinish;
        }
        public void setFinish(bool finish) {
            this.finish = finish;
        }
        public bool getTukitenken(int index) {
            if (tukitenken == null)
                return true;
            return tukitenken[index];
        }
        public void setTukitenken(int index, bool tukitenken) {
            this.tukitenken[index] = tukitenken;
        }
        public bool getNentenken(int index) {
            if (nentenken == null)
                return false;
            return nentenken[index];
        }
        public void setNentenken(int index, bool nentenken) {
            this.nentenken[index] = nentenken;
        }
        public bool isTukitenken(string month) {
            if (Common.isNumeric(month) == false)
                return false;
            month = Common.trimLeftZero(month);
            if (int.Parse(month) < 1 && 12 < int.Parse(month)) {
                return false;
            }
            return tukitenken[int.Parse(month) - 1];
        }
    }
}
