using Models.Ext;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class AttendanceService
    {
        /// <summary>
        /// 获取学员总数
        /// </summary>
        /// <returns></returns>
        public int GetAllStudent()
        {
            string sql = "select count(*) from Students";
            try
            {
                return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            }
            catch (Exception ex)
            {
                throw new Exception("无法获取学员总数：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取实到人数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isToday"></param>
        /// <returns></returns>
        public int GetAttendStudents(DateTime dt, bool isToday)
        {
            DateTime dt1;
            if (isToday)//当天时间直接获取服务器时间
            {
                dt1 = Convert.ToDateTime(SQLHelper.GetServerTime());
            }
            else dt1 = dt;
            DateTime dt2 = dt1.AddDays(1.0);//结束时间=开始时间+1
            string sql = "select count(distinct CardNo) from Attendance where Dtime between '{0}' and '{1}'";
            sql = string.Format(sql, dt1, dt2);
            try
            {
                return Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            }
            catch(Exception ex)
            {
                throw new Exception("无法获取学员总数：" + ex.Message);
            }
        }
        /// <summary>
        /// 添加打卡记录
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public string AddRecord(string cardNo)
        {
            string sql = "insert into Attendance (cardNo) values ('{0}')";
            sql = string.Format(sql, cardNo);
            try
            {
                SQLHelper.Update(sql);
                return "Success";
            }
            catch (Exception ex)
            {
                return "打卡失败：" + ex.Message;
            }
        }
        /// <summary>
        /// 根据日期和学号查询考勤
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<StudentExt> GetStudentByDate(DateTime beginTime, DateTime endTime, string name)
        {
            string sql = "select StudentId,StudentName,Gender,DTime,ClassName from Students";
            sql += " inner join StudentClass on Students.ClassId=StudentClass.ClassId";
            sql += " inner join Attendance on Students.CardNo=Attendance.CardNo";
            sql += " where DTime between '{0}' and '{1}'";
            sql = string.Format(sql, beginTime, endTime);
            if (name != null && name.Length != 0)
            {
                sql += string.Format(" and StudentName='{0}'", name);
            }
            sql += " order by DTime ASC";
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> list = new List<StudentExt>();
            while (objReader.Read())
            {
                list.Add(new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    DTime = Convert.ToDateTime(objReader["DTime"]),
                    ClassName = objReader["ClassName"].ToString(),
                });
            }
            return list;
        }
    }
}
