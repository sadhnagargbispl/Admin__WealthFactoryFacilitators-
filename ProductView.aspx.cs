using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data.SqlClient;
using System.Globalization;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Web;
using Irony.Parsing;
public partial class ProductView : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    public string formNo;
    string sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    ModuleFunction objModuleFun;
    string ReqNo;

    protected void Page_Load(object sender, EventArgs e)
    {
        //objDAL = new DAL(HttpContext.Current.Session["MlmDatabase" + Session["CompID"].ToString()]);
        string scrname;
        //objModuleFun = new ModuleFunction(HttpContext.Current.Session["MlmDatabase" + Session["CompID"].ToString()]);

        if (!string.IsNullOrEmpty(Request["OrderID"]))
        {
            //ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["OrderID"]));
            ReqNo = Request["OrderID"];
        }

        if (!Page.IsPostBack)
        {
            if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(Request["OrderID"]))
                {
                    LblOrderNo.Text = Request["OrderID"];
                    BindData();
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            }
        }
    }
    public void BindData(string SrchCond = "")
    {
        try
        {
            string sql = "";
            sql = "Exec Sp_getProductView '" + Request["OrderID"] + "'";
            dtData = new DataTable();
            dtData = objDAL.GetData(sql);
            GvData.DataSource = dtData;
            GvData.DataBind();

            if (dtData.Rows.Count > 0)
            {
                LblMemberID.Text = dtData.Rows[0]["Member ID"].ToString();
                LblMemberName.Text = dtData.Rows[0]["Member Name"].ToString();
            }

            Session["GproducrviewData"] = dtData;
        }
        catch (Exception ex)
        {
            // Handle exception (if needed, you can log ex.Message or ex.StackTrace)
        }
    }

    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvData.PageIndex = e.NewPageIndex;
        GvData.DataSource = Session["GproducrviewData"];
        GvData.DataBind();
    }
}
