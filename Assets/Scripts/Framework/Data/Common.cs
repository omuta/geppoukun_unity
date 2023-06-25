using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime;
using System.Reflection;
using PreGeppou.Framework;
using System.IO;
using UnityEngine.Device;

namespace PreGeppou.Data {
    static class Common {
        public static readonly string purchaseID = "purchase_geppou";
	    public static readonly string purchaseIDm = "purchase_geppou_month";
	    private static string SerialKey;
        public static bool NewExamMake = false;
        public static string[] Course = { "purchase_geppou", "purchase_geppou_month" };
        public static string[] CourceTitle = { "スタンダード" };
        public static string[] GenreTitle = { "月報くん" };
        public static string format = "Key=%s&NotificationId=%s&OrderId=%s&PackageName=%s&ProductId=%s&DeveloperPayload=%s&PurchaseTime=%s&PurchaseState=%s&PurchaseToken=%s";

        public static string getWareki(SystemData systemData, string date) {
            WarekiData wareki = systemData.wareki;
            TenkenData tenkenData = systemData.tenkenData;
            string ret = "";
            if (tenkenData.itemTenkenNijtijiYMD.text == null)
                return ret;

            string YMD = date;
            //		if((YMD.IndexOf ("年") == -1 && YMD.IndexOf ("月") == -1 && YMD.IndexOf ("日") == -1) || YMD.split("/").length != 3) return ret;
            // 西暦で出力が選択されている場合
            if (systemData.settei.textCyouhyouUseSeirekiFlag != null && systemData.settei.textCyouhyouUseSeirekiFlag == "1") {
                return tenkenData.itemTenkenNijtijiYMD.text.Substring(0, 4);
            }

            string targetYMD = YMD.Replace("年", "").Replace("月", "").Replace("日", "").Replace("/", "");
            int targetYear = int.Parse(targetYMD.Substring(0, 4));
            int year = targetYear - 1989 + 1;
            ret = year.ToString();
            if (!string.IsNullOrEmpty(wareki.textWareki3)) {
                string baseYMD = wareki.textWarekiStart3.Replace("年", "").Replace("月", "").Replace("日", "");
                int baseYaer = int.Parse(wareki.textWarekiStart3.Substring(0, 4));
                if (targetYMD.CompareTo(baseYMD) < 0) {
                    ret = wareki.textWareki3;
                }
            }
            if (!string.IsNullOrEmpty(wareki.textWareki2)) {
                string baseYMD = wareki.textWarekiStart2.Replace("年", "").Replace("月", "").Replace("日", "");
                int baseYear = int.Parse(wareki.textWarekiStart2.Substring(0, 4));
                if (baseYMD.CompareTo(targetYMD) < 0) {
                    year = targetYear - baseYear + 1;
                    if (year == 1) {
                        ret = "元";
                    } else {
                        ret = year.ToString();
                    }
                }
            }
            return ret;
        }

        public static string getWarekiNengouAlphabet(WarekiData _wareki, string strYear, string Month, string Day) {
            if (isNumeric(strYear) == false)
                return "";
            string Nengo = getNengouAlphabet(_wareki, strYear, Month, Day);
            int wareki = int.Parse(strYear);
            wareki -= 1988;
            return Nengo + " " + wareki.ToString();
        }

        public static string getNengou(SystemData systemData) {
            if (string.IsNullOrEmpty(systemData.wareki.textWareki2)) {
                systemData.wareki.textWareki2 = "令和";
                systemData.wareki.textWarekiAlphabet2 = "R";
                systemData.wareki.textWarekiStart2 = "2019年05月01日";
            }

            WarekiData wareki = systemData.wareki;
            TenkenData tenkenData = systemData.tenkenData;
            if (tenkenData.itemTenkenNijtijiYMD.text == null)
                return "";
            string YMD = tenkenData.itemTenkenNijtijiYMD.text;
            if (YMD.IndexOf("年") == -1 || YMD.IndexOf("月") == -1 || YMD.IndexOf("日") == -1)
                return "";
            // 西暦で出力が選択されている場合
            if (systemData.settei.textCyouhyouUseSeirekiFlag != null && systemData.settei.textCyouhyouUseSeirekiFlag == "1") {
                return "";
            }

            string ret = "令和";
            string targetYMD = YMD.Replace("年", "").Replace("月", "").Replace("日", "");
            if (!string.IsNullOrEmpty(wareki.textWareki3)) {
                string baseYMD = wareki.textWarekiStart3.Replace("年", "").Replace("月", "").Replace("日", "");
                if (targetYMD.CompareTo(baseYMD) < 0) {
                    ret = wareki.textWareki3;
                }
            }
            if (!string.IsNullOrEmpty(wareki.textWareki2)) {
                string baseYMD = wareki.textWarekiStart2.Replace("年", "").Replace("月", "").Replace("日", "");
                if (baseYMD.CompareTo(targetYMD) < 0) {
                    ret = wareki.textWareki2;
                }
            }
            return ret;
        }

        public static string getNengouAlphabet(WarekiData wareki, string strYear, string Month, string Day) {
            string ret = "H";
            string baseYMD = strYear + Month + Day;
            if (!string.IsNullOrEmpty(wareki.textWareki3)) {
                string YMD = wareki.textWarekiStart3.Replace("年", "").Replace("月", "").Replace("日", "");
                if (0 < baseYMD.CompareTo(YMD)) {
                    ret = wareki.textWarekiAlphabet3;
                }
            }
            if (!string.IsNullOrEmpty(wareki.textWareki2)) {
                string YMD = wareki.textWarekiStart2.Replace("年", "").Replace("月", "").Replace("日", "");
                if (0 < baseYMD.CompareTo(YMD)) {
                    ret = wareki.textWarekiAlphabet2;
                }
            }
            return ret;
        }

        public static bool isEmpty(Item item) {
            if (item == null || item.text == null || item.text == "" || item.text == "0") {
                if (item == null) {
                    item = new Item();
                }
                return true;
            }
            return false;
        }

        public static bool isNumeric(string strData) {
            if (strData == null)
                return false;
            try {
                double.Parse(strData);
                return true;
            } catch (Exception e) {
                return false;
            }
        }
        public static bool isDate(string strData) {
            try {
                DateTime.Parse(strData);
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public static bool isTime(string strData) {
            try {
                DateTime.Parse(strData);
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public static bool isFormula(string formula) {
            formula = formula.Replace("　", "");
            char[] target = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', ',', '(', ')', '[', ']', '+', '-', '*', '/', ' ' };
            for (int i = 0; i < formula.Length; i++) {
                bool flag = false;
                for (int j = 0; j < target.Length; j++) {
                    if (formula[i] == target[j]) {
                        flag = true;
                        break;
                    }
                }
                if (flag == false) {
                    return false;
                }
            }
            return true;
        }

        public static string getPlus(string strNum1, string strNum2) {
            string retstring = "";
            if (strNum1 == string.Empty)
                strNum1 = "0";
            if (strNum2 == string.Empty)
                strNum2 = "0";
            strNum1 = strNum1.Trim();
            strNum2 = strNum2.Trim();
            strNum1 = strNum1.Replace(",", "");
            strNum2 = strNum2.Replace(",", "");
            if (isNumeric(strNum1) && isNumeric(strNum2)) {
                decimal  dec1 = decimal.Parse(strNum1), dec2 = decimal.Parse(strNum2);
                decimal  dec = dec1 + dec2;
                retstring = dec.ToString();
            }
            return retstring;
        }

        public static string getPlus(string[] strNums) {
            string retstring = "";
            string[] strNum = new string[strNums.Length];
            decimal result = 0;

            for (int i = 0; i < strNums.Length; i++) {
                strNum[i] = strNums[i];
                if (strNum[i] == "") {
                    strNum[i] = "0";
                }
                strNum[i] = strNum[i].Trim();
                strNum[i] = strNum[i].Replace(",", "");
                decimal  dec = 0;
                if (isNumeric(strNum[i])) {
                    dec = decimal.Parse(strNum[i]);
                }
                result += dec;
            }
            retstring = result.ToString();
            return retstring;
        }

        public static string getPlus(string strNum1, string strNum2, string strNum3, string strNum4) {
            string retstring = "";
            if (strNum1 == "") {
                strNum1 = "0";
            }
            if (strNum2 == "") {
                strNum2 = "0";
            }
            if (strNum3 == "") {
                strNum3 = "0";
            }
            if (strNum4 == "") {
                strNum4 = "0";
            }
            strNum1 = strNum1.Trim();
            strNum2 = strNum2.Trim();
            strNum3 = strNum3.Trim();
            strNum4 = strNum4.Trim();
            strNum1 = strNum1.Replace(",", "");
            strNum2 = strNum2.Replace(",", "");
            strNum3 = strNum3.Replace(",", "");
            strNum4 = strNum4.Replace(",", "");
            if (isNumeric(strNum1) && isNumeric(strNum2) && isNumeric(strNum3) && isNumeric(strNum4)) {
                decimal dec1 = decimal.Parse(strNum1);
                decimal dec2 = decimal.Parse(strNum2);
                decimal dec3 = decimal.Parse(strNum3);
                decimal dec4 = decimal.Parse(strNum4);
                decimal  dec = dec1 + dec2 + dec3+ dec4;
                retstring = dec.ToString();
            }
            return retstring;
        }

        public static string getMinus(string strNum1, string strNum2) {
            string retstring = "";
            if (strNum1 == "") {
                strNum1 = "0";
            }
            if (strNum2 == "") {
                strNum2 = "0";
            }
            strNum1 = strNum1.Trim();
            strNum2 = strNum2.Trim();
            strNum1 = strNum1.Replace(",", "");
            strNum2 = strNum2.Replace(",", "");
            if (isNumeric(strNum1) && isNumeric(strNum2)) {
                decimal dec1 = decimal.Parse(strNum1);
                decimal dec2 = decimal.Parse (strNum2);
                decimal dec = dec1 - dec2;
                retstring = dec.ToString();
            }
            return retstring;
        }
        public static String getMultiply(Item itemNum1, Item itemNum2, int keta) {
            if (isEmpty(itemNum1) == true)
                return "0";
            if (isEmpty(itemNum2) == true)
                return "0";
            if (isZero(itemNum1) || isZero(itemNum2))
                return "0";
            return Common.getMultiply(itemNum1.text, itemNum2.text, keta);
        }

        public static string getMultiply(string strNum1, string strNum2, int keta) {
            string retstring = "";
            if (strNum1 == "") {
                strNum1 = "0";
            }
            if (strNum2 == "") {
                strNum2 = "0";
            }
            strNum1 = strNum1.Trim();
            strNum2 = strNum2.Trim();
            strNum1 = strNum1.Replace(",", "");
            strNum2 = strNum2.Replace(",", "");
            if (isNumeric(strNum1) && isNumeric(strNum2)) {
                decimal dec1 = decimal.Parse(strNum1);
                decimal dec2 = decimal.Parse(strNum2);
                decimal  dec = Math.Round(dec1 * dec2, keta);
                retstring = dec.ToString();
            }
            return retstring;
        }

        public static string getDivide(string strNum1, string strNum2, int keta) {
            string retstring = "";
            if (strNum1 == "") {
                strNum1 = "0";
            }
            if (strNum2 == "") {
                strNum2 = "0";
            }
            strNum1 = strNum1.Trim();
            strNum2 = strNum2.Trim();
            strNum1 = strNum1.Replace(",", "");
            strNum2 = strNum2.Replace(",", "");
            if (strNum2 == "0") {
                return "";
            }
            if (strNum2 == "0.0") {
                return "";
            }
            if (strNum2 == "0.00") {
                return "";
            }
            if (isNumeric(strNum1) && isNumeric(strNum2)) {
                decimal dec1 = decimal.Parse(strNum1);
                decimal dec2 = decimal.Parse(strNum2);
                decimal  dec = Math.Round( dec1 / dec2, keta);
                retstring = dec.ToString();
            }
            return retstring;
        }

        static public int ROUND_OFF = 0;
        static public int ROUND_UP = 1;
        static public int ROUND_DOWN = 2;
        public static Item round(Item strNum, int mode) {
            String retString = "";
            if (isZero(strNum))
                return strNum;
            if (Common.isNumeric(strNum.text) == false)
                return strNum;
            if (mode == ROUND_OFF) {
                retString = Math.Truncate(double.Parse(strNum.text)).ToString();
            } else if (mode == ROUND_UP) {
                retString = Math.Ceiling(double.Parse(strNum.text)).ToString();
            } else if (mode == ROUND_DOWN) {
                retString = Math.Truncate(double.Parse(strNum.text)).ToString();
            }
            strNum.text = retString;
            return strNum;
        }
        public static string round(string strNum, int mode) {
            String retString = "";
            if (string.IsNullOrEmpty(strNum))
                return strNum;
            if (Common.isNumeric(strNum) == false)
                return strNum;
            if (mode == ROUND_OFF) {
                retString = Math.Truncate(double.Parse(strNum)).ToString();
            } else if (mode == ROUND_UP) {
                retString = Math.Ceiling(double.Parse(strNum)).ToString();
            } else if (mode == ROUND_DOWN) {
                retString = Math.Truncate(double.Parse(strNum)).ToString();
            }
            strNum = retString;
            return strNum;
        }

        public static bool isZero(Item item) {
            if (item == null || string.IsNullOrEmpty(item.text) || item.text == "0") {
                return false;
            }
            try {
                double iszero = double.Parse(item.text);
                return (iszero == 0.0);
            } catch (Exception e) {
                return false;
            }
        }

        public static string formatKensinti(string data, string n) {
            string retstring = "";
            data = data.Replace(",", "");
            data = data.Trim();
            if (data == "") {
                data = "0";
            }
            if (isNumeric(data) && isNumeric(data)) {
                try {
                    retstring = string.Format($"{{0:f{n}}}", double.Parse(data));
                } catch (Exception e) {
                    retstring = "";
                }
            }
            if (retstring == string.Format($"{{0:f{n}}}", 0)) {
                retstring = "";
            }
            return retstring;
        }

        public static string formatInteger(string data) {
            string retstring = "";
            data = data.Replace(",", "");
            data = data.Trim();
            if (data == ""){
                data = "0";
            }
            if (isNumeric(data) && isNumeric(data)) {
                if (data.IndexOf(".") != -1) {
                    data = data.Substring(0, data.IndexOf("."));
                }
                try {
                    retstring = string.Format("%1$,3d", long.Parse(data));
                } catch (Exception e) {
                    retstring = "";
                }
            }
            return retstring;
        }

        public static string formatDecimal(string data) {
            string retstring = "";
            data = data.Replace(",", "");
            data = data.Trim();
            if (data == "") {
                data = "0";
            }
            if (isNumeric(data)) {
                try {
                    retstring = string.Format("{0:#,0}", double.Parse(data));
                } catch (Exception e) {
                    retstring = "";
                }
            }
            return retstring;
        }

        /**
         * 引数で渡されてた文字列の左トリムを行います
         * @param target 置換対象となる文字列
         * @return 変換後の文字列
         */
        public static string trimLeftCharacter(string target) {
            if (target == null) {
                return "";
            }
            return Regex.Replace(target, "^ +", string.Empty);
        }

        /**
         * 引数で渡されてた文字列の右トリムを行います
         * @param target 置換対象となる文字列
         * @return 変換後の文字列
         */
        public static string trimRightCharacter(string target) {
            if (target == null) {
                return "";
            }
            return Regex.Replace(target, " +$", "");
        }

        public static string trimLeftZero(string str) {
            return str.TrimStart();
        }

        public static string ReplaceFirst(
            this string self,
            string oldValue,
            string newValue
        ) {
            var startIndex = self.IndexOf(oldValue);

            if (startIndex == -1)
                return self;

            return self
                    .Remove(startIndex, oldValue.Length)
                    .Insert(startIndex, newValue)
                ;
        }

        public static string ReplaceFirst
        (
            this string self,
            string oldValue,
            string newValue,
            StringComparison comparisonType
        ) {
            var startIndex = self.IndexOf(oldValue, comparisonType);

            if (startIndex == -1)
                return self;

            return self
                    .Remove(startIndex, oldValue.Length)
                    .Insert(startIndex, newValue)
                ;
        }
        public static string getNissu(string strDate1, string strDate2) {
            DateTime date1;
            DateTime date2;
            int diffDays = 0;
            strDate1 = strDate1.Trim();
            strDate2 = strDate2.Trim();
            if (string.IsNullOrEmpty(strDate1) || string.IsNullOrEmpty(strDate2)) {
                return "";
            }
            if(strDate1.Length == 8) {
                strDate1 = strDate1.Insert(4, "/").Insert(7, "/");
            }
            if (strDate2.Length == 8) {
                strDate2 = strDate2.Insert(4, "/").Insert(7, "/");
            }
            if (strDate1.Contains("年")) {
                strDate1 = strDate1.Replace("年", "/").Replace("月", "/").Replace("日", "");
            }
            if (strDate2.Contains("年")) {
                strDate2 = strDate2.Replace("年", "/").Replace("月", "/").Replace("日", "");
            }
            try {
                string[] arrDate1 = strDate1.Split("/");
                if (arrDate1.Length != 3) {
                    return "";
                }
                string[] arrDate2 = strDate2.Split("/");
                if (arrDate2.Length != 3) {
                    return "";
                }
                int y1 = int.Parse(arrDate1[0]);
                int m1 = int.Parse(arrDate1[1]);
                int d1 = int.Parse(arrDate1[2]);
                int y2 = int.Parse(arrDate2[0]);
                int m2 = int.Parse(arrDate2[1]);
                int d2 = int.Parse(arrDate2[2]);
                date1 = new DateTime(y1, m1, d1);
                date2 = new DateTime(y2, m2, d2);

                diffDays = (int)(date2 - date1).TotalDays;
            } catch (Exception e) {
                return "";
            }
            return diffDays.ToString();
        }

        public static string getHour(string strTime) {
            int index = strTime.IndexOf("時");
            if (index == -1)
                return "";
            if (index - 2 < 0)
                return "";
            string hour = strTime.Substring(index - 2, index);
            return hour;
        }

        public static string getMinute(string strTime) {
            int index = strTime.IndexOf("分");
            if (index == -1)
                return "";
            if (index - 2 < 0)
                return "";
            string minute = strTime.Substring(index - 2, index);
            return minute;
        }

        /**
         * ファイル名から拡張子を取り除いた名前を返します。
         * @param fileName ファイル名
         * @return ファイル名
         */
        public static string getPreffix(string fileName) {
            if (fileName == null)
                return "";
            int point = fileName.LastIndexOf(".");
            if (point != -1) {
                return fileName.Substring(0, point);
            }
            return fileName;
        }

        /**
         * コピー元のパス[srcPath]から、コピー先のパス[destPath]へ
         * ファイルのコピーを行います。
         * コピー処理にはFileChannel#transferToメソッドを利用します。
         * 尚、コピー処理終了後、入力・出力のチャネルをクローズします。
         * @param srcPath    コピー元のパス
         * @param destPath    コピー先のパス
         * @throws IOException    何らかの入出力処理例外が発生した場合
         */
        public static void FileCopy(string inputFile, string outputFile) {
            try {
                File.Copy(inputFile, outputFile);
            } catch (IOException e) {
                throw e;
            }
	    }

	    public static void mkDirSDCard(string path) {
            try {
                if (Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            } catch(IOException e) {
                throw e;
            }
        }

        //public static HSSFFont selectColor(string color, HSSFFont font) {
        //    font.setColor(HSSFColor.BLACK.index);

        //    if (color == null)
        //        return font;
        //    if (color.equals("黒色")) {
        //        font.setColor(HSSFColor.BLACK.index);
        //    } else if (color.equals("青色")) {
        //        font.setColor(HSSFColor.BLUE.index);
        //    } else if (color.equals("緑色")) {
        //        font.setColor(HSSFColor.GREEN.index);
        //    } else if (color.equals("オレンジ色")) {
        //        font.setColor(HSSFColor.ORANGE.index);
        //    } else if (color.equals("赤色")) {
        //        font.setColor(HSSFColor.RED.index);
        //    } else if (color.equals("白色")) {
        //        font.setColor(HSSFColor.WHITE.index);
        //    }
        //    return font;
        //}

        //public static HSSFFont selectStyle(string style, HSSFFont font) {
        //    font.setItalic(false);
        //    font.setBoldweight(Font.BOLDWEIGHT_NORMAL);

        //    if (style == null)
        //        return font;
        //    if (style.equals("標準")) {
        //        font.setItalic(false);
        //        font.setBoldweight(Font.BOLDWEIGHT_NORMAL);
        //    } else if (style.equals("斜体")) {
        //        font.setItalic(true);
        //        font.setBoldweight(Font.BOLDWEIGHT_NORMAL);
        //    } else if (style.equals("太字")) {
        //        font.setItalic(false);
        //        font.setBoldweight((short)(HSSFFont.BOLDWEIGHT_BOLD * 10));
        //    } else if (style.equals("太字 斜体")) {
        //        font.setItalic(true);
        //        font.setBoldweight((short)(HSSFFont.BOLDWEIGHT_BOLD * 10));
        //    }
        //    return font;
        //}

        public static string TM(string tm) {
            if (tm == null) {
                return "";
            }
            return tm;
        }

        public static string ExchangeTM(string tm) {
            if (tm == null) {
                return "";
            }
            return tm.Replace("時", ":").Replace("分", "");
        }

        public static string ExchangeYMD(string ymd) {
            if (ymd == null) {
                return "";
            }
            return ymd.Replace("年", "/").Replace("月", "/").Replace("日", "");
        }

        public static string ExchangeDecimal(string _decimal) {
            if (_decimal == null) {
                return "";
            }
            return Common.formatInteger(_decimal);
        }

        /**
         * 日付の妥当性チェックを行います。
         * 指定した日付文字列（yyyy/MM/dd or yyyy-MM-dd）が
         * カレンダーに存在するかどうかを返します。
         * @param strDate チェック対象の文字列
         * @return 存在する日付の場合true
         */
        public static bool checkDate(string strDate) {
            strDate = ExchangeYMD(strDate);
            if (strDate == null || strDate.Length != 10) {
                throw new ArgumentException(
                        $"引数の文字列[{strDate}]は不正です。");
            }
            strDate = strDate.Replace('-', '/');
            // 日付/時刻解析を厳密に行うかどうかを設定する。
            try {
                DateTime dt;
                if (DateTime.TryParse(strDate, out dt)) {
                    //変換出来たら、dtにその値が入る
                    Console.WriteLine("{0}はDateTime{1}に変換できます。", strDate, dt);
                    return true;
                } else {
                    Console.WriteLine("{0}はDateTimeに変換できません。", strDate);
                    return false;
                }
            } catch (Exception e) {
                return false;
            }
        }

        public static string makeYMD() {
            return DateTime.Now.ToString("yyyy年MM月dd日");
        }

        public static string makeYMD2(DateTime calendar) {
            return calendar.ToString("yyyyMM");
        }

        public static string makeNenGetsu(string strCalendar) {
            DateTime calendar = toCalendar(strCalendar);
            return calendar.ToString("yyyy年MM月");
        }

        public static string makeGetsu(string strCalendar) {
            DateTime calendar = toCalendar(strCalendar);
            return calendar.ToString("MM") + "月";
        }

        public static string makeYM(string strCalendar) {
            if (string.IsNullOrEmpty(strCalendar)) {
                return "";
            }
            return makeYM(toCalendar(strCalendar));
        }

        public static bool isEmpty(string item) {
            if (item == null || item == "") {
                return true;
            }
            return false;
        }

        public static string makeYM(DateTime calendar) {
            return calendar.ToString("yyyyMM");
        }

        // 作業日を作成する
        public static DateTime makeNextYMD() {
            return DateTime.Now.AddMonths(1);
        }

        // 作業日を作成する
        public static DateTime makePrevYMD() {
            return DateTime.Now.AddMonths(-1);
        }

        // 作業日を作成する
        public static DateTime makePrevYMD(string YM, int index) {
            DateTime calendar = toCalendar(YM + "01日");
            return calendar.AddMonths(index * (-1));
        }

        // 作業日を作成する
        public static DateTime makePrevYMD(int index) {
            return DateTime.Now.AddMonths(index * (-1));
        }

        // 作業日を作成する
        public static DateTime makePrevYMD(DateTime calendar) {
            return calendar.AddMonths(-1);
        }

        // 点検年月を作成する
        public static string makeYYMM(DateTime calendar) {
            return $"{calendar.ToString("yyyy")}年{calendar.ToString("MM")}月";
        }

        // 点検年月を作成する
        public static string makeMM(DateTime calendar) {
            return $"{calendar.ToString("MM")}月";
        }

        public static string makeTime() {
            return DateTime.Now.ToString("HH時mm分");
        }

        public static double toDouble(string strDouble) {
            Double ret;
            if (strDouble == string.Empty) {
                return 0;
            }
            try {
                ret = double.Parse(strDouble);
            } catch (Exception e) {
                return 0;
            }
            return ret;
        }

        public static int toInteger(string strInt) {
            int ret;
            if (strInt == string.Empty) {
                return 0;
            }
            try {
                strInt = strInt.Replace(".0", "");
                ret = int.Parse(strInt);
            } catch (Exception e) {
                return 0;
            }
            return ret;
        }

        public static long toLong(string strLong) {
            long ret;
            if (strLong == "") {
                return (long)0;
            }
            try {
                strLong = strLong.Replace(".0", "");
                ret = long.Parse(strLong);
            } catch (Exception e) {
                return (long)0;
            }
            return ret;
        }

        public static DateTime toDate(string strDate) {
            DateTime date = DateTime.Now;
            try {
                if (strDate != null) {
                    strDate = strDate.Replace("年", "/").Replace("月", "/").Replace("日", "");
                }
                date = DateTime.Parse(strDate);
            } catch (Exception e) {
            }
            return date;
        }

        static public int getInteger(string count) {
            if (count == "") {
                return 0;
            } else {
                if (isNumeric(count)) {
                    return int.Parse(count);
                } else {
                    return 0;
                }
            }
        }

        //static public bool equals(string string, string stringCompare) {
        //    if (string == null)
        //        return false;
        //    return string.equals(stringCompare);
        //}

        static public string putstring(string str) {
            if (str == null || str == string.Empty) {
                return "";
            }
            return str;
        }

        static public string getPrimaryAccount() {
            //Account[] accounts = AccountManager.get(context).getAccountsByType("com.google");
            /// TODO スマホからアカウントを取得する
            return "";
        }

        static public string getVersion() {
            string versionName = "";
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
            versionName = Application.version;
#endif
            return "Version : " + versionName;
        }

        public static int getVersionCode() {
            int versionCode = 0;
            // TODO OSのバージョンを取得する
            return versionCode;
        }

        /**
         * 指定した日付文字列（yyyy/MM/dd or yyyy-MM-dd）
         * における月末日付を返します。
         *
         * @param strDate 対象の日付文字列
         * @return 月末日付
         */
        public static string getLastDay(string strDate) {
            strDate = ExchangeYMD(strDate);
            if (strDate == null || strDate.Length != 10) {
                throw new ArgumentException(
                        "引数の文字列[" + strDate + "]" +
                        "は不正です。");
            }
            int yyyy = int.Parse(strDate.Substring(0, 4));
            int MM = int.Parse(strDate.Substring(5, 7));
            int dd = DateTime.DaysInMonth(yyyy, MM);
            DateTime lastDay = new DateTime(yyyy, MM, dd);
            return lastDay.ToString("yyyy年MM月dd日");
        }

        public static string getMonth(string strDate) {
            strDate = ExchangeYMD(strDate);
            if (strDate == null || strDate.Length != 10) {
                throw new ArgumentException(
                        "引数の文字列[" + strDate + "]" +
                        "は不正です。");
            }
            int yyyy = int.Parse(strDate.Substring(0, 4));
            int MM = int.Parse(strDate.Substring(5, 2));
            int dd = int.Parse(strDate.Substring(8, 2));
            DateTime thisMonth = new DateTime(yyyy, MM, dd);
            return thisMonth.ToString("MM月");
        }

        public static string makeKensinDate(string strDate, string day) {
            strDate = ExchangeYMD(strDate);
            if (strDate == null || strDate.Length != 10) {
                throw new ArgumentException(
                        "引数の文字列[" + strDate + "]" +
                        "は不正です。");
            }
            int yyyy = int.Parse(strDate.Substring(0, 4));
            int MM = int.Parse(strDate.Substring(5, 2));
            int dd = int.Parse(day);
            DateTime kensinDate = new DateTime(yyyy, MM, dd);
            return kensinDate.ToString("yyyy年MM月dd日");
        }

        public static void BlueToothOn() {
            // TODO BlueTooth ON 
            //BluetoothAdapter bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
            //bluetoothAdapter.enable();
        }

        public static void BlueToothOff() {
            // TODO BlueTooth OFF
            //BluetoothAdapter bluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
            //bluetoothAdapter.enable();
        }

        public static string escape(string str) {
            //		string = string.ReplaceAll("\\\\", "\\\\\\\\");
            //		string = string.ReplaceAll("\\*", "\\\\*");
            //		string = string.ReplaceAll("\\+", "\\\\+");
            //		string = string.ReplaceAll("\\.", "\\\\.");
            //		string = string.ReplaceAll("\\?", "\\\\?");
            //		string = string.ReplaceAll("\\{", "\\\\{");
            //		string = string.ReplaceAll("\\}", "\\\\}");
            //		string = string.ReplaceAll("\\[", "\\\\[");
            //		string = string.ReplaceAll("\\]", "\\\\]");
            //		string = string.ReplaceAll("\\(", "\\\\(");
            //		string = string.ReplaceAll("\\)", "\\\\)");
            //		string = string.ReplaceAll("\\^", "\\\\^");
            //		string = string.ReplaceAll("\\$", "\\\\$");
            //		string = string.ReplaceAll("\\-", "\\\\-");
            //		string = string.ReplaceAll("\\|", "\\\\|");
            //		string = string.ReplaceAll("\\/", "\\\\/");
            return str;
        }

        // SDカードのマウント先をゲットするメソッド
        public static string getMountSDCard() {
#if UNITY_EDITOR
        string path = Directory.GetCurrentDirectory();
#else
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
            return path;
        }

        // 引数に渡したpathがマウントされているかどうかチェックするメソッド
        //public static bool isMounted(string path) {
        //    bool isMounted = false;

        //    Scanner scanner = null;
        //    try {
        //        // マウントポイントを取得する
        //        File mounts = new File("/proc/mounts");   // 注2
        //        scanner = new Scanner(new FileInputStream(mounts));
        //        // マウントポイントに該当するパスがあるかチェックする
        //        while (scanner.hasNextLine()) {
        //            if (scanner.nextLine().contains(path)) {
        //                // 該当するパスがあればマウントされているってこと
        //                isMounted = true;
        //                break;
        //            }
        //        }
        //    } catch (FileNotFoundException e) {
        //        throw new RuntimeException(e);
        //    } finally {
        //        if (scanner != null) {
        //            scanner.close();
        //        }
        //    }

        //    // マウント状態をreturn
        //    return isMounted;
        //}

        public static string stringTokusyuMoji = "ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ ⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ ①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳ ㊤㊥㊦㊧㊨ ㍉㍍㌔㌘㌧㌦㍑㌫㌢ ㎝㎏㎡ ㏍℡№ ㍻㍼㍽㍾ ㈱㈲㈹";
        public static int isTokusyuMoji(string checkstring) {
            for (int i = 0; i < checkstring.Length; i++) {
                for (int j = 0; j < stringTokusyuMoji.Length; j++) {
                    if (stringTokusyuMoji[j] == ' ')
                        continue;
                    if (checkstring[i] == stringTokusyuMoji[j]) {
                        return j;
                    }
                }
            }
            return -1;
        }

        public static string removeCaracter(string data, string target) {
            if (data == null) {
                return "";
            }
            return data.Replace(target, "");
        }

        public static long getMaxData(long r, long s, long t) {
            long max = 0;
            if (r < t && s < t) {
                max = t;
            } else if (r < s && t < s) {
                max = s;
            } else {
                max = r;
            }
            return max;
        }

        public static string getNowDate() {
            DateTime date = new DateTime();
            return date.ToString("yyyy年MM月dd日 HH時mm分ss秒");
        }

        public static string getDateFormat(string filePath) {
            return File.GetLastWriteTime(filePath).ToString("yyyy年MM月dd日 HH時mm分ss秒");
        }

        public static string getDateYYYYMMDDKKMM(string date) {
            string[] resultDate = date.Split("分");
            return resultDate[0] + "分";
        }

        /**
         * 現在の日付・時刻から指定の【年数】を加算・減算した結果を返します。
         * @param addYear 加算・減算する年数
         * @return    計算後の Calendar インスタンス。
         */
        public static DateTime addYear(int addYear) {
            return DateTime.Now.AddYears(addYear);
        }
        /**
         * 現在の日付・時刻から指定の【月数】を加算・減算した結果を返します。
         * @param addMonth 加算・減算する月数
         * @return    計算後の Calendar インスタンス。
         */
        public static DateTime addMonth(int addMonth) {
            return DateTime.Now.AddMonths(addMonth);
        }
        /**
         * 現在の日付・時刻から指定の【日数】を加算・減算した結果を返します。
         * @param addDate 加算・減算する日数
         * @return    計算後の Calendar インスタンス。
         */
        public static DateTime addDate(int addDate) {
            return DateTime.Now.AddDays(addDate);
        }
        /**
         * 現在の日付・時刻から指定の【時間】を加算・減算した結果を返します。
         * @param addHour 加算・減算する時間
         * @return    計算後の Calendar インスタンス。
         */
        public static DateTime addHour(int addHour) {
            return DateTime.Now.AddHours(addHour);
        }
        /**
         * 現在の日付・時刻から指定の【分】を加算・減算した結果を返します。
         * @param addMinute 加算・減算する分
         * @return    計算後の Calendar インスタンス。
         */
        public static DateTime addMinute(int addMinute) {
            return DateTime.Now.AddMinutes(addMinute);
        }
        /**
         * 現在の日付・時刻から指定の【秒】を加算・減算した結果を返します。
         * @param addSecond 加算・減算する秒
         * @return    計算後の Calendar インスタンス。
         */
        public static DateTime addSecond(int addSecond) {
            return DateTime.Now.AddSeconds(addSecond);
        }
        /**
         * 現在の日付・時刻から指定の時間量を加算・減算した結果を返します。
         * 年、月、日、時間、分、秒、ミリ秒の各時間フィールドに対し、
         * 任意の時間量を設定できます。
         * たとえば、現在の日付時刻から 10 日前を計算する場合は以下となります。
         * Calendar cal = add(null,0,0,-10,0,0,0,0);
         *
         * 各時間フィールドの値がその範囲を超えた場合、次の大きい時間フィールドが
         * 増分または減分されます。
         * たとえば、以下では1時間と5分進めることになります。
         * Calendar cal = add(null,0,0,0,0,65,0,0);
         *
         * 各時間フィールドに設定する数量が0の場合は、現在の値が設定されます。
         * java.util.GregorianCalendarの内部処理では以下の分岐を行っている。
         *     if (amount == 0) {
         *         return;
         *     }
         *
         * @param cal 日付時刻の指定があればセットする。
         *     nullの場合、現在の日付時刻で新しいCalendarインスタンスを生成する。
         * @param addYear 加算・減算する年数
         * @param addMonth 加算・減算する月数
         * @param addDate 加算・減算する日数
         * @param addHour 加算・減算する時間
         * @param addMinute 加算・減算する分
         * @param addSecond 加算・減算する秒
         * @param addMillisecond 加算・減算するミリ秒
         * @return    計算後の Calendar インスタンス。
         */
        //public static Calendar add(Calendar cal,
        //                           int addYear, int addMonth, int addDate,
        //                           int addHour, int addMinute, int addSecond,
        //                           int addMillisecond) {
        //    if (cal == null) {
        //        cal = Calendar.getInstance();
        //    }
        //    cal.add(Calendar.YEAR, addYear);
        //    cal.add(Calendar.MONTH, addMonth);
        //    cal.add(Calendar.DATE, addDate);
        //    cal.add(Calendar.HOUR_OF_DAY, addHour);
        //    cal.add(Calendar.MINUTE, addMinute);
        //    cal.add(Calendar.SECOND, addSecond);
        //    cal.add(Calendar.MILLISECOND, addMillisecond);
        //    return cal;
        //}

        public static readonly string[] PROHIBITED_SYMBOLS = {
            "\\", "/", ":", "*", "?", "\'", "\"", "<", ">"
        };

        public static bool checkFileProhibitionWordForWindows(string name) {
            bool hit = false;
            foreach (string character in PROHIBITED_SYMBOLS){
                if (name.Contains(character)) {
                    hit = true;
                    break;
                }
            }

            return hit;
	    }

	    public static string getFileExtension(string fileName) {
            if (fileName == null) {
                return null;
            }

            return Path.GetExtension(fileName);
        }

        public static void ExchangeCharCode(string filepath) {
            string? error;
            try {
                string str;
                using (StreamReader sr = new StreamReader(filepath, Encoding.GetEncoding("Shift_JIS"))) {
                    str = sr.ReadToEnd();
                }

                string outFilePath = GetOutputPath(filepath + ".bak");
                using (StreamWriter sw = new StreamWriter(outFilePath, false, Encoding.UTF8)) {
                    sw.Write(str);
                }

                File.Move(filepath + ".bak", filepath);
            } catch (FileNotFoundException e) {
                error = e.StackTrace;
            } catch (IOException e) {
                error = e.StackTrace;
            }
        }

        static string GetOutputPath(string filePath) {
            FileInfo fileInfo = new FileInfo(filePath);
            return GetOutputFolder(filePath) + "\\" + fileInfo.Name;
        }

        static string GetOutputFolder(string filePath) {
            FileInfo fileInfo = new FileInfo(filePath);
            string outputFolderPath = fileInfo.DirectoryName + "\\output";
            if (!Directory.Exists(outputFolderPath))
                Directory.CreateDirectory(outputFolderPath);
            return outputFolderPath;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /////////////////////////////////////////////////////////////////////////////////////////////////

        /// </summary>
        /// <returns></returns>
        public static string getSerialKey() {
            return SerialKey;
        }

        //public static string getSerialKey(Service service) {
        //    return Settings.Secure.getstring(service.getContentResolver(), Settings.System.ANDROID_ID);
        //}

        //public static void setSerialKey(string key) {
        //    SerialKey = key;
        //}
        //public static string getSerialKey(Activity activity) {
        //    return Settings.Secure.getstring(activity.getContentResolver(), Settings.System.ANDROID_ID);
        //}

        //static string[] strGenre = { "月報くん" };
        //public static string getGenreName(string no) {
        //    int ino = Integer.valueOf(no);
        //    return strGenre[ino];
        //}
        //public static string getCourceName(string no) {
        //    int ino = Integer.valueOf(no);
        //    return CourceTitle[ino - 1];
        //}

        //// 購入情報をこちらのDBに登録する
        //public static BillingService mBillingService;
        //public static void setBillingDataToServer(Activity activity) {
        //    mBillingService = new BillingService();
        //    mBillingService.setContext(activity);

        //    // mBillingService.requestPurchase("everything_useable_month", Consts.ITEM_TYPE_INAPP, mPayloadContents)
        //    //    	PurchaseDatabase mPurchaseDatabase = new PurchaseDatabase(activity);
        //    //        Cursor cursor = mPurchaseDatabase.queryAllPurchasedItems();
        //    //        if (cursor == null) {
        //    //            return;
        //    //        }
        //    //        final Set<string> ownedItems = new HashSet<string>();
        //    //        try {
        //    //            int productIdCol = cursor.getColumnIndexOrThrow(PurchaseDatabase.PURCHASED_PRODUCT_ID_COL);
        //    //            while (cursor.moveToNext()) {
        //    //                string productId = cursor.getstring(productIdCol);
        //    //                ownedItems.add(productId);
        //    //            }
        //    //        } finally {
        //    //            cursor.close();
        //    //        }
        //}

        public static string getCouseIDtoCourceName(string id) {
            int i = 0;
            for (; i < Course.Length; i++)
                if (Course[i] == id) {
                    break;
                }
            return CourceTitle[i];
        }

        static public void saveText(string str, string FileName) {
            string? error;
            // ストリームを開く
            try {
#if UNITY_EDITOR
                string path = Directory.GetCurrentDirectory();
#else
                string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
                path += ("/" + FileName);
                using (StreamWriter writer = new StreamWriter(path, false)) {
                    writer.WriteLine(str);
                    writer.Flush();
                }
            } catch (FileNotFoundException e) {
                error = e.StackTrace;
            } catch (IOException e) {
                error = e.StackTrace;
            }
        }

        static public string readText(string FileName) {
            string? error;
            string str = string.Empty;
            try {
#if UNITY_EDITOR
                string path = Directory.GetCurrentDirectory();
#else
                string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
                FileInfo info = new FileInfo(path + "/" + FileName);
                StreamReader reader = new StreamReader(info.OpenRead());
                str = reader.ReadToEnd();
            } catch (Exception e) {
                error = e.StackTrace;
            }
            return str;
        }

        static public void setBillingCheckDate() {
            saveText(DateTime.Now.ToString("yyyyMMdd"), "BillingCheck.dat");
        }

        static int warningKakin = 30;
        static int errorKakin = 150;
        static public int getBillingCheck() {
            string date = readText("BillingCheck.dat");
            if (date == null)
                return 2;
            date = date.Trim();
            if (date.Length != 8) {
                return 0;
            }
            DateTime calendar = toCalendar(date);
            DateTime nowDate = DateTime.Now;
            long difference = (long)(nowDate - calendar).TotalDays;
            long day = difference / 1000 / 60 / 60 / 24;

            if (day < warningKakin) {
                return 0; // 最後に課金チェックをしてから30日未満なら何も表示しない
            } else if (warningKakin <= day && day < errorKakin) {
                return 1; // 最後に課金チェックをしてから100日以上150日未満なら課金チェックをするように警告を表示する
            } else {
                return 2; // 最後に課金チェックをしてから150日以上経過したら利用停止にする
            }
        }

        /**
         * 指定された日付・時刻文字列を、可能であれば
         * Calendarクラスに変換します。
         * 以下の形式の日付文字列を変換できます。
         *
         * ●変換可能な形式は以下となります。
         *  yyyy/MM/dd
         *  yy/MM/dd
         *  yyyy-MM-dd
         *  yy-MM-dd
         *  yyyyMMdd
         *
         * 上記に以下の時間フィールドが組み合わされた状態
         * でも有効です。
         *  HH:mm
         *  HH:mm:ss
         *  HH:mm:ss.SSS
         *
         * @param strDate 日付・時刻文字列。
         * @return 変換後のCalendarクラス。
         * @throws IllegalArgumentException
         *         日付文字列が変換不可能な場合
         *         または、矛盾している場合（例：2000/99/99）。
         */
        public static DateTime toCalendar(string strDate) {
            strDate = strDate.Replace("年", "/").Replace("月", "/").Replace("日", "");
            strDate = Format(strDate);

            int yyyy = int.Parse(strDate.Substring(0, 4));
            int MM = int.Parse(strDate.Substring(5, 7));
            int dd = int.Parse(strDate.Substring(8, 10));
            int HH = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;
            int SSS = DateTime.Now.Millisecond;

            DateTime cal = new DateTime(yyyy, MM - 1, dd);
            int len = strDate.Length;
            switch (len) {
                case 10:
                    break;
                case 16: // yyyy/MM/dd HH:mm
                    HH = int.Parse(strDate.Substring(11, 13));
                    mm = int.Parse(strDate.Substring(14, 16));
                    cal = new DateTime(yyyy, MM - 1, dd, HH, mm, 0);
                    break;
                case 19: //yyyy/MM/dd HH:mm:ss
                    HH = int.Parse(strDate.Substring(11, 13));
                    mm = int.Parse(strDate.Substring(14, 16));
                    ss = int.Parse(strDate.Substring(17, 19));
                    cal = new DateTime(yyyy, MM - 1, dd, HH, mm, ss);
                    break;
                case 23: //yyyy/MM/dd HH:mm:ss.SSS
                    HH = int.Parse(strDate.Substring(11, 13));
                    mm = int.Parse(strDate.Substring(14, 16));
                    ss = int.Parse(strDate.Substring(17, 19));
                    SSS = int.Parse(strDate.Substring(20, 23));
                    cal = new DateTime(yyyy, MM - 1, dd, HH, mm, ss, SSS);
                    break;
                default:
                    throw new ArgumentException(
                            "引数の文字列[" + strDate +
                            "]は日付文字列に変換できません");
            }
            return cal;
        }

        /**
         * 様々な日付、時刻文字列をデフォルトの日付・時刻フォーマット
         * へ変換します。
         *
         * ●デフォルトの日付フォーマットは以下になります。
         *     日付だけの場合：yyyy/MM/dd
         *     日付+時刻の場合：yyyy/MM/dd HH:mm:ss.SSS
         *
         * @param str 変換対象の文字列
         * @return デフォルトの日付・時刻フォーマット
         * @throws IllegalArgumentException
         *     日付文字列が変換不可能な場合
         */
        private static string Format(string strDateTime) {
            if (strDateTime == null || strDateTime.Trim().Length < 8) {
                string[] split = strDateTime.Split("/");
                if (split.Length == 2) {
                    strDateTime = split[0] + "/" + strDateTime;
                } else {
                    return "";
                }
                //	        throw new IllegalArgumentException(
                //	                "引数の文字列["+ str +
                //	                "]は日付文字列に変換できません");
            }
            strDateTime = strDateTime.Trim();
            string yyyy = null;
            string MM = null;
            string dd = null;
            string HH = null;
            string mm = null;
            string ss = null;
            string SSS = null;
            // "-" or "/" が無い場合
            if (strDateTime.IndexOf("/") == -1 && strDateTime.IndexOf("-") == -1) {
                if (strDateTime.Length == 8) {
                    yyyy = strDateTime.Substring(0, 4);
                    MM = strDateTime.Substring(4, 6);
                    dd = strDateTime.Substring(6, 8);
                    return yyyy + "/" + MM + "/" + dd;
                }
                yyyy = strDateTime.Substring(0, 4);
                MM = strDateTime.Substring(4, 6);
                dd = strDateTime.Substring(6, 8);
                HH = strDateTime.Substring(9, 11);
                mm = strDateTime.Substring(12, 14);
                ss = strDateTime.Substring(15, 17);
                return yyyy + "/" + MM + "/" + dd + " " + HH + ":" + mm + ":" + ss;
            }
            string[] arrDateTime = strDateTime.Split(new char[] {'_', '/', '-', ':', '.', ' ' });
            int no = 0;
            string result = string.Empty;
            foreach (var datetime in arrDateTime) {
                switch (no++) {
                    case 0:// 年の部分
                        yyyy = fillstring(strDateTime, datetime, "L", "20", 4);
                        result += yyyy;
                        break;
                    case 1:// 月の部分
                        MM = fillstring(strDateTime, datetime, "L", "0", 2);
                        result += "/" + MM;
                        break;
                    case 2:// 日の部分
                        dd = fillstring(strDateTime, datetime, "L", "0", 2);
                        result += "/" + dd;
                        break;
                    case 3:// 時間の部分
                        HH = fillstring(strDateTime, datetime, "L", "0", 2);
                        result += " " + HH;
                        break;
                    case 4:// 分の部分
                        mm = fillstring(strDateTime, datetime, "L", "0", 2);
                        result += ":" + mm;
                        break;
                    case 5:// 秒の部分
                        ss = fillstring(strDateTime, datetime, "L", "0", 2);
                        result += ":" + ss;
                        break;
                    case 6:// ミリ秒の部分
                        SSS = fillstring(strDateTime, datetime, "R", "0", 3);
                        result += "." + SSS;
                        break;
                }
            }
            return result;
        }
        private static string fillstring(string strDate, string str,
                                     string position, string addStr,
                                     int len) {
            if (str.Length > len) {
                throw new ArgumentException(
                    "引数の文字列[" + strDate +
                    "]は日付文字列に変換できません");
            }
            return fillstring(str, position, len, addStr);
        }

        /**
         * 文字列[str]に対して、補充する文字列[addStr]を
         * [position]の位置に[len]に満たすまで挿入します。
         *
         * ※[str]がnullや空リテラルの場合でも[addStr]を
         * [len]に満たすまで挿入した結果を返します。
         * @param str 対象文字列
         * @param position 前に挿入 ⇒ L or l 後に挿入 ⇒ R or r
         * @param len 補充するまでの桁数
         * @param addStr 挿入する文字列
         * @return 変換後の文字列。
         */
        private static string fillstring(string str, string position, int len, string addStr) {
            if (addStr == null || addStr.Length == 0) {
                throw new ArgumentException
                    ("挿入する文字列の値が不正です。addStr=" + addStr);
            }
            if (str == null) {
                str = "";
            }
            while (len > str.Length) {
                if (String.Compare(position, "L", true) == 0) {
                    int sum = str.Length + addStr.Length;
                    if (sum > len) {
                        addStr = addStr.Substring(0, addStr.Length - (sum - len));
                        str.Insert(0, addStr);
                    } else {
                        str.Insert(0, addStr);
                    }
                } else {
                    str += addStr;
                }
            }
            if (str.Length == len) {
                return str;
            }
            return str.Substring(0, len);
        }

        /// ------------------------------------------------------------------------
        /// <summary>
        ///     指定した精度の数値に切り上げします。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り上げられた数値。</returns>
        /// ------------------------------------------------------------------------
        public static double RoundUp(double dValue, int iDigits) {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            return dValue > 0 ? (double)(Math.Ceiling((decimal)dValue / dCoef) * dCoef) :
                                (double)(Math.Floor((decimal)dValue / dCoef) * dCoef);
        }

        public static double RoundDown(double dValue, int iDigits) {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            return dValue > 0 ? (double)(Math.Truncate((decimal)dValue / dCoef) * dCoef) :
                                (double)(Math.Ceiling((decimal)dValue / dCoef) * dCoef);
        }

        public static double Round(double dValue, int iDigits) {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            return dValue > 0 ? (double)(Math.Round((decimal)dValue / dCoef) * dCoef) :
                                (double)(Math.Round((decimal)dValue / dCoef) * dCoef);
        }
        public static decimal RoundUp(decimal dValue, int iDigits) {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            return dValue > 0 ? (decimal)(Math.Ceiling((decimal)dValue / dCoef) * dCoef) :
                                (decimal)(Math.Floor((decimal)dValue / dCoef) * dCoef);
        }

        public static decimal RoundDown(decimal dValue, int iDigits) {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            return dValue > 0 ? (decimal)(Math.Truncate((decimal)dValue / dCoef) * dCoef) :
                                (decimal)(Math.Ceiling((decimal)dValue / dCoef) * dCoef);
        }

        public static decimal Round(decimal dValue, int iDigits) {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            return dValue > 0 ? (decimal)(Math.Round((decimal)dValue / dCoef) * dCoef) :
                                (decimal)(Math.Round((decimal)dValue / dCoef) * dCoef);
        }


        public static string getSystemPath() {
            string path = string.Empty;
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
            path =  Application.persistentDataPath;
#else
            path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
#endif
            return path;
        }
    }
}
