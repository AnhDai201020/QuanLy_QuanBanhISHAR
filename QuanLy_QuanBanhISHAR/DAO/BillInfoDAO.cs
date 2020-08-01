
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLy_QuanBanhISHAR.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }

        public void DeleteBillInfoByFoodID(int id)
        {
            DAO.DataProvider.Instance.ExecuteQuery("Delete dbo.BILLINFO WHERE idFood = " + id);
        }

        public List<DTO.BillInfo> GetListBillInfo(int id)
        {
            List<DTO.BillInfo> listBillInfo = new List<DTO.BillInfo>();

            DataTable data = DAO.DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BILLINFO WHERE idBill = " + id);

            foreach (DataRow item in data.Rows)
            {
                DTO.BillInfo info = new DTO.BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DAO.DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
