using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DAL;
using Models.Ext;

namespace StudentManager
{
    public partial class FrmStudentManage : Form
    {
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        List<StudentExt> list = new List<StudentExt>();
        public FrmStudentManage()
        {
            InitializeComponent();
            //填充班级下拉框
            this.cboClass.DataSource = objClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;
        }
       
        //按照班级查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一个班级", "查询提示");
                this.cboClass.Focus();
                return;
            }
            list = objStudentService.GetStudentByClassId(this.cboClass.SelectedValue.ToString());
            this.dgvStudentList.AutoGenerateColumns = false;//禁止生成不需要的列
            this.dgvStudentList.DataSource = list;
        }
        //根据学号查询
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入学号", "输入提示");
                this.txtStudentId.Focus();
                return;
            }
            //根据学号查询学员对象
            StudentExt objStudent = objStudentService.GetStudentByStudentId(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("未找到该学员信息", "输入提示");
                this.txtStudentId.Focus();
                this.txtStudentId.SelectAll();
                return;
            }
            else
            {
                FrmStudentInfo objStudentInfo = new FrmStudentInfo(objStudent);
                objStudentInfo.Show();
            }

         
        }
        private void txtStudentId_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length == 0) return;
            if (e.KeyValue == 13) btnQueryById_Click(null,null);
        }
        //双击选中的学员对象并显示详细信息
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }
        //修改学员对象
        private void btnEidt_Click(object sender, EventArgs e)
        {
            //判断是否有修改信息
            if (this.dgvStudentList.RowCount == 0 || this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("没有要修改的信息", "修改提示");
                return;
            }
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            StudentExt objStudent = objStudentService.GetStudentByStudentId(studentId);
            //显示修改窗体
            FrmEditStudent objEditForm = new FrmEditStudent(objStudent);
            DialogResult result=objEditForm.ShowDialog();
            //判断是否修改成功
            if (result == DialogResult.OK)
            {
                btnQuery_Click(null, null);
            }

          
        }
        //删除学员对象
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("没有要删除的对象", "删除提示");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("请选择要删除的对象", "删除提示");
                return;
            }
            //删除确认
            DialogResult result = MessageBox.Show("确认要删除吗？", "删除确认", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;
            //获取要删除的学号
            string studentId = this.dgvStudentList.CurrentRow.Cells["studentId"].Value.ToString();
            try
            {
                if (objStudentService.DeleteStudent(studentId) == 1)
                {
                    MessageBox.Show("删除成功！", "删除提示");
                    btnQuery_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "删除信息");
            }
        }     
        //姓名降序
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (list == null || this.dgvStudentList.RowCount == 0) return;
            this.list.Sort(new NameDESC());
            this.dgvStudentList.DataSource = null;
            this.dgvStudentList.DataSource = list;
        }
        //学号降序
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (list == null || this.dgvStudentList.RowCount == 0) return;
            this.list.Sort(new StudentIdDESC());
            this.dgvStudentList.DataSource = null;
            this.dgvStudentList.DataSource = list;
        }
        //打印当前学员信息
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0 || this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("没有打印的信息", "打印提示");
                return;
            }
            //获取打印学员对象
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            StudentExt objStudent = objStudentService.GetStudentByStudentId(studentId);
            //调用Excel模板打印
            StudentManager.PrintStudent objPrint = new PrintStudent();
            objPrint.ExecutePrint(objStudent);
         
        }
     
        //关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

    }
    //姓名降序
    class NameDESC : IComparer<StudentExt>
    {
        public int Compare(StudentExt x, StudentExt y)
        {
            return y.StudentName.CompareTo(x.StudentName);
        }
    }
    //学号降序
    class StudentIdDESC : IComparer<StudentExt>
    {
        public int Compare(StudentExt x, StudentExt y)
        {
            return y.StudentId.CompareTo(x.StudentId);
        }
    }


}