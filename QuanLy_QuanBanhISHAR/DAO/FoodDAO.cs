using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLy_QuanBanhISHAR.DAO
{
    public class FoodDAO
    {

        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<DTO.Food> GetFoodCategoryID(int id)
        {
            List<DTO.Food> list = new List<DTO.Food>();

            string query = "select * from FOOD where idCategory = " + id;
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                DTO.Food food = new DTO.Food(item);
                list.Add(food);
            }

            return list;
        }

        public List<DTO.Food> GetListFood()
        {
            List<DTO.Food> list = new List<DTO.Food>();

            string query = "select * from FOOD";
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                DTO.Food food = new DTO.Food(item);
                list.Add(food);
            }

            return list;
        }

        public List<DTO.Food> SeachFoodByName(string name)
        {
            List<DTO.Food> list = new List<DTO.Food>();

            string query = string.Format("select * from FOOD where name like N'%{0}%'", name);
            DataTable data = DAO.DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                DTO.Food food = new DTO.Food(item);
                list.Add(food);
            }

            return list;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT dbo.FOOD ( NAME, idCategory, prire ) values ( N'{0}', {1}, {2})", name, id, price);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("Update dbo.Food set name = N'{0}', idCategory = {1} , prire = {2} where id = {3}", name, id, price, idFood);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            DAO.BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood);
            string query = string.Format("Delete dbo.FOOD where id = {0}", idFood);
            int result = DAO.DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
