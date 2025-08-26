using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegistartionPoolReport : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                    Fillkit();

                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void Fillkit()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_ddlPageSize1");
            ddlPageSize.DataSource = Ds.Tables[0];
            ddlPageSize.DataValueField = "ddlPageSize";
            ddlPageSize.DataTextField = "ddlPageSize";
            ddlPageSize.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
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
    private string GetFormNo(string idno)
    {
        DataTable Dt = new DataTable();
        DAL Obj = new DAL();
        string formno;
        string qry = Obj.IsoStart + "Select FormNo from " + Obj.DBName + ".. M_MemberMaster where IdNo = '" + idno.Trim() + "'" + Obj.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (Dt.Rows.Count > 0)
        {
            formno = Dt.Rows[0]["FormNo"].ToString();
        }
        else
        {
            formno = "0";
        }
        return formno;
    }
    public void BindData(int PageIndex)
    {
        lblError.Text = "";
        try
        {
            string Idno = "0";
            //Idno = txtMemId.Text != "" ? txtMemId.Text : "0";
            if (txtMemId.Text != "")
            {
                Idno = GetFormNo(txtMemId.Text);
            }
            else
            {
                Idno = "0";
            }

            GvData1.DataSource = null;
            GvData1.DataBind();
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@status", DdlStatus.SelectedValue);
            prms[2] = new SqlParameter("@PageIndex", PageIndex);
            prms[3] = new SqlParameter("@PageSize", 1000000);
            prms[4] = new SqlParameter("@IsExport", "N");
            prms[5] = new SqlParameter("@RecordCount", ParameterDirection.Output)
            { Direction = ParameterDirection.Output };
            Ds = SqlHelper.ExecuteDataset(constr1, "Sp_RegistartionPoolReport21", prms);
            GvData1.DataSource = Ds.Tables[0];
            GvData1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GvData1.DataBind();
            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["RegistartionPoolReport"] = Ds.Tables[0];
            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";
            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblCount.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"].ToString();
                GvData1.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData1.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            //string FromSessid = "0";
            //string ToSessid = "0";
            //string Idno = "0";
            //Idno = txtMemId.Text != "" ? txtMemId.Text : "0";
            string Idno = "0";
            //Idno = txtMemId.Text != "" ? txtMemId.Text : "0";
            if (txtMemId.Text != "")
            {
                Idno = GetFormNo(txtMemId.Text);
            }
            else
            {
                Idno = "0";
            }
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@status", DdlStatus.SelectedValue);
            prms[2] = new SqlParameter("@PageIndex", 1);
            prms[3] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[4] = new SqlParameter("@IsExport", "Y");
            prms[5] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "Sp_RegistartionPoolReport1", prms);
            Session["RegistartionPoolReportExcel"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["RegistartionPoolReportExcel"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "RegistartionPoolReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=RegistartionPoolReport.xlsx");
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
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
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData1.PageIndex = e.NewPageIndex;
            GvData1.DataSource = Session["RegistartionPoolReport"];
            GvData1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void DdlStatus_SelectedIndexChanged(object sender, EventArgs e)
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
}
