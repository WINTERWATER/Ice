using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models;

namespace StudentManager
{
    public partial class FrmAddStudent : Form
    {
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        public FrmAddStudent()
        {
            InitializeComponent();
            this.cboClassName.DataSource = objClassService.GetAllClass();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
            this.cboClassName.SelectedIndex = -1;
        }
        //�����ѧԱ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //������֤
            if (this.txtStudentName.Text.Trim().Length == 0)
            {
                MessageBox.Show("������ѧԱ����", "��֤��ʾ");
                this.txtStudentName.Focus();
                return;
            }
            if (!this.rdoMale.Checked && !this.rdoFemale.Checked)
            {
                MessageBox.Show("��ѡ���Ա�", "��֤��ʾ");
                return;
            }
            if ((DateTime.Now.Year - Convert.ToDateTime(this.dtpBirthday.Text).Year) < 18)
            {
                MessageBox.Show("����������18��", "��֤��ʾ");
                this.dtpBirthday.Focus();
                return;
            }
            if (this.txtStudentIdNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("���������֤�ţ�", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                return;
            }
            if (this.cboClassName.SelectedIndex == -1)
            {
                MessageBox.Show("��ѡ��༶", "��֤��ʾ");
                return;
            }
            if (this.txtCardNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("�����뿼�鿨��", "��֤��ʾ");
                this.txtCardNo.Focus();
                return;
            }
            if (!DataValidate.IsIdentityCard(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("��������ȷ�����֤��", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            string month = string.Empty;
            string day = string.Empty;
            if (Convert.ToDateTime(this.dtpBirthday.Text).Month < 10)
            {
                month = "0" + Convert.ToDateTime(this.dtpBirthday.Text).Month;
            }
            else
                month = Convert.ToDateTime(this.dtpBirthday.Text).Month.ToString();
            if (Convert.ToDateTime(this.dtpBirthday.Text).Day < 10)
            {
                day = "0" + Convert.ToDateTime(this.dtpBirthday.Text).Day;
            }
            else
                day = Convert.ToDateTime(this.dtpBirthday.Text).Day.ToString();
            string birthday = Convert.ToDateTime(this.dtpBirthday.Text).Year.ToString() + month + day;
            if (!this.txtStudentIdNo.Text.Trim().Contains(birthday))
            {
                MessageBox.Show("�������ڲ�һ��", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            if(objStudentService.IsCardNoExited(this.txtCardNo.Text.Trim()))
            {
                MessageBox.Show("���ڿ����Ѿ�����", "��֤��ʾ");
                this.txtCardNo.Focus();
                this.txtCardNo.SelectAll();
                return;
            }
            if (objStudentService.IsIDCardExited(this.txtStudentIdNo.Text.Trim()))
            {
                MessageBox.Show("���֤���Ѿ�����", "��֤��ʾ");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }
            //��װѧԱ����
            Student objStudent = new Student()
            {
                StudentName = this.txtStudentName.Text.Trim(),
                Gender = this.rdoMale.Checked ? "��" : "Ů",
                Birthday = Convert.ToDateTime(this.dtpBirthday.Text),
                Age=DateTime.Now.Year-Convert.ToDateTime(this.dtpBirthday.Text).Year,
                ClassId=Convert.ToInt32(this.cboClassName.SelectedValue),
                StudentIdNo=this.txtStudentIdNo.Text.Trim(),
                CardNo=this.txtCardNo.Text.Trim(),
                PhoneNumber=this.txtPhoneNumber.Text.Trim(),
                StudentAddress=this.txtAddress.Text.Trim(),
                StuImage=this.pbStu.Image==null?"":new SerializeObjectToString().SerializeObject(this.pbStu.Image)
            };
            //�ύ����
            try
            {
                int result = objStudentService.AddStudent(objStudent);
                if (result == 1)
                {
                    DialogResult dresult = MessageBox.Show("��ӳɹ����Ƿ�������?", "���ѯ��", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);
                    //����ı���
                    foreach (Control item in this.groupBox1.Controls)
                    {
                        if (item is TextBox)
                            item.Text = "";
                        else if (item is RadioButton)
                            ((RadioButton)item).Checked = false;
                        else if (item is ComboBox)
                            ((ComboBox)item).SelectedIndex = -1;
                    }
                    this.pbStu.Image = null;
                    this.txtStudentName.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��Ϣ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        //�رմ���
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }      
        //ѡ������Ƭ
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();//�ô��봴����Ҫ�Ͽؼ�
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.pbStu.Image = Image.FromFile(fileDialog.FileName);
            }
        }

        private void cboClassName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}