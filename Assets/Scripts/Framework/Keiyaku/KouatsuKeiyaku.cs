using PreGeppou.Data;
using PreGeppou.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework.Keiyaku
{
    internal class KouatsuKeiyaku : PreGeppou.Keiyaku.BaseKeiyaku
    {
        static private string[] titleIppan = { "夏季", "その他季", "", "" };
        static private string KeisanSikiIppan = "([夏季料金単価]*[①使用電力量]+[その他季料金単価]*[②使用電力量])+[燃料費調整額]";

        public KouatsuKeiyaku(string textDenryokugaisyaMei, string[] textMeterTitle, decimal? kihonRyoukin, decimal[] ryoukin, string textKeisanSiki, int? image, int? textColor, int? bgColor)
            : base(textDenryokugaisyaMei, titleIppan, kihonRyoukin, ryoukin, KeisanSikiIppan, image, textColor, bgColor)
        {
            setSyubetu(Kouatsu);
        }

        // タイトルを返す
        public string getTitle1(SystemData systemData)
        {
            if (!DataManager.isNull(systemData.tenkenData.itemKensinKongetuHiduke) && Common.isEmpty(systemData.tenkenData.textKakoTuki[0]))
            {
                string month = Common.getMonth(systemData.tenkenData.itemKensinKongetuHiduke.text);
                if (month == "7" || month == "8" || month == "9")
                {
                    return textMeterTitle[0];
                }
                else
                {
                    return textMeterTitle[1];
                }
            }
            return "0";
        }
        public string getTitle2(SystemData systemData)
        {
            return textMeterTitle[1];
        }
        public string getTitle3(SystemData systemData)
        {
            return textMeterTitle[2];
        }
        public string getTitle4(SystemData systemData)
        {
            return textMeterTitle[3];
        }

        // 料金を返す
        public decimal getRyoukin1(SystemData systemData)
        {
            if (!DataManager.isNull(systemData.tenkenData.itemKensinKongetuHiduke) && Common.isEmpty(systemData.tenkenData.textKakoTuki[0]))
            {
                string month = Common.getMonth(systemData.tenkenData.itemKensinKongetuHiduke.text);
                if (month == "7" || month == "8" || month == "9")
                {
                    return ryoukin[0];
                }
                else
                {
                    return ryoukin[1];
                }
            }
            return new decimal(0);
        }
        public decimal getRyoukin2(SystemData systemData)
        {
            return ryoukin[1];
        }
        public decimal getRyoukin3(SystemData systemData)
        {
            return ryoukin[2];
        }
        public decimal getRyoukin4(SystemData systemData)
        {
            return ryoukin[3];
        }
    }
}
