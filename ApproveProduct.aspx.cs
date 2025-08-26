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
public partial class ApproveProduct : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    public string formNo;
    string sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnApproove.Attributes.Add("onclick", DisableTheButton(this.Page, btnApproove));
        BtnReject.Attributes.Add("onclick", DisableTheButton(this.Page, BtnReject));
        BtnSearch.Attributes.Add("onclick", DisableTheButton(this.Page, BtnSearch));
        btnApproove.Attributes.Add("onclick", DisableTheButton(this.Page, btnApproove));
        BtnRejects.Attributes.Add("onclick", DisableTheButton(this.Page, BtnRejects));

        if (!Page.IsPostBack)
        {
            if (Session["AStatus"].ToString() == "OK")
            {
                if (Request.QueryString.HasKeys())
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]))
                    {
                        TxtMemID.Text = Request.QueryString["key"];
                        ChkMem.Checked = true;
                        BindData(" AND b.IDNo='" + Request.QueryString["key"] + "'");
                    }
                }
                else
                {
                    BindData();
                }
            }
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
    public void BindData(string condition = "")
    {
        if (!string.IsNullOrWhiteSpace(TxtMemID.Text))
        {
            condition += " AND MemberID = '" + TxtMemID.Text.Trim() + "'";
        }

        if (RbReqStatus.SelectedValue != "A")
        {
            condition += " And IsApprove = '" + RbReqStatus.SelectedValue + "'";
        }

        DivRemark.Visible = false;

        if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
        {
            if (CmbType.SelectedValue == "N")
            {
                condition += " And Cast(Convert(Varchar,RecTimeStamp,106) as DateTime) >= '" + txtStartDate.Text + "'";
            }
            else if (CmbType.SelectedValue == "A")
            {
                condition += " and Cast(Convert(Varchar,RecTimeStamp,106) as DateTime) >= '" + txtStartDate.Text + "'";
            }
            else
            {
                condition += " and Isapprove = '" + CmbType.SelectedValue + "' And Cast(Convert(Varchar,RecTimeStamp,106) as DateTime) >= '" + txtStartDate.Text + "'";
            }
        }

        if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
        {
            if (CmbType.SelectedValue == "Y")
            {
                condition += " And Cast(Convert(Varchar,RecTimeStamp,106) as DateTime) <= '" + txtEndDate.Text + "'";
            }
            else if (CmbType.SelectedValue == "A")
            {
                condition += " and Cast(Convert(Varchar,RecTimeStamp,106) as DateTime) <= '" + txtEndDate.Text + "'";
            }
            else
            {
                condition += " and Isapprove = '" + CmbType.SelectedValue + "' And Cast(Convert(Varchar,RecTimeStamp,106) as DateTime) <= '" + txtEndDate.Text + "'";
            }
        }

        string url = string.Empty;
        //string tempProtocol = HttpContext.Current.Request.Url.AbsoluteUri;
        //url = (tempProtocol.StartsWith("http://") ? "http://" : "https://") +
        //      HttpContext.Current.Request.Url.Host.ToUpper()
        //          .Replace("HTTP://", "")
        //          .Replace("HTTPS://", "")
        //          .Replace("WWW.", "")
        //          .Replace("BASICMLM.", "")
        //          .Replace("ADMIN.", "CPANEL.")
        //          .ToLower();

        string sql = "SELECT * FROM V#ProductRequest WHERE 1 = 1 " + condition + " ORDER BY RecTimeStamp DESC";

        DataTable dtData = new DataTable();
        dtData = objDAL.GetData(sql);
        GvData.DataSource = dtData;
        GvData.DataBind();

        Session["GProductData"] = dtData;

        if (dtData.Rows.Count > 0)
        {
            lblMsg.Visible = false;
            btnApproove.Visible = true;
            //BtnRejects.Visible = true;
            btnExport.Visible = true;
            lblReqs.Text = dtData.Rows.Count.ToString();
            //lblTotalAmont.Text = dtData.Compute("Sum(TotalAmount)", "").ToString();
            //LblTotalBV.Text = dtData.Compute("Sum(TotalBV)", "").ToString();
            LblEmailno.Text = dtData.Rows[0]["Email"].ToString();
        }
        else
        {
            lblMsg.Text = "No data to display.!";
            lblMsg.Visible = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            btnApproove.Visible = false;
            BtnRejects.Visible = false;
            btnExport.Visible = false;
            lblReqs.Text = "0";
            lblTotalAmont.Text = "0.00";
        }
    }


    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GProductData"];
            GvData.DataBind();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        string condition = "";
        BindData(condition);
    }

    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("ChkSelectAll"))
                .Attributes.Add("onclick", "javascript:SelectAll('" + ((CheckBox)e.Row.FindControl("ChkSelectAll")).ClientID + "')");
        }
    }

    protected void ChkMem_CheckedChanged(object sender, EventArgs e)
    {
        TxtMemID.Enabled = ChkMem.Checked;
    }

    protected void btnApproove_Click(object sender, EventArgs e)
    {
        DivRemark.Visible = true;
        btnApprove.Visible = true;
        BtnReject.Visible = false;
    }

    protected void BtnRejects_Click(object sender, EventArgs e)
    {
        DivRemark.Visible = true;
        btnApprove.Visible = false;
        BtnReject.Visible = true;
        //AprvAction("R", "R")
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string condition = "";
            DivRemark.Visible = false;
            DataGrid dg = new DataGrid();

            if (!string.IsNullOrWhiteSpace(TxtMemID.Text))
            {
                condition += " AND MemberID = '" + TxtMemID.Text.Trim() + "'";
            }

            if (RbReqStatus.SelectedValue != "A")
            {
                condition += " And IsApprove = '" + RbReqStatus.SelectedValue + "'";
            }

            DivRemark.Visible = false;

            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                if (CmbType.SelectedValue == "N")
                {
                    condition += " And Cast(Convert(Varchar, RecTimeStamp, 106) as DateTime) >= '" + txtStartDate.Text + "'";
                }
                else if (CmbType.SelectedValue == "A")
                {
                    condition += " and Cast(Convert(Varchar, RecTimeStamp, 106) as DateTime) >= '" + txtStartDate.Text + "'";
                }
                else
                {
                    condition += " and Isapprove = '" + CmbType.SelectedValue + "' And Cast(Convert(Varchar, RecTimeStamp, 106) as DateTime) >= '" + txtStartDate.Text + "'";
                }
            }

            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                if (CmbType.SelectedValue == "Y")
                {
                    condition += " And Cast(Convert(Varchar, RecTimeStamp, 106) as DateTime) <= '" + txtEndDate.Text + "'";
                }
                else if (CmbType.SelectedValue == "A")
                {
                    condition += " and Cast(Convert(Varchar, RecTimeStamp, 106) as DateTime) <= '" + txtEndDate.Text + "'";
                }
                else
                {
                    condition += " and Isapprove = '" + CmbType.SelectedValue + "' And Cast(Convert(Varchar, RecTimeStamp, 106) as DateTime) <= '" + txtEndDate.Text + "'";
                }
            }

            string url = string.Empty;
            //string tempProtocol = HttpContext.Current.Request.Url.AbsoluteUri;
            //url = (tempProtocol.StartsWith("http://") ? "http://" : "https://") +
            //      HttpContext.Current.Request.Url.Host.ToUpper()
            //          .Replace("HTTP://", "")
            //          .Replace("HTTPS://", "")
            //          .Replace("WWW.", "")
            //          .Replace("BASICMLM.", "")
            //          .Replace("ADMIN.", "CPANEL.")
            //          .ToLower();

            string sql = "SELECT * FROM V#ProductRequest WHERE 1 = 1 " + condition + " ORDER BY RecTimeStamp DESC";

            DataTable dtTemp = new DataTable();
            dtTemp = objDAL.GetData(sql);

            dg.DataSource = dtTemp;
            dg.DataBind();

            ExportToExcel("ApproveProduct.xls", dg);
        }
        catch (Exception ex)
        {
            // Handle exception (log it, show a message, etc.)
        }
    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw;

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";

        dg.EnableViewState = false;
        htw = new System.Web.UI.HtmlTextWriter(sw);
        dg.RenderControl(htw);

        Response.Write(sw.ToString());
        Response.End();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtARemark.Text))
        {
            LblARemark.Visible = true;
            LblARemark.Text = "Please Enter Remark";
            return;
        }
        else
        {
            LblARemark.Visible = false;
            AprvAction("Y", "A");
        }

    }
    private void AprvAction(string AprvType, string ApprvStatus)
    {
        string Query = "";
        //DAL objDAL = new DAL(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
        string PartyCode = "";
        string OrderType = "";
        string sql = "";
        CheckBox Chk;
        string sqlstr = "";
        Label lbl, lblReqno, lblIdno, LblAmount, lblPaymode, LblOrderNo, LblActype;
        int Cnt = 0;
        string Remark = "";
        int c = 0, sessid = 0;
        string voucherno = "";
        DataTable dt = new DataTable();
        DataTable Dt1 = new DataTable();

        foreach (GridViewRow Gvr in GvData.Rows)
        {
            Chk = (CheckBox)Gvr.FindControl("chkSelect");
            lbl = (Label)Gvr.FindControl("LblGrpID");
            lblReqno = (Label)Gvr.FindControl("LblReqNo");
            lblIdno = (Label)Gvr.FindControl("LblIdNo");
            LblAmount = (Label)Gvr.FindControl("lblAmount");
            //lblPaymode = (Label)Gvr.FindControl("LblPaymode");
            //LblActype = (Label)Gvr.FindControl("lblactype");
            LblOrderNo = (Label)Gvr.FindControl("LblOrderNo");

            if (Chk.Checked && Chk.Enabled)
            {
                if (AprvType == "Y")
                {
                    //Remark = $"Approve Payment Request On ReqNo: {lblReqno.Text} for Idno: {lblIdno.Text}";
                    string strSql_ = "Select * from TrnorderDetail Where orderno = '" + LblOrderNo.Text + "' ";
                    DataSet ds1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql_);

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            //Query += $"Insert Into TrnorderDetail(OrderNo, FormNo, ProductID, Qty, Rate, NetAmount, RecTimeStamp, DispDate, DispStatus, DispQty, " +
                            //         $"RemQty, DispAmt, MRP, DP, ProductName, ImgPath, RP, BV, FSEssId, Prodtype, PV) " +
                            //         $"Select '{LblOrderNo.Text}', '{lbl.Text}', prodid, '{ds1.Tables[0].Rows[i]["qty"]}', DP, " +
                            //         $"DP * {ds1.Tables[0].Rows[i]["qty"]}, getDate(), '', 'N', 0, {ds1.Tables[0].Rows[i]["qty"]}, " +
                            //         $"0, MRP, Dp, ProductName, '', 0, BV, (Select ISNULL(Max(FsessID), 1) FROM " +
                            //         $"{HttpContext.Current.Session["InvDatabase" + Session["CompID"]]}..M_FiscalMaster), 'P', PV " +
                            //         $"From {HttpContext.Current.Session["InvDatabase" + Session["CompID"]]}..M_ProductMaster where ActiveStatus='Y' " +
                            //         $"and OnWebsite='Y' and Prodid = '{ds1.Tables[0].Rows[i]["ProductID"]}';";
                            Query = "Update TrnorderDetail Set DispStatus = '" + AprvType + "', Dispdate=getdate() where Orderno = '" + ds1.Tables[0].Rows[i]["Orderno"] + "';";
                            Query += "Update Trnorder Set DispatchStatus = '" + AprvType + "', Remark = '" + TxtARemark.Text + "',DispatchDate=getdate() where Orderno = '" + ds1.Tables[0].Rows[i]["Orderno"] + "';";
                            c++;
                        }
                    }

                    //PartyCode = Session["WR"].ToString();
                    //OrderType = Session["ProductStatus"].ToString() == "Y" ? "O" : "T";

                    //Query += "Insert INTO TrnOrder(OrderNo, OrderDate, MemFirstName, MemLastName, Address1, Address2, CountryID, CountryName, StateCode, City, PinCode, " +
                    //         $"Mobl, EMail, FormNo, UserType, Passw, PayMode, ChDDNo, ChDate, ChAmt, BankName, BranchName, Remark, OrderAmt, OrderItem, OrderQty, ActiveStatus, " +
                    //         $"HostIp, RecTimeStamp, IsTransfer, DispatchDate, DispatchStatus, DispatchQty, RemainQty, DispatchAmount, Shipping, SessID, RewardPoint, " +
                    //         $"CourierName, DocketNo, OrderFor, IsConfirm, OrderType, Discount, OldShipping, ShippingStatus, IdNo, FSessId, BankAmt, OtherAmt, WalletAmt, " +
                    //         $"TravelPoint, KitName, ForVadicGurukul) " +
                    //         $"select '{LblOrderNo.Text}', Cast(Convert(varchar, GETDATE(), 106) as Datetime), MemFirstName, MemLastName, address1, Address2, " +
                    //         $"CountryID, CountryName, StateCode, City, CASE WHEN PinCode = '' THEN 0 ELSE Pincode END as Pincode, Mobl, EMail, '{lbl.Text}', '', " +
                    //         $"Passw, '', 0, '', 0, '', '', '{TxtARemark.Text}', '0', '0', '0', 'Y', 'H', Getdate(), 'Y', '', 'N', 0, '0', 0, 0, '{sessid}', 0, '', 0, 'WR', 'Y', " +
                    //         $"'{OrderType}', 0, '{lbl.Text}', 'Y', '{lblIdno.Text}', '1', '0', '0', '0', 0, '{KitName1}', 'N' from M_memberMaster where formno = '{lbl.Text}';";
                    ////Query += $"Update TrnorderDetail Set DispStatus = '"+ AprvType + "', Remark = '{TxtARemark.Text}',Dispdate=getdate() where OrderNo = '{LblOrderNo.Text}';";
                    ////Query += $"Update Trnorder Set DispStatus = '"+ AprvType + "', Remark = '{TxtARemark.Text}',Dispdate=getdate() where OrderNo = '{LblOrderNo.Text}';";

                    sqlstr = "Begin Try Begin Transaction " + Query + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                }
                else
                {
                    //Remark = $"Reject Payment Request On ReqNo: {lblReqno.Text} for Idno: {lblIdno.Text}";
                    Query += "Update TrnorderDetail Set DispStatus = '" + AprvType + "',Dispdate=getdate() where OrderNo = '" + LblOrderNo.Text + " ';";
                    Query += "Update Trnorder Set DispatchStatus = '" + AprvType + "', Remark = '{TxtARemark.Text}',DispatchDate=getdate() where OrderNo = '" + LblOrderNo.Text + "';";
                    sqlstr = "Begin Try Begin Transaction " + Query + " Commit Transaction End Try BEGIN CATCH ROLLBACK Transaction END CATCH";
                }

                Cnt++;
            }
            int a = 0;
            if (!string.IsNullOrEmpty(sqlstr))
            {
                a = objDAL.UpdateData(sqlstr);
            }

            string MsgTxt = "";
            if (AprvType == "Y")
            {
                MsgTxt = "Approved";
            }
            else
            {
                MsgTxt = "Rejected";
            }

            if (a > 0)
            {
                Session["remark"] = TxtARemark.Text;
                Session["GETDATE"] = DateTime.Now;
                lblMsg.Text = "" + Cnt + " Requests " + MsgTxt + " Successfully.";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                TxtARemark.Text = string.Empty;
                BindData();
            }
            else
            {
                lblMsg.Text = "Request Already " + MsgTxt + "";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
