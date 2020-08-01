using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLy_QuanBanhISHAR
{
    public partial class FrmAccountFrofile : Form
    {
        private DTO.Account loginAccount;

        public DTO.Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }


        public FrmAccountFrofile(DTO.Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        void ChangeAccount(DTO.Account acc)
        {
            txtUserName.Text = loginAccount.UserName;
            txtDisplayName.Text = loginAccount.DisplayName;
        }
        void UpdateAccountInfo()
        {
            string displayName = txtDisplayName.Text;
            string password = txtPassWord.Text;
            string newpass = txtNewPassWord.Text;
            string reenterPass = txtReEnterPass.Text;
            string username = txtUserName.Text;

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới !!!");
            }
            else
            {
                if (DAO.AccountDAO.Instance.UpdateAccount(username, displayName, password, newpass))
                {
                    MessageBox.Show("Cập Nhật Thành Công (^-^)");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(DAO.AccountDAO.Instance.GetAccontByUserName(username)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu !!!");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    public class AccountEvent : EventArgs
    {
        private DTO.Account acc;

        public DTO.Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }

        public AccountEvent(DTO.Account acc)
        {
            this.Acc = acc;
        }
    }
}
