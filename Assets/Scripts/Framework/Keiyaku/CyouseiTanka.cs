using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Keiyaku {
    internal class CyouseiTanka {
        public DateTime start;
        public decimal nenryohi1;
        public decimal nenryohi2;
        public decimal taiyoukou;
        public decimal shinEnergy;

        public CyouseiTanka() {
            this.nenryohi1 = 0;
            this.nenryohi2 = 0;
            this.taiyoukou = 0;
            this.shinEnergy = 0;
        }

        public CyouseiTanka(decimal nenryohi1, decimal nenryohi2, decimal taiyoukou, decimal shinEnergy) {
            this.nenryohi1 = nenryohi1;
            this.nenryohi2 = nenryohi2;
            this.taiyoukou = taiyoukou;
            this.shinEnergy = shinEnergy;
        }
    }
}
