using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;

namespace StudentManager
{
    public partial class FrmAttendanceQuery : Form
    {

        private AttendanceService attStudentService = new AttendanceService();

        public FrmAttendanceQuery()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //考勤查询
            DateTime dt1 = Convert.ToDateTime(this.dtpTime.Text);
            DateTime dt2 = dt1.AddDays(1.0);
            this.dgvStudentList.AutoGenerateColumns = false;
            this.dgvStudentList.DataSource = attStudentService.GetStudentByDate(dt1, dt2, this.txtName.Text.Trim());
            //设置显示样式
            new DataGridViewStyle().DgvStyle2(this.dgvStudentList);
            //考勤人数显示
            this.lblCount.Text = attStudentService.GetAllStudent().ToString();
            this.lblReal.Text = attStudentService.GetAttendStudents(Convert.ToDateTime(this.dtpTime.Text),false).ToString();
            this.lblAbsenceCount.Text = (Convert.ToInt32(this.lblCount.Text) - Convert.ToInt32(this.lblReal.Text)).ToString();            
        }
        //添加行号
        private void dgvStudentList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewStyle.DgvRowPostPaint(this.dgvStudentList, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
