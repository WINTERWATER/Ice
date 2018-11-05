using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Models;
using DAL;


namespace StudentManager
{
    public partial class FrmScoreQuery : Form
    {
        private ScoreListService objScoreService = new ScoreListService();
        private DataSet ds = null;
        public FrmScoreQuery()
        {
            InitializeComponent();
            this.cboClass.DataSource = new StudentClassService().GetAllStuClass().Tables[0];
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            //显示全部考试成绩
            ds = objScoreService.GetAllScoreList();
            this.dgvScoreList.DataSource = ds.Tables[0];
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);


        }  
   
        //根据班级名称动态筛选
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ds == null) return;
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName='" + this.cboClass.Text.Trim() + "'";
        }
        //显示全部成绩
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            this.ds.Tables[0].DefaultView.RowFilter = "ClassName like '%%'";
        }
        //根据C#成绩动态筛选
        private void txtScore_TextChanged(object sender, EventArgs e)
        {
            if (ds == null || this.txtScore.Text.Trim().Length == 0) return;
            if (!DataValidate.IsInteger(this.txtScore.Text.Trim())) return;
            else
            {
                this.ds.Tables[0].DefaultView.RowFilter = "CSharp>" + this.txtScore.Text.Trim();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

