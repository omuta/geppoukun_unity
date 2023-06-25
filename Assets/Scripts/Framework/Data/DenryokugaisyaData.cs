using PreGeppou.Keiyaku;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data
{
    internal class DenryokugaisyaData {
        private static readonly long serialVersionUID = 1L;
        public string textDenryokugaisyaMei;
        public string textNenryouhi;
        public string textTaiyoHatuden;
        public string textSaiseiEnergy;
        public List<Ryokin> listRyokin = new List<Ryokin>();
        public int image;
        public int textColor;
        public int bgColor;
        public DenryokugaisyaData(string textDenryokugaisyaMei, List<Ryokin> listRyokin, int? image, int? textColor, int? bgColor) {
            this.textDenryokugaisyaMei = textDenryokugaisyaMei;
            this.listRyokin = listRyokin;
            this.image = (int)image;
            this.textColor = (int)textColor;
            this.bgColor = (int)bgColor;
        }

        public string getTextDenryokugaisyaMei() {
            return textDenryokugaisyaMei;
        }

        public int getImage() {
            return image;
        }

        public int getTextColor() {
            return textColor;
        }

        public int getBgColor() {
            return bgColor;
        }

        public Ryokin getRyokin(string date) {
            Ryokin retRyokin = null;
            DateTime targetDate = Common.toDate(date);
            if (targetDate == null) {
                string today = DateTime.Now.ToString("yyyyMMdd");
                targetDate = Common.toDate(today);
            }
            if (listRyokin == null)
                return null;
            foreach (Ryokin ryokin in listRyokin) {
                if (targetDate < ryokin.getKikanStart() || ryokin.getKikanStart() == targetDate) {
                    retRyokin = ryokin;
                } else {
                    if (retRyokin == null) {
                        retRyokin = ryokin;
                    }
                    break;
                }
            }
            return retRyokin;
        }

        public string getNenryohi() {
            if (Common.isEmpty(textNenryouhi) == true) {
                return "0";
            }
            return textNenryouhi == "" ? "0" : textNenryouhi;
        }

        public string getTaiyoukou() {
            if (Common.isEmpty(textTaiyoHatuden) == true) {
                return "0";
            }
            return textTaiyoHatuden == "" ? "0" : textTaiyoHatuden;
        }

        public string getShinEnergy() {
            if (Common.isEmpty(textSaiseiEnergy) == true) {
                return "0";
            }
            return textSaiseiEnergy == "" ? "0" : textSaiseiEnergy;
        }

        //	public CyouseiTanka getCyouseihi(string date){
        //		Date targetDate = Common.toDate(date);
        //
        //		Keiyaku.Ryokin retRyokin = getRyokin(date);
        //		CyouseiTanka cyouseihi = retRyokin.getCyouseihi(targetDate);
        //		return cyouseihi;
        //	}

        public void setRyokin(List<Ryokin> listRyokin) {
            this.listRyokin = listRyokin;
        }
    }
}
