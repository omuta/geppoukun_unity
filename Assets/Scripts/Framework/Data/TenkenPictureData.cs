using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Data {
    internal class TenkenPictureData {
        private static readonly long serialVersionUID = 1L;

        public String Image = "";
        public String Comment = "";
        public String Date = "";
        public bool Print = false;
        public bool Delete = false;

        // コンストラクタ
        public TenkenPictureData(String Image, String Date, String Comment, bool Print) {
            this.Image = Image;
            this.Date = Date;
            this.Comment = Comment;
            this.Print = Print;
            this.Delete = false;
        }

        public void setDate(String Date) {
            this.Date = Date;
        }

        public String getDate() {
            return this.Date;
        }

        public void setImage(String Image) {
            this.Image = Image;
        }

        public String getImage() {
            return this.Image;
        }

        public void setComment(String Comment) {
            this.Comment = Comment;
        }

        public String getComment() {
            return this.Comment;
        }

        public void setPrint(bool Print) {
            this.Print = Print;
        }

        public bool getPrint() {
            return this.Print;
        }

        public void setDelete(bool Delete) {
            this.Delete = Delete;
        }

        public bool getDelete() {
            return this.Delete;
        }
    }
}
