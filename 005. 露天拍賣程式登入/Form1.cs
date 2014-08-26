using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace testtest
{
    public partial class Form1 : Form
    {
        #region include WinAPI

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        #endregion

        private int times = 0; //執行經過時間
        private int adds = 1000; //執行累加時間
        private bool isFirst = true; //判斷第一次執行
        private string savepath = "C:\\test\\text.csv";
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.DocumentCompleted += (sdr, arg) =>
            {
                if (isFirst)
                {

                    InjectionJavascript(wb, "$('#userid').val('" + UserID.Text + "');");
                    InjectionJavascript(wb, "$('input[name=userpass]').val('" + UserPassword.Text + "');");

                    #region Step1 Login

                    times += adds;
                    var step1 = new System.Windows.Forms.Timer();
                    step1.Interval = times;
                    step1.Tick += (o, args) =>
                    {
                        step1.Stop();
                        Login(wb);
                    };
                    step1.Start();

                    #endregion

                    #region Step2 Download file

                    times += adds;
                    var step2 = new System.Windows.Forms.Timer();
                    step2.Interval = times;
                    step2.Tick += (sender1, eventArgs) =>
                    {
                        step2.Stop();
                        wb.Navigate(
                            new Uri(
                                string.Format(
                                    //"http://www.twse.com.tw/ch/trading/exchange/MI_INDEX/MI_INDEX_print.php?genpage=genpage/Report201402/A11220140227MS.php&type=csv",
                                    "http://mybid.ruten.com.tw/export/export_sel_full.php?s_year={0}&s_month={1}&s_day={2}",
                                    DateTime.Now.Year, DateTime.Now.Month.ToString("D2"),
                                    DateTime.Now.Day.ToString("D2"))), false);
                    };
                    step2.Start();

                    #endregion

                    #region Step3 Send key (auto save)

                    times += adds;
                    var step3 = new System.Windows.Forms.Timer();
                    step3.Interval = times;
                    step3.Tick += (sender1, eventArgs) =>
                    {
                        step3.Stop();

                        #region delete file
                        if (!Directory.Exists(Path.GetDirectoryName(savepath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(savepath));
                        }

                        if (File.Exists(savepath))
                        {
                            File.Delete(savepath);
                        }
                        #endregion

                        SendKeys.Send("{LEFT}");
                        SendKeys.Send("{ENTER}");
                    };
                    step3.Start();

                    #endregion

                    #region Step4 Send key (auto enter file name)

                    times += adds;
                    var step4 = new System.Windows.Forms.Timer();
                    step4.Interval = times;
                    step4.Tick += (sender1, eventArgs) =>
                    {
                        step4.Stop();
                        SendKeys.Send(savepath);
                        SendKeys.Send("{ENTER}");
                    };
                    step4.Start();

                    #endregion

                    #region Step5 Send key (auto close)

                    times += adds;
                    var step5 = new System.Windows.Forms.Timer();
                    step5.Interval = times;
                    step5.Tick += (sender1, eventArgs) =>
                    {
                        step5.Stop();
                        SendKeys.Send("{ENTER}");
                    };
                    step5.Start();

                    #endregion
                    
                    isFirst = false;
                }
                
            };
        }

        private void InjectionJavascript(WebBrowser wb, string js)
        {
            HtmlDocument doc = wb.Document;
            HtmlElement head = doc.GetElementsByTagName("head")[0];
            HtmlElement s = doc.CreateElement("script");
            s.SetAttribute("text", js);
            head.AppendChild(s);
        }

        private void Login(WebBrowser wb)
        {
            InjectionJavascript(wb, "$.browser.version=6;"); //偽裝成ie6

            #region get click point from image

            var xy = "";
            using (var bmp = new Bitmap(wb.Width - 12, wb.Height - 12))
            {
                var g = Graphics.FromImage(bmp);
                g.CopyFromScreen(wb.PointToScreen(wb.Location), new Point(0, 0), new Size(wb.Width - 12, wb.Height - 12));
                for (int i = 660; i < 920; i += 2)
                {
                    for (int j = 280; j < 400; j += 2)
                    {
                        var color = bmp.GetPixel(i, j);
                        if (color.R != 253)
                        {
                            xy = (i + 30) + "," + (j + 30);
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(xy))
                    {
                        break;
                    }
                }
            }

            #endregion

            #region send mouse click

            int x = int.Parse(xy.Split(',')[0]);
            ; // X coordinate of the click
            int y = int.Parse(xy.Split(',')[1]);
            ; // Y coordinate of the click
            IntPtr handle = wb.Handle;
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != "Internet Explorer_Server") // The class control for the browser
            {
                handle = GetWindow(handle, 5); // Get a handle to the child window
                GetClassName(handle, className, className.Capacity);
            }
            IntPtr lParam = (IntPtr) ((y << 16) | x); // The coordinates
            IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl)
            const uint downCode = 0x201; // Left click down code
            const uint upCode = 0x202; // Left click up code
            SendMessage(handle, downCode, wParam, lParam); // Mouse button down
            SendMessage(handle, upCode, wParam, lParam); // Mouse button up

            #endregion
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserID.Text) && !string.IsNullOrEmpty(UserPassword.Text))
            {
                isFirst = true;
                wb.Navigate(new Uri("https://member.ruten.com.tw/user/login.htm"), false);
            }
            else
            {
                MessageBox.Show("Please enter user id and password.");
            }
        }

        
    }

    
}