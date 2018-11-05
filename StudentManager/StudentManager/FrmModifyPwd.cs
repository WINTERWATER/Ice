using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace StudentManager
{
    public partial class FrmModifyPwd : Form
    {
        private AdminService objAdminService = new AdminService();
        public FrmModifyPwd()
        {
            InitializeComponent();
        }
        //修改密码
        private void btnModify_Click(object sender, EventArgs e)
        {
            //校验用户输入的旧密码是否正确
            if (this.txtOldPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入原密码", "修改提示");
                this.txtOldPwd.Focus();
                return;
            }
            if (this.txtOldPwd.Text.Trim() != Program.currentAdmin.LoginPwd)
            {
                MessageBox.Show("请输入正确的密码", "输入提示");
                this.txtOldPwd.Focus();
                this.txtOldPwd.SelectAll();
                return;
            }
            if (this.txtNewPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入新密码", "修改提示");
                this.txtNewPwd.Focus();
                return;
            }
            if (this.txtNewPwd.Text.Trim().Length < 6 || this.txtNewPwd.Text.Trim().Length > 18)
            {
                MessageBox.Show("新密码长度必须在6-18位之间", "修改提示");
                this.txtNewPwd.Focus();
                this.txtNewPwd.SelectAll();
                return;
            }
            if (this.txtNewPwd.Text.Trim() != this.txtNewPwdConfirm.Text.Trim())
            {
                MessageBox.Show("两次输入密码不一致", "修改提示");
                //this.txtNewPwdConfirm.Focus();
                return;
            }

            int result = objAdminService.ModifyPwd(Program.currentAdmin.LoginId.ToString(),this.txtNewPwd.Text.Trim());
            if (result == 1)
            {
                MessageBox.Show("密码修改成功！", "修改提示");
                Program.currentAdmin.LoginPwd = this.txtNewPwd.Text.Trim().ToString();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
