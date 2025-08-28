using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangeSponsor : System.Web.UI.Page
{
    DAL obj;
    string sql = "";
    string scrname = "";
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var str = "exec('Create table Trnactivecadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," + "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnfundtransferbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')";
            int i = 0;
            i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);
        }
        catch (Exception Ex)
        {
        }
        try
        {
            this.BtnLegShift.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnLegShift));
            if (!IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomStringAdmin(6);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public string GenerateRandomStringAdmin(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }
        return sResult;
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please wait...';");
            sb.Append("this.disabled = true;");
            sb.Append(pge.Page.GetPostBackEventReference(btn));
            sb.Append(";");
            return sb.ToString();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    protected void BtnLegShift_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            BtnLegShift.Enabled = false;

            if (string.IsNullOrWhiteSpace(TxtIDNo.Text))
            {
                lblError.Text = "Enter Member ID.";
                BtnLegShift.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('" + lblError.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                return;
            }
            else if (string.IsNullOrWhiteSpace(TxtSpIDNo.Text))
            {
                lblError.Text = "Enter Sponsor ID.";
                BtnLegShift.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('" + lblError.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                return;
            }
            else
            {
                if (!Check_IdNo(TxtFormNo, LblMemName, TxtIDNo))
                {
                    lblError.Text = "Invalid Member ID.";
                    BtnLegShift.Enabled = true;
                    scrname = "<SCRIPT language='javascript'>alert('" + lblError.Text + "');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                if (!Check_IdNo(TxtFormNos, LblSponserName, TxtSpIDNo))
                {
                    lblError.Text = "Invalid Sponsor ID.";
                    BtnLegShift.Enabled = true;
                    scrname = "<SCRIPT language='javascript'>alert('" + lblError.Text + "');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                //string sql = "SELECT * FROM " + objDal.DBName + "..R_MemTreeRelation WHERE FormNo = '" + Convert.ToInt32(TxtFormNo.Text) + "' AND FormNoDwn = '" + Convert.ToInt32(TxtFormNos.Text) + "'";
                //DataTable dt = new DataTable();
                //dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                //if (dt.Rows.Count > 0)
                //{
                //    lblError.Text = "Sponsor couldn't be changed.";
                //    string alertMessage = "alert('Sponsor couldn't be changed.');location.replace('ChangeSponsor.aspx');";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", alertMessage, true);
                //    BtnLegShift.Enabled = false;
                //    return;
                //}
                //else
                //{
                    Leg_Shift();
                //}
                BtnLegShift.Enabled = true;
                TxtIDNo.Text = "";
                TxtSpIDNo.Text = "";
                LblMemName.Text = "";
                LblSponserName.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void TxtSpIDNo_TextChanged(object sender, EventArgs e)
    {
        Check_IdNo(TxtFormNos, LblSponserName, TxtSpIDNo);
    }
    protected void TxtIDNo_TextChanged(object sender, EventArgs e)
    {
        Check_IdNo(TxtFormNo, LblMemName, TxtIDNo);
        Get_SponsorDetail();
    }
    private void Get_SponsorDetail()
    {
        string sql = "SELECT LTRIM(RTRIM(Prefix)) + ' ' + MemFirstName + ' ' + MemLastName AS MemName, IDNo, FormNo " +
                     "FROM " + objDal.DBName + "..M_MemberMaster " +
                     "WHERE FormNo IN (SELECT RefFormno FROM " + objDal.DBName + "..M_MemberMaster WHERE FormNo = '" + TxtFormNo.Text.Trim() + "')";
        DataTable dt_ = new DataTable();
        dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0]; ;
        if (dt_.Rows.Count > 0)
        {
            LblOldSponsor.Text = dt_.Rows[0]["IDNo"].ToString() + " [" + dt_.Rows[0]["MemName"].ToString() + "]";
        }
    }
    private bool Check_IdNo(TextBox txtf, Label lblm, TextBox txtid)
    {
        obj = new DAL();

        string sql = "SELECT LTRIM(RTRIM(Prefix)) + ' ' + MemFirstName + ' ' + MemLastName AS MemName, FormNo " +
                     "FROM " + objDal.DBName + "..M_MemberMaster WHERE IDNO = '" + txtid.Text.Trim() + "'";
        DataTable dt_ = new DataTable();
        dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
        if (dt_.Rows.Count == 0)
        {
            lblm.Text = "Please enter correct Member ID.";
            lblm.ForeColor = System.Drawing.Color.Red;
            txtid.Text = string.Empty;
            BtnLegShift.Enabled = false;
            return false;
        }
        else
        {
            txtf.Text = dt_.Rows[0]["FormNo"].ToString();
            lblm.Text = dt_.Rows[0]["MemName"].ToString();
            lblm.ForeColor = System.Drawing.Color.Black;
            BtnLegShift.Enabled = true;
            return true;
        }
    }
    private void Leg_Shift()
    {
        int updateeffect;
        string StrSql = "Insert into Trnactivecadmin (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
        updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrSql));
        if (updateeffect > 0)
        {
            try
        {
            int legNo = 0;
            string remark = "";

            remark = "Sponsor Change of Idno=" + TxtIDNo.Text +
            ", Old Sponsor IDno=" + LblOldSponsor.Text +
            ", New Sponsor IDno=" + TxtSpIDNo.Text;
            string sql = "SELECT CASE WHEN MAX(LegNo) IS NULL THEN '1' ELSE MAX(LegNo) + 1 END AS LegNo FROM " + objDal.DBName + "..R_MemTreeRelation WHERE FormNo = '" + Convert.ToInt32(TxtFormNos.Text) + "' AND MLevel = 1";
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                legNo = Convert.ToInt32(dt.Rows[0]["LegNo"]);
            }

            string SqlStr = "BEGIN TRANSACTION Exec Sp_RefLegShift '" + Convert.ToInt32(TxtFormNo.Text) + "','" +
                   Convert.ToInt32(TxtFormNos.Text) + "','" + legNo + "' COMMIT TRANSACTION";
            SqlStr += "; INSERT INTO UserHistory(UserId, UserName, PageName, Activity, ModifiedFlds, RecTimeStamp, MemberId) VALUES " +
                   "('" + Convert.ToInt32(Session["UserID"]) + "', '" + Session["UserName"] + "', 'Changer Sponsor', " +
                   "'Change Sponsor', '" + remark + "', GETDATE(), '" + TxtFormNo.Text + "')";

            int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, SqlStr));
            if (i != 0)
            {
                lblError.Text = "Sponsor Id Changed Successfully!!";

            }
            else
            {
                lblError.Text = "Problem in Sponsor change.";
            }
            string alertMessage = "alert('" + lblError.Text + "');location.replace('ChangeSponsor.aspx');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", alertMessage, true);
            //string scrname = "<SCRIPT language='javascript'>alert('" + lblError.Text + "');</SCRIPT>";
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            //Response.Redirect("ChangeSponsor.aspx");
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            return;
        }
        }
        else
        {
            Response.Redirect("ChangeSponsor.aspx");
        }
    }
}
