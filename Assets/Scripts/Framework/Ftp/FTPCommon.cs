using PreGeppou.Data;
using PreGeppou.Framework.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework.Ftp {
    internal class FTPCommon {
        public static bool isRestoreInitial(string _fileName) {
            string fileName = "initial";
            if (_fileName != null) {
                fileName = _fileName;
            }
            try {
                if (File.Exists(Path.Combine(Common.getSystemPath(), fileName))) {
                    return false;
                } else {
                    File.CreateText(Path.Combine(Common.getSystemPath(), fileName));
                    return true;
                }
            } catch (FileNotFoundException e) {
                //e.printStackTrace();
                return true;
            }
            return false;
        }

        public static void rename() {
            string fileFrom = Common.getSystemPath() + "/_initial";
            string fileTo = Common.getSystemPath() + "/initial";
            File.Move(fileFrom, fileTo);
        }

        public static void setRemoteInitial() {
            try {
                File.WriteAllText(Path.Combine(Common.getSystemPath(), "_initial"), "_initial");
            } catch (FileNotFoundException e) {
                //e.printStackTrace();
            } catch (IOException e) {
                //e.printStackTrace();
            }
        }

        //public static int contain(FTPFile[] arrFiles, string fileName) {
        //    int hit = -1;
        //    for (int i = 0; i < arrFiles.length; i++) {
        //        if (arrFiles[i].getName().equals(fileName)) {
        //            hit = i;
        //            break;
        //        }
        //    }
        //    return hit;
        //}
    }
}
