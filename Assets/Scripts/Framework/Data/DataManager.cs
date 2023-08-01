using Microsoft.VisualBasic;
using PreGeppou.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PreGeppou.Framework;
using System.IO;
using PreGeppou.Framework.Ftp;
using PreGeppou.Framework.Keiyaku;
using PreGeppou.Framework.Data;

using PreGeppou.Keiyaku;
using System.Text.Json;

namespace PreGeppou.Data {
    internal class DataManager {
        private static string FileNameJigyoujyou = "Jigyoujyou.json";
        private static string FileNameTenkou = "Tenkou.json";
        private static string FileNameTenkenKasyo = "TenkenKasyo.json";
        private static string FileNameTenkenKigou = "TenkenKigou.json";
        private static string FileNameTenkensya = "Tenkensya.json";
        private static string FileNameSettei = "Settei.json";
        private static string FileNameWareki = "Wareki.json";
        private static string FileNameSurpportersPassword = "SurpportersPassword";
        private static string FileNameDeviceInformation = "DeviceInformation.txt";
        private static string FileNameDenryokugaisya = "Denryokugaisya.json";
        //	static Activity activity;
        public static readonly int TENEKN_DATA_READ_MODE_THIS_MONTH = 0;
        public static readonly int TENEKN_DATA_READ_MODE_LAST_MONTH = 1;
        public static readonly int TENEKN_DATA_READ_MODE_SAME_MONTH_LAST_YEAR = 2;
        static string TAG = "DataControl";
        public static readonly Object[] sDataLock = new Object[0];

        public DataManager() {
            //		this.activity = activity;
        }

        // 事業所情報読み込み
        public static List<JigyousyoData> readJigyousyo() {
            List<JigyousyoData> retList = null;
            try {
                retList = new List<JigyousyoData>();
                lock (DataManager.sDataLock) {
                    List<JigyousyoData> jigyoujyouData = null;
                    string data = string.Empty;
                    try {
                        data = loadText(FileNameJigyoujyou);
                        if (data == null) {
                            deleteFile(FileNameJigyoujyou);
                            return null;
                        }
                        jigyoujyouData = JsonSerializer.Deserialize<List<JigyousyoData>>(data);
                        //jigyoujyouData = Collections.synchronizedList((List<JigyousyoData>) gson.fromJson(data, listOfJigyoujyouData));
                    } catch (IOException e) {
                        //e.printStackTrace();
                        return null;
                    }
                    foreach (JigyousyoData jigyoujyou in jigyoujyouData) {
                        retList.Add(jigyoujyou);
                    }
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to load to file");
            }
            return retList;
        }

        // 事業所情報書込み
        public static void writeJigyousyo(List<JigyousyoData> list) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize<List<JigyousyoData>>(list); // ["shika",koala]
                                                                                      // ファイル書き込み
                    saveText(stringJson, FileNameJigyoujyou);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file");
            }
        }

        // ファイルの有無を調べる
        public static bool existsTenken(SystemData systemData) {
            string path = Common.getSystemPath();
            return File.Exists(path + "/" + makeTenkenFileName(systemData));
        }

        // 点検情報読み込み
        public static SystemData readTenken(SystemData systemData, string fileName) {
            return readTenken(systemData, fileName, true);
        }

        public static SystemData readTenken(SystemData systemData, string fileName, bool readData) {
            try {
                lock (DataManager.sDataLock) {
                    string data = null;
                    systemData = getTenkenYM(systemData);
                    string path = Common.getSystemPath();
                    if (fileName == null) {
                        fileName = systemData.tenkenFileName;
                    }
                    if (Common.isEmpty(fileName)) {
                        fileName = makeTenkenFileName(systemData);
                    }
                    systemData.tenkenFileName = fileName;
                    // ファイルの有無を調べる
                    // 月替わり処理
                    if (!File.Exists(path + "/" + fileName) && readData) { // ファイルがない場合は前回データを取得してデータを生成する
                        SystemData systemDataBakup = systemData.clone();
                        systemData = getLastData(systemData, TENEKN_DATA_READ_MODE_THIS_MONTH);
                        if (systemData == null)
                            systemData = systemDataBakup;
                        // 編集したデータかどうかを入れるフラグ初期化()
                        systemData.tenkenData.textKakoSiyouDenryokuDataEdit = false;
                        systemData.tenkenData.textKakoDemandDataEdit = false;
                        systemData.tenkenData.textKakoTukiEdit = false;
                        systemData.tenkenData.ZennenDougetuSiyouDenryokuEdit = false;
                        systemData.tenkenData.ZennenDougetuDemandEdit = false;
                        systemData.tenkenData.ZennenDougetuTukiEdit = false;
                        if (systemData.settei.flagRikirituSettei == true) { // 力率自動計算に設定されている場合は、今月力率を消す
                            systemData.tenkenData.itemKensinKongetuRikiritu.text = "";
                        }
                        // 月替わり処理
                        systemData.tenkenData = makeNewTenkenData(systemData.tenkenData);
                        // 推移表を作成する 新規データ構築の時だけ推移表を作成する 既存データを読み込むときにこれをやると無限ループになる
                        systemData.tenkenData = makeKakoSuiiData(systemData, false);
                        setKensinYMD(systemData);
                    } else { // 当月のデータを読み込む
                        data = loadText(fileName);
                        if (string.IsNullOrEmpty(data)) {
                            deleteFile(fileName);
                            systemData.tenkenData = new TenkenData();
                            return systemData; // 初回起動時で点検データがない場合は生成する
                        }
                        systemData.tenkenData = JsonSerializer.Deserialize<TenkenData>(data);
                        if (systemData.tenkenData == null) {
                            systemData.tenkenData = new TenkenData();
                        }
                    }
                }
            } catch (IOException e) {
                //e.printStackTrace();
                systemData.tenkenData = new TenkenData();
            } catch (Exception e) {
                //Log.e(TAG, "Unable to read to file [readTenken]");
            }
            return systemData; // 初回起動時で点検データがない場合は生成する
        }

        // 点検情報書込み
        public static void writeTenken(SystemData systemData, string fileName) {
            try {
                if (fileName == null)
                    return;
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(systemData.tenkenData);//["shika",koala]
                    // ファイル書き込み
                    saveText(stringJson, fileName);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file[writeTenken]");
            }
        }

        public static void renameFileName(SystemData systemData, string oldTitle, string oldSubTitle,
                string newTitle, string newSubTitle) {
            if (systemData.mode == 1) { // 編集モードの場合
                string oldFileName = oldTitle + "_" + oldSubTitle;
                string newFileName = newTitle + "_" + newSubTitle;
                File.Copy(oldFileName, newFileName);
            }
        }

        public static int checkSupportersPassword(int status) {
            string password = readSupportersPassword();
            if (CheckPassword.checkPassword(password)) {
                status = 0;
            }
            return status;
        }

        // 事業所情報読み込み
        public static string readSupportersPassword() {
            string password = "";
            try {
                lock (DataManager.sDataLock) {
                    try {
                        password = loadText(FileNameSurpportersPassword);
                    } catch (IOException e) {
                        return e.StackTrace;
                    }
                }
            } catch (Exception e) {
                // Log.e(TAG, "Unable to load to file");
            }
            return password;
        }

        // 事業所情報書込み
        public static void writeSupportersPassword(string password) {
            try {
                lock (DataManager.sDataLock) {
                    // ファイル書き込み
                    saveText(Common.getSystemPath(), FileNameSurpportersPassword);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file");
            }
        }

        // MACADDRESS書込み
        public static void writeDeviceInformation() {
            try {
                lock (DataManager.sDataLock) {
                    // ファイル書き込み
                    //saveText("MACAddress : " + CheckPassword.getMACAddress() + "\n" +
                    //         "PrimaryAccount : " + Common.getPrimaryAccount() + "\n" +
                    //         "Version : " + Common.getVersion() + "\n" +
                    //         "OS Version : " + Common.getVersionCode() + " " + android.os.Build.VERSION.SDK_INT + "\n" +
                    //         "Model : " + android.os.Build.MODEL,
                    //         FileNameDeviceInformation);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file");
            }
        }

        public static SystemData getLastData(SystemData systemData, int mode) {
            SystemData retSystemData = null;
            try {
                lock (DataManager.sDataLock) {
                    SystemData newSystemData = systemData.clone();
                    //ファイルの一覧を検索するディレクトリパスを指定する
                    string path = Common.getSystemPath();
                    string fileNameAsterisk = makeTenkenFileNameAsterisk(newSystemData);
                    string[] files = Directory.GetFiles(path, fileNameAsterisk);
                    StringComparer cmp = StringComparer.OrdinalIgnoreCase;
                    Array.Sort(files, cmp);

                    foreach (string filename in files) {
                        string data = null;
                        data = loadText(filename);
                        if (string.IsNullOrEmpty(data) || data == "null") { // 空の時はnullが返ってくる
                            deleteFile(filename);
                            newSystemData.tenkenData = new TenkenData();
                            newSystemData.tenkenData = makeNewTenkenData(newSystemData.tenkenData);
                            return null; // 初回起動時で点検データがない場合は生成する
                        }
                        TenkenData? tenkenData = JsonSerializer.Deserialize<TenkenData>(data);
                        newSystemData.tenkenData = tenkenData;
                        // 読み込んだ月が今月だった場合
                        if (Common.makeYM(getText(newSystemData.tenkenData.itemTenkenNijtijiYMD)) == Common.makeYM(DateTime.Now)) {
                            continue;
                        }

                        // 読み込んだ月が点検対象月でなかった場合
                        //					newSystemData = getTenkenYM(newSystemData);
                        if (getGetujiTenkenInfo(newSystemData.jigyousyoData, getText(tenkenData.itemTenkenNijtijiYMD)) == 0) {
                            continue;
                        } else {
                            if (mode == TENEKN_DATA_READ_MODE_THIS_MONTH) {
                                retSystemData = newSystemData;
                                break;
                            } else {
                                // systemData の "systemData.tenkenData.itemTenkenNijtijiYMD" よりもひとつ古い点検データを取得する 20160314
                                if (Common.makeYM(getText(newSystemData.tenkenData.itemTenkenNijtijiYMD)).CompareTo(Common.makeYM(getText(systemData.tenkenData.itemTenkenNijtijiYMD))) < 0) {
                                    retSystemData = newSystemData;
                                    break;
                                }
                                // ここまで20160314
                            }
                        }
                    }
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file");
                return null; // ファイルが見つからない場合
            }
            return retSystemData;
        }

        public static SystemData getSameMonthLastYearData(SystemData systemData) {
            SystemData _systemData = systemData;
            try {
                lock (sDataLock) {
                    string data = null;
                    string path = Common.getSystemPath();
                    string serchFileName = makeTenkenFileNameSameMonthLastYear(_systemData);
                    data = loadText(serchFileName);
                    if (data == "null") {
                        deleteFile(serchFileName);
                        _systemData.tenkenData = new TenkenData();
                        _systemData.tenkenData = makeNewTenkenData(_systemData.tenkenData);
                        return null; // 初回起動時で点検データがない場合は生成する
                    }
                    _systemData.tenkenData = JsonSerializer.Deserialize<TenkenData>(data);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to load to file [getSameMonthLastYearData]");
                return null;
            }
            return _systemData;
        }

        // 点検情報書込み
        public static List<TenkenKasyoData> readTenkenKasyo(SystemData systemData) {
            try {
                lock (DataManager.sDataLock) {

                    string data = null;
                    string fileName = makeTenkenKasyoFileName(systemData);
                    // ファイルの有無を調べる
                    //File existsFile = new File(fileName);
                    try {
                        data = loadText(fileName);
                        if (string.IsNullOrEmpty(data)) {
                            deleteFile(fileName);
                            systemData.tenkenData = new TenkenData();
                            return systemData.listTenkenKasyo; // 初回起動時で点検データがない場合は生成する
                        }
                        systemData.listTenkenKasyo = JsonSerializer.Deserialize<List<TenkenKasyoData>>(data);
                    } catch (IOException e) {
                        //e.StackTrace;
                        return systemData.listTenkenKasyo; // 初回起動時で点検データがない場合は生成する
                    }
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to read to file [readTenkenKasyo]");
            }
            return systemData.listTenkenKasyo;
        }

        // 点検情報書込み
        public static void writeTenkenKasyo(SystemData systemData) {
            try {
                lock (DataManager.sDataLock) {
                    string fileName = makeTenkenKasyoFileName(systemData);
                    string stringJson = JsonSerializer.Serialize<List<TenkenKasyoData>>(systemData.listTenkenKasyo);//["shika",koala]
                                                                                                                    // ファイル書き込み
                    saveText(stringJson, fileName);
                }
            } catch (Exception e) {
                // Log.e(TAG, "Unable to write to file [writeTenkenKasyo]");
            }
        }

        public static string makeTenkenKasyoFileName(SystemData systemData) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string fileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_TenkenKasyo.json";
            return fileName;
        }

        //   public static FilenameFilter getFileExtensionFilter(string extension) {
        //       string _extension = extension;
        //       return new FilenameFilter() {
        //           public bool accept(File file, string name) {
        //               bool ret = name.endsWith(_extension);
        //               return ret;
        //           }
        //       };
        //}

        public static void renameFile(string fileFrom, string fileTo) {
            string path = Common.getSystemPath();
            if (File.Exists(path + fileFrom)) {
                deleteFile(fileTo);
            }
            try {
                File.Move(path + fileFrom, path + fileTo);
                //ファイル名変更成功
                //System.out.println("ファイル名変更成功");
            } catch (Exception e) {
                //ファイル名変更失敗
                //System.out.println("ファイル名変更失敗");
            }
        }

        // 点検データを新規作成する
        private static TenkenData makeNewTenkenData(TenkenData tenkenData) {
            TenkenData newTenkenData = tenkenData.clone();
            newTenkenData.itemTenkenNijtijiYMD.text = DataManager.makeYMD();
            newTenkenData.itemTenkenNijtijiTime.text = Common.makeTime();
            newTenkenData.itemGaiKion.text = "";
            newTenkenData.itemSitunaiKion.text = "";
            newTenkenData.itemSitudo.text = "";
            newTenkenData.itemTenkou.text = "";
            newTenkenData.itemSeiribangou.text = tenkenData.itemSeiribangou.text;
            newTenkenData.itemKensinSengetuHiduke.text = tenkenData.itemKensinKongetuHiduke.text;
            newTenkenData.itemKensinSengetu1.text = tenkenData.itemKensinKongetu1.text;
            newTenkenData.itemKensinSengetu2.text = tenkenData.itemKensinKongetu2.text;
            newTenkenData.itemKensinSengetu3.text = tenkenData.itemKensinKongetu3.text;
            newTenkenData.itemKensinSengetu4.text = tenkenData.itemKensinKongetu4.text;
            newTenkenData.itemKensinSengetuSaidaiDenryoku.text = tenkenData.itemKensinKongetuSaidaiDenryoku.text;
            newTenkenData.itemKensinSengetuGenzaiDenryoku.text = tenkenData.itemKensinKongetuGenzaiDenryoku.text;
            newTenkenData.itemKensinSengetuRikiritu.text = tenkenData.itemKensinKongetuRikiritu.text;
            newTenkenData.itemKensinSengetuYukoDenryoku.text = tenkenData.itemKensinKongetuYukoDenryoku.text;
            newTenkenData.itemKensinSengetuMukoDenryoku.text = tenkenData.itemKensinKongetuMukoDenryoku.text;
            newTenkenData.itemKensinKongetuHiduke.text = "";
            newTenkenData.itemKensinKongetuSaidaiDenryoku.text = "";
            newTenkenData.itemKensinKongetuGenzaiDenryoku.text = "";
            newTenkenData.itemKensinKongetuRikiritu.text = "";
            newTenkenData.itemKensinKongetuYukoDenryoku.text = "";
            newTenkenData.itemKensinKongetuMukoDenryoku.text = "";
            newTenkenData.itemKensinKongetu1.text = "";
            newTenkenData.itemKensinKongetu2.text = "";
            newTenkenData.itemKensinKongetu3.text = "";
            newTenkenData.itemKensinKongetu4.text = "";
            newTenkenData.itemGenzaiSengetuHiduke.text = tenkenData.itemGenzaiKongetuHiduke.text;
            newTenkenData.itemGenzaiSengetu1.text = tenkenData.itemGenzaiKongetu1.text;
            newTenkenData.itemGenzaiSengetu2.text = tenkenData.itemGenzaiKongetu2.text;
            newTenkenData.itemGenzaiSengetu3.text = tenkenData.itemGenzaiKongetu3.text;
            newTenkenData.itemGenzaiSengetu4.text = tenkenData.itemGenzaiKongetu4.text;
            newTenkenData.itemGenzaiSengetuSaidaiDenryoku.text = tenkenData.itemGenzaiKongetuSaidaiDenryoku.text;
            newTenkenData.itemGenzaiSengetuGenzaiDenryoku.text = tenkenData.itemGenzaiKongetuGenzaiDenryoku.text;
            newTenkenData.itemGenzaiSengetuRikiritu.text = tenkenData.itemGenzaiKongetuRikiritu.text;
            newTenkenData.itemGenzaiSengetuYukoDenryoku.text = tenkenData.itemGenzaiKongetuYukoDenryoku.text;
            newTenkenData.itemGenzaiSengetuMukoDenryoku.text = tenkenData.itemGenzaiKongetuMukoDenryoku.text;
            newTenkenData.itemGenzaiKongetuHiduke.text = "";
            newTenkenData.itemGenzaiKongetuSaidaiDenryoku.text = "";
            newTenkenData.itemGenzaiKongetuGenzaiDenryoku.text = "";
            newTenkenData.itemGenzaiKongetuRikiritu.text = "";
            newTenkenData.itemGenzaiKongetuYukoDenryoku.text = "";
            newTenkenData.itemGenzaiKongetuMukoDenryoku.text = "";
            newTenkenData.itemGenzaiKongetu1.text = "";
            newTenkenData.itemGenzaiKongetu2.text = "";
            newTenkenData.itemGenzaiKongetu3.text = "";
            newTenkenData.itemGenzaiKongetu4.text = "";
            // メーター交換の変数初期化
            newTenkenData.itemGenzaiKoukanAto1.text = "";
            newTenkenData.itemGenzaiKoukanAto2.text = "";
            newTenkenData.itemGenzaiKoukanAto3.text = "";
            newTenkenData.itemGenzaiKoukanAto4.text = "";
            newTenkenData.itemGenzaiKoukanMae1.text = "";
            newTenkenData.itemGenzaiKoukanMae2.text = "";
            newTenkenData.itemGenzaiKoukanMae3.text = "";
            newTenkenData.itemGenzaiKoukanMae4.text = "";
            newTenkenData.itemKensinKoukanAto1.text = "";
            newTenkenData.itemKensinKoukanAto2.text = "";
            newTenkenData.itemKensinKoukanAto3.text = "";
            newTenkenData.itemKensinKoukanAto4.text = "";
            newTenkenData.itemKensinKoukanMae1.text = "";
            newTenkenData.itemKensinKoukanMae2.text = "";
            newTenkenData.itemKensinKoukanMae3.text = "";
            newTenkenData.itemKensinKoukanMae4.text = "";
            // 電力量等は、前月のデータを引き継ぐ
            newTenkenData.itemKensinbi.text = tenkenData.itemKensinbi.text;
            newTenkenData.itemJyouritu.text = tenkenData.itemJyouritu.text;
            newTenkenData.itemKeiyakuDenryoku.text = tenkenData.itemKeiyakuDenryoku.text;
            newTenkenData.itemKeiyakuDenryokuKakuteiYM.text = tenkenData.itemKeiyakuDenryokuKakuteiYM.text;
            newTenkenData.itemSetubiYouryou.text = tenkenData.itemSetubiYouryou.text;
            return newTenkenData;
        }

        // 点検データを新規作成する
        public static TenkenData makeKakoSuiiData(SystemData systemData, bool make) {
            // 過去の推移データが１件でも登録済みなら推移データを１ヵ月ずつずらす処理
            bool hit = false;
            for (int i = 0; i < systemData.tenkenData.textKakoTuki.Length; i++) {
                if (systemData.tenkenData.textKakoTuki != null &&
                   systemData.tenkenData.textKakoTuki[i] != null &&
                   string.IsNullOrEmpty(systemData.tenkenData.textKakoTuki[i]) == false) {
                    hit = true;
                    break;
                }
            }
            if (hit == true && make == false) {
                // 過去データを1ヵ月ずつずらす
                for (int i = systemData.tenkenData.textKakoTuki.Length - 1; 0 < i; i--) {
                    systemData.tenkenData.textKakoTuki[i] = systemData.tenkenData.textKakoTuki[i - 1];
                    systemData.tenkenData.textKakoSiyouDenryokuData[i] = systemData.tenkenData.textKakoSiyouDenryokuData[i - 1];
                    systemData.tenkenData.textKakoDemandData[i] = systemData.tenkenData.textKakoDemandData[i - 1];
                }

                SystemData newSystemData = systemData.clone();
                if (systemData.tenkenData.flgSuiihyouKongetuStart == false) { // 先月から開始モード
                    DateTime calendar = Common.makePrevYMD(1);
                    systemData.tenkenData.textKakoTuki[0] = Common.makeYYMM(calendar);
                    string path = Common.getSystemPath();
                    string fileName = makeTenkenFileNameYM(newSystemData, calendar);
                    newSystemData = readTenken(newSystemData, fileName, false);
                    systemData.tenkenData.textKakoSiyouDenryokuData[0]
                            = Common.formatDecimal(getText(newSystemData.tenkenData.itemKensinDenryokuryou));
                    string saidaiDenryoku = Common.getMultiply(
                            newSystemData.tenkenData.itemKensinKongetuSaidaiDenryoku.text,
                            newSystemData.tenkenData.itemJyouritu.text, 0);
                    saidaiDenryoku = Common.round(saidaiDenryoku, Common.ROUND_OFF);
                    saidaiDenryoku = Common.formatDecimal(saidaiDenryoku);
                    systemData.tenkenData.textKakoDemandData[0] = saidaiDenryoku;
                } else { // 今月からスタート
                         //				if(systemData.tenkenData.flgGraphScale){
                    systemData.tenkenData.textKakoTuki[0] = Common.makeNenGetsu(getText(systemData.tenkenData.itemTenkenNijtijiYMD));
                    //				}else{
                    //					systemData.tenkenData.textKakoTuki[0] = Common.makeGetsu(getText(systemData.tenkenData.itemTenkenNijtijiYMD));
                    //				}
                    systemData.tenkenData.textKakoSiyouDenryokuData[0]
                            = Common.formatDecimal(Common.round(getText(newSystemData.tenkenData.itemKensinDenryokuryou), Common.ROUND_UP));
                    string saidaiDenryoku = Common.getMultiply(
                            newSystemData.tenkenData.itemKensinKongetuSaidaiDenryoku,
                            newSystemData.tenkenData.itemJyouritu, 0);
                    saidaiDenryoku = Common.round(saidaiDenryoku, Common.ROUND_OFF);
                    saidaiDenryoku = Common.formatDecimal(saidaiDenryoku);
                    systemData.tenkenData.textKakoDemandData[0] = saidaiDenryoku;
                }
            } else { // 推移表にデータが１件も無い場合は、過去のデータファイルを読み込んで推移表を作成する
                SystemData newSystemData = systemData.clone();
                string monthTarget;
                int index = 0;
                int indexSuiihyouKongetuStart = 0;
                //ファイルの一覧を検索するディレクトリパスを指定する
                string path = Common.getSystemPath();
                string fileName = makeTenkenFileNameAsterisk(newSystemData);
                string[] files = Directory.GetFiles(path, fileName);
                Array.Sort(files);
                Array.Reverse(files);

                //対象事業場の過去のファイル名のリスト取得する
                foreach (string file in files) {
                    newSystemData = readTenken(newSystemData, file, false);

                    execDenryokuryou(newSystemData);
                    execDenryokuryouHeikin(newSystemData);

                    // 推移表が「今月も含めた推移」か「先月からの推移」か？
                    if (systemData.tenkenData.flgSuiihyouKongetuStart == false) { // 先月から開始モード
                                                                                  // 先月からの推移であれば、今月のデータは推移表に設定しない
                        if (Common.makeYM(getText(newSystemData.tenkenData.itemTenkenNijtijiYMD)) ==
                            Common.makeYM(getText(systemData.tenkenData.itemTenkenNijtijiYMD)) && index == 0) {
                            //index++;
                            continue;
                        } else {
                        }
                    } else { // 今月データからの推移表開始モード
                             // 今月分のデータがあれば今月データを配列の先頭にセットする
                        if (Common.makeYM(getText(newSystemData.tenkenData.itemTenkenNijtijiYMD)) != (
                            Common.makeYM(getText(systemData.tenkenData.itemTenkenNijtijiYMD))) && index == 0) {
                            systemData.tenkenData.textKakoTuki[index] = Common.makeNenGetsu(getText(systemData.tenkenData.itemTenkenNijtijiYMD));
                            systemData.tenkenData.textKakoSiyouDenryokuData[index] = "";
                            systemData.tenkenData.textKakoDemandData[index] = "";
                            index++;
                        } else { // 今月データからのスタートで、今月分のデータがなければ配列の先頭は空のままスキップする
                        }
                    }
                    monthTarget = Common.getMonth(getText(newSystemData.tenkenData.itemTenkenNijtijiYMD));
                    if (systemData.jigyousyoData.isTukitenken(monthTarget) == true) {
                        systemData.tenkenData.textKakoTuki[index] = Common.makeNenGetsu(getText(newSystemData.tenkenData.itemTenkenNijtijiYMD));

                        if (systemData.tenkenData.flgSuiihyouDenryokuryou == false) { // 月使用電力量が選択されている
                            if (systemData.tenkenData.itemCheckboxYusen.text == "1") { // 確定値をセットする
                                systemData.tenkenData.textKakoSiyouDenryokuData[index]
                                        = Common.formatDecimal(Common.round(getText(newSystemData.tenkenData.itemKensinDenryokuryou), Common.ROUND_UP));
                            } else { // 現在値をセットする
                                systemData.tenkenData.textKakoSiyouDenryokuData[index]
                                        = Common.formatDecimal(Common.round(getText(newSystemData.tenkenData.itemGenzaiDenryokuryou), Common.ROUND_UP));
                            }
                        } else { // 一日平均電力量が選択されている
                            if (systemData.tenkenData.itemCheckboxYusen.text == "1") { // 確定値をセットする
                                Item kensinNissuu = new Item(Common.getNissu(newSystemData.tenkenData.itemKensinKongetuHiduke.text, newSystemData.tenkenData.itemKensinSengetuHiduke.text),
                                        newSystemData.tenkenData.itemKensinSengetuHiduke.color,
                                        newSystemData.tenkenData.itemKensinSengetuHiduke.style,
                                        newSystemData.tenkenData.itemKensinSengetuHiduke.bgcolor);
                                systemData.tenkenData.textKakoSiyouDenryokuData[index]
                                        = Common.formatInteger(Common.getDivide(newSystemData.tenkenData.itemKensinDenryokuryou.text, kensinNissuu.text, 2));
                            } else { // 現在値をセットする
                                Item kensinNissuu = new Item(Common.getNissu(newSystemData.tenkenData.itemGenzaiKongetuHiduke.text, newSystemData.tenkenData.itemGenzaiSengetuHiduke.text),
                                        newSystemData.tenkenData.itemGenzaiSengetuHiduke.color,
                                        newSystemData.tenkenData.itemGenzaiSengetuHiduke.style,
                                        newSystemData.tenkenData.itemGenzaiSengetuHiduke.bgcolor);
                                systemData.tenkenData.textKakoSiyouDenryokuData[index]
                                        = Common.formatInteger(Common.getDivide(newSystemData.tenkenData.itemGenzaiDenryokuryou.text, kensinNissuu.text, 2));
                            }
                        }
                        string saidaiDenryoku = Common.getMultiply(
                        newSystemData.tenkenData.itemKensinKongetuSaidaiDenryoku,
                                newSystemData.tenkenData.itemJyouritu, 0);
                        saidaiDenryoku = Common.round(saidaiDenryoku, Common.ROUND_OFF);
                        saidaiDenryoku = Common.formatDecimal(saidaiDenryoku);
                        systemData.tenkenData.textKakoDemandData[index] = saidaiDenryoku;
                        index++;
                        if (index == systemData.tenkenData.textKakoDemandData.Length) {
                            break;
                        }
                    }
                }
                if (systemData.tenkenData.flgSuiihyouKongetuStart == false) { // 先月から開始モード
                    indexSuiihyouKongetuStart = 1;
                }
                // 点検未実施でファイルがない月の年月を登録する
                for (; index < systemData.tenkenData.textKakoDemandData.Length; index++) {
                    DateTime calendar = Common.makePrevYMD(index + indexSuiihyouKongetuStart);
                    systemData.tenkenData.textKakoTuki[index] = Common.makeYYMM(calendar);
                }
            }
            // TODO:そもそも既存の過去のデータに上書きして良いのか？
            // 今月分のデータを前回データから継承する処理を行ってからやるべきではないか？
            systemData.tenkenData.flgTenkenFinish = false;

            return systemData.tenkenData;
        }

        public static string getText(Item item) {
            if (item == null || item.text == null) {
                return "";
            }
            return item.text;
        }

        public static SystemData createKensinData(SystemData systemData) {
            if (isEmpty(systemData.tenkenData.itemKensinKongetuHiduke)) {
                return systemData;
            }
            string dateKensin = systemData.tenkenData.itemKensinKongetuHiduke.text;
            if (isNull(dateKensin) || dateKensin == "") {
                return systemData;
            }
            // データ読込み・または生成
            systemData.tenkenData.textKensinNijtijiYear = dateKensin.Substring(0, dateKensin.IndexOf("年"));
            systemData.tenkenData.textKensinNijtijiMonth = dateKensin.Substring(dateKensin.IndexOf("年") + 1,
                    dateKensin.IndexOf("月"));
            systemData.tenkenData.textKensinNijtijiDay = dateKensin.Substring(dateKensin.IndexOf("月") + 1,
                    dateKensin.IndexOf("日"));
            return systemData;
        }

        public static SystemData setKongetuSuiiData(SystemData systemData) {
            if (systemData.tenkenData.flgSuiihyouKongetuStart == false)
                return systemData;
            string jyouritu = systemData.tenkenData.itemJyouritu.text;

            if (!isNull(systemData.tenkenData.itemKensinKongetuHiduke) && Common.isEmpty(systemData.tenkenData.textKakoTuki[0])) {
                systemData.tenkenData.textKakoTuki[0] = toYYYYMM(systemData.tenkenData.itemKensinKongetuHiduke.text);
            }
            if (!isEmpty(systemData.tenkenData.itemKensinKongetuSaidaiDenryoku)
                    &&
                    (Common.isEmpty(systemData.tenkenData.textKakoDemandData[0]) || isZero(systemData.tenkenData.textKakoDemandData[0]))) {
                systemData.tenkenData.textKakoDemandData[0] = Common.getMultiply(systemData.tenkenData.itemKensinKongetuSaidaiDenryoku.text, jyouritu, 0);
            }

            if (systemData.tenkenData.flgSuiihyouDenryokuryou == false) { // 月使用電力量が選択されている
                if (!isEmpty(systemData.tenkenData.itemKensinDenryokuryou)) {
                    if (systemData.tenkenData.itemCheckboxYusen.text == "1") {
                        systemData.tenkenData.textKakoSiyouDenryokuData[0] = systemData.tenkenData.itemKensinDenryokuryou.text;
                    } else {
                        systemData.tenkenData.textKakoSiyouDenryokuData[0] = systemData.tenkenData.itemGenzaiDenryokuryou.text;
                    }
                }
            } else { // 一日平均電力量が選択されている
                if (!isEmpty(systemData.tenkenData.itemKensinHeikinDenryokuryou)) {
                    if (systemData.tenkenData.itemCheckboxYusen.text == "1") {
                        systemData.tenkenData.textKakoSiyouDenryokuData[0] = systemData.tenkenData.itemKensinHeikinDenryokuryou.text;
                    } else {
                        systemData.tenkenData.textKakoSiyouDenryokuData[0] = systemData.tenkenData.itemGenzaiHeikinDenryokuryou.text;
                    }
                }
            }

            return systemData;
        }

        public static SystemData setSuiiData(SystemData systemData, SystemData orgSystemData, int index) {
            string jyouritu = systemData.tenkenData.itemJyouritu.text;

            if (!isNull(orgSystemData.tenkenData.itemKensinKongetuHiduke)
                    && Common.isEmpty(systemData.tenkenData.textKakoTuki[index])) {
                systemData.tenkenData.textKakoTuki[index] = toYYYYMM(orgSystemData.tenkenData.itemKensinKongetuHiduke.text);
            }
            if (!isNull(orgSystemData.tenkenData.itemKensinKongetuSaidaiDenryoku) &&
                    (Common.isEmpty(systemData.tenkenData.textKakoDemandData[index]) || isZero(systemData.tenkenData.textKakoDemandData[index]))) {
                systemData.tenkenData.textKakoDemandData[index] = Common.getMultiply(orgSystemData.tenkenData.itemKensinKongetuSaidaiDenryoku.text, jyouritu, 0);
            }
            execDenryokuryou(systemData);
            execDenryokuryouHeikin(systemData);
            if (systemData.tenkenData.flgSuiihyouDenryokuryou == false) { // 月使用電力量が選択されている
                if (!isNull(systemData.tenkenData.itemKensinDenryokuryou)
                        &&
                        (Common.isEmpty(systemData.tenkenData.textKakoSiyouDenryokuData[index]) || isZero(systemData.tenkenData.textKakoSiyouDenryokuData[index]))) {
                    if (systemData.tenkenData.itemCheckboxYusen.text == "1") {
                        systemData.tenkenData.textKakoSiyouDenryokuData[index] = systemData.tenkenData.itemKensinDenryokuryou.text;
                    } else {
                        systemData.tenkenData.textKakoSiyouDenryokuData[index] = systemData.tenkenData.itemGenzaiDenryokuryou.text;
                    }
                }
            } else { // 一日平均電力量が選択されている
                if (!isNull(systemData.tenkenData.itemKensinHeikinDenryokuryou)
                        &&
                        (Common.isEmpty(systemData.tenkenData.textKakoSiyouDenryokuData[index]) || isZero(systemData.tenkenData.textKakoSiyouDenryokuData[index]))) {
                    if (systemData.tenkenData.itemCheckboxYusen.text == "1") {
                        systemData.tenkenData.textKakoSiyouDenryokuData[index] = systemData.tenkenData.itemKensinHeikinDenryokuryou.text;
                    } else {
                        systemData.tenkenData.textKakoSiyouDenryokuData[index] = systemData.tenkenData.itemGenzaiHeikinDenryokuryou.text;
                    }
                }
            }
            return systemData;
        }

        public static string toYYYYMM(string str) {
            DateTime date;
            if (DateTime.TryParseExact(str, "yyyy年MM月dd日", null, DateTimeStyles.AssumeLocal, out date)) {
                return "";
            }
            return date.ToString("yyyy年MM月");
        }

        public static string getYakanritu(SystemData systemData, BaseKeiyaku keiyaku) {
            decimal? kensin1 = null;
            decimal? kensin2 = null;
            decimal? kensin3 = null;
            decimal? kensin4 = null;
            decimal? goukei = 0;

            // 検針値と現在値のどちらを印刷するか？
            if (equals(systemData.tenkenData.itemCheckboxYusen, "2")) { // 現在値を印刷する
                if (isEmpty(systemData.tenkenData.itemGenzaiSengetu1) || isEmpty(systemData.tenkenData.itemGenzaiSengetu2) ||
                           isEmpty(systemData.tenkenData.itemGenzaiSengetu3) || isEmpty(systemData.tenkenData.itemGenzaiSengetu4)) {
                    return "";
                }
                if (isNumeric(systemData.tenkenData.itemGenzaiKongetu1)) {
                    kensin1 = decimal.Parse(systemData.tenkenData.itemGenzaiKongetu1.text);
                    kensin1 = kensin1 - decimal.Parse(systemData.tenkenData.itemGenzaiSengetu1.text);
                    goukei = goukei + kensin1;
                }
                if (isNumeric(systemData.tenkenData.itemGenzaiKongetu2)) {
                    kensin2 = decimal.Parse(systemData.tenkenData.itemGenzaiKongetu2.text);
                    kensin2 = kensin2 - decimal.Parse(systemData.tenkenData.itemGenzaiSengetu2.text);
                    goukei = goukei + kensin2;
                }
                if (isNumeric(systemData.tenkenData.itemGenzaiKongetu3)) {
                    kensin3 = decimal.Parse(systemData.tenkenData.itemGenzaiKongetu3.text);
                    kensin3 = kensin3 - (decimal.Parse(systemData.tenkenData.itemGenzaiSengetu3.text));
                    goukei = goukei + kensin3;
                }
                if (isNumeric(systemData.tenkenData.itemGenzaiKongetu4)) {
                    kensin4 = decimal.Parse(systemData.tenkenData.itemGenzaiKongetu4.text);
                    kensin4 = kensin4 - decimal.Parse(systemData.tenkenData.itemGenzaiSengetu4.text);
                    goukei = goukei + kensin4;
                }
            } else {
                if (isEmpty(systemData.tenkenData.itemKensinSengetu1) || isEmpty(systemData.tenkenData.itemKensinSengetu2) ||
                           isEmpty(systemData.tenkenData.itemKensinSengetu3) || isEmpty(systemData.tenkenData.itemKensinSengetu4)) {
                    return "";
                }
                if (isNumeric(systemData.tenkenData.itemKensinKongetu1)) {
                    kensin1 = decimal.Parse(systemData.tenkenData.itemKensinKongetu1.text);
                    kensin1 = kensin1 - decimal.Parse(systemData.tenkenData.itemKensinSengetu1.text);
                    goukei = goukei + kensin1;
                }
                if (isNumeric(systemData.tenkenData.itemKensinKongetu2)) {
                    kensin2 = decimal.Parse(systemData.tenkenData.itemKensinKongetu2.text);
                    kensin2 = kensin2 - decimal.Parse(systemData.tenkenData.itemKensinSengetu2.text);
                    goukei = goukei + kensin2;
                }
                if (isNumeric(systemData.tenkenData.itemKensinKongetu3)) {
                    kensin3 = decimal.Parse(systemData.tenkenData.itemKensinKongetu3.text);
                    kensin3 = kensin3 - decimal.Parse(systemData.tenkenData.itemKensinSengetu3.text);
                    goukei = goukei + kensin3;
                }
                if (isNumeric(systemData.tenkenData.itemKensinKongetu4)) {
                    kensin4 = decimal.Parse(systemData.tenkenData.itemKensinKongetu4.text);
                    kensin4 = kensin4 - decimal.Parse(systemData.tenkenData.itemKensinSengetu4.text);
                    goukei = goukei + kensin4;
                }
            }
            decimal? yakanritu = null;

            decimal multiply100 = 100;
            try {
                if (keiyaku.getSyubetu() == 2) { // 夜間率計算
                    yakanritu = Common.Round((decimal)(kensin4 / goukei), 2);
                    yakanritu = yakanritu * multiply100;
                } else if (keiyaku.getSyubetu() == 3) {
                    decimal kyujitu = 0;
                    kyujitu = (decimal)(kensin2 + kensin4);
                    yakanritu = Common.Round((decimal)(kyujitu / goukei), 2);
                    yakanritu = yakanritu * multiply100;
                }
            } catch (Exception e) {
                return "";
            }
            if (yakanritu != null) {
                return yakanritu.ToString();
            } else {
                return "";
            }
        }

        // 点検データを新規作成する
        public static SystemData remakeTenkenData(SystemData systemData) {
            TenkenData newTenkenData = systemData.tenkenData;
            //		for(int i=0; i<systemData.tenkenData.textKakoSiyouDenryokuData.length-1; i++){
            //			newTenkenData.textKakoTuki[i+1] = systemData.tenkenData.textKakoTuki[i];
            //			newTenkenData.textKakoSiyouDenryokuData[i+1] = systemData.tenkenData.textKakoSiyouDenryokuData[i];
            //		}
            if (systemData.tenkenData.flgSuiihyouKongetuStart == true) {
                string jyouritu = systemData.tenkenData.itemJyouritu.text;
                // 今月分も含む推移であれば、今月のデータを推移表に設定する
                newTenkenData.textKakoTuki[0] = systemData.tenkenYM;
                newTenkenData.textKakoSiyouDenryokuData[0] = systemData.tenkenData.itemKensinDenryokuryou.text;
                newTenkenData.textKakoDemandData[0] = Common.getMultiply(systemData.tenkenData.itemKensinKongetuSaidaiDenryoku.text, jyouritu, 0);
            }
            systemData.tenkenData = newTenkenData;
            return systemData;
        }

        public static string getNowYM() {
            return DateTime.Now.ToString("yyyy_MM");
        }

        //public static string getSDCardPath() {
        //    return Environment.getExternalStorageDirectory().tostring() + "/geppou/cyouhyou";
        //}

        public static SystemData getTenkenYM(SystemData systemData) {
            // TODO:今月の作業を完了した場合は次月を設定するように機能を追加する
            systemData.tenkenYM = DateTime.Now.ToString("yyyy/MM");
            return systemData;
        }

        public static string makeYMD() {
            // TODO:今月の作業を完了した場合は次月を設定するように機能を追加する
            return Common.makeYMD();
            // return Common.makeNextYMD();
        }

        public static string makeTenkenFileNameYM(SystemData systemData, DateTime calendar) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_" + Common.makeYMD2(calendar) + ".dat";
            TenkenFileName = TenkenFileName.Replace("(", "（").Replace(")", "）").Replace("{", "｛").Replace("}", "｝")
                                           .Replace("[", "［").Replace("]", "］").Replace("-", "－");
            return TenkenFileName;
        }

        public static string makeTenkenFileNameYM(string title, string subtitle) {
            DateTime dateTime = DateTime.Now;
            string TenkenFileName = title + "_" + subtitle + "_" + Common.makeYMD2(dateTime) + ".dat";
            TenkenFileName = TenkenFileName.Replace("(", "（").Replace(")", "）").Replace("{", "｛").Replace("}", "｝")
                                           .Replace("[", "［").Replace("]", "］").Replace("-", "－");
            return TenkenFileName;
        }

        public static string makeTenkenFileNameAsterisk(SystemData systemData) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_*" + ".dat";
            TenkenFileName = TenkenFileName.Replace("(", "（").Replace(")", "）").Replace("{", "｛").Replace("}", "｝")
                                           .Replace("[", "［").Replace("]", "］").Replace("-", "－");
            return TenkenFileName;
        }


        public static string makeTenkenImageFileNameAsterisk(SystemData systemData) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_*" + ".png";
            TenkenFileName = TenkenFileName.Replace("(", "（").Replace(")", "）").Replace("{", "｛").Replace("}", "｝")
                                           .Replace("[", "［").Replace("]", "］").Replace("-", "－");
            return TenkenFileName;
        }

        public static string makeTenkenFileNameSameMonthLastYear(SystemData systemData) {
            string[] param;
            string thisMonth = "";
            string sameMonthLastYear = "";
            string TenkenFileName = "";
            if (systemData.tenkenFileName != null) {
                param = Common.getPreffix(systemData.tenkenFileName).Split("_");
                if (0 < param.Length) {
                    thisMonth = param[2];
                    DateTime calendar;
                    calendar = new DateTime(int.Parse(thisMonth.Substring(0, 4)), int.Parse(thisMonth.Substring(4, 6)), 1);
                    sameMonthLastYear = calendar.AddYears(-1).ToString("yyyyMMdd");
                    JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
                    TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_"
                            + sameMonthLastYear + ".dat";
                }
            }
            return TenkenFileName;
        }

        public static string makeTenkenFileName(SystemData systemData) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_"
                    + systemData.tenkenYM + ".dat";
            return TenkenFileName;
        }

        public static string makeTenkenImageFileName(SystemData systemData) {
            // 既存のファイルリストを取得する
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_";
            string path = Common.getSystemPath();
            string fileName = DataManager.makeTenkenImageFileNameAsterisk(systemData);
            string[] files = Directory.GetFiles(path, fileName);
            Array.Sort(files, StringComparer.OrdinalIgnoreCase);
            if (files == null || files.Length == 0) {
                return TenkenFileName + "00000.png";
            }
            string lastNumber = files[0].Substring(TenkenFileName.Length, files[0].Length - 4);
            int nextNumber = 0;
            if (isNumeric(lastNumber) == true) {
                nextNumber = int.Parse(lastNumber) + 1;
            }

            return TenkenFileName + string.Format("%1$05d", nextNumber) + ".png";
        }

        public static string makeLastYearTenkenFileName(SystemData systemData) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_"
                    + toLastYYYYMM(systemData.tenkenYM) + ".dat";
            return TenkenFileName;
        }

        // 前年同月を取得する
        public static string toLastYYYYMM(string str) {
            DateTime date = DateTime.Parse(str);
            date = date.AddYears(-1);
            return date.ToString("yyyyMM");
        }

        public static string makeTenkenFileNameforSerch(SystemData systemData) {
            JigyousyoData jigyousyoData = systemData.listJigyousyo[systemData.positionJigyousyo];
            string TenkenFileName = jigyousyoData.getTextTitle() + "_" + jigyousyoData.getTextSubTitle() + "_*" + ".dat";
            return TenkenFileName;
        }

        // 天候情報読み込み
        public static List<TenkouData> readTenkou() {
            List<TenkouData> TenkouData = null;
            string data = null;
            string error;
            try {
                data = loadText(FileNameTenkou);
                if (data == null || data == "null" || data == "") {
                    deleteFile(FileNameTenkou);
                    return null;
                }
                TenkouData = JsonSerializer.Deserialize<List<TenkouData>>(data);
            } catch (IOException e) {
                //error = e.StackTrace();
                return null;
            }
            List<TenkouData> retList = new List<TenkouData>();
            foreach (TenkouData jigyoujyou in TenkouData) {
                retList.Add(jigyoujyou);
            }
            return retList;
        }

        // 天候情報書込み
        public static void writeTenkou(List<TenkouData> list) {
            try {
                lock (DataManager.sDataLock) {
                    if (list != null) {
                        string stringJson = JsonSerializer.Serialize(list); //["shika",koala]
                                                                            // ファイル書き込み
                        saveText(stringJson, FileNameTenkou);
                    }
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [writeTenkou]");
            }
        }

        // 点検箇所情報読み込み
        //	public static List<TenkenKasyoData> readTenkenKasyo(Activity activity, SystemData systemData){
        //		try{
        //			List<TenkenKasyoData> retList = new List<TenkenKasyoData>();
        //			synchronized (FilesBackupAgent.sDataLock) {
        //				List<TenkenKasyoData> tenkenKasyoData = null;
        //				Type listOfTenkouData = new TypeToken<List<TenkenKasyoData>>(){}.getType();
        //				string data = null;
        //				Gson gson = new Gson();
        //				try {
        //					data = loadText(activity, FileNameTenkenKasyo);
        //					if(data.equals("null")){
        //						deleteFile(activity, FileNameTenkenKasyo);
        //						return null;
        //					}
        //					tenkenKasyoData = Collections.synchronizedList((List<TenkenKasyoData>)gson.fromJson(data, listOfTenkouData));
        //				} catch (IOException e) {
        //					e.printStackTrace();
        //					for(string tenkenKasyo : defaultTenkenKasyo){
        //						TenkenKasyoData jigyoujyou = new TenkenKasyoData(new Item(tenkenKasyo), 0, 0, 0);
        //						retList.add(jigyoujyou);
        //					}
        //					return retList;
        //				}
        //				for(TenkenKasyoData jigyoujyou : tenkenKasyoData){
        //					retList.add(jigyoujyou);
        //				}
        //			}
        //			systemData.listTenkenKasyo = retList;
        //		} catch (Exception e) {
        //		    Log.e(TAG, "Unable to write to file [writeTenkou]");
        //	    }
        //		return systemData.listTenkenKasyo;
        //	}

        // 点検箇所情報書込み
        public static void writeTenkenKasyo(List<TenkenKasyoData> list, int dummy) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(list); //["shika",koala]
                                                                 // ファイル書き込み
                    saveText(stringJson, FileNameTenkenKasyo);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [writeTenkou]");
            }
        }

        // 点検記号情報読み込み
        public static List<TenkenKigouData> readTenkenKigou(SystemData systemData) {
            List<TenkenKigouData> retList = new List<TenkenKigouData>();
            try {
                lock (DataManager.sDataLock) {
                    List<TenkenKigouData> tenkenKigouData = null;
                    string data = null;
                    try {
                        data = loadText(FileNameTenkenKigou);
                        if (data == "null") {
                            deleteFile(FileNameTenkenKigou);
                            return null;
                        }
                        tenkenKigouData = JsonSerializer.Deserialize<List<TenkenKigouData>>(data);
                    } catch (IOException e) {
                        //e.printStackTrace();
                        return null;
                    }
                    foreach (TenkenKigouData jigyoujyou in tenkenKigouData) {
                        retList.Add(jigyoujyou);
                    }
                    systemData.listTenkenKigou = retList;
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to read to file [readTenkenKigou]");
            }
            return systemData.listTenkenKigou;
        }

        // 点検記号情報書込み
        public static void writeTenkenKigou(List<TenkenKigouData> list) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(list); //["shika",koala]
                                                                 // ファイル書き込み
                    saveText(stringJson, FileNameTenkenKigou);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [writeTenkenKigou]");
            }
        }

        // 点検記号情報読み込み
        public static List<TeikeibunData> readTenkenTeikeibun(SystemData systemData) {
            try {
                lock (DataManager.sDataLock) {
                    List<TeikeibunData> retList = new List<TeikeibunData>();
                    List<TeikeibunData> tenkenKigouData = null;
                    string data = null;
                    try {
                        data = loadText(FileNameTenkenKigou);
                        if (data == "null") {
                            deleteFile(FileNameTenkenKasyo);
                            return null;
                        }
                        tenkenKigouData = JsonSerializer.Deserialize<List<TeikeibunData>>(data);
                    } catch (IOException e) {
                        //e.printStackTrace();
                        return null;
                    }
                    foreach (TeikeibunData jigyoujyou in tenkenKigouData) {
                        retList.Add(jigyoujyou);
                    }
                    systemData.listTenkenTeikeibun = retList;
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to read to file [readTenkenTeikeibun]");
            }
            return systemData.listTenkenTeikeibun;
        }

        // 点検記号情報書込み
        public static void writeTenkenTeikeibun(List<TeikeibunData> list) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(list); //["shika",koala]
                                                                 // ファイル書き込み
                    saveText(stringJson, FileNameTenkenKigou);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [writeTenkenTeikeibun]");
            }
        }

        // 点検者情報読み込み
        public static TenkensyaData readTenkensya(SystemData systemData) {
            TenkensyaData tenkensyaData = null;
            try {
                lock (DataManager.sDataLock) {
                    string data = string.Empty;
                    try {
                        data = loadText(FileNameTenkensya);
                        if (data == null) {
                            deleteFile(FileNameTenkensya);
                            return new TenkensyaData("", 0, 0, 0);
                        }
                        tenkensyaData = (TenkensyaData)JsonSerializer.Deserialize<TenkensyaData>(data);
                    } catch (IOException e) {
                        //e.printStackTrace();
                        return new TenkensyaData("", 0, 0, 0);
                    }
                    systemData.tenkensyaData = tenkensyaData;
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to read to file [readTenkensya]");
            }
            return systemData.tenkensyaData;
        }

        // 点検者情報書込み
        public static void writeTenkensya(TenkensyaData tenkensyaData) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(tenkensyaData);
                    // ファイル書き込み
                    saveText(stringJson, FileNameTenkensya);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [writeTenkensya]");
            }
        }

        // システム設定情報読み込み
        public static SystemData readSettei(SystemData systemData) {
            try {
                lock (DataManager.sDataLock) {
                    //SdLog.i("readSettei");
                    SetteiData setteiData = new SetteiData("");
                    string data = null;
                    try {
                        data = loadText(FileNameSettei);
                        //SdLog2.i("data:" + data);
                        if (data == null) {
                            deleteFile(FileNameSettei);
                            return null;
                        }
                        setteiData = (SetteiData)JsonSerializer.Deserialize<SetteiData>(data);
                        // setteiDataが空の場合は、3を設定する
                        if (setteiData == null) {
                            //SdLog2.i("setteiData==null:" + data);
                            setteiData = new SetteiData("");
                            setteiData.textKensinKeta = "3";
                            setteiData.textPrintKigouSize = "20";
                        }
                        if (Common.isEmpty(setteiData.textKensinKeta)) {
                            //SdLog2.i("setteiData.textKensinKeta == null:" + data);
                            setteiData.textKensinKeta = "3";
                        }
                        if (Common.isEmpty(setteiData.textPrintKigouSize)) {
                            //SdLog2.i("setteiData.textPrintKigouSize == null:" + data);
                            setteiData.textPrintKigouSize = "20";
                        }
                        if (setteiData.screenOffTimeout == null) {
                            //SdLog2.i("setteiData.screenOffTimeout == null:" + data);
                            setteiData.screenOffTimeout = 15;
                        }
                    } catch (IOException e) {
                        setteiData.textKensinKeta = "3";
                        //e.printStackTrace();
                        return systemData;
                    }
                    systemData.settei = setteiData;
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [readSettei]");
            }
            return systemData;
        }

        // システム設定情報書込み
        public static void writeSettei(SetteiData setteiData) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(setteiData);
                    // ファイル書き込み
                    saveText(stringJson, FileNameSettei);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [readSettei]");
            }
        }

        // システム設定情報読み込み
        public static SystemData readWareki(SystemData systemData) {
            try {
                lock (DataManager.sDataLock) {
                    //SdLog.i("readWareki");
                    WarekiData warekiData = new WarekiData();
                    string data = null;
                    try {
                        data = loadText(FileNameWareki);
                        //SdLog2.i("data:" + data);
                        if (data == null) {
                            deleteFile(FileNameWareki);
                            return null;
                        }
                        warekiData = (WarekiData)JsonSerializer.Deserialize<SystemData>(data);
                        //warekiData = (WarekiData) gson.fromJson(data, WarekiData.class);
                    } catch (IOException e) {
                        //e.printStackTrace();
                        return systemData;
                    }
                    systemData.wareki = warekiData;
                }
                //		} catch (InvocationTargetException e){
                //		    Log.e(TAG, "Unable to write to file [readSettei]");
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [readSettei]");
            }
            return systemData;
        }

        // システム設定情報書込み
        public static void writeWareki(WarekiData warekiData) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(warekiData);
                    // ファイル書き込み
                    saveText(stringJson, FileNameWareki);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [readSettei]");
            }
        }

        public static void saveText(string str, string fileName) {
            // ストリームを開く
            try {
                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8)) {
                    sw.WriteLine(str);
                }
            } catch (FileNotFoundException e) {
                //e.printStackTrace();
            } catch (IOException e) {
                //e.printStackTrace();
            }
        }

        public static void copyFile(string fromFileName, string toFileName) {
            if (fromFileName == toFileName) {
                return;
            }
            string path = Common.getSystemPath();
            // ストリームを開く
            string[] listFile = Directory.GetFiles(path);
            foreach (string file in listFile) {
                if (file.StartsWith(fromFileName)) {
                    string[] fromFileNameArry = file.Split("_");
                    string[] toFileNameArry = toFileName.Split("_");
                    if (fromFileNameArry.Length != 3)
                        continue;
                    if (toFileNameArry.Length != 2) {
                        string[] toFileNameArry2 = new string[2];
                        toFileNameArry2[0] = new string(toFileNameArry[0]);
                        toFileNameArry2[1] = new string("");
                        toFileNameArry = toFileNameArry2;
                    }
                    string toName = toFileNameArry[0] + "_" + toFileNameArry[1] + "_" + fromFileNameArry[2];
                    try {
                        copy(file, toName);
                        deleteFile(fromFileName);
                    } catch (IOException e) {
                    }
                }
            }
        }

        /**
          * ファイルをコピーします。
          * @param file コピー元ファイル
          * @param newFileName コピー先ファイル名
          */
        private static void copy(string file, string newFileName) {
            try {
                string text = File.ReadAllText(file);
                File.WriteAllText(newFileName, text);
            } catch (FileNotFoundException e) {
                //e.printStackTrace();
            } catch (IOException e) {
                //e.printStackTrace();
            }
        }

        public static string loadText(string FileName) {
            string path = Common.getSystemPath();
            string text = File.ReadAllText(Path.Combine(path, FileName));
            string retstring;
            if (2 < text.Length) {
                retstring = text.Substring(0, text.Length - 2);
            } else {
                retstring = string.Empty;
            }
            return retstring.Trim();
        }

        public static void deleteAllTenkenFile(SystemData systemData) {
            //ファイルの一覧を検索するディレクトリパスを指定する
            string path = Common.getSystemPath();
            string fileNameAsterisk = DataManager.makeTenkenFileNameAsterisk(systemData);
            string[] files = Directory.GetFiles(path, fileNameAsterisk);
            Array.Sort(files);

            foreach (string file in files) {
                try {
                    deleteFile(file);
                } catch (IOException e) {
                    //e.printStackTrace();
                }
            }
        }

        public static void deleteFile(string FileName) {
            try {
                string path = Common.getSystemPath();
                lock (DataManager.sDataLock) {
                    File.Delete(Path.Combine(path, FileName));
                }
            } catch (Exception e) {
                // Log.e(TAG, "Unable to delete file [deleteFile]");
            }
            return;
        }

        // フリーフォーマット出力用ファイルバックアップ処理
        public static void backupFreeFormatFile(string textCyouhyouFreeFormatFilePath,
                                                string textCyouhyouFreeFormatFileName,
                                                string textCyouhyouFreeFormatFlag) {
            try {
                if (Common.isEmpty(textCyouhyouFreeFormatFlag) ||
                        textCyouhyouFreeFormatFlag == "0") { // フリーフォーマット帳票設定OFF
                                                             // なにもしない
                } else {
                    // フリーフォーマットファイルを月報くんのディレクトリにコピーする
                    string filePathNameFrom = Path.Combine(textCyouhyouFreeFormatFilePath, textCyouhyouFreeFormatFileName);
                    string filePathNameTo = Path.Combine(Common.getSystemPath(), textCyouhyouFreeFormatFileName);
                    File.Copy(filePathNameFrom, filePathNameTo);
                }
            } catch (Exception e) {
                // Log.e(TAG, "Unable to write to file[writeTenken]");
            }
        }

        // フリーフォーマット出力用ファイルバックアップ処理
        public static void backupFreeFormatFile(string textCyouhyouFreeFormatFilePath,
                                                string textCyouhyouFreeFormatFileName) {
            backupFreeFormatFile(textCyouhyouFreeFormatFilePath, textCyouhyouFreeFormatFileName, "1");
        }

        // 電気使用量等を計算する
        public static SystemData execDenryokuryou(SystemData systemData) {
            string jouritu = systemData.tenkenData.itemJyouritu.text;
            // メーター交換なし
            if (isEmpty(systemData.tenkenData.itemGenzaiKoukanAto1) &&
                isEmpty(systemData.tenkenData.itemKensinKoukanMae1)) {
                systemData.tenkenData.itemGenzaiDenryokuryou1.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu1, systemData.tenkenData.itemGenzaiSengetu1),
                        jouritu, 2);
                systemData.tenkenData.itemGenzaiDenryokuryou2.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu2, systemData.tenkenData.itemGenzaiSengetu2),
                        jouritu, 2);
                systemData.tenkenData.itemGenzaiDenryokuryou3.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu3, systemData.tenkenData.itemGenzaiSengetu3),
                        jouritu, 2);
                systemData.tenkenData.itemGenzaiDenryokuryou4.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu4, systemData.tenkenData.itemGenzaiSengetu4),
                        jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou1.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu1, systemData.tenkenData.itemKensinSengetu1),
                        jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou2.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu2, systemData.tenkenData.itemKensinSengetu2),
                        jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou3.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu3, systemData.tenkenData.itemKensinSengetu3),
                        jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou4.text = Common.getMultiply(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu4, systemData.tenkenData.itemKensinSengetu4),
                        jouritu, 2);
            } else { // メーター交換あり
                systemData.tenkenData.itemGenzaiDenryokuryou1.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu1, systemData.tenkenData.itemGenzaiKoukanAto1),
                        getMinusKenshin(systemData.tenkenData.itemGenzaiKoukanMae1, systemData.tenkenData.itemGenzaiSengetu1)), jouritu, 2);
                systemData.tenkenData.itemGenzaiDenryokuryou2.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu2, systemData.tenkenData.itemGenzaiKoukanAto2),
                        getMinusKenshin(systemData.tenkenData.itemGenzaiKoukanMae2, systemData.tenkenData.itemGenzaiSengetu2)), jouritu, 2);
                systemData.tenkenData.itemGenzaiDenryokuryou3.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu3, systemData.tenkenData.itemGenzaiKoukanAto3),
                        getMinusKenshin(systemData.tenkenData.itemGenzaiKoukanMae2, systemData.tenkenData.itemGenzaiSengetu3)), jouritu, 2);
                systemData.tenkenData.itemGenzaiDenryokuryou4.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemGenzaiKongetu4, systemData.tenkenData.itemGenzaiKoukanAto4),
                        getMinusKenshin(systemData.tenkenData.itemGenzaiKoukanMae2, systemData.tenkenData.itemGenzaiSengetu4)), jouritu, 2);

                systemData.tenkenData.itemKensinDenryokuryou1.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu1, systemData.tenkenData.itemKensinKoukanAto1),
                        getMinusKenshin(systemData.tenkenData.itemKensinKoukanMae1, systemData.tenkenData.itemKensinSengetu1)), jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou2.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu2, systemData.tenkenData.itemKensinKoukanAto2),
                        getMinusKenshin(systemData.tenkenData.itemKensinKoukanMae2, systemData.tenkenData.itemKensinSengetu2)), jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou3.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu3, systemData.tenkenData.itemKensinKoukanAto3),
                        getMinusKenshin(systemData.tenkenData.itemKensinKoukanMae3, systemData.tenkenData.itemKensinSengetu3)), jouritu, 2);
                systemData.tenkenData.itemKensinDenryokuryou4.text = Common.getMultiply(Common.getPlus(getMinusKenshin(
                        systemData.tenkenData.itemKensinKongetu4, systemData.tenkenData.itemKensinKoukanAto4),
                        getMinusKenshin(systemData.tenkenData.itemKensinKoukanMae4, systemData.tenkenData.itemKensinSengetu4)), jouritu, 2);
            }
            systemData.tenkenData.itemGenzaiDenryokuryou1.text = Keta(systemData.tenkenData.itemGenzaiDenryokuryou1.text,
                    systemData);
            systemData.tenkenData.itemGenzaiDenryokuryou2.text = Keta(systemData.tenkenData.itemGenzaiDenryokuryou2.text,
                    systemData);
            systemData.tenkenData.itemGenzaiDenryokuryou3.text = Keta(systemData.tenkenData.itemGenzaiDenryokuryou3.text,
                    systemData);
            systemData.tenkenData.itemGenzaiDenryokuryou4.text = Keta(systemData.tenkenData.itemGenzaiDenryokuryou4.text,
                    systemData);
            systemData.tenkenData.itemKensinDenryokuryou1.text = Keta(systemData.tenkenData.itemKensinDenryokuryou1.text,
                    systemData);
            systemData.tenkenData.itemKensinDenryokuryou2.text = Keta(systemData.tenkenData.itemKensinDenryokuryou2.text,
                    systemData);
            systemData.tenkenData.itemKensinDenryokuryou3.text = Keta(systemData.tenkenData.itemKensinDenryokuryou3.text,
                    systemData);
            systemData.tenkenData.itemKensinDenryokuryou4.text = Keta(systemData.tenkenData.itemKensinDenryokuryou4.text,
                    systemData);

            systemData.tenkenData.itemGenzaiDenryokuryou.text = Common.getPlus(
                    systemData.tenkenData.itemGenzaiDenryokuryou1.text, systemData.tenkenData.itemGenzaiDenryokuryou2.text,
                    systemData.tenkenData.itemGenzaiDenryokuryou3.text, systemData.tenkenData.itemGenzaiDenryokuryou4.text);
            systemData.tenkenData.itemKensinDenryokuryou.text = Common.getPlus(
                    systemData.tenkenData.itemKensinDenryokuryou1.text, systemData.tenkenData.itemKensinDenryokuryou2.text,
                    systemData.tenkenData.itemKensinDenryokuryou3.text, systemData.tenkenData.itemKensinDenryokuryou4.text);
            return systemData;
        }


        // 一日平均使用量を計算する
        public static SystemData execDenryokuryouHeikin(SystemData systemData) {
            // 前回検針からの日数を取得する
            Item genzaiNissuu = new Item(Common.getNissu(systemData.tenkenData.itemGenzaiKongetuHiduke.text, systemData.tenkenData.itemGenzaiSengetuHiduke.text),
                                          systemData.tenkenData.itemGenzaiKongetuHiduke.color,
                                          systemData.tenkenData.itemGenzaiKongetuHiduke.style,
                                         systemData.tenkenData.itemGenzaiKongetuHiduke.bgcolor);
            Item kensinNissuu = new Item(Common.getNissu(systemData.tenkenData.itemKensinSengetuHiduke.text, systemData.tenkenData.itemKensinSengetuHiduke.text),
                                         systemData.tenkenData.itemKensinSengetuHiduke.color,
                                         systemData.tenkenData.itemKensinSengetuHiduke.style,
                                         systemData.tenkenData.itemKensinSengetuHiduke.bgcolor);
            systemData.tenkenData.itemGenzaiHeikinDenryokuryou = new Item(Common.formatInteger(Common.getDivide(systemData.tenkenData.itemGenzaiDenryokuryou.text, genzaiNissuu.text, 2)),
                    systemData.tenkenData.itemGenzaiDenryokuryou.color,
                     systemData.tenkenData.itemGenzaiDenryokuryou.style,
                     systemData.tenkenData.itemGenzaiDenryokuryou.bgcolor);
            systemData.tenkenData.itemKensinHeikinDenryokuryou = new Item(Common.formatInteger(Common.getDivide(systemData.tenkenData.itemKensinDenryokuryou.text, kensinNissuu.text, 2)),
                    systemData.tenkenData.itemKensinDenryokuryou.color,
                     systemData.tenkenData.itemKensinDenryokuryou.style,
                     systemData.tenkenData.itemKensinDenryokuryou.bgcolor);
            return systemData;
        }

        private static string Keta(string value, SystemData systemData) {
            int keta = 0;
            if (systemData.settei.textKensinKeta != null) {
                if (systemData.settei.textKensinKeta == "") {
                    systemData.settei.textKensinKeta = "3";
                }
                keta = int.Parse(systemData.settei.textKensinKeta);
            }
            decimal ret = (decimal)Common.Round(double.Parse(value), keta * (-1));
            return ret.ToString();
        }

        static string[] defaultTenkenKasyo = { "区分開閉器", "電線、ケーブル", "支持物、碍子類", "受電盤、外柵施錠設備類", "断路器、電力ヒューズ",
                    "遮断機", "開閉器", "計器用変圧器", "変流器", "避雷器", "地絡継電器、過電流継電器", "高圧カットアウト",
                    "変圧器", "電力コンデンサ、リアクトル", "接地線", "配電盤", "開閉器類", "配線(屋外)", "配線(屋内)",
                    "設地線", "配線器具", "照明器具", "低圧コンデンサ", "電動機", "溶接機", "電熱器", "蓄電池設備" };

        public static bool checkBillingGeppou() {
            // TODO 課金チェック

            //if (Debug.isDebuggerConnected()) {
            //    return true;
            //}
            //return MainActivity.mIsBilling;
            return true;
        }

        public static bool checkBillingGraph() {
            //if (Debug.isDebuggerConnected()) {
            //    return true;
            //}

            bool flagBilling = false;
            string data = Common.readText("Billing.dat");
            //SdLog.i("checkBillingGraph data: " + data);
            if (data != null) {
                string[] strSplit = data.Split("&");
                string[] strPurchaseGeppou = strSplit[4].Split("=");
                string[] strPurchased = strSplit[7].Split("=");
                if (strSplit != null && "purchase_geppou_graph" == strPurchaseGeppou[1] &&
                        "0" == strPurchased[1]) {
                    flagBilling = true;
                }
            }
            return flagBilling;
        }

        /// <summary>
        /// リソースファイルを元にデータファイルを作成する
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="id"></param>
        public static void FileCopy(string fileName, int id) {
            // TODO 作成

            // SDカードにディレクトリを作成する
            //Common.mkDirSDCard("");
            //Common.mkDirSDCard("geppou");
            //Common.mkDirSDCard("geppou/cyouhyou");
            //// ファイルの有無を調べる
            //// 月替わり処理
            //if (!File.Exists(getSDCardPath() + "/" + fileName)) { // ファイルがない場合はコピーする
            //    File copyFile = new File(getSDCardPath() + "/" + fileName);
            //    InputStream input = null;
            //    FileOutputStream output = null;
            //    try {
            //        input = context.getResources().openRawResource(id);
            //        output = new FileOutputStream(copyFile);
            //        byte[] buff = new byte[1024 * 4];
            //        int size = 0;
            //        while ((size = input.read(buff)) >= 0) {
            //            output.write(buff, 0, size);
            //        }
            //        output.flush();
            //        output.close();
            //        input.close();
            //        output = null;
            //        input = null;
            //    } catch (Exception e) {
            //        try {
            //            if (output != null)
            //                output.close();
            //            if (input != null)
            //                input.close();
            //        } catch (Exception e2) {
            //        }
            //    }
            //}
        }

        public static void FileDelete(string fileName) {
            // SDカードにディレクトリを作成する
            Common.mkDirSDCard("");
            Common.mkDirSDCard("geppou");
            Common.mkDirSDCard("geppou/cyouhyou");

            //File.Delete(getSDCardPath() + "/" + fileName);
        }

        public static void FileCopy(string filePath1, string filePath2) {
            lock (DataManager.sDataLock) {
                try {
                    File.Copy(filePath1, filePath2);
                } catch (Exception e) {
                    //e.printStackTrace();
                }
            }
        }

        // 電力会社情報読み込み
        public static List<DenryokugaisyaData> readDenryokugaisya(SystemData systemData) {
            List<DenryokugaisyaData> retList = new List<DenryokugaisyaData>();
            try {
                lock (DataManager.sDataLock) {
                    List<DenryokugaisyaData> denryokugaisyaData = null;
                    string data = null;
                    try {
                        data = DataManager.loadText(FileNameDenryokugaisya);
                        if (data == null) {
                            DataManager.deleteFile(FileNameDenryokugaisya);
                            return null;
                        }
                        denryokugaisyaData = JsonSerializer.Deserialize<List<DenryokugaisyaData>>(data);
                    } catch (IOException e) {
                        //e.printStackTrace();
                        return null;
                    }
                    foreach (DenryokugaisyaData denryokugaisya in denryokugaisyaData) {
                        retList.Add(denryokugaisya);
                    }
                    systemData.listDenryokugaisyaData = retList;
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to read to file [readDenryokugaisya]");
            }
            return systemData.listDenryokugaisyaData;
        }

        public static DenryokugaisyaData getDenryokuGaisya(SystemData systemData) {
            DenryokugaisyaData denryokugaisyaData = null;
            foreach (DenryokugaisyaData data in systemData.listDenryokugaisyaData) {
                if (data.textDenryokugaisyaMei
                        == systemData.listJigyousyo[systemData.positionJigyousyo].textDenryokugaisya) {
                    denryokugaisyaData = data;
                    break;
                }
            }
            if (denryokugaisyaData == null) {
                denryokugaisyaData = systemData.listDenryokugaisyaData[0];
            }
            return denryokugaisyaData;
        }

        public static BaseKeiyaku getKeiyakuSyubetu(SystemData systemData, DenryokugaisyaData denryokugaisyaData) {
            BaseKeiyaku keiyakuSyubetuData = null;
            foreach (BaseKeiyaku data in denryokugaisyaData.getRyokin(systemData.tenkenData.itemKensinKongetuHiduke.text).getKeiyakusyubetuList()) {//   DataManager.getKeiyakuSyubetu.textKeiyakuSyubetuMei listKeiyaku) {
                if (data.textKeiyakuSyubetuMei == systemData.listJigyousyo[systemData.positionJigyousyo].textKeiyakusyubetu) {
                    keiyakuSyubetuData = data;
                    break;
                }
            }
            if (keiyakuSyubetuData == null) {
                keiyakuSyubetuData = denryokugaisyaData.getRyokin(systemData.tenkenData.itemKensinKongetuHiduke.text).getKeiyakusyubetuList()[0];
            }
            return keiyakuSyubetuData;
        }

        // 点検記号情報書込み
        public static void writeDenryokugaisya(List<DenryokugaisyaData> list) {
            try {
                lock (DataManager.sDataLock) {
                    string stringJson;
                    stringJson = JsonSerializer.Serialize(list); //["shika",koala]
                                                                 // ファイル書き込み
                    saveText(stringJson, FileNameDenryokugaisya);
                }
            } catch (Exception e) {
                //Log.e(TAG, "Unable to write to file [writeDenryokugaisya]");
            }
        }

        public static int getDenryokugaisyaIndex(SystemData dataSystem, string DenryokugaisyaMei) {
            int index = 0;
            foreach (DenryokugaisyaData denryokugaisya in dataSystem.listDenryokugaisyaData) {
                if (denryokugaisya.textDenryokugaisyaMei == DenryokugaisyaMei) {
                    break;
                }
                index++;
            }
            if (dataSystem.listDenryokugaisyaData.Count == index) {
                index = 0;
            }
            return index;
        }

        public static int getKeiyakusyubetuIndex(List<BaseKeiyaku> listKeiyaku, JigyousyoData jigyoujyouData) {
            if (jigyoujyouData == null)
                return 0;
            int position = 0;
            foreach (BaseKeiyaku keiyakusyubetu in listKeiyaku) {
                if (keiyakusyubetu.textKeiyakuSyubetuMei == jigyoujyouData.textKeiyakusyubetu) {
                    break;
                }
                position++;
            }
            if (listKeiyaku.Count == position)
                position = 0;
            return position;
        }

        // 電力会社初期値設定
        public static SystemData getDenryokugaisyaListData(SystemData systemData) {
            List<DenryokugaisyaData> listDenryokugaisyaDataData = readDenryokugaisya(systemData);
            if (listDenryokugaisyaDataData == null || listDenryokugaisyaDataData.Count == 0 || listDenryokugaisyaDataData[0].listRyokin == null) {
                listDenryokugaisyaDataData = new List<DenryokugaisyaData>();
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("北海道電力", KeiyakuSyubetuDataMake.makeHokkaidoDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("東北電力", KeiyakuSyubetuDataMake.makeTouhokuDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("東京電力", KeiyakuSyubetuDataMake.makeTokyoDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("中部電力", KeiyakuSyubetuDataMake.makeCyuubuDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("北陸電力", KeiyakuSyubetuDataMake.makeHokurikuDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("関西電力", KeiyakuSyubetuDataMake.makeKansaiDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("中国電力", KeiyakuSyubetuDataMake.makeCyuugokuDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("四国電力", KeiyakuSyubetuDataMake.makeSikokuDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("九州電力", KeiyakuSyubetuDataMake.makeKyuusyuDenryoku(), null, null, null));
                listDenryokugaisyaDataData.Add(new DenryokugaisyaData("沖縄電力", KeiyakuSyubetuDataMake.makeOkinawaDenryoku(), null, null, null));
            }

            if (listDenryokugaisyaDataData[0].listRyokin == null || listDenryokugaisyaDataData[0].listRyokin.Count == 0) {
                if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "北海道電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeHokkaidoDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "東北電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeTouhokuDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "東京電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeTokyoDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "中部電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeCyuubuDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "北陸電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeHokurikuDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "関西電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeKansaiDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "中国電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeCyuugokuDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "四国電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeSikokuDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "九州電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeKyuusyuDenryoku());
                } else if (listDenryokugaisyaDataData[0].textDenryokugaisyaMei == "沖縄電力") {
                    listDenryokugaisyaDataData[0].setRyokin(KeiyakuSyubetuDataMake.makeOkinawaDenryoku());
                }
            }
            systemData.listDenryokugaisyaData = listDenryokugaisyaDataData;
            return systemData;
        }

        public static bool equals(string item, string target) {
            if (item == null) {
                return false;
            }
            return item == target;
        }

        public static bool equals(Item item, string target) {
            if (item == null || item.text == null) {
                return false;
            }
            return item.text == target;
        }

        public static bool contains(Item item, string target) {
            if (isEmpty(item)) {
                return false;
            }
            return item.text.Contains(target);
        }

        public static bool contains(string item, string target) {
            if (Common.isEmpty(item)) {
                return false;
            }
            return item.Contains(target);
        }

        public static bool isNull(Item item) {
            if (item == null) {
                return true;
            }
            return false;
        }

        public static bool isNull(string item) {
            if (item == null) {
                return true;
            }
            return false;
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

        public static bool isZero(Item item) {
            if (item == null || item.text == "" || item.text == "0") {
                return false;
            }
            try {
                double iszero = double.Parse(item.text);
                return (iszero == 0.0);
            } catch (Exception e) {
                return false;
            }
        }

        public static bool isZero(string item) {
            if (item == null || item == "") {
                return false;
            }
            try {
                double iszero = double.Parse(item);
                return (iszero == 0.0);
            } catch (Exception e) {
                return false;
            }
        }

        public static bool isNumeric(Item item) {
            if (isEmpty(item)) {
                return false;
            }
            return Common.isNumeric(item.text);
        }

        public static bool isNumeric(string item) {
            if (Common.isEmpty(item)) {
                return false;
            }
            return Common.isNumeric(item);
        }

        public static SystemData Convert(string data) {
            string[] arrData = data.Split("\r");
            SystemData systemData = new SystemData();
            return systemData;
        }

        public static Framework.PrintItem getHukaritu(Item heikin, Item saidaiDenryoku) {
            double dHeikin = 0.0;
            double dSaidaiDenryoku = 0.0;
            try {
                dHeikin = double.Parse(heikin.text.Replace(",", ""));
                dSaidaiDenryoku = double.Parse(saidaiDenryoku.text);
            } catch (Exception e) {
                return new Framework.PrintItem("", true);
            }
            double fukaritu = dHeikin * 100 / dSaidaiDenryoku;
            fukaritu = fukaritu / 24.0;
            string strFukaritu = string.Format("%.1f", fukaritu);
            return new Framework.PrintItem(strFukaritu, true);
        }

        public static readonly int ROUND_OFF = 0;
        public static readonly int ROUND_UP = 1;
        public static readonly int ROUND_DOWN = 2;

        public static Item round(Item strNum, int mode) {
            string retstring = "";
            if (isZero(strNum))
                return strNum;
            if (Common.isNumeric(strNum.text) == false)
                return strNum;
            if (mode == ROUND_OFF) {
                retstring = Math.Round(double.Parse(strNum.text)).ToString();
            } else if (mode == ROUND_UP) {
                retstring = Math.Ceiling(double.Parse(strNum.text)).ToString();
            } else if (mode == ROUND_DOWN) {
                retstring = Math.Floor(double.Parse(strNum.text)).ToString();
            }
            strNum.text = retstring;
            return strNum;
        }

        public static string round(string number, int mode) {
            string retstring = "";
            if (isZero(number))
                return number;
            if (Common.isNumeric(number) == false)
                return number;
            if (mode == ROUND_OFF) {
                retstring = Math.Round(double.Parse(number)).ToString();
            } else if (mode == ROUND_UP) {
                retstring = Math.Ceiling(double.Parse(number)).ToString();
            } else if (mode == ROUND_DOWN) {
                retstring = Math.Floor(double.Parse(number)).ToString();
            }
            number = retstring;
            return number;
        }

        public static SystemData setTenkenKigouListData(SystemData systemData) {
            List<TenkenKigouData> listTenkenKigou = DataManager.readTenkenKigou(systemData);
            if (listTenkenKigou == null || listTenkenKigou.Count == 0 || listTenkenKigou[0].itemTenkenKigou == null) {
                listTenkenKigou = new List<TenkenKigouData>();
                listTenkenKigou.Add(new TenkenKigouData(new Item("レ"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("○"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("良"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("△"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("×"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("否"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("－"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("①"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("②"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("③"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("④"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("⑤"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("⑥"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("⑦"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("⑧"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("⑨"), null, null, null));
                listTenkenKigou.Add(new TenkenKigouData(new Item("⑩"), null, null, null));
            }
            systemData.listTenkenKigou = listTenkenKigou;
            return systemData;
        }

        /**
         * スピナーの内容を設定します。TODO
         */
        public static List<string> setSpinnerItem(SystemData systemData) {
            //    // スピナーボタンに表示するテキスト
            //    ArrayAdapter<string> adapter = new ArrayAdapter<string>(context, R.layout.tenken_tenkou_spinner_layout);
            //    // スピナーを押したときに表示するチェックテキスト
            //    adapter.setDropDownViewResource(R.layout.tenken_tenkou_spinner_dropdown_layout);
            //    for (TenkenKigouData kigou : systemData.listTenkenKigou) {
            //    adapter.add(kigou.itemTenkenKigou.text);
            //}
            return null;
        }

        /**
         * スピナーの内容を設定します。TODO
         */
        public static List<string> setSpinnerItem(SystemData systemData, List<string> list) {
            //    // スピナーボタンに表示するテキスト
            //    ArrayAdapter<string> adapter = new ArrayAdapter<string>(context, R.layout.tenken_tenkou_spinner_layout);
            //    // スピナーを押したときに表示するチェックテキスト
            //    adapter.setDropDownViewResource(R.layout.tenken_tenkou_spinner_dropdown_layout);
            //    for (string item : list) {
            //    adapter.add(item);
            //}
            return null;
        }

        public static int getSpinnerIndex(List<TenkenKigouData> listKigou, string textKigou) {
            if (textKigou == null)
                return 0;
            int position = 0;
            foreach (TenkenKigouData kigou in listKigou) {
                if (kigou.itemTenkenKigou.text == null) {
                    break;
                }
                if (kigou.itemTenkenKigou.text == textKigou) {
                    break;
                }
                position++;
            }
            if (listKigou.Count == position)
                position = 0;
            return position;
        }

        public static int getSpinnerIndex(List<string> listKigou, string textKigou, int dummy) {
            if (textKigou == null)
                return 0;
            int position = 0;
            foreach (string kigou in listKigou) {
                if (kigou == null) {
                    break;
                }
                if (kigou == textKigou) {
                    break;
                }
                position++;
            }
            if (listKigou.Count == position)
                position = 0;
            return position;
        }

        private static Dictionary<string, Item> setRelation(string[] arrData, SystemData systemData) {
            Dictionary<string, Item> map = new Dictionary<string, Item>();
            map.Add("TK", systemData.tenkenData.itemTenkenNijtijiYMD);
            map.Add("TJ", systemData.tenkenData.itemJyouritu);
            map.Add("TKTE", systemData.tenkenData.itemKeiyakuDenryoku);
            map.Add("TJARS", systemData.tenkenData.itemDenatuRS);
            map.Add("TJAST", systemData.tenkenData.itemDenatuST);
            map.Add("TJATR", systemData.tenkenData.itemDenatuTR);
            map.Add("TJRR", systemData.tenkenData.itemDenryuR);
            map.Add("TJRS", systemData.tenkenData.itemDenryuS);
            map.Add("TJRT", systemData.tenkenData.itemDenryuT);
            map.Add("TJD", systemData.tenkenData.itemDenryoku);
            map.Add("TJR", systemData.tenkenData.itemRikiritu);
            map.Add("TKK1", systemData.tenkenData.itemKensinKongetu1);
            map.Add("TKK2", systemData.tenkenData.itemKensinKongetu2);
            map.Add("TKK3", systemData.tenkenData.itemKensinKongetu3);
            map.Add("TKK4", systemData.tenkenData.itemKensinKongetu4);
            map.Add("TKZ1", systemData.tenkenData.itemKensinSengetu1);
            map.Add("TKZ2", systemData.tenkenData.itemKensinSengetu2);
            map.Add("TKZ3", systemData.tenkenData.itemKensinSengetu3);
            map.Add("TKZ4", systemData.tenkenData.itemKensinSengetu4);
            map.Add("TKHMK", systemData.tenkenData.itemKensinKongetuMukoDenryoku);
            map.Add("TKHMZ", systemData.tenkenData.itemKensinSengetuMukoDenryoku);
            map.Add("TKKT1", systemData.tenkenData.itemGenzaiKongetu1);
            map.Add("TKKT2", systemData.tenkenData.itemGenzaiKongetu2);
            map.Add("TKKT3", systemData.tenkenData.itemGenzaiKongetu3);
            map.Add("TKKT4", systemData.tenkenData.itemGenzaiKongetu4);
            map.Add("TKZT1", systemData.tenkenData.itemGenzaiSengetu1);
            map.Add("TKZT2", systemData.tenkenData.itemGenzaiSengetu2);
            map.Add("TKZT3", systemData.tenkenData.itemGenzaiSengetu3);
            map.Add("TKZT4", systemData.tenkenData.itemGenzaiSengetu4);
            map.Add("TKHM", systemData.tenkenData.itemGenzaiKongetuMukoDenryoku);
            map.Add("TKZHM", systemData.tenkenData.itemGenzaiSengetuMukoDenryoku);
            map.Add("TKD", systemData.tenkenData.itemKensinDenryokuryou);
            map.Add("TKR", systemData.tenkenData.itemKensinKongetuRikiritu);
            map.Add("TM1", systemData.tenkenData.itemKensinKoukanMae1);
            map.Add("TM2", systemData.tenkenData.itemKensinKoukanMae2);
            map.Add("TM3", systemData.tenkenData.itemKensinKoukanMae3);
            map.Add("TM4", systemData.tenkenData.itemKensinKoukanMae4);
            map.Add("TA1", systemData.tenkenData.itemKensinKoukanAto1);
            map.Add("TA2", systemData.tenkenData.itemKensinKoukanAto2);
            map.Add("TA3", systemData.tenkenData.itemKensinKoukanAto3);
            map.Add("TA4", systemData.tenkenData.itemKensinKoukanAto4);
            map.Add("TMT1", systemData.tenkenData.itemGenzaiKoukanMae1);
            map.Add("TMT2", systemData.tenkenData.itemGenzaiKoukanMae2);
            map.Add("TMT3", systemData.tenkenData.itemGenzaiKoukanMae3);
            map.Add("TMT4", systemData.tenkenData.itemGenzaiKoukanMae4);
            map.Add("TAT1", systemData.tenkenData.itemGenzaiKoukanAto1);
            map.Add("TAT2", systemData.tenkenData.itemGenzaiKoukanAto2);
            map.Add("TAT3", systemData.tenkenData.itemGenzaiKoukanAto3);
            map.Add("TAT4", systemData.tenkenData.itemGenzaiKoukanAto4);
            //
            //    	map.put("TT01", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            //    	map.put("", systemData.tenkenData.);
            return map;
        }

        public static void setInsatuCyouhyouList(List<string> listInsatuCyouhyou) {
            listInsatuCyouhyou.Add("印刷帳票1"); // 関東仕様
            listInsatuCyouhyou.Add("印刷帳票1-季時別"); // 関東仕様
            listInsatuCyouhyou.Add("印刷帳票1-グラフ付"); // 関東仕様
            listInsatuCyouhyou.Add("印刷帳票1-変圧器12台"); // 関東仕様
            listInsatuCyouhyou.Add("印刷帳票2"); // 中部仕様
            listInsatuCyouhyou.Add("印刷帳票3"); // 関西仕様
            listInsatuCyouhyou.Add("印刷帳票3-グラフ付"); // 関西仕様
            listInsatuCyouhyou.Add("印刷帳票3-グラフ付2"); // 関西仕様
            listInsatuCyouhyou.Add("印刷帳票3-変圧器12台"); // 関西仕様
            listInsatuCyouhyou.Add("印刷帳票3-変圧器12台-社章なし"); // 関西仕様
            listInsatuCyouhyou.Add("印刷帳票3-関電管"); // 関西電気管理センター
            listInsatuCyouhyou.Add("印刷帳票3-関電管12"); // 関西電気管理センター
            listInsatuCyouhyou.Add("印刷帳票4"); // 中国仕様
            listInsatuCyouhyou.Add("印刷帳票5"); // 九州仕様
            listInsatuCyouhyou.Add("印刷帳票5-グラフ付"); // 九州仕様
        }

        // 常用発電機
        public static void setJHatudenkiList(List<string> listInsatuCyouhyou) {
            listInsatuCyouhyou.Add("常用1 印刷帳票");
            listInsatuCyouhyou.Add("常用2 印刷帳票");
        }

        // 非常用発電機
        public static void setHJHatudenkiList(List<string> listInsatuCyouhyou) {
            listInsatuCyouhyou.Add("非常用1 印刷帳票");
            listInsatuCyouhyou.Add("非常用2 印刷帳票");
        }

        public static string setKensinbiText(SystemData systemData) {
            if (isEmpty(systemData.tenkenData.itemKensinbi) == true) {
                return Common.makeKensinDate(systemData.tenkenData.itemTenkenNijtijiYMD.text, "1");
            }
            if (isEmpty(systemData.tenkenData.itemTenkenNijtijiYMD) == true) {
                return Common.makeKensinDate(systemData.tenkenData.itemTenkenNijtijiYMD.text, "1");
            }
            string month = Common.getMonth(systemData.tenkenData.itemTenkenNijtijiYMD.text);
            string date = systemData.tenkenData.itemKensinbi.text;
            if (month == "4" || month == "6" || month == "9" || month == "11") {
                if (date == "31") {
                    return Common.getLastDay(systemData.tenkenData.itemTenkenNijtijiYMD.text);
                }
            } else if (month == "2" == true) {
                if (0 < date.CompareTo("29")) {
                    return Common.getLastDay(systemData.tenkenData.itemTenkenNijtijiYMD.text);
                }
            }
            return Common.makeKensinDate(systemData.tenkenData.itemTenkenNijtijiYMD.text, date);
        }

        public static void setupOldVersionData() {
            string dir = Common.getSystemPath() + "/geppou_convert";
            string convertDirectory = "jp.increase.geppou";

            if (File.Exists(dir)) {
                string[] fileList = Directory.GetFiles(dir);
                foreach (string file in fileList) {
                    try {
                        if (file == "Jigyoujyou.json") {
                            File.Copy(file, "/data/data/" + convertDirectory + "/files/" + Path.GetFileName(file));
                        }
                        File.Copy(file, "/data/data/" + convertDirectory + "/files/" + Path.GetFileName(file));
                    } catch (IOException e) {
                        //e.printStackTrace();
                    }
                }
            }
            try {
                File.Delete(dir);
            } catch (IOException e) {
                //e.printStackTrace();
            }
        }

        public static void downloadServerData(SystemData systemdata) {
            // FTPサーバー上に過去の点検データがある場合は、それを取得する
            if (FTPCommon.isRestoreInitial(null)) {
                //new DataReceive(context, systemdata, true, new FTPDataRcvResultProc()).execute("ftp.increase.main.jp", "21", "main.jp-increase", "sqG14Uwv",
                //        Common.getPrimaryAccount(context), "initial");
            }
        }

        public static void downloadServerData2(SystemData systemdata) {
            // FTPサーバー上に過去の点検データがある場合は、それを取得する
            //DataReceive.wait = true;
            //new DataReceive(context, systemdata, true, new FTPDataRcvResultProc()).execute("ftp.increase.main.jp", "21", "main.jp-increase", "sqG14Uwv",
            //        Common.getPrimaryAccount(context), "initial");
        }

        public static void downloadServerInformation(SystemData systemdata) {
            // FTPサーバー上に過去の点検データがある場合は、それを取得する
            //new DataReceive(context, systemdata, false, new FTPInformationRcvResultProc()).execute("ftp.increase.main.jp", "21", "main.jp-increase", "sqG14Uwv",
            //        "_Information", "dont initial");
        }

        public static void downloadServerRyokin(SystemData systemdata) {
            //new DataReceive(context, systemdata, false, new FTPInformationRcvResultProc()).execute("ftp.increase.main.jp", "21", "main.jp-increase", "sqG14Uwv",
            //        "_Ryokin", "dont initial");
        }

        public static void uploadServerData(SystemData systemdata) {
            //new DataSend(context).execute("ftp.increase.main.jp", "21", Common.getPrimaryAccount(context), "main.jp-increase", "sqG14Uwv", stringUtils.join(context.fileList(), ","), "true");
        }

        public static bool isShownDialog = false;
        public static void showInformationDialog() {
            string loadText;
            string[] text;

            if (isShownDialog == true)
                return; // 月報くんを起動してから一度はダイアログを表示した場合は表示しない
            isShownDialog = true;

            try {
                loadText = DataManager.loadText("information.txt");
                text = loadText.Split("\r\n");
                for (int i = 0; i < text.Length; i++) {
                    i = Information.setInformation(text, i);
                }
            } catch (IOException e) {
                //e.printStackTrace();
            }

            if (InformationActivity.informations != null && InformationActivity.informations[0].text != null) {
                string message = "";
                foreach (Information informations in InformationActivity.informations) {
                    message += informations.getInformation() + "\r\n\r\n";
                }
                message = message.Substring(0, message.Length - 4);
                // メッセージをダイアログに常時する
                //activity.setInformationDialog(activity, message);
            }
        }

        public static int getNenjiTenkenInfo(JigyousyoData jigyousyoData) {
            string ym = makeYMD();
            if (ym.Length != 11)
                return 0;
            string momth = ym.Substring(5, 7);
            if (Common.isNumeric(momth) == false)
                return 0;
            int m = int.Parse(momth);
            if (jigyousyoData.nentenken[m - 1] == true) {
                return 1;
            }
            if (m == 12)
                m = 1;
            else
                m++;
            if (jigyousyoData.nentenken[m - 1] == true) {
                return 2;
            }

            return 0;
        }

        public static int getGetujiTenkenInfo(JigyousyoData jigyousyoData, string ym) {
            if (ym.Length != 11)
                return 0;
            string momth = ym.Substring(5, 7);
            if (Common.isNumeric(momth) == false) {
                return 0;
            }
            int m = int.Parse(momth);
            if (jigyousyoData.tukitenken[m - 1] == true) {
                return 1;
            }

            return 0;
        }

        public static int getGetujiTenkenInfo(JigyousyoData jigyousyoData) {
            string ym = makeYMD();
            if (ym.Length != 11)
                return 0;
            string momth = ym.Substring(5, 7);
            if (Common.isNumeric(momth) == false)
                return 0;
            int m = int.Parse(momth);
            if (jigyousyoData.tukitenken[m - 1] == true) {
                return 1;
            }

            return 0;
        }

        // 月額使用電力量/契約電力を計算する
        public static string getSiyoudenryokuDivKeiyakuDenryoku(Item Heikin, Item Keiyaku) {
            string ret = "";

            ret = Common.getMultiply(Common.getDivide(Heikin.text, Keiyaku.text, 2), "30", 2);


            //		Item genzaiNissuu = new Item(Common.getNissu(systemData.tenkenData.itemGenzaiKongetuHiduke.text, systemData.tenkenData.itemGenzaiSengetuHiduke.text),
            //			 	 systemData.tenkenData.itemGenzaiKongetuHiduke.color, systemData.tenkenData.itemGenzaiKongetuHiduke.bgcolor);
            //		Item kensinNissuu = new Item(Common.getNissu(systemData.tenkenData.itemKensinKongetuHiduke.text, systemData.tenkenData.itemKensinSengetuHiduke.text),
            //				 systemData.tenkenData.itemKensinSengetuHiduke.color, systemData.tenkenData.itemKensinSengetuHiduke.bgcolor);
            //
            //		if(DataManager.equals(systemData.tenkenData.itemCheckboxYusen, "1")){
            //			ret = Common.divide(getText(systemData.tenkenData.itemGenzaiDenryokuryou),
            //								Common.multiply(genzaiNissuu.text, getText(systemData.tenkenData.itemKeiyakuDenryoku)));
            //		}else{
            //			ret = Common.divide(getText(systemData.tenkenData.itemKensinDenryokuryou),
            //								Common.multiply(kensinNissuu.text, getText(systemData.tenkenData.itemKeiyakuDenryoku)));
            //		}

            return ret;
        }

        // SDカードに点検データを書き出す
        //public static void writeToSDCard() {
        //    try {
        //        if (Common.getMountSDCard() != null) {
        //            //ファイルの一覧を検索するディレクトリパスを指定する
        //            string path = Common.getSystemPath();
        //            string[] files = Directory.GetFiles(path, "*");
        //                    Array.Sort(files);

        //            if (Directory.Exists(Common.getMountSDCard() + "/geppou/data")) {
        //                //System.out.println("ファイルは存在します");
        //            } else {
        //                //System.out.println("ファイルは存在しません");
        //                Directory.CreateDirectory(Common.getMountSDCard() + "/geppou/data");
        //            }

        //            foreach (var file in files) {
        //                FileCopy(file, pathSDCard + "/" + file.getName());
        //}
        //			}
        //		} catch (Exception e) {
        //    Log.e(TAG, "Unable to write to file");
        //    return; // ファイルが見つからない場合
        //}
        //	}

        public static void setHyuosiJigyoujyouName(SystemData systemData) {
            if (isEmpty(systemData.tenkenData.hyousiData.itemAtena)) {
                systemData.tenkenData.hyousiData.itemAtena = new Item("");
            }
            if (isEmpty(systemData.tenkenData.hyousiData.itemJigyoujyoumei)) {
                systemData.tenkenData.hyousiData.itemJigyoujyoumei = new Item("");
            }
            if (systemData.tenkenData.hyousiData.itemAtena.text == "") {
                systemData.tenkenData.hyousiData.itemAtena.text = systemData.jigyousyoData.textTitle + " " + systemData.jigyousyoData.textSubTitle;
            }
            if (systemData.tenkenData.hyousiData.itemJigyoujyoumei.text == "") {
                systemData.tenkenData.hyousiData.itemJigyoujyoumei.text = systemData.jigyousyoData.textTitle + " " + systemData.jigyousyoData.textSubTitle;
            }
        }

        public static void setKensinYMD(SystemData systemData) {
            //フォーマットパターンを指定して表示する
            DateTime datetime = DateTime.Now;
            if (DataManager.isEmpty(systemData.tenkenData.itemGenzaiKongetuHiduke)) {
                systemData.tenkenData.itemGenzaiKongetuHiduke = new Item();
                systemData.tenkenData.itemGenzaiKongetuHiduke.text = datetime.ToString("yyyy年MM月dd日");
                //System.out.println(sdf.format(cal.getTime()));
            }
            datetime = datetime.AddMonths(-1);
            if (DataManager.isEmpty(systemData.tenkenData.itemGenzaiSengetuHiduke)) {
                systemData.tenkenData.itemGenzaiSengetuHiduke = new Item();
                systemData.tenkenData.itemGenzaiSengetuHiduke.text = datetime.ToString("yyyy年MM月dd日");
                //System.out.println(sdf.format(cal.getTime()));
            }
            datetime = DateTime.Now;
            if (DataManager.isEmpty(systemData.tenkenData.itemKensinKongetuHiduke)) {
                int year = datetime.Year;        // 現在の年を取得
                int month = datetime.Month;  // 現在の月を取得
                int day = datetime.Day;
                if (!DataManager.isEmpty(systemData.tenkenData.itemKensinbi)) {
                    day = int.Parse(systemData.tenkenData.itemKensinbi.text);
                }
                systemData.tenkenData.itemKensinKongetuHiduke = new Item();
                systemData.tenkenData.itemKensinKongetuHiduke.text = string.Format("{0:0000}年{1:00}月{2:00}日", year, month, day);
                // 日付が正しくない時は補正する(2014/02/31 → 2014/02/29)
                if (!Common.checkDate(systemData.tenkenData.itemKensinKongetuHiduke.text)) {
                    day = int.Parse(Common.getLastDay(systemData.tenkenData.itemKensinKongetuHiduke.text));
                    systemData.tenkenData.itemKensinKongetuHiduke.text = string.Format("{0:0000}年{1:00}月{2:00}日", year, month, day);
                }
            }
            datetime = datetime.AddMonths(-1);            // 先月の月を取得
            if (DataManager.isEmpty(systemData.tenkenData.itemKensinSengetuHiduke)) {
                int year = datetime.Year;        // 現在の年を取得
                int month = datetime.Month;  // 現在の月を取得
                int day = datetime.Day;
                if (!DataManager.isEmpty(systemData.tenkenData.itemKensinbi)) {
                    day = int.Parse(systemData.tenkenData.itemKensinbi.text);
                }
                systemData.tenkenData.itemKensinSengetuHiduke = new Item();
                systemData.tenkenData.itemKensinSengetuHiduke.text = string.Format("{0:0000}年{1:00}月{2:00}日", year, month, day);
                // 日付が正しくない時は補正する(2014/02/31 → 2014/02/29)
                if (!Common.checkDate(systemData.tenkenData.itemKensinSengetuHiduke.text)) {
                    day = int.Parse(Common.getLastDay(systemData.tenkenData.itemKensinKongetuHiduke.text));
                    systemData.tenkenData.itemKensinKongetuHiduke.text = string.Format("{0:0000}年{1:00}月{2:00}日", year, month, day);
                }
                //System.out.println(sdf.format(cal.getTime()));
            }
        }


        public static void setKensinYMDDenryokuryou(SystemData systemData) {
            //フォーマットパターンを指定して表示する
            DateTime datetime = DateTime.Now;
            if (DataManager.isEmpty(systemData.tenkenData.itemGenzaiKongetuHiduke)) {
                systemData.tenkenData.itemGenzaiKongetuHiduke = new Item();
                systemData.tenkenData.itemGenzaiKongetuHiduke.text = datetime.ToString("yyyy年MM月dd日");
                //System.out.println(sdf.format(cal.getTime()));
            }
            datetime = datetime.AddMonths(-1);            // 先月の月を取得
            if (DataManager.isEmpty(systemData.tenkenData.itemGenzaiSengetuHiduke)) {
                systemData.tenkenData.itemGenzaiSengetuHiduke = new Item();
                systemData.tenkenData.itemGenzaiSengetuHiduke.text = datetime.ToString("yyyy年MM月dd日");
                // System.out.println(sdf.format(cal.getTime()));
            }
            datetime = DateTime.Now;
            int year = datetime.Year;        // 現在の年を取得
            int month = datetime.Month;  // 現在の月を取得
            int day = datetime.Day;
            if (!DataManager.isEmpty(systemData.tenkenData.itemKensinbi)) {
                day = int.Parse(systemData.tenkenData.itemKensinbi.text);
            }
            systemData.tenkenData.itemKensinKongetuHiduke = new Item();
            systemData.tenkenData.itemKensinKongetuHiduke.text = string.Format("{0:0000}年{1:00}月{2:00}日", year, month, day);
            // 日付が正しくない時は補正する(2014/02/31 → 2014/02/29)
            if (!Common.checkDate(systemData.tenkenData.itemKensinKongetuHiduke.text)) {
                day = int.Parse(Common.getLastDay(systemData.tenkenData.itemKensinKongetuHiduke.text));
                systemData.tenkenData.itemKensinKongetuHiduke.text = string.Format("{0:0000}年{1:00}月{2:00}日", year, month, day);
            }
            //System.out.println(sdf.format(cal.getTime()));
        }

        public static string getYYMM(Item itemData) {
            Item workData = itemData.clone();
            if (workData == null || workData.text == null)
                return "";
            string strDate = workData.text;
            strDate = strDate.Replace("年", "/").Replace("月", "");
            string[] MMDD;
            MMDD = strDate.Split("/");
            if (MMDD.Length != 2)
                return "";
            return MMDD[0].Substring(2, 4) + "/" + MMDD[1];
        }

        public static string getMM(Item itemData) {
            Item workData = itemData.clone();
            if (workData == null)
                return "";
            string strDate = workData.text;
            strDate = strDate.Replace("年", "/").Replace("月", "");
            string[] MMDD;
            MMDD = strDate.Split("/");
            if (MMDD.Length != 2)
                return "";
            return MMDD[1];
        }

        public static string getMMDD(Item itemData) {
            Item workData = itemData.clone();
            if (workData == null)
                return "";
            string strDate = workData.text;
            strDate = strDate.Replace("年", "/").Replace("月", "/").Replace("日", "");
            string[] MMDD;
            MMDD = strDate.Split("/");
            if (MMDD.Length != 3)
                return "";
            return MMDD[1] + "月" + MMDD[2] + "日";
        }

        public static string getYYMMWarekiNengou(SystemData systemData, WarekiData _wareki, Item itemDate) {
            Item workData = itemDate.clone();
            if (workData == null)
                return "";
            workData.text = workData.text.Replace("年", "/").Replace("月", "/").Replace("日", "");
            string[] YYYYMM = workData.text.Split("/");
            if (YYYYMM.Length != 2)
                return "";
            string Nengo = Common.getNengouAlphabet(_wareki, YYYYMM[0], YYYYMM[1], "01");
            if (workData.text.Substring(workData.text.Length - 1, workData.text.Length) == "/") {
                workData.text += "01";
            }
            string wareki = Common.getWareki(systemData, workData.text);
            return Nengo + " " + wareki + "年" + YYYYMM[1] + "月";
        }

        public static string getPlus(Item itemNum1, Item itemNum2, Item itemNum3, Item itemNum4) {
            if (DataManager.isEmpty(itemNum1) == true)
                return "";
            if (DataManager.isEmpty(itemNum2) == true)
                return "";
            if (DataManager.isEmpty(itemNum3) == true)
                return "";
            if (DataManager.isEmpty(itemNum4) == true)
                return "";
            return Common.getPlus(itemNum1.text, itemNum2.text, itemNum3.text, itemNum4.text);
        }

        public static string getMinus(Item itemNum1, Item itemNum2) {
            if (DataManager.isEmpty(itemNum1) == true)
                return "";
            if (DataManager.isEmpty(itemNum2) == true)
                return "";
            return Common.getMinus(itemNum1.text, itemNum2.text);
        }

        public static string getMinusKenshin(Item itemNum1, Item itemNum2) {
            if (DataManager.isEmpty(itemNum1) == true)
                return "";
            if (DataManager.isEmpty(itemNum2) == true)
                return "";

            string retstring = "";
            itemNum1.text = itemNum1.text.Trim();
            itemNum2.text = itemNum2.text.Trim();
            itemNum1.text = itemNum1.text.Replace(",", "");
            itemNum2.text = itemNum2.text.Replace(",", "");
            if (isNumeric(itemNum1) && isNumeric(itemNum2)) {
                decimal dec1 = decimal.Parse(itemNum1.text), dec2 = decimal.Parse(itemNum2.text);
                decimal dec = dec1 - dec2;
                if (dec1.CompareTo(dec2) < 0) {
                    int keta = (dec2 == 0) ? 1 : ((int)Math.Log10((double)dec2) + 1);
                    decimal maxKenshinti = (decimal)Math.Pow(10, keta);
                    dec = maxKenshinti - dec2;
                    dec = dec + dec1;
                }
                retstring = dec.ToString();
            }
            return retstring;
        }

        public static string getMeterReset(Item itemNum1, Item itemNum2) {
            if (DataManager.isEmpty(itemNum1) == true)
                return "";
            if (DataManager.isEmpty(itemNum2) == true)
                return "";

            string retstring = "";
            itemNum1.text = itemNum1.text.Trim();
            itemNum2.text = itemNum2.text.Trim();
            itemNum1.text = itemNum1.text.Replace(",", "");
            itemNum2.text = itemNum2.text.Replace(",", "");
            if (isNumeric(itemNum1) && isNumeric(itemNum2)) {
                decimal dec1 = decimal.Parse(itemNum1.text), dec2 = decimal.Parse(itemNum2.text);
                decimal dec = dec1 - dec2;
                if (dec1.CompareTo(dec2) < 0) {
                    int keta = (dec2 == 0) ? 1 : ((int)Math.Log10((double)dec2) + 1);
                    decimal maxKenshinti = (decimal)Math.Pow(10, keta);
                    dec = maxKenshinti - dec2;
                }
                retstring = dec.ToString();
            }
            return retstring;
        }

        public static string getMultiply(Item itemNum1, Item itemNum2, int keta) {
            if (DataManager.isEmpty(itemNum1) == true)
                return "0";
            if (DataManager.isEmpty(itemNum2) == true)
                return "0";
            if (isZero(itemNum1) || isZero(itemNum2))
                return "0";
            return Common.getMultiply(itemNum1.text, itemNum2.text, keta);
        }

        public static string makeFukaritu(Item itemR, Item itemS, Item itemT, Item itemTeikaku) {
            Item iR = itemR.clone();
            Item iS = itemS.clone();
            Item iT = itemT.clone();
            Item iTeikaku = itemTeikaku.clone();
            if (DataManager.isEmpty(iR) == true)
                iR = new Item("0");
            if (DataManager.isEmpty(iS) == true)
                iS = new Item("0");
            if (DataManager.isEmpty(iT) == true)
                iT = new Item("0");
            if (DataManager.isEmpty(iTeikaku) == true)
                return "";

            long r = Common.toLong(iR.text);
            long s = Common.toLong(iS.text);
            long t = Common.toLong(iT.text);
            double teikaku = Common.toDouble(iTeikaku.text);
            double maxDenryu = (double)Common.getMaxData(r, s, t);
            double fukaritu = 0.0;
            if (teikaku != 0) {
                fukaritu = maxDenryu / teikaku * 100.0;
            } else {
                return "";
            }
            decimal bd = (decimal)fukaritu;
            bd = Common.Round(bd, -2); // 小数点第2位を四捨五入

            return bd.ToString("#,###.#");
        }

        public static string getDivide(Item itemNum1, Item itemNum2, int keta) {
            if (DataManager.isEmpty(itemNum1) == true)
                return "0";
            if (DataManager.isEmpty(itemNum2) == true)
                return "0";
            if (isZero(itemNum1) || isZero(itemNum2))
                return "0";
            return Common.getDivide(itemNum1.text, itemNum2.text, keta);
        }

        public static string getRikiritu(SystemData systemData) {
            string retRikiritu = "";

            if (systemData.settei.flagRikirituSettei == true) { // 力率自動計算に設定されている場合は、今月力率を消す
                if (DataManager.equals(systemData.tenkenData.itemCheckboxYusen, "2")) { // 現在値を印刷する
                    retRikiritu = getDivide(systemData.tenkenData.itemGenzaiKongetuMukoDenryoku, systemData.tenkenData.itemGenzaiKongetuYukoDenryoku, 4);
                } else {
                    retRikiritu = getDivide(systemData.tenkenData.itemKensinKongetuMukoDenryoku, systemData.tenkenData.itemKensinKongetuYukoDenryoku, 4);
                }
                if (0 <= retRikiritu.CompareTo("0.0000") && retRikiritu.CompareTo("0.1004") <= 0)
                    retRikiritu = "100";
                else if (0 <= retRikiritu.CompareTo("0.1005") && retRikiritu.CompareTo("0.1752") <= 0)
                    retRikiritu = "99";
                else if (0 <= retRikiritu.CompareTo("0.1753") && retRikiritu.CompareTo("0.2279") <= 0)
                    retRikiritu = "98";
                else if (0 <= retRikiritu.CompareTo("0.2280") && retRikiritu.CompareTo("0.2718") <= 0)
                    retRikiritu = "97";
                else if (0 <= retRikiritu.CompareTo("0.2719") && retRikiritu.CompareTo("0.3106") <= 0)
                    retRikiritu = "96";
                else if (0 <= retRikiritu.CompareTo("0.3107") && retRikiritu.CompareTo("0.3461") <= 0)
                    retRikiritu = "95";
                else if (0 <= retRikiritu.CompareTo("0.3462") && retRikiritu.CompareTo("0.3793") <= 0)
                    retRikiritu = "94";
                else if (0 <= retRikiritu.CompareTo("0.3794") && retRikiritu.CompareTo("0.4108") <= 0)
                    retRikiritu = "93";
                else if (0 <= retRikiritu.CompareTo("0.4109") && retRikiritu.CompareTo("0.4409") <= 0)
                    retRikiritu = "92";
                else if (0 <= retRikiritu.CompareTo("0.4410") && retRikiritu.CompareTo("0.4701") <= 0)
                    retRikiritu = "91";
                else if (0 <= retRikiritu.CompareTo("0.4702") && retRikiritu.CompareTo("0.4984") <= 0)
                    retRikiritu = "90";
                else if (0 <= retRikiritu.CompareTo("0.4985") && retRikiritu.CompareTo("0.5261") <= 0)
                    retRikiritu = "89";
                else if (0 <= retRikiritu.CompareTo("0.5262") && retRikiritu.CompareTo("0.5533") <= 0)
                    retRikiritu = "88";
                else if (0 <= retRikiritu.CompareTo("0.5534") && retRikiritu.CompareTo("0.5801") <= 0)
                    retRikiritu = "87";
                else if (0 <= retRikiritu.CompareTo("0.5802") && retRikiritu.CompareTo("0.6066") <= 0)
                    retRikiritu = "86";
                else if (0 <= retRikiritu.CompareTo("0.6067") && retRikiritu.CompareTo("0.6329") <= 0)
                    retRikiritu = "85";
                else
                    retRikiritu = "85";
                if (DataManager.equals(systemData.tenkenData.itemCheckboxYusen, "2")) { // 現在値を印刷する
                    systemData.tenkenData.itemGenzaiKongetuRikiritu.text = retRikiritu;
                } else {
                    systemData.tenkenData.itemKensinKongetuRikiritu.text = retRikiritu;
                }
            } else {
                retRikiritu = systemData.tenkenData.itemKensinKongetuRikiritu.text;
            }
            return retRikiritu;
        }

        public static void execSaidaiDenryoku(SystemData systemData) {
            string saidaiDenryoku = DataManager.getMultiply(
                    systemData.tenkenData.itemKensinKongetuSaidaiDenryoku,
                    systemData.tenkenData.itemJyouritu, 0);
            saidaiDenryoku = DataManager.round(saidaiDenryoku, DataManager.ROUND_OFF);
            //		saidaiDenryoku = Common.formatDecimal(saidaiDenryoku);
            long lSaidaiDenryoku = Common.toLong(saidaiDenryoku);
            long lSaidaiDenryokuNow = 0L;
            if (!isEmpty(systemData.tenkenData.itemKeiyakuDenryoku)) {
                lSaidaiDenryokuNow = Common.toLong(systemData.tenkenData.itemKeiyakuDenryoku.text);
            }

            // 現在の契約電力より、今月のデマンドが大きかった場合
            if (lSaidaiDenryokuNow < lSaidaiDenryoku) {
                systemData.tenkenData.itemKeiyakuDenryoku.text = saidaiDenryoku;
                if (DataManager.isEmpty(systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM)) {
                    systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM = new Item();
                }
                if (DataManager.isEmpty(systemData.tenkenData.itemTenkenNijtijiYMD)) {
                    return;
                }
                systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM.text = Common.makeYYMM(Common.toCalendar(systemData.tenkenData.itemTenkenNijtijiYMD.text));
            }
            // これ以降、昨年同月から一年経過して、昨年の契約電力のままということは、一年経過した時点で契約電力が下がるという処理
            if (DataManager.isEmpty(systemData.tenkenData.itemTenkenNijtijiYMD)) {
                return;
            }
            if (systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM.text.Length == 0 || systemData.tenkenData.itemTenkenNijtijiYMD.text.Length == 0) {
                return;
            }
            DateTime calSaidai = Common.toCalendar(systemData.tenkenData.itemTenkenNijtijiYMD.text);
            calSaidai = calSaidai.AddYears(-1);
            string SaidaiYM_minus_1 = Common.makeYYMM(calSaidai);
            string SaidaiYM = Common.makeYYMM(Common.toCalendar(systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM.text + "/01"));
            long KeiyakuDenryoku = Common.toLong(saidaiDenryoku);
            //		systemData.tenkenData.itemKeiyakuDenryoku.text = saidaiDenryoku;
            //		systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM.text = Common.makeYYMM(jp.increase.Billing.Common.toCalendar(systemData.tenkenData.itemTenkenNijtijiYMD.text));
            if (SaidaiYM == SaidaiYM_minus_1 || SaidaiYM.CompareTo(SaidaiYM_minus_1) < 0) {
                for (int i = 0; i < 12 - 2; i++) { // 過去11ヶ月のうち一番高いデマンドを設定する
                    if (KeiyakuDenryoku < Common.toLong(systemData.tenkenData.textKakoDemandData[i])) {
                        KeiyakuDenryoku = Common.toLong(systemData.tenkenData.textKakoDemandData[i]);
                        systemData.tenkenData.itemKeiyakuDenryoku.text = systemData.tenkenData.textKakoDemandData[i];
                        systemData.tenkenData.itemKeiyakuDenryokuKakuteiYM.text = systemData.tenkenData.textKakoTuki[i];
                    }
                }
            }
        }

        public static SystemData setFreeFormatFileName(SystemData systemData) {
            if (Common.isEmpty(systemData.jigyousyoData.textCyouhyouFreeFormatFileName) != true) {
                systemData.tenkenData.textCyouhyouFreeFormatFlag = "1";
                systemData.tenkenData.textCyouhyouFreeFormatFileName = systemData.settei.textCyouhyouFreeFormatFileName;
                systemData.jigyousyoData.textCyouhyouFreeFormatFileName = ""; // 初期化する
                DataManager.writeJigyousyo(systemData.listJigyousyo);
                //	        DataManager.writeTenken(context, systemData, systemData.tenkenFileName);
            }
            return systemData;
        }
    }

    // 点検データの並べ替えに使用する
    //class TenkenDataComparator implements Comparator<File> {

    //    @Override

    //    public int compare(File arg0, File arg1) { // 同じファイル名はありえないので比較処理しない
    //    int n1 = arg0.getName().length();
    //    int n2 = arg1.getName().length();
    //    return n1 < n2 ? 1 : -1;
    //}
    //}
}
