using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Source"。
public class Source : ISource
{
    public bool UploadFile(MemoryStream ms, string filename)
    {
        var result = false;
        try
        {
            var savePath = string.Format("d:\\test\\{0}", filename);
            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            }
            using (var fs = File.Create(savePath))
            {
                var bb = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(bb, 0, (int)ms.Length);
                fs.Write(bb, 0, (int)ms.Length);
            }
            result = true;
        }
        catch
        {
            result = false;
        }
        return result;
    }
}

