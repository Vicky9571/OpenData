using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysis.Util
{
    public static class CaculataBMI
    {
        public static double Caculate(string weight, string height)
        {
            return Math.Round(Convert.ToDouble(weight) / Math.Pow(Convert.ToDouble(height), 2),2);
        }
    }
}