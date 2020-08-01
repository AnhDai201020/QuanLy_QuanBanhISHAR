using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLy_QuanBanhISHAR.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        /// <summary>
        /// Thành công : bill ID
        /// thất bại -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public int GetuncheckBillByTable(int id)
        {
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BILL WHERE idTable = " + id + " AND status = 0");

            if (data.Rows.Count > 0)
            {
                DTO.Bill bill = new DTO.Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void Checkout(int id, int discount, float totalPrice)
        {
            string query = "update dbo.BILL set DateCheckOut = GETDATE(), status = 1, " + "discount = " + discount + ", totalPrice = " + totalPrice + " where ID = " + id;
            DAO.DataProvider.Instance.ExecuteNonQuery(query);
        }


        public void InsertBill(int id)
        {
            DAO.DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] { id });
        }

        public DataTable GetBillListByDate(DateTime checkin, DateTime checkOut)
        {
            return DAO.DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkin , @checkOut", new object[] { checkin, checkOut });
        }
        public DataTable GetBillListByDateAndPage(DateTime checkin, DateTime checkOut, int pageNum)
        {
            return DAO.DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDateAndPage @checkin , @checkOut , @page", new object[] { checkin, checkOut, pageNum });
        }
        public int GetNumBillByDate(DateTime checkin, DateTime checkOut)
        {
            return (int)DAO.DataProvider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkin , @checkOut", new object[] { checkin, checkOut });
        }



        public int GetMaxIDBill()
        {
            try
            {
                return (int)DAO.DataProvider.Instance.ExecuteScalar("select Max(id) From dbo.BILL");
            }
            catch
            {
                return 1;
            }
        }
    }
}
