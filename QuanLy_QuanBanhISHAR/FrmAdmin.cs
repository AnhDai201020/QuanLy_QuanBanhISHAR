using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLy_QuanBanhISHAR
{
    public partial class FrmAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        public DTO.Account loginAccount;
        public FrmAdmin()
        {
            InitializeComponent();
            Load();


        }
        #region methods
        List<DTO.Food> SeachFoodByName(string name)
        {
            List<DTO.Food> listFood = DAO.FoodDAO.Instance.SeachFoodByName(name);

            return listFood;
        }

        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadDateTimePickerBill();
            LoadListFood();
            LoadAccount();
            LoadListTable();
            LoadListCategory();
            AddCategoryBinding();
            AddFoodBinding();
            AddTableBinding();
            LoadcategoryIntoCombobox(cbCategory);
            AddAccouuntBinding();

        }
        void LoadListBillByDate(DateTime checkin, DateTime checkout)
        {
            dtgvBill.DataSource = DAO.BillDAO.Instance.GetBillListByDate(checkin, checkout);
        }

        void AddAccouuntBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = DAO.AccountDAO.Instance.GetListAccount();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            txtTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
            
        }
        void AddCategoryBinding()
        {
            txtCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void LoadcategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = DAO.CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadListFood()
        {
            foodList.DataSource = DAO.FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (DAO.AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm Tài Khoản Thành Công!");
            }
            else
            {
                MessageBox.Show("Thêm Tài Khoản Thất Bại!!!");
            }
            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (DAO.AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa Tài Khoản Thành Công!");
            }
            else
            {
                MessageBox.Show("Sửa Tài Khoản Thất Bại!!!");
            }
            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui Lòng Đừng Xóa Chính Bạn Chứ !!! ");
                return;
            }
            if (DAO.AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa Tài Khoản Thành Công!");
            }
            else
            {
                MessageBox.Show("Xóa Tài Khoản Thất Bại!!!");
            }
            LoadAccount();

        }
        void ResetPass(string userName)
        {
            if (DAO.AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt Lại Mật Khẩu Thành Công!");
            }
            else
            {
                MessageBox.Show("Đặt Lại Mật Khẩu Thất Bại!!!");
            }

        }
        void LoadListTable()
        {
            dtgvTable.DataSource = DAO.TableDAO.Instance.GetListTable();
        }
        void LoadListCategory()
        {
            dtgvCategory.DataSource = DAO.CategoryDAO.Instance.GetListCategory();
        }


        #endregion

        #region event
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            ResetPass(userName);
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmType.Value;

            EditAccount(userName, displayName, type);
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    DTO.Category category = DAO.CategoryDAO.Instance.GetCategoryByID(id);
                    cbCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (DTO.Category item in cbCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbCategory.SelectedIndex = index;
                }
            }
            catch
            {

            }



        }

        private void btnAddfood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbCategory.SelectedItem as DTO.Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (DAO.FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn!!!");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbCategory.SelectedItem as DTO.Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);

            if (DAO.FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn!!!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodID.Text);

            if (DAO.FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn!!!");
            }
        }


        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnSeachFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SeachFoodByName(txtSeachFoodName.Text);
        }
        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txtPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = DAO.BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
                lastPage++;

            txtPageBill.Text = lastPage.ToString();
        }

        private void txtPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = DAO.BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtPageBill.Text));
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            if (page > 1)
                page--;
            txtPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            int sumRecord = DAO.BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            if (page < sumRecord)
                page++;
            txtPageBill.Text = page.ToString();
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;

            if (DAO.TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn !!!");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int idTable = Convert.ToInt32(txtTableID.Text);
            string status = txtStatus.Text;


            if (status == "Trong")
            {
                if (DAO.TableDAO.Instance.DeleteTable(idTable))
                {
                    MessageBox.Show("Xóa bàn thành công");
                    LoadListTable();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa bàn !!!");
                }
            }
            else
            {
                MessageBox.Show("Bàn đang có khách không thể xóa!!! Vui lòng thanh toán trước khi xóa bàn !!!");
            }


        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            int idTable = Convert.ToInt32(txtTableID.Text);

            if (DAO.TableDAO.Instance.UpdateTable(idTable, name))
            {
                MessageBox.Show("Sửa bàn thành công");
                LoadListTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn !!!");
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;

            if (DAO.CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadListCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục !!!");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xin Lỗi chức năng chưa Hoàn Thiện!!!");
        }

        private void btnEditCtegory_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Xin Lỗi chức năng chưa Hoàn Thiện!!!");
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        #endregion

        

        

    }
}
