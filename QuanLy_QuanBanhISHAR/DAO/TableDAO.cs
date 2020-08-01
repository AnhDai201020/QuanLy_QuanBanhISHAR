using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLy_QuanBanhISHAR.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 70;
        public static int TableHeight = 70;
        private TableDAO()
        {

        }

        public void SwitchTable(int id1, int id2)
        {
            DAO.DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });
        }
        public List<DTO.Table> LoadTableList()
        {
            List<DTO.Table> tableList = new List<DTO.Table>();
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                DTO.Table table = new DTO.Table(item);
                tableList.Add(table);
            }
            return tableList;
        }

        public List<DTO.Table> GetListTable()
        {
            List<DTO.Table> list = new List<DTO.Table>();

            string query = "select * from TABLEFOOD";
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                DTO.Table table = new DTO.Table(item);
                list.Add(table);
            }

            return list;
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT dbo.TABLEFOOD ( NAME) values ( N'{0}')", name);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateTable(int idTable, string name)
        {
            string query = string.Format("Update dbo.TABLEFOOD set name = N'{0}' where id = {1}", name, idTable);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteTable(int idTable)
        {
            string query = string.Format("Delete dbo.TABLEFOOD where id = {0}", idTable);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
