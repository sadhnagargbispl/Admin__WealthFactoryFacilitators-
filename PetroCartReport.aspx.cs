using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Runtime.InteropServices.ComTypes;
using Irony.Parsing;
public partial class PetroCartReport : System.Web.UI.Page
{
    string scrname;
    static DataTable dtExcelData = new DataTable();
    DAL objDAL = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnUploadExcel.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnUploadExcel));
            //this.btnDownload.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnDownload));
            this.btnConfirm.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnConfirm));
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {

                    // BindData();

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
    protected void DeleteGroup(object sender, EventArgs e)
    {
        // DivRemark.Visible = true;
    }
    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
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
    public void BindPaymode()
    {
        //try
        //{
        //    DataTable dtData = new DataTable();
        //    string SqlStr = objDAL.IsoStart + " exec Sp_Getpaymode " + objDAL.IsoEnd;
        //    dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, SqlStr).Tables[0];
        //    ddlPayment.DataSource = dtData;
        //    ddlPayment.DataValueField = "PId";
        //    ddlPayment.DataTextField = "PayMode";
        //    ddlPayment.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}
    }
    public void BindData()
    {
        try
        {
            string startDate;
            string endDate;
            string ID = "";
            string WalletAddress;
            string ThnHash;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");

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
            if (string.IsNullOrEmpty(TxtMemID.Text))
            {
                ID = "";
            }
            else
            {
                ID = TxtMemID.Text;
            }

            DataTable Dt_GetApi = new DataTable();
            string sql = "exec sp_GetPetroCartReport '" + ID + "','" + startDate + "', '" + endDate + "','N'";
            Dt_GetApi = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = Dt_GetApi;
            GvData.DataBind();
            Session["GData"] = Dt_GetApi;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
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
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
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
            string startDate;
            string endDate;
            string ID = "";
            string WalletAddress;
            string ThnHash;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");

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
            if (string.IsNullOrEmpty(TxtMemID.Text))
            {
                ID = "";
            }
            else
            {
                ID = TxtMemID.Text;
            }

            DataTable Dt_GetApi = new DataTable();
            string sql = "exec sp_GetPetroCartReport '" + ID + "','" + startDate + "', '" + endDate + "','Y'";
            Dt_GetApi = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            Session["InvestmentReportExcel"] = Dt_GetApi;
            ExportExcel();
        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["InvestmentReportExcel"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "InvestmentReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=PetroCardPurchase.xlsx");
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
    protected void BtnUploadExcel_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            lblMessage.Visible = false;
            string filePath = Path.GetTempFileName();
            FileUpload1.SaveAs(filePath);
            dtExcelData = ReadExcelData(filePath);
            if (dtExcelData.Rows.Count > 0)
            {
                gvExcelData.DataSource = dtExcelData;
                gvExcelData.DataBind();
                btnConfirm.Visible = true;
                btnConfirm.Enabled = true;
            }
            else
            {
                btnConfirm.Visible = false;
                btnConfirm.Enabled = false;
                lblMessage.Text = "No data found in the uploaded file.";
            }
        }
        else
        {
            lblMessage.Text = "Please select an Excel file.";
        }
    }
    private DataTable ReadExcelData(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        DataTable dt = new DataTable();
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
                return dt;

            // Define table columns
            dt.Columns.Add("Order No", typeof(string));
            dt.Columns.Add("Member ID", typeof(string));
            dt.Columns.Add("Member Name", typeof(string));
            dt.Columns.Add("DispatchStatus", typeof(string));
            dt.Columns.Add("DispatchDate", typeof(DateTime));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("CardNo", typeof(string));
            dt.Columns.Add("DocketNo", typeof(string));
            dt.Columns.Add("CourierName", typeof(string));

            int rowCount = worksheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)  // Assuming 1st row is header
            {
                DataRow dr = dt.NewRow();
                dr["Order No"] = worksheet.Cells[row, 1].Text?.Trim();
                dr["Member ID"] = worksheet.Cells[row, 2].Text?.Trim();
                dr["Member Name"] = worksheet.Cells[row, 3].Text?.Trim();
                dr["DispatchStatus"] = worksheet.Cells[row, 4].Text?.Trim();

                var dateCell = worksheet.Cells[row, 5].Value;
                if (dateCell != null)
                {
                    if (dateCell is double) // Excel OADate format
                    {
                        dr["DispatchDate"] = DateTime.FromOADate((double)dateCell);
                    }
                    else if (DateTime.TryParse(dateCell.ToString(), out DateTime parsedDate))
                    {
                        dr["DispatchDate"] = parsedDate;
                    }
                    else
                    {
                        dr["DispatchDate"] = DBNull.Value; // Store NULL for invalid dates
                    }
                }
                else
                {
                    dr["DispatchDate"] = DBNull.Value; // Store NULL if the cell is empty
                }

                dr["Remark"] = worksheet.Cells[row, 6].Text?.Trim();
                dr["CardNo"] = worksheet.Cells[row, 7].Text?.Trim();
                dr["DocketNo"] = worksheet.Cells[row, 8].Text?.Trim();
                dr["CourierName"] = worksheet.Cells[row, 9].Text?.Trim();

                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            DataTable dt = GetSampleData(); // Sample data
            var ws = wb.Worksheets.Add(dt, "Sample Format");

            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                byte[] content = stream.ToArray();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SampleFormat.xlsx");
                Response.BinaryWrite(content);
                Response.End();
            }
        }
    }
    private DataTable GetSampleData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Order No", typeof(string));
        dt.Columns.Add("Member ID", typeof(string));
        dt.Columns.Add("Member Name", typeof(string));
        dt.Columns.Add("DispatchStatus", typeof(string));
        dt.Columns.Add("DispatchDate", typeof(DateTime));
        dt.Columns.Add("Remark", typeof(string));
        dt.Columns.Add("CardNo", typeof(string));
        dt.Columns.Add("DocketNo", typeof(string));
        dt.Columns.Add("CourierName", typeof(string));
        dt.Rows.Add("1", "BH123456", "BIG HITTER", "Y", DateTime.Now, "Sample remark", "453646", "S323", "Sample Courier Name");
        return dt;
    }
    protected void gvExcelData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvExcelData.PageIndex = e.NewPageIndex;
            gvExcelData.DataSource = Session["BulkData"];
            gvExcelData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        int cnt = 0;
        int cnt1 = 0;
        foreach (DataRow row in dtExcelData.Rows)
        {
            DataTable Dt_GetApi = new DataTable();
            if (row["CardNo"].ToString().Replace("'", "") != "")
            {
                string sql = "select * from " + objDAL.DBName + "..Petrocardpurchase where cardno = '" + row["CardNo"].ToString().Replace("'", "") + "'";
                Dt_GetApi = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                if (Dt_GetApi.Rows.Count > 0)
                {
                    cnt1++;
                }
                else
                {
                    string Sql = "Update Petrocardpurchase set CardNo = '" + row["CardNo"].ToString().Replace("'", "") + "',CourierName = '" + row["CourierName"] + "',";
                    Sql += "DocketNo = '" + row["DocketNo"].ToString().Replace("'", "") + "',Approvedate = '" + row["DispatchDate"] + "',ApproveRemark = '" + row["Remark"] + "',UserStatus = '" + row["DispatchStatus"] + "', ";
                    Sql += " LastModified='Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "' where idno = '" + row["Member ID"] + "'";
                    string Str_Sql = string.Empty;
                    Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
                    int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
                    cnt++;
                }
            }
        }
        gvExcelData.DataSource = null;
        gvExcelData.DataBind();
        btnConfirm.Enabled = false;
        btnConfirm.Visible = false;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + cnt + " Data uploaded successfully." + cnt1 + " Total Record Already Exist.!');location.replace('PetroCartReport.aspx');", true);
        return;
    }
}
