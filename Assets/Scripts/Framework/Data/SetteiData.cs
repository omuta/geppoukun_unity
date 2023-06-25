using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class SetteiData {
        private static readonly long serialVersionUID = 1L;
        public string textKensinKeta;
        public string textPrintKigouSize;
        public int screenOffTimeout;
        public string colorKonText;
        public string colorKonBack;
        public string colorZenText;
        public string colorZenBack;
        public string colorZennenText;
        public string colorZennenBack;
        public string textPrintMarginLeft;
        public string textPrintMarginRight;
        public string textPrintMarginTop;
        public string textPrintMarginBottom;
        public bool flagJigyousyoSetteiNomal;
        public bool flagRikirituSettei;
        public bool flagTaiyoukouSettei;
        public bool flagKakugetugraphSettei;
        public string textCyouhyouFreeFormatFileName;
        public string textCyouhyouFreeFormatFlag;
        public string textCyouhyouUseSeirekiFlag;
        public string textDefaultCyouhyouFormat = "印刷帳票1";
        // URI カメラのURI情報を保持(画面遷移で使用する)
        public Uri uri;
        // Intent ガメラのIntent保持
        public int intentData;
        public SetteiData(string textKensinKeta) {
            this.textKensinKeta = textKensinKeta;
            flagRikirituSettei = false;
        }
        public string getKensinKeta() {
            return textKensinKeta;
        }
    }
}
