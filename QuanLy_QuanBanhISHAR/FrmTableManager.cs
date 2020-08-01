using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace QuanLy_QuanBanhISHAR
{
    public partial class FrmTableManager : Form
    {
        private DTO.Account loginAccount;

        public DTO.Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }
        public FrmTableManager(DTO.Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            loadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + loginAccount.DisplayName + ")";
        }

        void loadCategory()
        {
            List<DTO.Category> listCategory = DAO.CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListCategoryID(int id)
        {
            List<DTO.Food> listFood = DAO.FoodDAO.Instance.GetFoodCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<DTO.Table> tableList = DAO.TableDAO.Instance.LoadTableList();

            foreach (DTO.Table item in tableList)
            {
                Button btn = new Button() { Width = DAO.TableDAO.TableWidth, Height = DAO.TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trong":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);

            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = DAO.MenuDAO.Instance.GetListMenuByTable(id);

            float totalPrice = 0;


            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }
            txtTotalPrice.Text = totalPrice.ToString();


        }
        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = DAO.TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion


        #region Events
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as DTO.Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAccountFrofile f = new FrmAccountFrofile(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdmin f = new FrmAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += f_InsertFood;
            f.UpdateFood += f_UpdateFood;
            f.DeleteFood += f_DeleteFood;
            f.ShowDialog();
        }

        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as DTO.Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as DTO.Table).ID);
            LoadTable();
        }

        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as DTO.Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as DTO.Table).ID);
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as DTO.Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as DTO.Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            DTO.Category selected = cb.SelectedItem as DTO.Category;
            id = selected.ID;

            LoadFoodListCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            DTO.Table table = lsvBill.Tag as DTO.Table;

            if (table == null)
            {
                MessageBox.Show("Hãy Chọn Bàn");
                return;
            }

            int idBill = DAO.BillDAO.Instance.GetuncheckBillByTable(table.ID);
            int foodID = (cbFood.SelectedItem as DTO.Food).ID;
            int count = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                DAO.BillDAO.Instance.InsertBill(table.ID);
                DAO.BillInfoDAO.Instance.InsertBillInfo(DAO.BillDAO.Instance.GetMaxIDBill(), foodID, count);

            }
            else
            {
                DAO.BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID);
            LoadTable();

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            DTO.Table table = lsvBill.Tag as DTO.Table;

            int idBill = DAO.BillDAO.Instance.GetuncheckBillByTable(table.ID);
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txtTotalPrice.Text);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\n Tổng tiền - (Tổng tiền / 100) x Giảm giá = {1} - ({1} / 100) x{2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông Báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    DAO.BillDAO.Instance.Checkout(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {

            int id1 = (lsvBill.Tag as DTO.Table).ID;
            int id2 = (cbSwitchTable.SelectedItem as DTO.Table).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn Chuyển Bàn {0} qua bàn {1}", (lsvBill.Tag as DTO.Table).Name, (cbSwitchTable.SelectedItem as DTO.Table).Name), "Thông Báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                DAO.TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }



        #endregion





    }
}
