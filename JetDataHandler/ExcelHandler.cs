using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetDataHandler
{
    class ExcelHandler
    {
        public static DataTable ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
            string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
            string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                                                                               //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串

            OleDbDataAdapter ada = new OleDbDataAdapter(sql, conn);
            DataSet set = new DataSet();
            ada.Fill(set);
            return set.Tables[0];
        }

    }

}
