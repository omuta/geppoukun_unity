using PreGeppou.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TenkenData {
        private static readonly long serialVersionUID = 1L;
        // 事業所名
        public String textJigyousyo1;
        public String textJigyousyo2;
        // 点検日時・天候
        public Item itemTenkenNijtijiYMD = new Item();
        public String textTenkenNijtijiYear;
        public String textTenkenNijtijiMonth;
        public String textTenkenNijtijiDay;
        public String textKensinNijtijiYear;
        public String textKensinNijtijiMonth;
        public String textKensinNijtijiDay;
        public Item itemTenkenNijtijiTime = new Item();
        public Item itemTenkou = new Item();
        public Item itemGaiKion = new Item();
        public Item itemSitunaiKion = new Item();
        public Item itemSitudo = new Item();
        public Item itemSeiribangou = new Item();
        // 電力量計
        public Item itemKensinbi = new Item();
        public Item itemJyouritu = new Item();
        public Item itemKeiyakuDenryoku = new Item();
        public Item itemKeiyakuDenryokuKakuteiYM = new Item();
        public Item itemSetubiYouryou = new Item();
        // 高圧受電盤
        public Item itemDenatuRS = new Item();
        public Item itemDenatuST = new Item();
        public Item itemDenatuTR = new Item();
        public Item itemDenryuR = new Item();
        public Item itemDenryuS = new Item();
        public Item itemDenryuT = new Item();
        public Item itemDenryoku = new Item();
        public Item itemRikiritu = new Item();
        public bool checkJyudenbanNasi = false;
        public Item itemJyudenbanNasi = new Item();
        // 検針
        public Item itemGenzaiKongetuHiduke = new Item();
        public Item itemGenzaiKongetu1 = new Item();
        public Item itemGenzaiKongetu2 = new Item();
        public Item itemGenzaiKongetu3 = new Item();
        public Item itemGenzaiKongetu4 = new Item();
        public Item itemGenzaiKongetuSaidaiDenryoku = new Item();
        public Item itemGenzaiKongetuGenzaiDenryoku = new Item();
        public Item itemGenzaiKongetuRikiritu = new Item();
        public Item itemGenzaiKongetuYukoDenryoku = new Item();
        public Item itemGenzaiKongetuMukoDenryoku = new Item();
        public Item itemGenzaiSengetuHiduke = new Item();
        public Item itemGenzaiSengetu1 = new Item();
        public Item itemGenzaiSengetu2 = new Item();
        public Item itemGenzaiSengetu3 = new Item();
        public Item itemGenzaiSengetu4 = new Item();
        public Item itemGenzaiSengetuSaidaiDenryoku = new Item();
        public Item itemGenzaiSengetuGenzaiDenryoku = new Item();
        public Item itemGenzaiSengetuRikiritu = new Item();
        public Item itemGenzaiSengetuYukoDenryoku = new Item();
        public Item itemGenzaiSengetuMukoDenryoku = new Item();
        public Item itemGenzaiDenryokuryou1 = new Item();
        public Item itemGenzaiDenryokuryou2 = new Item();
        public Item itemGenzaiDenryokuryou3 = new Item();
        public Item itemGenzaiDenryokuryou4 = new Item();
        public Item itemGenzaiDenryokuryou = new Item();
        public Item itemGenzaiHeikinDenryokuryou = new Item();

        public Item itemKensinKongetuHiduke = new Item();
        public Item itemKensinKongetu1 = new Item();
        public Item itemKensinKongetu2 = new Item();
        public Item itemKensinKongetu3 = new Item();
        public Item itemKensinKongetu4 = new Item();
        public Item itemKensinKongetuSaidaiDenryoku = new Item();
        public Item itemKensinKongetuGenzaiDenryoku = new Item();
        public Item itemKensinKongetuRikiritu = new Item();
        public Item itemKensinKongetuYukoDenryoku = new Item();
        public Item itemKensinKongetuMukoDenryoku = new Item();
        public Item itemKensinSengetuHiduke = new Item();
        public Item itemKensinSengetu1 = new Item();
        public Item itemKensinSengetu2 = new Item();
        public Item itemKensinSengetu3 = new Item();
        public Item itemKensinSengetu4 = new Item();
        public Item itemKensinSengetuSaidaiDenryoku = new Item();
        public Item itemKensinSengetuGenzaiDenryoku = new Item();
        public Item itemKensinSengetuRikiritu = new Item();
        public Item itemKensinSengetuYukoDenryoku = new Item();
        public Item itemKensinSengetuMukoDenryoku = new Item();
        public Item itemCheckboxYusen = new Item();
        public Item itemKensinDenryokuryou1 = new Item();
        public Item itemKensinDenryokuryou2 = new Item();
        public Item itemKensinDenryokuryou3 = new Item();
        public Item itemKensinDenryokuryou4 = new Item();
        public Item itemKensinDenryokuryou = new Item();
        public Item itemKensinHeikinDenryokuryou = new Item();

        public String textYakanritu;
        // 検針(メーター交換)
        public Item itemGenzaiKoukanMae1 = new Item();
        public Item itemGenzaiKoukanMae2 = new Item();
        public Item itemGenzaiKoukanMae3 = new Item();
        public Item itemGenzaiKoukanMae4 = new Item();
        public Item itemGenzaiKoukanAto1 = new Item();
        public Item itemGenzaiKoukanAto2 = new Item();
        public Item itemGenzaiKoukanAto3 = new Item();
        public Item itemGenzaiKoukanAto4 = new Item();
        public Item itemKensinKoukanMae1 = new Item();
        public Item itemKensinKoukanMae2 = new Item();
        public Item itemKensinKoukanMae3 = new Item();
        public Item itemKensinKoukanMae4 = new Item();
        public Item itemKensinKoukanAto1 = new Item();
        public Item itemKensinKoukanAto2 = new Item();
        public Item itemKensinKoukanAto3 = new Item();
        public Item itemKensinKoukanAto4 = new Item();
        public String[] textKakoSiyouDenryokuData = new String[25];
        public String[] textKakoDemandData = new String[25];
        public String[] textKakoTuki = new String[25];
        public bool textKakoSiyouDenryokuDataEdit = false;
        public bool textKakoDemandDataEdit = false;
        public bool textKakoTukiEdit = false;
        public String ZennenDougetuSiyouDenryoku;
        public String ZennenDougetuDemand;
        public String ZennenDougetuTuki;
        public bool ZennenDougetuSiyouDenryokuEdit = false;
        public bool ZennenDougetuDemandEdit = false;
        public bool ZennenDougetuTukiEdit = false;
        public bool flgTenkenFinish = false; // 点検完了フラグ
                                                // 変圧器
        public List<HenatukiData> henatukiData = new List<HenatukiData>();
        // 点検箇所
        public List<Item> listTenkenKasyoData = new List<Item>();
        // 記事
        public Item[] itemKiji = new Item[10];
        // メール送信情報[報告書]
        public Item mailAddressReport = new Item();
        public Item mailSubjetReport = new Item();
        public Item mailBodyReport = new Item();
        // メール送信情報[現場写真]
        public Item mailAddressPhoto = new Item();
        public Item mailSubjetPhoto = new Item();
        public Item mailBodyPhoto = new Item();
        public Item mailFilePathSDCard = new Item();
        // 常用発電機
        public List<HatudenkiJyouyouData> listHatudenkiJyouyouData = new List<HatudenkiJyouyouData>();
        // 非常用発電機
        public List<HatudenkiHijyouyouData> listHatudenkiHijyouyouData = new List<HatudenkiHijyouyouData>();
        // 太陽光発電機
        public List<HatudenkiTaiyoukouData> listHatudenkiTaiyoukouData = new List<HatudenkiTaiyoukouData>();
        // その他のデータ
        public OtherData[,] listOtherData = new OtherData[5,30];
        // 表紙
        public HyousiData hyousiData = new HyousiData();
        // 帳票フォーマット
        public String textCyouhyouFormat = "印刷帳票1";
        public String textCyouhyouGraph;
        public String textCyouhyouHyousi;
        public String textCyouhyouSuiihyou;
        public String textCyouhyouRyoukin;
        public bool flgSuiihyouKongetuStart;
        public String textCyouhyouFreeFormatFlag;
        public String textCyouhyouFreeFormatFileName = "";
        public String textCyouhyouFreeFormatFilePath = "";
        public String textTenkensyaKatagaki;
        public String textCyouhyouKigouFontSize;
        public bool flgPrintFontStyle;
        public String textSendmail;
        public String textExcelToPDF;   // エクセルファイルをPDFに変換してからメール送信する
                                        // 推移表の電力量に月利用量を出力するか一日平均利用量を出力するか
        public bool flgSuiihyouDenryokuryou;
        public bool flgGraphOrder; // グラフを昇順に表示するか降順に表示するか
        public bool flgGraphScale; // グラフの目盛りをYY/MM にするか MM にするか
                                      // 常用発電機帳票フォーマット
        public String textJHCyouhyouFormat;
        // 非常用発電機帳票フォーマット
        public String textHJHCyouhyouFormat;
        // 画像リスト
        public List<TenkenPictureData> listTenkenPictureData = new List<TenkenPictureData>();
        // 自作のカメラActivityを使うか false:スマホにプリインストールのカメラ使用
        public String textUseOriginalCamera;
        // コンストラクタ
        public TenkenData() {
            henatukiData.Add((HenatukiData)new HenatukiData(new Item("電灯第1バンク"), null, null, null));
            henatukiData.Add((HenatukiData)new HenatukiData(new Item("動力第1バンク"), null, null, null));
        }
        public List<HenatukiData> getHenatukiData() {
            if (henatukiData[0].itemHenatukiMei == null) {
                henatukiData.Add((HenatukiData)new HenatukiData(new Item("電灯第1バンク"), null, null, null));
                henatukiData.Add((HenatukiData)new HenatukiData(new Item("動力第1バンク"), null, null, null));
            }
            return henatukiData;
        }
        public void setHenatukiData(List<HenatukiData> list) {
            henatukiData = list;
        }
        public TenkenData clone() {
            return (TenkenData)MemberwiseClone();
        }
    }
}
