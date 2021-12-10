using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework.Helpers
{
    public class NPOIHelper
    {
        public static List<string> ReadExcel(string filePath)
        {

            List<string> rowList = new List<string>();
            ISheet sheet;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                sheet = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                rowList.Add(row.GetCell(j).ToString());
                            }
                        }
                    }

                }
            }
            return rowList;
        }
       
        public static string ReadExcel(string filePath, int rowCell, int columnCell)
        {
            string content = null;
            try
            {
                List<string> rowList = new List<string>();
                ISheet sheet;
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.Position = 0;
                    XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                    sheet = xssWorkbook.GetSheetAt(0);
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;
                    IRow row = sheet.GetRow(rowCell);
                    content = row.GetCell(columnCell).ToString();
                }

            }
            catch(IOException ex)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException(String.Format("{0} does not exist.\r\n Stack Trace:\r\n {1)", filePath, ex.StackTrace));
            }
            catch(Exception ex)
            {
                throw new Exception(String.Format("Problem occurred in setting property of package. " +
                    "Check properties use to create FileStream. \r\n Current Exception occurred {0}. \r\n " +
                    "Stack trace: \r\n {1}", ex.GetType().Name, ex.StackTrace));
            }
            

            return content;
        }

    }
}
