using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace QuanLy_QuanBanhISHAR.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }
        private AccountDAO() { }

        public bool Login(string username, string password)
        {

            string query = "USP_Login @username , @password";
            DataTable result = DAO.DataProvider.Instance.ExecuteQuery(query, new object[] { username, password });
            return result.Rows.Count > 0;

        }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DAO.DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, pass, newPass });
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DAO.DataProvider.Instance.ExecuteQuery("select UserName, DisplayName, Type from ACCOUNT");
        }

        public DTO.Account GetAccontByUserName(string userName)
        {
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery("select * from ACCOUNT where UserName = '" + userName + "'");
            foreach (DataRow item in data.Rows)
            {
                return new DTO.Account(item);
            }
            return null;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("INSERT dbo.ACCOUNT ( UserName, DisplayName, Type ) values ( N'{0}', N'{1}', {2})", name, displayName, type);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("Update dbo.ACCOUNT set DisplayName = N'{1}' , Type = {2} where UserName = N'{0}'", name, displayName, type);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteAccount(string name)
        {

            string query = string.Format("Delete dbo.ACCOUNT where UserName = N'{0}'", name);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool ResetPassword(string name)
        {
            string query = string.Format("Update dbo.ACCOUNT set password = N'0' where UserName = N'{0}'", name);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
