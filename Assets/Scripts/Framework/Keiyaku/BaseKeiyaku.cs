using PreGeppou.Data;
using PreGeppou.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Keiyaku
{
    internal class BaseKeiyaku {
        private static readonly long serialVersionUID = 1L;
        public string textKeiyakuSyubetuMei;
        public string[] textMeterTitle;
        public decimal? kihonRyoukin;
        public decimal [] ryoukin;
        public string textKeisanSiki;
        public int? image;
        public int? textColor;
        public int? bgColor;
        public int? syubetu;
        public static readonly int Kouatsu = 1;
	public static readonly int Kijibetsu = 2;
	public static readonly int Kyujitsu = 3;

	public static readonly string Kenshin_1 = "検針項目は1行";
	public static readonly string Kenshin_4 = "検針項目は4行";

	public BaseKeiyaku(string textDenryokugaisyaMei,
                       string[] textMeterTitle,
                       decimal?  kihonRyoukin,
                       decimal [] ryoukin,
                       string textKeisanSiki,
                       int? image,
                       int? textColor,
                       int? bgColor) {
            this.textKeiyakuSyubetuMei = textDenryokugaisyaMei;
            this.textMeterTitle = textMeterTitle;
            this.kihonRyoukin = kihonRyoukin;
            this.ryoukin = ryoukin;
            this.textKeisanSiki = textKeisanSiki;
            this.image = image;
            this.textColor = textColor;
            this.bgColor = bgColor;
            this.syubetu = 0;
        }

        public BaseKeiyaku() {
            string[] textMeterTitle = { "その他季", "夏季", "", "" };
            decimal [] ryoukin = { new decimal (0), new decimal (0), new decimal (0), new decimal (0) };
            this.textKeiyakuSyubetuMei = "業務用高圧";
            this.textMeterTitle = textMeterTitle;
            this.kihonRyoukin = new decimal (0);
            this.ryoukin = ryoukin;
            this.textKeisanSiki = "";
            this.image = null;
            this.textColor = null;
            this.bgColor = null;
            this.syubetu = 1;
        }


        public int? getSyubetu() {
            return this.syubetu;
        }

        public void setSyubetu(int shubetu) {
            this.syubetu = shubetu;
        }

        public string getTextDenryokugaisyaMei() {
            return textKeiyakuSyubetuMei;
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

        // タイトルを返す
        public string getTitle1(SystemData systemData) {
            return textMeterTitle[0];
        }
        public string getTitle2(SystemData systemData) {
            return textMeterTitle[1];
        }
        public string getTitle3(SystemData systemData) {
            return textMeterTitle[2];
        }
        public string getTitle4(SystemData systemData) {
            return textMeterTitle[3];
        }

        // 料金を設定する
        public void setKihonRyoukin(string ryoukin) {
            kihonRyoukin = Convert.ToDecimal(ryoukin);
        }
        public void setRyoukin1(string ryoukin) {
            this.ryoukin[0] = Convert.ToDecimal(ryoukin);
        }
        public void setRyoukin2(string ryoukin) {
            this.ryoukin[1] = Convert.ToDecimal(ryoukin);
        }
        public void setRyoukin3(string ryoukin) {
            this.ryoukin[2] = Convert.ToDecimal(ryoukin);
        }
        public void setRyoukin4(string ryoukin) {
            this.ryoukin[3] = Convert.ToDecimal(ryoukin);
        }

        // 料金を返す
        public string getKihonRyoukin() {
            if (kihonRyoukin == null)
                return "0";
            return kihonRyoukin.ToString();
        }
        public string getRyoukin1() {
            if (ryoukin == null || ryoukin[0] == null)
                return "0";
            return ryoukin[0].ToString();
        }
        public string getRyoukin2() {
            if (ryoukin == null || ryoukin[1] == null)
                return "0";
            return ryoukin[1].ToString();
        }
        public string getRyoukin3() {
            if (ryoukin == null || ryoukin[2] == null)
                return "0";
            return ryoukin[2].ToString();
        }
        public string getRyoukin4() {
            if (ryoukin == null || ryoukin[3] == null)
                return "0";
            return ryoukin[3].ToString();
        }
    }
}
