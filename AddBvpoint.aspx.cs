using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AddBvpoint : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    string scrname;
    DAL objDAL = new DAL();

    ModuleFunction objModuleFun = new ModuleFunction();
    string CTypeIdQS;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
        {
            HdnCheckTrnns.Value = GenerateRandomStringAdmin(6);
            // Do nothing, user is authenticated
        }
        else
        {
            Response.Redirect("Default.aspx");
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
        {
            if (!Page.IsPostBack)
            {
                // Code for handling non-postback events
            }
        }
        else
        {
            Response.Redirect("logout.aspx");
        }
    }
    protected void BtnFundTransfer_Click(object sender, EventArgs e)
    {
        try
        {
            int updateeffect;
            string StrSql = "Insert into Trnactivecadmin (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            updateeffect = objDAL.SaveData(StrSql);

            if (updateeffect > 0)
            {
                string query = "";
                string formNo = TxtFormNo.Text;
                string voucherNo = "";
                string scrName;

                lblError.Text = "";
                BtnFundTransfer.Enabled = false;
                if (string.IsNullOrWhiteSpace(TxtIDNo.Text))
                {
                    lblError.Text = "Enter Member ID.";
                    return;
                }
                else if (Convert.ToDecimal(TxtFund.Text) <= 0)
                {
                    lblError.Text = "Enter BV Value.";
                    return;
                }

                string remark = "";

                query = "INSERT INTO TrnBV(Sessid, Formno, LegNo, BV, Remark, RecTimeStamp, ActiveStatus,Bvtype,Dsessid) VALUES " +
                        "('" + Session["CurrentSessn"] + "', '" + Convert.ToInt32(TxtFormNo.Text) + "', '" + Convert.ToInt32(RbtLeg.SelectedValue) + "', " +
                        "'" + Convert.ToDecimal(TxtFund.Text) + "', '" + TxtRemarks.Text + "', GETDATE(), 'Y','" + RbtType.SelectedValue + "', " +
                        "CONVERT(VARCHAR, GETDATE(), 112))";
                if (RbtType.SelectedValue == "T")
                {
                    query += ";INSERT INTO Repurchincome(Sessid, Formno, BillNo, Billdate, Repurchincome, Imported, BillType, SoldBy, MSessid, KitId, Dsessid, Remarks,PVValue,PlanType) " +
                             "VALUES ('" + Session["CurrentSessn"] + "', '" + Convert.ToInt32(TxtFormNo.Text) + "', '', GETDATE(), 0, 'N', 'T', 'WR', " +
                             "'" + Session["CurrentSessn"] + "', 0, CONVERT(VARCHAR, GETDATE(), 112), '" + TxtRemarks.Text.Trim() + "','" + Convert.ToDecimal(TxtFund.Text) + "'," +
                             "'I')";
                }

                if (objDAL.SaveData(query) != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('BV Point Added in " + RbtLeg.SelectedItem.Text + " Successfully!!');location.replace('BVpoint.aspx');", true);
                    TxtFund.Text = "";
                    TxtIDNo.Text = "";
                    TxtFormNo.Text = "";
                    TxtRemarks.Text = "";
                    LblMemName.Text = "";
                    LblAmount.Text = "";
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!!');</SCRIPT>";
                }
                //scrName = "<script language='javascript'>window.top.location.reload();</script>";
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrName, false);
            }
            else
            {
                Response.Redirect("AddBvpoint.aspx");
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    private bool Check_IdNo()
    {

        string sql = "SELECT LTRIM(RTRIM(Prefix)) + ' ' + MemFirstName + ' ' + MemLastName AS MemName, FormNo, Mobl " +
                     " FROM  " + objDAL.DBName + "..M_membermaster " +
                     " WHERE IDNO = '" + TxtIDNo.Text.Trim() + "'";

        DataTable dt_ = new DataTable();
        DataTable dt1;
        dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

        if (dt_.Rows.Count == 0)
        {
            LblMemName.Text = "Please enter correct Member ID.";
            LblMemName.ForeColor = System.Drawing.Color.Red;
            TxtIDNo.Text = "";
            BtnFundTransfer.Enabled = false;
            return false;
        }
        else
        {
            sql = "SELECT * FROM TrnBV WHERE Formno = '" + dt_.Rows[0]["FormNo"].ToString() + "' AND Sessid = '" + Session["CurrentSessn"] + "' AND ActiveStatus = 'Y' and BvType='" + RbtType.SelectedValue + "'";
            objDAL = new DAL();
            dt1 = new DataTable();
            dt1 = objDAL.GetData(sql);

            if (dt1.Rows.Count > 0)
            {
                LblMemName.Text = "This ID already has " + RbtType.SelectedItem.Text + " BV in the current session.";
                BtnFundTransfer.Enabled = false;
                return false;
            }
            else
            {
                LblMemName.Text = dt_.Rows[0]["MemName"].ToString();
                LblMobl.Text = dt_.Rows[0]["Mobl"].ToString();
                LblMemName.ForeColor = System.Drawing.Color.Black;
                TxtFormNo.Text = dt_.Rows[0]["FormNo"].ToString();
                BtnFundTransfer.Enabled = true;
                return true;
            }
        }
    }
    protected void TxtIDNo_TextChanged(object sender, EventArgs e)
    {
        Check_IdNo();
    }

    protected void RbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Check_IdNo();
    }

    protected void rbranktype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Check_IdNo();
    }
}
