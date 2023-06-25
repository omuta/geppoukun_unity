using PreGeppou.Data;
using PreGeppou.Framework.Data;
using PreGeppou.Framework.Keiyaku;
using PreGeppou.Keiyaku;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PreGeppou.Framework {
    internal class RyokinExcel {
        readonly static int start_row = 6;
        readonly static int max_row = 100;
        readonly static int title_col = 0;
        readonly static int startCyousei_col = 7;
        readonly static int nenryo1_col = 9;
        readonly static int nenryo2_col = 10;
        readonly static int taiyou_col = 11;
        readonly static int sinene_col = 12;
        readonly static int mode_col = 1;
        readonly static int kihon_col = 2;
        readonly static int denryokuryo_col = 3;
        readonly static int kihon_row = 6;
        static Ryokin ryokin;

        public static string getFileName(DenryokugaisyaData DenryokuGaisya) {
            string fileName = "";
            if (DenryokuGaisya.getTextDenryokugaisyaMei() == "東京電力") {
                fileName = "ryokin_tokio.xls";
            }
            return fileName;
        }

        public static string getFileName(string DenryokuGaisya) {
            string fileName = "";
            if (DenryokuGaisya == "東京電力") {
                fileName = "ryokin_tokio.xls";
            }
            return fileName;
        }

        public static bool readRyoukin(DenryokugaisyaData DenryokuGaisya, List<Ryokin> listRyokin) {
            return readRyoukin(getFileName(DenryokuGaisya), listRyokin);
        }

        public static bool readRyoukin(string DenryokuGaisya, List<Ryokin> listRyokin) {
            ryokin = new Ryokin();
            Workbook workbook = null;
            HSSFSheet sheet = null;
            try {
                if (!File.Exists(Common.getSystemPath() + "/" + DenryokuGaisya)) {
                    return false;
                }
                workbook = WorkbookFactory.create(File.ReadAllText(Common.getSystemPath() + "/" + DenryokuGaisya));
            } catch (InvalidFormatException e) {
                //e.printStackTrace();
                return false;
            } catch (IOException e) {
                //e.printStackTrace();
                return false;
            } catch (Exception e) {
                //e.printStackTrace();
                return false;
            }
            for (int sheetCnt = 0; sheetCnt < workbook.getNumberOfSheets(); sheetCnt++) {
                sheet = (HSSFSheet)workbook.getSheetAt(sheetCnt);
                DateTime date = getKikanStart(sheet);
                if (date == null)
                    continue;
                ryokin.setKikanStart(date);
                for (int cellRow = start_row; cellRow < max_row; cellRow++) {
                    int mode = getMode(sheet, cellRow);
                    string title = getTitle(sheet, cellRow);
                    if (title == string.Empty)
                        break;
                    decimal kihonRyoukin = getKihonRyokin(sheet, cellRow);
                    decimal[] ryoukin = getDenryokuryoRyokin(sheet, cellRow, mode);
                    switch (mode) {
                        case 1:
                            ryokin.setKeiyakusyubetu(new KouatsuKeiyaku(title, null, kihonRyoukin, ryoukin, "", null, null, null));
                            break;
                        case 2:
                            ryokin.setKeiyakusyubetu(new KijibetsuKeiyaku(title, null, kihonRyoukin, ryoukin, "", null, null, null));
                            break;
                        case 3:
                            ryokin.setKeiyakusyubetu(new KyujitsuKeiyaku(title, null, kihonRyoukin, ryoukin, "", null, null, null));
                            break;
                    }
                }
            }
            listRyokin.Add(ryokin);
            return true;
        }

        // 適用開始の日を取得
        public static DateTime getKikanStart(HSSFSheet sheet) {
            DateTime? date;
            try {
                string sheetname = sheet.getSheetName();
                date = Common.toDate(sheetname);
            } catch (Exception e) {
                date = null;
            }
            return (DateTime)date;
        }

        // 契約種別取得
        public static int getMode(HSSFSheet sheet, int cellRow) {
            string mode = "0";
            try {
                mode = getCellData(sheet, mode_col, cellRow);
            } catch (Exception e) {
                mode = "1";
            }
            if (Common.toInteger(mode) == 0) {
                mode = "1";
            }
            return Common.toInteger(mode);
        }

        // 契約種別タイトル取得
        public static string getTitle(HSSFSheet sheet, int cellRow) {
            string title = "0";
            try {
                title = sheet.getRow(cellRow).getCell(title_col).getStringCellValue();
            } catch (Exception e) {
                title = "";
            }
            return title;
        }

        // 契約種別ごとの基本料金
        public static decimal getKihonRyokin(HSSFSheet sheet, int cellRow) {
            decimal? ryokin = null;
            //		cellRow = kihon_row + (cellRow - kihon_row) / 3;
            try {
                ryokin = getCellDataBydecimal(sheet, kihon_col, cellRow);
            } catch (Exception e) {
                ryokin = new decimal(0);
            }
            return (decimal)ryokin;
        }

        // 契約種別ごとの電力量料金
        public static decimal[] getDenryokuryoRyokin(HSSFSheet sheet, int cellRow, int mode) {
            decimal? ryokin1 = null;
            decimal? ryokin2 = null;
            decimal? ryokin3 = null;
            decimal? ryokin4 = null;
            try {
                switch (mode) {
                    case 1:
                        ryokin1 = getCellDataBydecimal(sheet, denryokuryo_col, cellRow);
                        ryokin2 = getCellDataBydecimal(sheet, denryokuryo_col + 2, cellRow);
                        ryokin3 = new decimal(0);
                        ryokin4 = new decimal(0);
                        break;
                    case 2:
                        ryokin1 = getCellDataBydecimal(sheet, denryokuryo_col, cellRow);
                        ryokin2 = getCellDataBydecimal(sheet, denryokuryo_col + 1, cellRow);
                        ryokin3 = getCellDataBydecimal(sheet, denryokuryo_col + 2, cellRow);
                        ryokin4 = getCellDataBydecimal(sheet, denryokuryo_col + 3, cellRow);
                        break;
                    case 3:
                        ryokin1 = getCellDataBydecimal(sheet, denryokuryo_col, cellRow);
                        ryokin2 = getCellDataBydecimal(sheet, denryokuryo_col + 1, cellRow);
                        ryokin3 = getCellDataBydecimal(sheet, denryokuryo_col + 2, cellRow);
                        ryokin4 = getCellDataBydecimal(sheet, denryokuryo_col + 3, cellRow);
                        break;
                }
            } catch (Exception e) {
                ryokin1 = new decimal(0);
                ryokin2 = new decimal(0);
                ryokin3 = new decimal(0);
                ryokin4 = new decimal(0);
            }
            decimal[] ryokin = { (decimal)ryokin1, (decimal)ryokin2, (decimal)ryokin3, (decimal)ryokin4 };
            return ryokin;
        }

        // 調整費の適用開始日取得
        public static CyouseiTanka getCyouseiKikan(HSSFSheet sheet, int cellRow) {
            CyouseiTanka tanka = new CyouseiTanka();
            string strCyouseiKikan;
            try {
                tanka.start = getCellDataByDate(sheet, startCyousei_col, cellRow);
                tanka.nenryohi1 = getCellDataBydecimal(sheet, nenryo1_col, cellRow);
                tanka.nenryohi2 = getCellDataBydecimal(sheet, nenryo2_col, cellRow);
                tanka.taiyoukou = getCellDataBydecimal(sheet, taiyou_col, cellRow);
                tanka.shinEnergy = getCellDataBydecimal(sheet, sinene_col, cellRow);
            } catch (Exception e) {
                tanka = null;
            }
            if (tanka.start == null) {
                tanka = null;
            }
            return tanka;
        }

        public static string getCellData(HSSFSheet sheet, int cellCol, int cellRow) {
            string data = "";
            Cell cell = sheet.getRow(cellRow).getCell(cellCol);
            switch (cell.getCellType()) {
                case Cell.CELL_TYPE_STRING:
                    data = cell.getStringCellValue();
                    break;
                case Cell.CELL_TYPE_NUMERIC:
                    data = cell.getNumericCellValue().ToString();
                    break;
                case Cell.CELL_TYPE_BOOLEAN:
                    data = cell.getBooleanCellValue().ToString();
                    break;
                case Cell.CELL_TYPE_FORMULA:
                    // return cell.getCellFormula();
                    data = getstringFormulaValue(cell);
                    break;
                case Cell.CELL_TYPE_BLANK:
                    data = getstringRangeValue(cell);
                    break;
                default:
                    //System.out.println(cell.getCellType());
                    data = "0";
                    break;
            }
            if (Common.isNumeric(data) == false) {
                data = "0";
            }
            return data;
        }

        public static DateTime getCellDataByDate(HSSFSheet sheet, int cellCol, int cellRow) {
            Cell cell = sheet.getRow(cellRow).getCell(cellCol);
            var datetime = DateTime.Now;// DateTime.ParseExact(cell.getDateCellValue().toGMTString(), "dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
            return datetime;
        }

        public static decimal getCellDataBydecimal(HSSFSheet sheet, int cellCol, int cellRow) {
            string data = "";
            Cell cell = sheet.getRow(cellRow).getCell(cellCol);
            switch (cell.getCellType()) {
                case Cell.CELL_TYPE_STRING:
                    data = cell.getStringCellValue();
                    break;
                case Cell.CELL_TYPE_NUMERIC:
                    data = cell.getNumericCellValue().ToString();
                    break;
                case Cell.CELL_TYPE_BOOLEAN:
                    data = cell.getBooleanCellValue().ToString();
                    break;
                case Cell.CELL_TYPE_FORMULA:
                    // return cell.getCellFormula();
                    data = getstringFormulaValue(cell);
                    break;
                case Cell.CELL_TYPE_BLANK:
                    data = getstringRangeValue(cell);
                    break;
                default:
                    // System.out.println(cell.getCellType());
                    data = "0";
                    break;
            }
            if (Common.isNumeric(data) == false) {
                data = "0";
            }
            return decimal.Parse(data);
        }

        // 結合セルの値をstringとして取得する例
        public static string getstringRangeValue(Cell cell) {
            int rowIndex = cell.getRowIndex();
            int columnIndex = cell.getColumnIndex();

            Sheet sheet = cell.getSheet();
            int size = sheet.getNumMergedRegions();
            for (int i = 0; i < size; i++) {
                CellRangeAddress range = (CellRangeAddress)sheet.getMergedRegion(i);
                if (range.isInRange(rowIndex, columnIndex)) {
                    Cell firstCell = getCell(sheet, range.getFirstRow(), range.getFirstColumn()); // 左上のセルを取得
                    return getstringValue(firstCell);
                }
            }
            return null;
        }

        public static Cell getCell(Sheet sheet, int rowIndex, int columnIndex) {
            Row row = sheet.getRow(rowIndex);
            if (row != null) {
                Cell cell = row.getCell(columnIndex);
                return cell;
            }
            return null;
        }

        // セルの値をstringとして取得する例
        public static string getstringValue(Cell cell) {
            if (cell == null) {
                return null;
            }
            switch (cell.getCellType()) {
                case Cell.CELL_TYPE_STRING:
                    return cell.getStringCellValue();
                case Cell.CELL_TYPE_NUMERIC:
                    return cell.getNumericCellValue().ToString();
                case Cell.CELL_TYPE_BOOLEAN:
                    return cell.getBooleanCellValue().ToString();
                case Cell.CELL_TYPE_FORMULA:
                    // return cell.getCellFormula();
                    return getstringFormulaValue(cell);
                case Cell.CELL_TYPE_BLANK:
                    return getstringRangeValue(cell);
                default:
                    // System.out.println(cell.getCellType());
                    return null;
            }
        }

        // セルの数式を計算し、stringとして取得する例
        public static string getstringFormulaValue(Cell cell) {
            //assert cell.getCellType() == Cell.CELL_TYPE_FORMULA;

            Workbook book = cell.getSheet().getWorkbook();
            CreationHelper helper = book.getCreationHelper();
            FormulaEvaluator evaluator = helper.createFormulaEvaluator();
            CellValue value = evaluator.evaluate(cell);
            switch (value.getCellType()) {
                case Cell.CELL_TYPE_STRING:
                    return value.getStringValue();
                case Cell.CELL_TYPE_NUMERIC:
                    return value.getNumberValue().ToString();
                case Cell.CELL_TYPE_BOOLEAN:
                    return value.getBooleanValue().ToString();
                default:
                    //System.out.println(value.getCellType());
                    return null;
            }
        }

        [Serializable]
        private class InvalidFormatException : Exception {
            public InvalidFormatException() {
            }

            public InvalidFormatException(string message) : base(message) {
            }

            public InvalidFormatException(string message, Exception innerException) : base(message, innerException) {
            }

            protected InvalidFormatException(SerializationInfo info, StreamingContext context) : base(info, context) {
            }
        }
    }

    internal class WorkbookFactory {
        internal static Workbook create(object value) {
            throw new NotImplementedException();
        }
    }

    class HSSFSheet {
        public Cell getRow(int i) {
            return null;
        }
        public Cell getCell(int i) {
            return null;
        }

        internal string getSheetName() {
            throw new NotImplementedException();
        }
    }

    class Cell {
        public const int CELL_TYPE_STRING = 0;
        public const int CELL_TYPE_NUMERIC = 1;
        public const int CELL_TYPE_BOOLEAN = 2;
        public const int CELL_TYPE_FORMULA = 3;
        public const int CELL_TYPE_BLANK = 4;

        public int getCellType() {
            return 0;
        }
        public string getStringCellValue() {
            return string.Empty;
        }
        public string getNumericCellValue() {
            return string.Empty;
        }
        public string getBooleanCellValue() {
            return string.Empty;
        }
        public string getDateCellValue() {
            return string.Empty;
        }

        public Cell getRow() {
            return null;
        }
        public Sheet getSheet() {
            return null;
        }

        public Workbook getWorkbook() {
            return null;
        }

        internal int getColumnIndex() {
            throw new NotImplementedException();
        }

        internal Cell getCell(int cellCol) {
            throw new NotImplementedException();
        }

        internal int getRowIndex() {
            throw new NotImplementedException();
        }
    }

    class Workbook {
        public CreationHelper getCreationHelper() {
            return null;
        }

        internal int getNumberOfSheets() {
            throw new NotImplementedException();
        }

        internal HSSFSheet getSheetAt(int sheetCnt) {
            throw new NotImplementedException();
        }
    }
    class CreationHelper {
        public FormulaEvaluator createFormulaEvaluator() {
            return null;
        }
    }

    class FormulaEvaluator {
        public CellValue evaluate(Cell cell) {
            return null;
        }
    }

    class CellValue {
        public int getCellType() {
            return 0;
        }

        public string getStringCellValue() {
            return string.Empty;
        }

        internal object getBooleanValue() {
            throw new NotImplementedException();
        }

        internal object getNumberValue() {
            throw new NotImplementedException();
        }

        internal string getStringValue() {
            throw new NotImplementedException();
        }
    }
    internal class Sheet {
        public int getNumMergedRegions() {
            throw new NotImplementedException();
        }

        public Workbook getWorkbook() {
            throw new NotImplementedException();
        }

        internal CellRangeAddress getMergedRegion(int i) {
            throw new NotImplementedException();
        }

        internal Row getRow(int rowIndex) {
            throw new NotImplementedException();
        }
    }

    internal class CellRangeAddress {
        internal int getFirstColumn() {
            throw new NotImplementedException();
        }

        internal int getFirstRow() {
            throw new NotImplementedException();
        }

        internal bool isInRange(int rowIndex, int columnIndex) {
            throw new NotImplementedException();
        }
    }

    internal class Row {
        internal Cell getCell(int columnIndex) {
            throw new NotImplementedException();
        }
    }
}
