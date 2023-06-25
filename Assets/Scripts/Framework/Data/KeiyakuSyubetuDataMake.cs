using PreGeppou.Framework;
using PreGeppou.Framework.Keiyaku;
using PreGeppou.Keiyaku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data
{
    internal class KeiyakuSyubetuDataMake {
        private static readonly long serialVersionUID = 1L;
        static private string[] titleIppan = { "夏季", "その他季", "", "" };
        static private string[] titleKijibetu = { "その他季昼間", "夏季昼間", "ピーク", "夜間" };
        static private string[] titleKyujitubetu = { "平日(夏季)", "平日(その他季)", "休日", "" };
        static private string[] titleKisetubetu = { "夏季", "その他季", "", "" };
        static private string[] ryoukin = { "", "", "", "", "" };
        static private string KihonRyoukin = "[料金単価]*[契約電力]*(185-[力率])/100";
        static private string KeisanSikiIppan = "([夏季料金単価]*[①使用電力量]+[その他季料金単価]*[②使用電力量])";
        static private string KeisanSikiKijibetu = "([夏季昼間料金単価]*[①使用電力量]+[ピーク料金単価]*[②使用電力量]+[その他季昼間単価]*[③使用電力量]+[夜間単価]*[④使用電力量])";
        static private string SaiseiKanouEnergy = "[太陽光発電促進付加金単価]*([①使用電力量]+[②使用電力量]+[③使用電力量]+[④使用電力量])";
        public static List<Ryokin> makeHokkaidoDenryoku() { // 北海道電力 料金データ
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(一般料金)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用電力(時間帯別料金)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用ウィークエンド電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用取引量別契約", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用蓄熱調整契約", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("エコ・アイスプラス", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("クックeプラス", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("オールeプラス", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("産業用取引量別契約", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("産業用蓄熱調整契約", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeTouhokuDenryoku() {  // 東北電力 料金データ
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力S", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧季節別時間帯別電力S", titleKijibetu, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力A", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧季節別時間帯別電力A", titleKijibetu, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力B", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧季節別時間帯別電力B", titleKijibetu, null, null, KeisanSikiIppan, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeTokyoDenryoku() {  // 東京電力 料金データ
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            // 東京電力の場合は、ダウンロードしたExcelファイルから料金データを読み込む
            if (RyokinExcel.readRyoukin(RyokinExcel.getFileName("東京電力"), listRyokin) == false) {
                ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(契約電力500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
                ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季節別時間帯別電力(契約電力500kW未満)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用休日高負荷電力(契約電力500kW未満)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(契約電力500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
                ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季節別時間帯別電力(契約電力500kW以上)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用休日高負荷電力(契約電力500kW以上)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
                ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧季節別時間帯別電力Ａ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ａ(契約電力500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
                ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧季節別時間帯別電力Ａ(契約電力500kW未満)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力(契約電力500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
                ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧季節別時間帯別電力(契約電力500kW以上)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ｂ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
                ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧季節別時間帯別電力Ｂ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
                listRyokin.Add(ryokin);
            }
            return listRyokin;
        }
        public static List<Ryokin> makeCyuubuDenryoku() { // 中部電力 料金データ
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用プランＡ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用プランＢ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用プランＣ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用ウィークエンドプランＡ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用ウィークエンドプランＢ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用ウィークエンドプランＣ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧業務用電力ＷＥ(休日平日別)プランＡ", titleKyujitubetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧業務用電力ＷＥ(休日平日別)プランＢ", titleKyujitubetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧業務用電力ＷＥ(休日平日別)プランＣ", titleKyujitubetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧業務用電力ＦＲ(季節別)プランＡ", titleKisetubetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧業務用電力ＦＲ(季節別)プランＢ", titleKisetubetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧業務用電力ＦＲ(季節別)プランＣ", titleKisetubetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＡ(第1種(季時別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＢ(第1種(季時別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＣ(第1種(季時別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＡ(第2種(季節別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＢ(第2種(季節別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＣ(第2種(季節別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＡ(第1種(季時別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＢ(第1種(季時別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＣ(第1種(季時別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＡ(第2種(季節別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＢ(第2種(季節別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧業務用電力プランＣ(第2種(季節別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧プランＬ（高圧電力第2種）", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧プランＨ（高圧電力第2種）", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力プランＡ(第１種季時別)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力プランＢ(第１種季時別)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力プランＡ(第２種季時別)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力プランＢ(第２種季時別)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＡ(第1種(季時別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＢ(第1種(季時別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＡ(第2種(季節別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＢ(第2種(季節別)20kVまたは30kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＡ(第1種(季時別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＢ(第1種(季時別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＡ(第2種(季節別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＢ(第2種(季節別)70kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＡ(第1種(季時別)140kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＢ(第1種(季時別)140kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＡ(第2種(季節別)140kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧電力プランＢ(第2種(季節別)140kV)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeHokurikuDenryoku() { // 北陸電力 料金データ
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ｂ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("季節別時間帯別電力Ａ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("季節別時間帯別電力Ｂ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用特別高圧電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用特別高圧季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("特別高圧季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeKansaiDenryoku() { // 関西電力
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力ＡＳ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力ＡＳ－ＴＯＵ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力ＢＳ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力ＢＳ－ＴＯＵ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("高圧電力ＡＬ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("高圧電力ＡＬ－ＴＯＵ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("高圧電力ＢＬ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("高圧電力ＢＬ－ＴＯＵ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ａ－ＴＯＵ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ｂ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ｂ－ＴＯＵ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeCyuugokuDenryoku() { // 中国電力
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            // 500ｋＷ未満
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用ＴＯＵ(500kW未満)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("最終保障電力Ａ(500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ａ(500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力ＴＯＵＡ(500kW未満)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("最終保障電力Ｂ(500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            // 500ｋＷ以上
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用ＴＯＵ(500kW以上)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("最終保障電力Ａ(500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ａ(500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧電力ＴＯＵＡ(500kW以上)", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("最終保障電力Ｂ(500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            // 特別高圧
            // 業務用
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力A(20kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力A(60kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧TOUA(20kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧TOUA(60kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            // 工場用
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力A(20kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力A(60kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力A(100kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧TOUA(20kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧TOUA(60kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧TOUA(100kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeSikokuDenryoku() { // 四国電力
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧Ａ季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ｂ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("高圧Ｂ季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧季節別時間帯別電力Ａ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("特別高圧電力Ｂ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("特別高圧季節別時間帯別電力Ｂ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            return listRyokin;
        }
        public static List<Ryokin> makeKyuusyuDenryoku() { // 九州電力
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季時別電力Ａ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("産業用電力Ａ(6kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("産業用電力Ａ(20kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("産業用電力Ａ(60kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("産業用電力Ａ(100kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("産業用季時別電力Ａ(6kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("産業用季時別電力Ａ(20kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("産業用季時別電力Ａ(60kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("産業用季時別電力Ａ(100kV)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
        public static List<Ryokin> makeOkinawaDenryoku() { // 沖縄電力
            List<Ryokin> listRyokin = new List<Ryokin>();
            Ryokin ryokin = new Keiyaku.Ryokin();
            // 500ｋＷ未満
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(500kW未満)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力(500kW以上)", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("業務用季節別時間帯別電力", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("業務用電力Ⅱ型", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku("業務用ウィークエンド電力", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ａ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("季節別時間帯別電力Ａ", titleKijibetu, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku("高圧電力Ｂ", titleIppan, null, null, KeisanSikiIppan, null, null, null));
            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku("季節別時間帯別電力Ｂ", titleKijibetu, null, null, KeisanSikiKijibetu, null, null, null));
            listRyokin.Add(ryokin);
            return listRyokin;
        }
    }
}
