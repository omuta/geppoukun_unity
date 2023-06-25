using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Keiyaku {
    internal class Ryokin {
        public DateTime start;
        public List<BaseKeiyaku> listKeiyakusyubetu;
        //	public List<CyouseiTanka> listCyouseiTanka;
        CyouseiTanka cyouseiTanka;

        public Ryokin() {
            listKeiyakusyubetu = new List<BaseKeiyaku>();
            cyouseiTanka = new CyouseiTanka();
            //		listCyouseiTanka = new List<CyouseiTanka>();
        }

        public DateTime getKikanStart() {
            if (this.start == null) {
                this.start = DateTime.Now;
            }
            return this.start;
        }

        public List<BaseKeiyaku> getKeiyakusyubetu() {
            return this.listKeiyakusyubetu;
        }

        public void setKikanStart(DateTime start) {
            this.start = start;
        }

        public void setKeiyakusyubetu(BaseKeiyaku keiyaku) {
            this.listKeiyakusyubetu.Add(keiyaku);
        }

        public BaseKeiyaku getKeiyakusyubetu(int index) {
            if (listKeiyakusyubetu.Count <= index)
                return new BaseKeiyaku();
            return listKeiyakusyubetu[index];
        }

        public List<BaseKeiyaku> getKeiyakusyubetuList() {
            return listKeiyakusyubetu;
        }

        public void addKeiyakusyubetu(int index, BaseKeiyaku keiyakusyubetu) {
            if (listKeiyakusyubetu.Count <= index)
                return;
            listKeiyakusyubetu.Insert(index, keiyakusyubetu);
        }

        public void removeKeiyakusyubetu(int index) {
            if (listKeiyakusyubetu.Count <= index)
                return;
            listKeiyakusyubetu.RemoveAt(index);
        }


        //	public void setCyouseiTanka(CyouseiTanka tanka){
        //		this.listCyouseiTanka.add(tanka);
        //	}
        //
        //	public CyouseiTanka getCyouseihi(DateTime DateTime){
        //		CyouseiTanka retCyouseihi = null;
        //		for(CyouseiTanka cyouseihi : listCyouseiTanka){
        ////			if(cyouseihi.start.before(DateTime)) break;
        //			if(cyouseihi.start.after(DateTime) || cyouseihi.start.equals(DateTime)){
        //				retCyouseihi = cyouseihi;
        //				break;
        //			}
        //		}
        //		if(retCyouseihi == null){
        //			CyouseiTanka cyouseiTanka_zero = new CyouseiTanka(new BigDecimal(0), new BigDecimal(0), new BigDecimal(0), new BigDecimal(0));
        //			retCyouseihi = cyouseiTanka_zero;
        //		}
        //		return retCyouseihi;
        //	}   }
    }
}
