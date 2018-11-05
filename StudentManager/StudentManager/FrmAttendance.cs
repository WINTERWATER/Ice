using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using Models.Ext;

namespace StudentManager
{
    public partial class FrmAttendance : Form
    {
        private AttendanceService objAttService = new AttendanceService();
        private StudentService objStudentService = new StudentService();
        public FrmAttendance()
        {
            InitializeComponent();
            this.lblCount.Text = objAttService.GetAllStudent().ToString();
            ShowStat();
        }
        /// <summary>
        /// 显示签到人数
        /// </summary>
        private void ShowStat()
        {
            this.lblReal.Text = objAttService.GetAttendStudents(DateTime.Now,true).ToString();
            this.lblAbsenceCount.Text = (Convert.ToInt32(this.lblCount.Text) - Convert.ToInt32(this.lblReal.Text)).ToString();
        }
        //显示当前时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblYear.Text = DateTime.Now.Year.ToString();
            this.lblMonth.Text = DateTime.Now.Month.ToString();
            this.lblDay.Text = DateTime.Now.Day.ToString();
            this.lblTime.Text = DateTime.Now.ToLongTimeString();
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday":
                    this.lblWeek.Text = "一";
                    break;
                case "Tuesday":
                    this.lblWeek.Text = "二";
                    break;
                case "Wednesday":
                    this.lblWeek.Text = "三";
                    break;
                case "Thurday":
                    this.lblWeek.Text = "四";
                    break;
                case "Friday":
                    this.lblWeek.Text = "五";
                    break;
                case "Saturday":
                    this.lblWeek.Text = "六";
                    break;
                case "Sunday":
                    this.lblWeek.Text = "天";
                    break;
            }
        }
        //学员打卡        
        private void txtStuCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtStuCardNo.Text.Trim().Length == 0 || e.KeyValue != 13) return;
            //显示学员信息
            StudentExt objStu = objStudentService.GetStudentByCardNo(this.txtStuCardNo.Text.Trim());
            if (objStu == null)
            {
                MessageBox.Show("卡号不正确");
                this.lblInfo.Text = "打卡失败";
                this.txtStuCardNo.SelectAll();
                this.lblStuName.Text = "";
                this.lblStuId.Text = "";
                this.lblStuClass.Text = "";
                this.pbStu.Image = null;                
            }
            else
            {
                this.lblStuName.Text =objStu.StudentName;
                this.lblStuId.Text = objStu.StudentId.ToString();
                this.lblStuClass.Text = objStu.ClassName;
                if (objStu.StuImage != null && objStu.StuImage.Length != 0)
                    this.pbStu.Image = (Image)new SerializeObjectToString().DeserializeObject(objStu.StuImage);
                else
                    this.pbStu.Image = Image.FromFile("default.png");
                //添加打卡记录
                string result = objAttService.AddRecord(this.txtStuCardNo.Text.Trim());
                if (result != "Success")
                {
                    this.lblInfo.Text = "打卡失败";
                    MessageBox.Show(result, "信息提示");
                }
                else
                {
                    this.lblInfo.Text = "打卡成功！";
                    ShowStat();//更新人数
                    this.txtStuCardNo.Text = "";
                    this.txtStuCardNo.Focus();
                }
            }
           
        }
        //结束打卡
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
    }
}
