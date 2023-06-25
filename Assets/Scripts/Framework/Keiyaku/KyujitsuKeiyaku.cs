using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Keiyaku {
    internal class KyujitsuKeiyaku : BaseKeiyaku {
        static private string[] titleKyujitubetu = { "平日(夏季)", "平日(その他季)", "休日", "" };
        static private string KeisanSikiKijibetu = "([夏季昼間料金単価]*[①使用電力量]+[ピーク料金単価]*[②使用電力量]+[その他季昼間単価]*[③使用電力量]+[夜間単価]*[④使用電力量])";

        public KyujitsuKeiyaku(string textDenryokugaisyaMei, string[] textMeterTitle, decimal? kihonRyoukin, decimal[] ryoukin, string textKeisanSiki, int? image, int? textColor, int? bgColor) 
            : base(textDenryokugaisyaMei, titleKyujitubetu, kihonRyoukin, ryoukin, KeisanSikiKijibetu, image, textColor, bgColor) {
            setSyubetu(Kyujitsu);
        }
    }
}
