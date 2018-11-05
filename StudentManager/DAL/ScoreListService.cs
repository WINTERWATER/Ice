using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using Models.Ext;

namespace DAL
{
    public class ScoreListService
    {
        /// <summary>
        /// 查询考试成绩
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public List<StudentExt> GetScoreList(string className)
        {
            string sql = "select StudentName,Students.StudentId,ClassName,Csharp,SQLServerDB from Students";
            sql += " inner join StudentClass on Students.ClassId=StudentClass.ClassId";
            sql += " inner join ScoreList on Students.StudentId=ScoreList.StudentId";
            if (className != null && className.Length != 0)
            {
                sql += string.Format(" where ClassName='{0}'", className);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            List <StudentExt> list= new List<StudentExt>();
            while (objReader.Read())
            {
                list.Add(new StudentExt()
                {
                    StudentId = Convert.ToInt32(objReader["StudentId"]),
                    StudentName = objReader["StudentName"].ToString(),
                    SQLServerDB=objReader["SQLServerDB"].ToString(),
                    CSharp=objReader["CSharp"].ToString(),
                    ClassName=objReader["ClassName"].ToString()                    
                });
            }
            objReader.Close();
            return list;
        }
        /// <summary>
        /// 查询考试综合结果
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetScoreInfo(string classId)
        {
            string sql = "";
            if (classId == null || classId.Length == 0)//全校
            {
                sql = "select stuCount=count(*),avgCsharp=avg(Csharp),avgDB=avg(SQLServerDB) from ScoreList;";
                sql += "select absentCount=count(*) from Students where StudentId not in(select StudentId from ScoreList);";
            }
            else//班级
            {
                sql = "select stuCount=count(*),avgCsharp=avg(Csharp),avgDB=avg(SQLServerDB) from ScoreList";
                sql += " inner join Students on Students.studentId=ScoreList.studentId where classId={0};";
                sql += "select absentCount=count(*) from Students where StudentId not in(select StudentId from ScoreList) and classId={1};";
                sql = string.Format(sql, classId, classId);
            }
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            Dictionary<string, string> scoreInfo = null;
            if (objReader.Read())
            {
                scoreInfo = new Dictionary<string, string>();
                scoreInfo.Add("stuCount", objReader["stuCount"].ToString());
                scoreInfo.Add("avgCsharp", objReader["avgCsharp"].ToString());
                scoreInfo.Add("avgDB", objReader["avgDB"].ToString());
            }
            if (objReader.NextResult())
            {
                if (objReader.Read())
                {
                    scoreInfo.Add("absentCount", objReader["absentCount"].ToString());
                }
            }
            objReader.Close();
            return scoreInfo;
        }
        /// <summary>
        /// 查询缺考姓名
        /// </summary>
        /// <returns></returns>
        public List<string> GetAbsentStu(string classId)
        {
            string sql = "select StudentName from Students where studentId not in(select studentId from ScoreList)";
            if (classId != null && classId.Length != 0)
            {
                sql += " and classId=" + classId;
            }
            IDataReader objReader = SQLHelper.GetReader(sql);
            List<string> list = new List<string>();
            while (objReader.Read())
            {
                list.Add(objReader["StudentName"].ToString());
            }
            objReader.Close();
            return list;
        }
        /// <summary>
        /// 查询全部考试成绩
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllScoreList()
        {
            string sql = "select Students.StudentId,StudentName,ClassName,CSharp,SQLServerDB from Students";
            sql += " inner join StudentClass on StudentClass.ClassId=Students.ClassId";
            sql += " inner join ScoreList on ScoreList.StudentId=Students.StudentId";
            return SQLHelper.GetDataSet(sql);
        }
    }
}
