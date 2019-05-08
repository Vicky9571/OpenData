using DataAnalysis.Models;
using DataAnalysis.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataAnalysis.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Display()
        {
            return View();
        }
        public JsonResult SubmitData(UserInfo info)
        {
        
           SqlConnection con = SqlHelper.GetConnection();
            string  gender=string.Empty;
            //插入数据
           var result = SqlHelper.ExecuteNonQuery(con, CommandType.Text, "insert into UserInfo (Gender,Height,Weight,Income,IsObese,IsDiabetes,Age,State) values('" + info.Gender + "','" + info.Height + "','" + info.Weight + "','" + info.Income + "','" + info.IsObese + "','" + info.IsDiabetes + "','" + info.Age + "','" + info.State + "')");

            var select = "select "+info.Age+","+info.Income+","+info.Gender+" from DataCollection where Location='" + info.State + "'and Year='2016'";
            Display data = new Display();
            data.State = info.State;
            switch (info.Gender)
            {
                case "Female_county":
                    data.Gender = "Female";
                    break;
                case "Male_county":
                    data.Gender = "Male";
                    break;
                default:
                    data.Gender = "Male";
                    break;
            }
            data.BMI = CaculataBMI.Caculate(info.Weight, info.Height);
            //查询
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, select);
            data.AgeRate = ds.Tables[0].Rows[0][0].ToString();
            data.MoneyRate = ds.Tables[0].Rows[0][1].ToString();
            data.GenderRate = ds.Tables[0].Rows[0][2].ToString();
            var pre = "select " + data.Gender + "," + info.Gender + "," + info.Income.Substring(0, 6) + "," + info.Age.Substring(0, 4) + "," + "Obesity from Prediction where Location='" + info.State + "'";
            DataSet preds = SqlHelper.ExecuteDataset(con, CommandType.Text, pre);
            var preGender = preds.Tables[0].Rows[0][0].ToString();
            var preGc = preds.Tables[0].Rows[0][1].ToString();
            var preMoney = preds.Tables[0].Rows[0][2].ToString();
            var preAge = preds.Tables[0].Rows[0][3].ToString();
            var obRate = preds.Tables[0].Rows[0][4].ToString();
            data.ObRate = Math.Round(Convert.ToDouble(obRate),2).ToString();
            var pe = Convert.ToDouble(preGender) / Convert.ToDouble(preGc) * 0.5;
            var pp = pe * Convert.ToDouble(preMoney) * Convert.ToDouble(preAge) * Convert.ToDouble(preGender) / 1000;
            data.ResultRate = Math.Round(pe, 2).ToString();
            return Json(data);
        }
    }
}