using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class Information {
        public string[] text = null;
        public void setInformation(string text) {
            if (this.text == null) {
                this.text = new string[1];
                this.text[0] = text;
            } else {
                string[] copy = new string[this.text.Length + 1];
                Array.Copy(this.text, 0, copy, 0, this.text.Length);
                this.text = copy;
                this.text[this.text.Length - 1] = text;
            }
        }

        public string getInformation() {
            if (this.text == null || this.text == null) {
                return "";
            }
            return string.Join("\r\n", this.text);
        }

        public static bool isTimeStamp(string text) {
            string t = "";
            if (text.Length != "[YYYY/MM/DD-YYYY/MM/DD]".Length) {
                return false;
            }
            try {
                t = text.Substring("[".Length, "[YYYY/MM/DD".Length);
                if (Common.isDate(text.Substring("[".Length, "[YYYY/MM/DD".Length)) == false) {
                    return false;
                }
                t = text.Substring("[YYYY/MM/DD-".Length, "[YYYY/MM/DD-YYYY/MM/DD".Length);
                if (Common.isDate(text.Substring("[YYYY/MM/DD-".Length, "[YYYY/MM/DD-YYYY/MM/DD".Length)) == false) {
                    return false;
                }
            } catch (System.Exception e) {
                return false;
            }
            return true;
        }

        public static bool isHit(string text) {
            DateTime calendar = DateTime.Now;

            string target = calendar.ToString("yyyy/MM/dd"); 
            string start = text.Substring("[".Length, "[YYYY/MM/DD".Length);
            string end = text.Substring("[YYYY/MM/DD-".Length, "[YYYY/MM/DD-YYYY/MM/DD".Length);

            if (0 <= target.CompareTo(start) && target.CompareTo(end) <= 0) {
                return true;
            }
            return false;
        }

        public static int setInformation(string[] text, int index) {
            // information配列を１個増やす
            if (InformationActivity.informations == null) {
                InformationActivity.informations = new Information[1];
                InformationActivity.informations[0] = new Information();
            } else {
                Information[] copyInformation = new Information[InformationActivity.informations.Length + 1];
                Array.Copy(InformationActivity.informations, 0, copyInformation, 0, InformationActivity.informations.Length);
                InformationActivity.informations = copyInformation;
                InformationActivity.informations[InformationActivity.informations.Length - 1] = new Information();
            }

            int i = index;
            for (; i < text.Length; i++) {
                if (isTimeStamp(text[i])) {
                    if (isHit(text[i])) {
                        for (int j = i + 1; j < text.Length; j++) {
                            //						if(text[j].equals("")) continue;
                            if (isTimeStamp(text[j])) {
                                return j - 1;
                            }
                            InformationActivity.informations[InformationActivity.informations.Length - 1].setInformation(text[j]);
                            index = j;
                        }
                    }
                }
                //			i = index = i+index;
            }

            return i;
        }
    }
    internal class InformationActivity {
        public static Information[] informations = null;
    }

}
