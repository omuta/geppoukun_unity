using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class SystemData {
        private static readonly long serialVersionUID = 1L;
        public List<JigyousyoData> listJigyousyo;
        public List<TenkouData> listTenkou = new List<TenkouData>();
        public List<TenkenKasyoData> listTenkenKasyo = new List<TenkenKasyoData>();
        public List<TenkenKigouData> listTenkenKigou = new List<TenkenKigouData>();
        public List<TeikeibunData> listTenkenTeikeibun = new List<TeikeibunData>();
        public List<DenryokugaisyaData> listDenryokugaisyaData = new List<DenryokugaisyaData>();
        public TenkenData tenkenData;
        public TenkenTHatsudenData tenkenTHData;
        public TenkensyaData tenkensyaData = new TenkensyaData("", null, null, null);
        public SetteiData settei = new SetteiData("");
        public WarekiData wareki = new WarekiData();
        // 帳票のフォーマットの違い等を制御
        //public Format format;
        public int mode = 0;
        public int positionJigyousyo = 0;
        public int positionHenatuki = 0;
        public int positionDenryokugaisya = 0;
        public int positionTenkou = 0;
        public int positionKeiyakuSyubetu = 0;
        public string tenkenYM = null;
        public int moveFromPosition = 0;
        public JigyousyoData jigyousyoData;
        public bool flgEditLock = false;
        public string tenkenFileName;
        public string textCyouhyouFreeFormatFilePath;
        public string textCyouhyouFreeFormatFileName;
        public Boolean textCyouhyouFreeFormatChenged;
        public SystemData() {
            tenkenData = new TenkenData();
            tenkenTHData = new TenkenTHatsudenData();
        }
        public SystemData clone() {
            SystemData cloneSystemData = null;
            try {
                //cloneSystemData = (SystemData)clone();
            } catch (Exception e) {
                // 省略
            }
            return cloneSystemData;
        }

        public DenryokugaisyaData getDenryokuGaisya() {
            return listDenryokugaisyaData[positionDenryokugaisya];
        }
    }
}
