using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\Users\Tommy\Desktop\111.jpg";

            var ms = new MemoryStream();

            using (var fs = new FileStream(file,FileMode.Open,FileAccess.Read))
            {
                var bb = new byte[fs.Length];
                fs.Read(bb, 0, (int)fs.Length);
                ms.Write(bb, 0, (int)fs.Length);
            }

            var sr2 = new ServiceReference1.SourceClient();
            Console.Write(sr2.UploadFile(ms, Path.GetFileName(file)));

        }
    }
}
