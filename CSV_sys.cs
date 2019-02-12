using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HAAGONtest
{
    internal class CSV_sys
    {
        /// <summary>
        ///  将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV的文件路径</param>
        /// <returns></returns>
        public DataTable OpenCSV(string fileName)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst == true)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();

            fs.Close();

            return dt;
        }

        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public void SaveCSV(DataTable dt, string fileName)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
            //MessageBox.Show("CSV文件保存成功！");
        }

        /// <summary>
        /// 创建电芯型号CSV
        /// </summary>
        /// <param name="path">CSV文件保存路径</param>
        /// <param name="id">id编号</param>
        /// <param name="data1">数据1</param>
        /// <param name="data2">数据2</param>
        /// <param name="flag">标识</param>
        public void writeCSV(string path, string id)
        {
            string filePath = path;

            if (!System.IO.File.Exists(filePath))   //文件不存在时，创建新文件，并写入文件标题
            {
                //创建文件流对象，
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                //创建文件流写入对象，绑定文件流对象
                StreamWriter sw = new StreamWriter(fs);
                //创建数据对象
                StringBuilder sb = new StringBuilder();
                sb.Append("ID");
                //把标题内容写入到文件流中
                sw.WriteLine(sb);
                sw.Flush();
                sw.Close();
                fs.Close();
            }

            //向CSV文件中写入数据内容
            StreamWriter msw = new StreamWriter(filePath, true, Encoding.Default);
            //创建数据对象
            StringBuilder msb = new StringBuilder();
            msb.Append(id);
            //把数据内容写入文件中
            msw.WriteLine(msb);
            msw.Flush();
            msw.Close();
        }

        /// <summary>
        /// 写入电芯产能CSV
        /// </summary>
        /// <param name="path">CSV文件保存路径</param>
        /// <param name="id">id编号</param>
        /// <param name="data1">数据1</param>
        /// <param name="data2">数据2</param>
        /// <param name="flag">标识</param>
        public void writeCSV(string path, string id, string data1, string data2, string data3, string data4, string data5, Enum_sys.ProdState _ProdQual)
        {
            string filePath = path;

            if (!System.IO.File.Exists(filePath))   //文件不存在时，创建新文件，并写入文件标题
            {
                //创建文件流对象，
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                //创建文件流写入对象，绑定文件流对象
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                //创建数据对象
                StringBuilder sb = new StringBuilder();
                sb.Append("数目").Append(",").Append("时间").Append(",").Append("型号").Append(",").Append("Al(mm)").Append(",").Append("Ni(mm)").Append(",").Append("宽度(mm)").Append(",").Append("状态");
                //把标题内容写入到文件流中
                sw.WriteLine(sb);
                sw.Flush();
                sw.Close();
                fs.Close();
            }

            //向CSV文件中写入数据内容
            StreamWriter msw = new StreamWriter(filePath, true, Encoding.Default);
            //创建数据对象
            StringBuilder msb = new StringBuilder();
            msb.Append(id).Append(",").Append(data1).Append(",").Append(data2).Append(",").Append(data3).Append(",").Append(data4).Append(",").Append(data5).Append(",").Append(_ProdQual);
            //把数据内容写入文件中
            msw.WriteLine(msb);
            msw.Flush();
            msw.Close();
        }

        /// <summary>
        /// 刷新产能数据
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="path"></param>
        /// <param name="Names"></param>
        /// <param name="HeaderTexts"></param>
        public void UpdataSheet(DataGridView dataGrid, string path, Enum_sys.ProdState _ProdQual, string[] Names, string[] HeaderTexts)
        {
            DataTable dt = new DataTable();
            dt = OpenCSV(path);
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();

            //dataGrid.RowsDefaultCellStyle.Font = new Font("宋体", 9, FontStyle.Strikeout);
            dataGrid.RowsDefaultCellStyle.Font = new Font("宋体", 9);
            dataGrid.RowsDefaultCellStyle.ForeColor = Color.Blue;

            for (int i = 0; i < Names.Length; i++)
            {
                DataGridViewColumn Column = new DataGridViewColumn();
                Column.Name = Names[i];
                Column.HeaderText = HeaderTexts[i];
                Column.CellTemplate = new DataGridViewTextBoxCell();
                dataGrid.Columns.Add(Column);
            }
            string[] values = new string[Names.Length];
            if ((dt.Rows.Count - 100) >= 0)
            {
                for (int i = dt.Rows.Count - 1; i >= dt.Rows.Count - 100; i--)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = dt.Rows[i].ItemArray[j].ToString() + "  ";
                    }
                    if (_ProdQual == Enum_sys.ProdState.全部)
                        dataGrid.Rows.Add(values);
                    else if (values[Names.Length - 1] == _ProdQual.ToString())
                        dataGrid.Rows.Add(values);
                }
            }
            else
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = dt.Rows[i].ItemArray[j].ToString() + "  ";
                    }
                    if (_ProdQual == Enum_sys.ProdState.全部)
                        dataGrid.Rows.Add(values);
                    else if (values[Names.Length - 1] == _ProdQual.ToString())
                        dataGrid.Rows.Add(values);
                }
            }
        }
    }
}