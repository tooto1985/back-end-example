using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoginTestModel;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["sid"] != null)
        {
            var sid = Request.Cookies["sid"].Value;
            var lte = new LoginTestEntities();

            var member = lte.Member.SingleOrDefault(x => x.Token == sid);

            if (member != null)
            {
                Success(member.Account);
            }
            else
            {
                Response.Cookies["sid"].Expires = DateTime.Now.AddDays(-1);
            }


        }

    }
    protected void CreateButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(AccountTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Text))
        {
            MessageLabel.Text = "請輸入帳號密碼";
            return;
        }

        var lte = new LoginTestEntities();

        var member = lte.Member.SingleOrDefault(x => x.Account == AccountTextBox.Text && x.Password == PasswordTextBox.Text);

        if (member != null)
        {
            member.Token = Guid.NewGuid().ToString();
            lte.SaveChanges();

            if (AutoLoginCheckBox.Checked)
            {
                Response.Cookies["sid"].Value = member.Token;
                Response.Cookies["sid"].Expires = DateTime.Now.AddDays(30);
            }


            Success(member.Account); //MessageLabel.Text = "登入成功";
        }
        else
        {
            MessageLabel.Text = "帳號密碼錯誤";
        }


    }

    private void Success(string name)
    {
        Session["name"] = name;
        Response.Redirect("~/Success.aspx");
    }
}