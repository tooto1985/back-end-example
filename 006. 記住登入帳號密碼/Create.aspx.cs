using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoginTestModel;

public partial class Create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Create_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(AccountTextBox.Text))
        {
            MessageLabel.Text = "沒有帳號";
            return;
        }

        if (string.IsNullOrEmpty(PasswordTextBox.Text))
        {
            MessageLabel.Text = "沒有密碼";
            return;
        }


        var lte = new LoginTestEntities();
        if (lte.Member.Count(x => x.Account == AccountTextBox.Text) > 0)
        {
            MessageLabel.Text = "重複帳號";
        }
        else
        {
            var member = new Member();
            member.Account = AccountTextBox.Text;
            member.Password = PasswordTextBox.Text;
            member.Token = Guid.NewGuid().ToString();
            lte.Member.AddObject(member);
            lte.SaveChanges();
            MessageLabel.Text = "建立完成";
        }


    }


}