using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLy_QuanBanhISHAR.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }

        private MenuDAO() { }

        public List<DTO.Menu> GetListMenuByTable(int id)
        {
            List<DTO.Menu> listMenu = new List<DTO.Menu>();

            string query = "SELECT f.NAME, bi.count, f.prire, f.prire*bi.count as toltalPrice FROM dbo.BILLINFO as bi, dbo.BILL as b, dbo.FOOD as f WHERE bi.idBill = b.ID and b.status = 0 and bi.idFood = f.ID and b.idTable ="+id;
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                DTO.Menu menu = new DTO.Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
