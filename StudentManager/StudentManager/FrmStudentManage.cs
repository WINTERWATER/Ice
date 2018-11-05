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
            //���༶������
            this.cboClass.DataSource = objClassService.GetAllClass();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;
        }
       
        //���հ༶��ѯ
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ��һ���༶", "��ѯ��ʾ");
                this.cboClass.Focus();
                return;
            }
            list = objStudentService.GetStudentByClassId(this.cboClass.SelectedValue.ToString());
            this.dgvStudentList.AutoGenerateColumns = false;//��ֹ���ɲ���Ҫ����
            this.dgvStudentList.DataSource = list;
        }
        //����ѧ�Ų�ѯ
        private void btnQueryById_Click(object sender, EventArgs e)
        {
            if (this.txtStudentId.Text.Trim().Length == 0)
            {
                MessageBox.Show("������ѧ��", "������ʾ");
                this.txtStudentId.Focus();
                return;
            }
            //����ѧ�Ų�ѯѧԱ����
            StudentExt objStudent = objStudentService.GetStudentByStudentId(this.txtStudentId.Text.Trim());
            if (objStudent == null)
            {
                MessageBox.Show("δ�ҵ���ѧԱ��Ϣ", "������ʾ");
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
        //˫��ѡ�е�ѧԱ������ʾ��ϸ��Ϣ
        private void dgvStudentList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }
        //�޸�ѧԱ����
        private void btnEidt_Click(object sender, EventArgs e)
        {
            //�ж��Ƿ����޸���Ϣ
            if (this.dgvStudentList.RowCount == 0 || this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("û��Ҫ�޸ĵ���Ϣ", "�޸���ʾ");
                return;
            }
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            StudentExt objStudent = objStudentService.GetStudentByStudentId(studentId);
            //��ʾ�޸Ĵ���
            FrmEditStudent objEditForm = new FrmEditStudent(objStudent);
            DialogResult result=objEditForm.ShowDialog();
            //�ж��Ƿ��޸ĳɹ�
            if (result == DialogResult.OK)
            {
                btnQuery_Click(null, null);
            }

          
        }
        //ɾ��ѧԱ����
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0)
            {
                MessageBox.Show("û��Ҫɾ���Ķ���", "ɾ����ʾ");
                return;
            }
            if (this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("��ѡ��Ҫɾ���Ķ���", "ɾ����ʾ");
                return;
            }
            //ɾ��ȷ��
            DialogResult result = MessageBox.Show("ȷ��Ҫɾ����", "ɾ��ȷ��", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;
            //��ȡҪɾ����ѧ��
            string studentId = this.dgvStudentList.CurrentRow.Cells["studentId"].Value.ToString();
            try
            {
                if (objStudentService.DeleteStudent(studentId) == 1)
                {
                    MessageBox.Show("ɾ���ɹ���", "ɾ����ʾ");
                    btnQuery_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ɾ����Ϣ");
            }
        }     
        //��������
        private void btnNameDESC_Click(object sender, EventArgs e)
        {
            if (list == null || this.dgvStudentList.RowCount == 0) return;
            this.list.Sort(new NameDESC());
            this.dgvStudentList.DataSource = null;
            this.dgvStudentList.DataSource = list;
        }
        //ѧ�Ž���
        private void btnStuIdDESC_Click(object sender, EventArgs e)
        {
            if (list == null || this.dgvStudentList.RowCount == 0) return;
            this.list.Sort(new StudentIdDESC());
            this.dgvStudentList.DataSource = null;
            this.dgvStudentList.DataSource = list;
        }
        //��ӡ��ǰѧԱ��Ϣ
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.dgvStudentList.RowCount == 0 || this.dgvStudentList.CurrentRow == null)
            {
                MessageBox.Show("û�д�ӡ����Ϣ", "��ӡ��ʾ");
                return;
            }
            //��ȡ��ӡѧԱ����
            string studentId = this.dgvStudentList.CurrentRow.Cells["StudentId"].Value.ToString();
            StudentExt objStudent = objStudentService.GetStudentByStudentId(studentId);
            //����Excelģ���ӡ
            StudentManager.PrintStudent objPrint = new PrintStudent();
            objPrint.ExecutePrint(objStudent);
         
        }
     
        //�ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

    }
    //��������
    class NameDESC : IComparer<StudentExt>
    {
        public int Compare(StudentExt x, StudentExt y)
        {
            return y.StudentName.CompareTo(x.StudentName);
        }
    }
    //ѧ�Ž���
    class StudentIdDESC : IComparer<StudentExt>
    {
        public int Compare(StudentExt x, StudentExt y)
        {
            return y.StudentId.CompareTo(x.StudentId);
        }
    }


}