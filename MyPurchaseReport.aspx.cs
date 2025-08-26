using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyPurchaseReport : System.Web.UI.Page
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
                     Fill_DDlPackage();
                    FillReport();
                   
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

    private void Fill_DDlPackage()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr1, "Sp_Fill_DDlPackage");
            DDlPackage.DataSource = Ds.Tables[0];
            DDlPackage.DataValueField = "kitid";
            DDlPackage.DataTextField = "kitname";
            DDlPackage.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void FillReport()
    {
        string TransactionID = "0";
        string WalletAddress = "";
        string startDate;
        string endDate;
        DateTime currentDate = DateTime.Now;
        string formattedDate = currentDate.ToString("dd-MMM-yyyy");

        if (!string.IsNullOrEmpty(txtMemId.Text))
        {
            TransactionID = txtMemId.Text.Trim();
        }
        else
        {
            TransactionID = "0";
        }

        if (string.IsNullOrEmpty(txtStartDate.Text))
        {
            startDate = "12-oct-2017";
        }
        else
        {
            startDate = txtStartDate.Text;
        }

        if (string.IsNullOrEmpty(txtEndDate.Text))
        {
            endDate = formattedDate;
        }
        else
        {
            endDate = txtEndDate.Text;
        }

        string sql = "exec sp_GetPurchaseReportNew '" + TransactionID + "', '" + startDate + "', '" + endDate + "','"+ DDlPackage .SelectedValue +"'";
        DataTable dtData = new DataTable();
        Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);
        GvData1.DataSource = Ds.Tables[0];
        GvData1.DataBind();
        int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["Recordcount"].ToString());
        lblCount.Text = "Total Record: " + recordCount.ToString();
        lblinv.Text = "Total Amount: " + Ds.Tables[1].Rows[0]["Total"].ToString();
        GvData1.Visible = true;
        Session["GData"] = dtData;
        ViewState["WithDrawDate"] = "BankCode";
        ViewState["Sort_Order"] = "ASC";
        btnExport.Enabled = Ds.Tables[0].Rows.Count > 0;
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillReport();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string TransactionID = "0";
            string WalletAddress = "";
            string startDate;
            string endDate;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");

            if (!string.IsNullOrEmpty(txtMemId.Text))
            {
                TransactionID = txtMemId.Text.Trim();
            }
            else
            {
                TransactionID = "0";
            }

            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = "12-oct-2017";
            }
            else
            {
                startDate = txtStartDate.Text;
            }

            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = formattedDate;
            }
            else
            {
                endDate = txtEndDate.Text;
            }

            string sql = "exec sp_GetPurchaseReportNew '" + TransactionID + "', '" + startDate + "', '" + endDate + "','"+ DDlPackage .SelectedValue +"'";

            DataTable dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["MyPurchaseReportExcel"] = dtData;
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
            DataTable dt = (DataTable)Session["MyPurchaseReportExcel"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "MyPurchaseReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=MyPurchaseReport.xlsx");
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
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData1.PageIndex = e.NewPageIndex;
            FillReport();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void DDlPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillReport();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
