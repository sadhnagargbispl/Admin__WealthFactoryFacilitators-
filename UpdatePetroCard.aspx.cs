using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
//using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class UpdatePetroCard : System.Web.UI.Page
{
    string GroupIdQS;
    ModuleFunction objModuleFun = new ModuleFunction();
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private int CurrentPageIndex;
    private readonly object pagerDataList;
    string KitIdQS;
    protected void Page_Load(object sender, EventArgs e)

    {
        try

        {
            BtnSave.Attributes.Add("onclick", DisableTheButton(Page, BtnSave));
            objModuleFun = new ModuleFunction();
            if (!string.IsNullOrEmpty(Request["petroid"]))
            {
                KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["petroid"]));
            }
            if (!Page.IsPostBack)
            {
                // ClearAll();
                if (Session["AStatus"] != null)
                {
                    if (!string.IsNullOrEmpty(Request["petroid"]))
                    {
                        BtnSave.Text = "Update";
                        BindData();
                    }
                }
                else
                {
                    string scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
                }
            }
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
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
    private void BindData()
    {
        string Dat1;
        try
        {
            string sql = objDal.IsoStart + "Select * From " + objDal.DBName + "..Petrocardpurchase  Where id = '" + KitIdQS + "' " + objDal.IsoEnd;
            Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                txtidno.Text = Dt.Rows[0]["Idno"].ToString();
                txtmembername.Text = Dt.Rows[0]["Name"].ToString();
                txtcardno.Text= Dt.Rows[0]["cardno"].ToString();
                txtcouriername.Text = Dt.Rows[0]["CourierName"].ToString();
                txtdocketno.Text = Dt.Rows[0]["DocketNo"].ToString();
                txtRemarks.Text = Dt.Rows[0]["ApproveRemark"].ToString();
                //txtcourierdate.Text = Convert.ToDateTime(Dt.Rows[0]["Approvedate"]).ToString("dd-MMM-yyyy");
                txtcourierdate.Text =Dt.Rows[0]["Approvedate"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["userstatus"].ToString();
                if (txtcardno.Text == "")
                {
                    txtcardno.ReadOnly = false;
                }
                else {
                    txtcardno.ReadOnly = true;
                }
                if (txtcouriername.Text == "")
                {
                    txtcouriername.ReadOnly = false;
                }
                else
                {
                    txtcouriername.ReadOnly = true;
                }
                if (txtcouriername.Text == "")
                {
                    txtdocketno.ReadOnly = false;
                }
                else
                {
                    txtdocketno.ReadOnly = true;
                }
                
                string approvedate = Dt.Rows[0]["approvedate"].ToString();
                if (approvedate == "")
                {
                    rdblist.Enabled = true;
                    txtcourierdate.ReadOnly = false;
                }
                else
                {
                    rdblist.Enabled = false;
                    txtcourierdate.ReadOnly =true;
                }
                //if (txtActiveStatus.Text.ToUpper() == "Y")
                //{
                //    rdblist.SelectedIndex = 0;
                //}
                //else
                //{
                //    rdblist.SelectedIndex = 1;
                //}
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql;
            string Str;
            string KitId = "";
            string JoinColr = "";
            if (txtcardno.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Enter card no.!');", true);
                return;
            }
            if (txtcouriername.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Enter Courier Name.!');", true);
                return;
            }
            if (txtdocketno.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Enter Docket No.!');", true);
                return;
            }
            if (txtcourierdate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Enter Courier date.!');", true);
            }
            if (rdblist.SelectedIndex == 0)
            {
                txtActiveStatus.Text = "Y";
            }
            else
            {
                txtActiveStatus.Text = "N";
            }
            Sql = "Update Petrocardpurchase set CardNo='"+ txtcardno.Text + "',CourierName='"+ txtcouriername.Text + "',DocketNo='"+ txtdocketno.Text +"',userstatus='" + rdblist.SelectedValue + "',approvedate ='"+txtcourierdate.Text + "',RectimeStamp=getdate(),ApproveRemark = '" + txtRemarks.Text + "'";
            Sql += "  where id='" + ClearInject(KitIdQS) + "'";
            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
            if (updateEffect > 0)
            {
                if (updateEffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Successfully Updated.!');location.replace('PetroCartReport.aspx');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Already Exist.!');", true);
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
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


    protected void txtcardno_TextChanged(object sender, EventArgs e)
    {
        string sql = objDal.IsoStart + "Select * From " + objDal.DBName + "..Petrocardpurchase  Where cardno = '" + txtcardno.Text + "' " + objDal.IsoEnd;
        Dt = new DataTable();
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
        if (Dt.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Card no Already exists other Id.!');", true);
            BtnSave.Enabled = false;
            txtcardno.Text = "";
        }
        else {
            BtnSave.Enabled = true;
        }
        }
}
