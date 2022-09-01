using IZhy.Common.BasicTools;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 一个简单的 Excel 工具类
    /// </summary>
    public static class ExcelTool
    {
        /// <summary>
        /// 将 DataTable 数据导入到 Excel 中
        /// </summary>
        /// <param name="dataTable">要导入的数据</param>
        /// <param name="filePath">Excel 文件的绝对路径></param>
        /// <param name="sheetName">Excel 的 sheet 名称，默认：sheet01</param>
        /// <param name="isWriteColumnName">是否写入列名，默认：是 true</param>
        /// <returns>返回导入成功的数据行数；返回 -1 表示失败或异常</returns>
        public static async Task<int> DataTableToExcelAsync(DataTable dataTable,
                                                            string filePath,
                                                            string sheetName = "sheet01",
                                                            bool isWriteColumnName = true)
        {
            return await Task.Run(() =>
            {
                IWorkbook workbook = null;
                try
                {
                    if (dataTable == null || dataTable.Rows.Count < 1 || string.IsNullOrWhiteSpace(filePath))
                    {
                        return -1;
                    }
                    if (string.IsNullOrWhiteSpace(sheetName))
                    {
                        sheetName = "sheet01";
                    }

                    #region 文件判断
                    try
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        }
                    }
                    catch (Exception)
                    {
                        return -1;
                    }

                    if (Path.GetExtension(filePath) == ".xlsx")
                    {
                        workbook = new XSSFWorkbook();
                    }
                    else if (Path.GetExtension(filePath) == ".xls")
                    {
                        workbook = new HSSFWorkbook();
                    }
                    else
                    {
                        return -1;
                    }
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    #endregion

                    int count;  //写入 Excel 的实际行数
                    int r;      //行
                    int c;      //列

                    #region 写入 dataTable 的列名，写入单元格中
                    if (isWriteColumnName)
                    {
                        var row = sheet.CreateRow(0);
                        for (c = 0; c < dataTable.Columns.Count; ++c)
                        {
                            row.CreateCell(c).SetCellValue(dataTable.Columns[c].ColumnName);
                        }
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }
                    #endregion

                    #region 遍历循环 dataTable 具体数据项
                    for (r = 0; r < dataTable.Rows.Count; ++r)
                    {
                        var row = sheet.CreateRow(count);
                        for (c = 0; c < dataTable.Columns.Count; ++c)
                        {
                            row.CreateCell(c).SetCellValue(Convert.ToString(dataTable.Rows[r][c]));
                        }
                        ++count;
                    }
                    #endregion

                    #region 将文件流写入到 Excel
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fs);
                    }
                    #endregion

                    return count;
                }
                catch (Exception ex)
                {
                    LogsTool.WriteEXLog($"DataTable 导出至 Excel 发生异常{Environment.NewLine}==> {filePath}", ex);
                    return -1;
                }
                finally
                {
                    workbook?.Close(); workbook = null;
                    dataTable?.Dispose(); dataTable = null;
                    GC.Collect();
                }
            });
        }

        /// <summary>
        /// 将 Excel 中的数据导入到 DataTable 中
        /// </summary>
        /// <param name="filePath">Excel 文件的绝对路径</param>
        /// <param name="sheetName">Excel 工作薄 sheet 的名称；默认 获取第一个 sheet</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名；默认：true 是</param>
        /// <returns></returns>
        public static async Task<DataTable> ExcelToDataTableAsync(string filePath, string sheetName = null, bool isFirstRowColumn = true)
        {
            return await Task.Run(() =>
            {
                IWorkbook workbook = null;
                try
                {
                    if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                    {
                        return null;
                    }

                    var data = new DataTable();

                    ISheet sheet = null;

                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        #region 读取 Excel 文件
                        if (Path.GetExtension(filePath) == ".xlsx")
                        {
                            workbook = new XSSFWorkbook(fs);
                        }
                        else if (Path.GetExtension(filePath) == ".xls")
                        {
                            workbook = new HSSFWorkbook(fs);
                        }

                        if (workbook != null)
                        {
                            if (string.IsNullOrWhiteSpace(sheetName))
                            {
                                sheet = workbook.GetSheetAt(0);
                            }
                            else
                            {
                                //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                                sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
                            }
                        }

                        if (sheet == null)
                        {
                            return null;
                        }
                        #endregion

                        #region Excel 处理转成 DataTable
                        var firstRow = sheet.GetRow(0);
                        //一行最后一个cell的编号 即总的列数
                        int cellCount = firstRow.LastCellNum;
                        int startRow;
                        if (isFirstRowColumn)
                        {
                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                            {
                                var cell = firstRow.GetCell(i);
                                var cellValue = cell.StringCellValue;
                                if (cellValue == null) continue;
                                var column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                            startRow = sheet.FirstRowNum + 1;
                        }
                        else
                        {
                            startRow = sheet.FirstRowNum;
                        }
                        //最后一列的标号
                        var rowCount = sheet.LastRowNum;
                        for (var i = startRow; i <= rowCount; ++i)
                        {
                            var row = sheet.GetRow(i);
                            //没有数据的行默认是null
                            if (row == null) continue;
                            var dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                //同理，没有数据的单元格都默认是null
                                if (row.GetCell(j) != null)
                                    dataRow[j] = Convert.ToString(row.GetCell(j));
                            }
                            data.Rows.Add(dataRow);
                        }
                        #endregion
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    LogsTool.WriteEXLog($"Excel 文件读取为 DataTable 发生异常{Environment.NewLine}==> {filePath}", ex);
                    return null;
                }
                finally
                {
                    workbook?.Close(); workbook = null;
                    GC.Collect();
                }
            });
        }
    }
}
