using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service"。
public class Service : IService
{
	public void DoWork()
	{
	}


    public string GetData()
    {
        return "Data from Service";
    }





    public List<Qoo> GetJson()
    {
        var ls = new List<Qoo>();
        for (var i = 0; i < 1000; i++)
        {
            ls.Add(new Qoo() { key = "1", message = "test", name = "cc" });
        }

        return ls;
    }
}


[DataContract]
public class Qoo {
    [DataMember]
    public string name { get; set; }
    [DataMember]
    public string key { get; set; }
    [DataMember]
    public string message { get; set; }

}
