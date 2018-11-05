using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using Models;

namespace DAL
{
    public class StudentClassService
    {
        public List<StudentClass> GetAllClass()
        {
            string sql = "select ClassId,ClassName from StudentClass";
            SqlDataReader objDataReader = SQLHelper.GetReader(sql);
            List<StudentClass> list = new List<StudentClass>();
            while (objDataReader.Read())
            {
                list.Add(new StudentClass()
                {
                    ClassId = Convert.ToInt32(objDataReader["ClassID"]),
                    ClassName = objDataReader["ClassName"].ToString()
                });
            }
            objDataReader.Close();
            return list;
        }
        public DataSet GetAllStuClass()
        {
            string sql = "select ClassId,ClassName from StudentClass";
            return SQLHelper.GetDataSet(sql);
        }

    }
}
