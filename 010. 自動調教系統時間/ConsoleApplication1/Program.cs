using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var hwq = HttpWebRequest.Create("http://www.google.com.tw").GetResponse();
            var datetime = DateTime.Parse(hwq.Headers["date"]);
            var localdatetime = TimeZone.CurrentTimeZone.ToLocalTime(datetime);
            RunCmd(string.Format("date {0}", localdatetime.ToShortDateString()));
            RunCmd(string.Format("time {0:hh:mm:ss}", localdatetime));
        }


        private static string RunCmd(string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";           //設定程序名
            p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
            p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出
            p.StartInfo.CreateNoWindow = true;          //設置不顯示窗口
            p.Start();   //啟動
            return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果

        }

    }
}
