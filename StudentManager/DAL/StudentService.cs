using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;
using Models.Ext;

namespace DAL
{
    public class StudentService
    {
        /// <summary>
        /// 查询身份证号是否存在
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public bool IsIDCardExited(string idcard)
        {
            string sql = "select count(*) from Students where StudentidNo={0}";
            sql = string.Format(sql, idcard);
            int count = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (count == 1) return true;
            else return false;
        }
        /// <summary>
        /// 查询卡号是否存在
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public bool IsCardNoExited(string CardNo)
        {
            string sql = "select count(*) from Students where StudentidNo={0}";
            sql = string.Format(sql, CardNo);
            int count = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (count == 1) return true;
            else return false;
        }
        public int AddStudent(Student objStudent)
        {
            string sql = "insert into Students (StudentName,Age,Gender,Birthday, CardNo,ClassId,StudentIdNo,PhoneNumber,StudentAddress,StuImage)";
            sql += "values('{0}',{1},'{2}','{3}','{4}',{5},{6},'{7}','{8}','{9}')";
            sql = string.Format(sql, objStudent.StudentName, objStudent.Age, objStudent.Gender, objStudent.Birthday, objStudent.CardNo, objStudent.ClassId,
                objStudent.StudentIdNo, objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.StudentAddress);
            try
            {
                return SQLHelper.Update(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("保存数据错误" + ex.Message);
            }
        }
        //班级查询学生数据
        public List<StudentExt> GetStudentByClassId(string classId)
        {
            string sql = "select StudentId,StudentName,Gender,Birthday,ClassName from Students";
            sql += " inner join StudentClass on Students.ClassId=StudentClass.ClassId";
            sql += " where Students.ClassId=" + classId;
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List<StudentExt> list = new List<StudentExt>();
            while (objReader.Read())
            {
                list.Add(new StudentExt()
                {
                    StudentId=Convert.ToInt32(objReader["StudentId"]),
                    StudentName=objReader["StudentName"].ToString(),
                    Gender=objReader["Gender"].ToString(),
                    Birthday=Convert.ToDateTime(objReader["Birthday"]),
                    ClassName=objReader["ClassName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }
        /// <summary>
        /// 学号查询学员对象
        /// </summary>
        /// <param name="stuId"></param>
        /// <returns></returns>
        public StudentExt GetStudentByStudentId(string stuId)
        {
            string whereSql = "where StudentId=" + stuId;
            return this.GetStudentBySql(whereSql);            
        }
        /// <summary>
        /// 根据卡号查询学员对象
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public StudentExt GetStudentByCardNo(string cardNo)
        {
            string whereSql = string.Format("where CardNo='{0}'", cardNo);
            return this.GetStudentBySql(whereSql);

        }
        public StudentExt GetStudentBySql(string whereSql)
        {
            string sql = "select StudentName,StudentId,Age,Gender,Birthday,CardNo,ClassName,StudentIdNo,PhoneNumber,StudentAddress,StuImage from Students";
            sql += " inner join StudentClass on Students.ClassId=StudentClass.ClassId ";
            sql += whereSql;
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            StudentExt objStudent = null;
            if (objReader.Read())
            {
                objStudent = new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    Gender = objReader["Gender"].ToString(),
                    Birthday = Convert.ToDateTime(objReader["Birthday"]),
                    ClassName = objReader["ClassName"].ToString(),
                    CardNo = objReader["CardNo"].ToString(),
                    StudentIdNo = objReader["StudentIdNo"].ToString(),
                    Age = Convert.ToInt32(objReader["Age"]),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    StudentAddress = objReader["StudentAddress"].ToString(),
                    StuImage = objReader["StuImage"] is DBNull ? "" : objReader["StuImage"].ToString()
                };
            }
            objReader.Close();
            return objStudent;

        }
        /// <summary>
        /// 查询身份证是否存在
        /// </summary>
        /// <param name="idNo"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public bool IsIdNoExisted(string idNo, string studentId)
        {
            string sql = "select count(*) from Students where StudentIdNo=" + idNo + " and StudentId<>" + studentId;
            int result = Convert.ToInt32(SQLHelper.GetSingleResult(sql));
            if (result == 1) return true;
            else return false;
        }
        /// <summary>
        /// 修改学员信息
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public int ModifyStudent(Student objStudent)
        {
            //SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("update Students set studentName='{0}',Gender='{1}',Birthday='{2}',");
            sqlBuilder.Append("StudentIdNo={3},Age={4},PhoneNumber='{5}',StudentAddress='{6}',CardNo='{7}',ClassId='{8}',StuImage='{9}'");
            sqlBuilder.Append(" where StudentId={10}");
            //解析对象
            string sql = string.Format(sqlBuilder.ToString(), objStudent.StudentName, objStudent.Gender, objStudent.Birthday, objStudent.StudentIdNo, objStudent.Age,
                objStudent.PhoneNumber, objStudent.StudentAddress, objStudent.CardNo, objStudent.ClassId, objStudent.StuImage, objStudent.StudentId);
            try
            {
                return Convert.ToInt32(SQLHelper.Update(sql));
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库操作异常！具体信息：\r\n" + ex.Message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除学员对象
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public int DeleteStudent(string studentId)
        {
            string sql = "delete from Students where StudentId=" + studentId;
            try
            {
                return SQLHelper.Update(sql);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    throw new Exception("当前学号被其他表引用不能删除");
                else
                    throw new Exception("删除学员发生错误:" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("删除学员发生错误:" + ex.Message);
            }
        }

    }
}
