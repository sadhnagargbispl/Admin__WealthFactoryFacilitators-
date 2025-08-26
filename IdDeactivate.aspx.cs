using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Activities.Expressions;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class IdDeactivate : System.Web.UI.Page
{
    DAL objDal = new DAL();
    string Sql = "";
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //this.BtnDedactive.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnDedactive));
            lblrecordcount.Text = "";

            if (!IsPostBack)
            {
                txtMemberId.Text = "";
                HdnCheckTrnns.Value = GenerateRandomStringAdmin(6);
                //if (Request.QueryString.HasKeys)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && Request.QueryString["key"] != null)
                    {
                        txtMemberId.Text = Request.QueryString["key"].ToString();
                        //ShowDetail();

                    }
                }
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
    private void ShowDetail()
    {
        try
        {
            string idNo;
            lblError.Text = "";
            DataTable Dt = new DataTable();
            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                idNo = ClearInject(txtMemberId.Text);
                string qry = objDal.IsoStart + "Select FormNo from " + objDal.DBName + "..M_MemberMaster WHERE email = '" + idNo + "'" + objDal.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    TxtFormNo.Text = Dt.Rows[0][0].ToString();
                }
                else
                {
                    lblError.Text = "Member ID not exist. Please provide correct member ID.";
                    lblError.Visible = true;
                    return;
                }
            }
            else
            {
                lblError.Text = "Member Id can not be blank. Please provide member ID to proceed.";
                lblError.Visible = true;
            }
        }
        catch (Exception EX)
        {
            throw new Exception(EX.Message);
        }
    }

    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //GvData.PageIndex = e.NewPageIndex;
            //GvData.DataSource = Session["GData"];
            //GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            StrObj = StrObj.Trim();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return StrObj;
    }
    protected void txtMemberId_TextChanged(object sender, EventArgs e)
    {
        string idNo;
        string Kitid;
        lblError.Text = "";
        DataTable Dt = new DataTable();
        idNo = ClearInject(txtMemberId.Text);
        string qry = objDal.IsoStart + "Select FormNo,Activestatus,kitid,memfirstname from " + objDal.DBName + "..M_MemberMaster WHERE email = '" + idNo + "'" + objDal.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (Dt.Rows.Count == 0)
        {
            lblError.Text = "Member ID not exist. Please provide correct member ID.";
            lblError.Visible = true;
            BtnDedactive.Enabled = false;
            txtMemberId.Text = "";
            return;
        }
        else if (Dt.Rows[0]["Activestatus"].ToString() == "N")
        {
            lblError.Text = "This Member ID already De-Active.";
            lblError.Visible = true;
            BtnDedactive.Enabled = false;
            txtMemberId.Text = "";
            return;
        }
        else if (Dt.Rows[0]["Activestatus"].ToString() == "Y")
        {
            LblMemberName.Text = Dt.Rows[0]["memfirstname"].ToString();
            TxtFormNo.Text = Dt.Rows[0]["FormNo"].ToString();
            LblKitid.Text = Dt.Rows[0]["kitid"].ToString();
            BtnDedactive.Enabled = true;
        }
        try
        {
            DataTable dt1 = new DataTable();
            Sql = objDal.IsoStart + "select COUNT(*) as Cnt from " + objDal.DBName + "..repurchincome where cast(Billdate as date) = cast(GETDATE() as date) AND formno = '" + TxtFormNo.Text + "'";
            Sql += " AND kitid = '" + LblKitid.Text + "' AND FORMAT(GETDATE(), 'HH:mm') <= '23:58' AND kitid <> 4 AND PlanType = 'I'" + objDal.IsoEnd;
            DataTable Dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];
            if (Dt_.Rows[0]["Cnt"].ToString() == "0")
            {
                lblError.Text = "You Are Not Able To This ID Roll Back.Please Contact To Admin.!";
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                BtnDedactive.Enabled = false;
                return;
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnDedactive_Click(object sender, EventArgs e)
    {

        try
        {
            int updateeffect_;
            string StrSql = "Insert into Trnactivecadmin (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            updateeffect_ = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrSql));
            if (updateeffect_ > 0)
            {
                lblError.Visible = false;
                if (string.IsNullOrWhiteSpace(txtMemberId.Text))
                {
                    lblError.Text = "Please Enter Member ID.";
                    lblError.Visible = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(TxtBillNo.Text))
                {
                    lblError.Text = "Please Enter Bill No.";
                    lblError.Visible = true;
                    return;
                }
                string Sql, scrname;
                Sql = "exec Sp_IddeactivateAdminINR '" + ClearInject(TxtFormNo.Text) + "','" + LblKitid.Text + "','" + TxtBillNo.Text + "'";
                scrname = "ID";
                string Str_Sql = string.Empty;
                Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
                int updateeffect;
                updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
                if (updateeffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + scrname + " Roll Back Successfully.!');location.replace('IdDeactivate.aspx');", true);
                    TxtFormNo.Text = "";
                    txtMemberId.Text = "";
                    TxtBillNo.Text = "";
                    BtnDedactive.Visible = false;
                    lblrecordcount.Text = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Id do not Roll Back!! ')", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Try Later');location.replace('IdDeactivate.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }

    protected void TxtBillNo_TextChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtBillNo.Text))
        {
            lblError.Text = "Please Enter Bill No.";
            lblError.Visible = true;
            return;
        }
        
        DataTable dt1 = new DataTable();
        string Sql_ = objDal.IsoStart + "Exec Sp_getBillNoINR '" + TxtFormNo.Text + "','" + LblKitid.Text + "','" + TxtBillNo.Text + "' " + objDal.IsoEnd;
        DataTable Dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql_).Tables[0];
        if (Dt_.Rows[0]["AuthorizationStatus"].ToString() == "False")
        {
            lblError.Text = "You Are Not Able To This ID Bill No.Please Check One By One Process.!";
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Red;
            BtnDedactive.Enabled = false;
            return;
        }
        else if (Dt_.Rows[0]["AuthorizationStatus"].ToString() == "FFalse")
        {
            lblError.Text = "This Bill No already Deleted.!";
            lblError.Visible = true;
            lblError.ForeColor = System.Drawing.Color.Red;
            BtnDedactive.Enabled = false;
            return;
        }
        else
        {
            lblError.Visible = false;
            BtnDedactive.Enabled = true;
        }
    }
}
