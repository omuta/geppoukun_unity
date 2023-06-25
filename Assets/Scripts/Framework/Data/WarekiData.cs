using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class WarekiData {
        public String textWareki2;
        public String textWareki3;
        public String textWarekiAlphabet2;
        public String textWarekiAlphabet3;
        public String textWarekiStart2;
        public String textWarekiStart3;

        public static explicit operator WarekiData(SystemData? v) {
            throw new NotImplementedException();
        }
    }
}
