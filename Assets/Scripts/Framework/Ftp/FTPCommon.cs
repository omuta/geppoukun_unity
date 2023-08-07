using FluentFTP;
using PreGeppou.Data;
using PreGeppou.Framework.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

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

        public static void GetListing() {
            using (var conn = new FtpClient("ftp.increase.main.jp", "main.jp-increase", "sqG14Uwv")) {
                conn.Connect();

                // get a recursive listing of the files & folders in a specific folder
                foreach (var item in conn.GetListing("/geppoukun/ryotan43711@gmail.com", FtpListOption.Recursive)) {
                    if(item.Type == FtpObjectType.File) {
                        string path = Path.Combine(Common.getSystemPath(), item.Name);
                        if (File.Exists(path)){
                            if(File.GetLastWriteTime(path) < conn.GetModifiedTime(item.FullName)){
                                conn.DownloadFile(path, item.FullName, FtpLocalExists.Overwrite);
                                File.SetLastWriteTime(path, conn.GetModifiedTime(item.FullName));
                            } else {
                                conn.UploadFile(path, item.FullName, FtpRemoteExists.Overwrite);
                                conn.SetModifiedTime(item.FullName, File.GetLastWriteTime(path));
                            }
                        } else {
                            conn.DownloadFile(path, item.FullName);
                            File.SetLastWriteTime(path, conn.GetModifiedTime(item.FullName));
                        }
                        Console.WriteLine("File!  " + item.FullName);
                        Console.WriteLine("File size:  " + conn.GetFileSize(item.FullName));
                        Console.WriteLine("Modified date:  " + conn.GetModifiedTime(item.FullName));
                        Console.WriteLine("Chmod:  " + conn.GetChmod(item.FullName));
                    }
                }
            }
        }


        public static void Connect() {
            using (var conn = new FtpClient()) {
                conn.Host = "localhost";
                conn.Credentials = new NetworkCredential("ftptest", "ftptest");

                conn.Connect();
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
