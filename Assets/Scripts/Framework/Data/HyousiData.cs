using PreGeppou.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class HyousiData {
        private static readonly long serialVersionUID = 1L;
        // 表紙宛名、事業場名
        public Item itemAtena = new Item();
        public Item itemJigyoujyoumei = new Item();

        // 点検区分
        public Item itemKubunHikikomi = new Item();
        public Item itemKubunHikikomiKaisyu = new Item();
        public Item itemKubunHikikomiBikou = new Item();
        public Item itemKubunJyuhaiden = new Item();
        public Item itemKubunJyuhaidenKaisyu = new Item();
        public Item itemKubunJyuhaidenBikou = new Item();
        public Item itemKubunKounai = new Item();
        public Item itemKubunKounaiKaisyu = new Item();
        public Item itemKubunKounaiBikou = new Item();
        public Item itemKubunSiyou = new Item();
        public Item itemKubunSiyouKaisyu = new Item();
        public Item itemKubunSiyouBikou = new Item();
        public Item itemKubunDenatu = new Item();
        public Item itemKubunDenatuKaisyu = new Item();
        public Item itemKubunDenatuBikou = new Item();
        public Item itemKubunHatudenki = new Item();
        public Item itemKubunHatudenkiKaisyu = new Item();
        public Item itemKubunHatudenkiBikou = new Item();

        // 特記事項
        public Item itemTokkijikou = new Item();
        public Item itemSougorenraku = new Item();

        // 添付資料
        public Item itemDenkiSisetu = new Item();
        public Item itemZetuenTeikou = new Item();
        public Item itemSettiTeikou = new Item();
        public Item itemHijyouyouHatudenki = new Item();
        public Item itemZetuenTsiryoku = new Item();
        public Item itemTeikitenken = new Item();
        public Item itemSeimituTenken = new Item();
        public Item itemKeidenkiSyadanki = new Item();
        public HyousiData() {
        }
    }
}
