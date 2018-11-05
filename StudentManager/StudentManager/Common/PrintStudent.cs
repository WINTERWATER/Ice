using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Drawing;
using System.IO;

using Models;
using Models.Ext;


namespace StudentManager
{
    
        class PrintStudent
        {
            /// <summary>
            /// 在Excel中打印学生信息
            /// </summary>
            /// <param name="objStudent"></param>     
            /// <returns></returns>
            public void ExecutePrint(StudentExt objStudent)
            {
                //【1】定义一个Excel工作簿
                Microsoft.Office.Interop.Excel.Application excelApp = new Application();

                //【2】获取已创建好的工作薄路径
                string excelBookPath = Environment.CurrentDirectory + "\\StudentInfo.xls";
                //【3】将现有工作簿加入已定义的工作簿集合
                excelApp.Workbooks.Add(excelBookPath);
                //【4】获取第1个工作表
                Worksheet objSheet = (Worksheet)excelApp.Worksheets[1];

                //【5】在当前的Excel中写入数据           
                if (objStudent.StuImage.Length != 0)
                { //将图片保存在指定的位置
                    Image objImage = (Image)new
                        StudentManager.SerializeObjectToString().DeserializeObject(objStudent.StuImage);
                    if (File.Exists(Environment.CurrentDirectory + "\\Student.jpg"))
                        File.Delete(Environment.CurrentDirectory + "\\Student.jpg");
                    else
                    {
                        //保存图片到系统目录（当前会保存在Debug或者Release文件夹中）
                        objImage.Save(Environment.CurrentDirectory + "\\Student.jpg");
                        //将图片插入的Excel中
                        objSheet.Shapes.AddPicture(Environment.CurrentDirectory +
                            "\\Student.jpg", MsoTriState.msoFalse,
                                                MsoTriState.msoTrue, 10, 50, 70, 80);
                        //使用完毕后删除保存的图片
                        File.Delete(Environment.CurrentDirectory + "\\Student.jpg");
                    }
                }
                //写入其他相关的数据  
                objSheet.Cells[4, 4] = objStudent.StudentId;//学号
                objSheet.Cells[4, 6] = objStudent.StudentName;//姓名
                objSheet.Cells[4, 8] = objStudent.Gender;//性别
                objSheet.Cells[6, 4] = objStudent.ClassName;//所在班级
                objSheet.Cells[6, 6] = objStudent.PhoneNumber;//联系电话     
                objSheet.Cells[8, 4] = objStudent.StudentAddress;//家庭住址      

                //【6】打印预览
                excelApp.Visible = true;
                excelApp.Sheets.PrintPreview(true);
                //【7】释放对象
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);//释放
                excelApp = null;
            }
        }    
}
