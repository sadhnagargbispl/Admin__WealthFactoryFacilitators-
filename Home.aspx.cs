using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Activities;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    DAL obj = new DAL();
    DataTable dt = new DataTable();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if ( Session["AStatus"] !=  null )
            {
                if (!Page.IsPostBack)
                {
                    FillData();
                    FillWalletSummary();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    protected void FillData()
    {
        try
        {
            DataTable dt = new DataTable();

            string sql;
            sql = obj.IsoStart + "  Exec AdminDashBoard " + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                RepMemberData.DataSource = dt;
                RepMemberData.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void FillWalletSummary()
    {
        try
        {
            string sql;
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt5 = new DataTable();
            DataTable dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DAL obj = new DAL();

            sql = obj.IsoStart + "  Exec Sp_WalletTotalSummary " + obj.IsoEnd;
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);
            dt1 = ds.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                RepWallet.DataSource = dt1;
                RepWallet.DataBind();
            }

            dt2 = ds.Tables[1];
            if (dt2.Rows.Count > 0)
            {
                RepFundWithrawal.DataSource = dt2;
                RepFundWithrawal.DataBind();
            }
            //dt5 = ds.Tables[5];
            //if (dt5.Rows.Count > 0)
            //{
            //    Retbusinss.DataSource = dt5;
            //    Retbusinss.DataBind();
            //}
            dt6 = ds.Tables[5];
            if (dt6.Rows.Count > 0)
            {
                RetDailyMonthlybusinss.DataSource = dt6;
                RetDailyMonthlybusinss.DataBind();
            }
            
        }
        catch (Exception ex)
        {
        }
    }
}