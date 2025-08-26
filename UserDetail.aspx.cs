using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class UserDetail : System.Web.UI.Page
{
    private DataTable dtData = new DataTable();
    private DataTable Dt = new DataTable();
    DataSet Ds = new DataSet();
    DAL Obj = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private DAL objDAL = new DAL();
    private string BankCodeQS = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objDAL = new DAL();
            string searchtext = (string)Session["Search"];
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["typeforreportshow"]))
                {
                    FillKit();
                    //FillDate();
                }
                else
                {
                    GvData.Visible = false;
                    gvContainer.Visible = false;
                    FillKit();
                    //FillDate();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }


    private void FillKit()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_GetKitMaster");
            CmbKit.DataSource = Ds.Tables[0];
            CmbKit.DataValueField = "KitID";
            CmbKit.DataTextField = "Kitname";
            CmbKit.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillDate()
    {
        try
        {
            objDAL = new DAL();
            string Str = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate ";
            dtData = objDAL.GetData(Str);
            if (dtData.Rows.Count > 0)
            {
                txtStartDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
                txtEndDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void BindData(int PageIndex)
    {
        try
        {

            lblCount.Text = "";
            string KitName = "";
            string BankName = "";
            string Idno = "";
            string startDate = "";
            string endDate = "";
            KitName = ChkKit.Checked ? CmbKit.SelectedValue : "";
            Idno = ChkMem.Checked ? txtMember.Text : "";
            BankName = ChkBank.Checked ? DdlBank.SelectedItem.Text : "";
            startDate = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            endDate = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            DataSet Ds_u = new DataSet();
            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[10];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@KitName", KitName);
            prms[2] = new SqlParameter("@BankName", BankName);
            prms[3] = new SqlParameter("@SearchType", "J");
            prms[4] = new SqlParameter("@StartDate", startDate);
            prms[5] = new SqlParameter("@EndDate", endDate);
            prms[6] = new SqlParameter("@PageIndex", PageIndex);
            prms[7] = new SqlParameter("@PageSize", 10000);
            prms[8] = new SqlParameter("@IsExport", "N");
            //prms[9] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            prms[9] = new SqlParameter("@RecordCount", SqlDbType.Int);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_getmemberProfileDetail1", prms);
            //Ds = SqlHelper.ExecuteDataset(constr1, CommandType.StoredProcedure, "sp_GetmemberProfileDetail", prms);
            GvData.DataSource = Ds.Tables[0];
            GvData.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GvData.DataBind();
            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["MemberList"] = Ds.Tables[0];
            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";
            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblCount.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"];
                GvData.Visible = true;
                gvContainer.Visible = true;
            }
            else
            {
                GvData.Visible = false;
                gvContainer.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["EX_GETMEMBERPROFILEDETAIL"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "MemberProfile");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=MemberProfile.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        {
            try
            {
                GvData.PageIndex = e.NewPageIndex;
                GvData.DataSource = Session["MemberList"];
                GvData.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string KitName = "0";
            string BankName = "0";
            string Idno = "0";
            string startDate = "0";
            string endDate = "0";
            KitName = ChkKit.Checked ? CmbKit.SelectedValue : "";
            Idno = ChkMem.Checked ? txtMember.Text : "";
            BankName = ChkBank.Checked ? DdlBank.SelectedItem.Text : "";
            startDate = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            endDate = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");

            SqlParameter[] prms = new SqlParameter[10];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@KitName", KitName);
            prms[2] = new SqlParameter("@BankName", BankName);
            prms[3] = new SqlParameter("@SearchType", CmbType.SelectedValue);
            prms[4] = new SqlParameter("@StartDate", startDate);
            prms[5] = new SqlParameter("@EndDate", endDate);
            prms[6] = new SqlParameter("@PageIndex", 1);
            prms[7] = new SqlParameter("@PageSize", 100000000);
            prms[8] = new SqlParameter("@IsExport", "Y");
            prms[9] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetmemberProfileDetail", prms);
            Session["EX_GETMEMBERPROFILEDETAIL"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "Error In Exporting File");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string Qry5 = "Select * from V#Admin";
            dtData = new DataTable();
            dtData = Obj.GetData(Qry5);
            if (dtData.Rows.Count > 0)
            {
                lblactive.Text = " Total Activation: " + dtData.Rows[0]["TotalActivate"];
                lbldeactive.Text = " Total Deactivation: " + dtData.Rows[0]["TodayDeactivate"];
            }

            BindData(1);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }


}


