<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DailyBonusReport.aspx.cs" Inherits="DailyBonusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Daily CashBack Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        Member ID :
                                  <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        FromDate :
                                  <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="display: inline">
                                  </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Todate :
                                  <asp:DropDownList ID="DDltodate" runat="server" class="form-control">
                                  </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        PageSize:
                                <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <asp:ListItem Text="200" Value="200" />
                                    <asp:ListItem Text="300" Value="300" />
                                    <asp:ListItem Text="400" Value="400" />
                                    <asp:ListItem Text="500" Value="500" />
                                    <asp:ListItem Text="1000" Value="1000" />
                                    <asp:ListItem Text="2000" Value="2000" />
                                </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <br />

                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " />
                                    </div>
                                </div>

                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">

                                                        <Columns>
                                                            <asp:BoundField DataField="SNo" HeaderText="SNo." SortExpression="SNo" />
                                                            <asp:TemplateField HeaderText="PayoutDate" SortExpression="PayoutDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="StartDate" runat="server" Text='<%# Eval("payoutDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="IdNo" SortExpression="IdNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member Name" SortExpression="Member Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("memfirstName") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Referral ID" SortExpression="Referral_ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblrefMemberId" runat="server" Text='<%# Eval("Referral ID") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Referral Name" SortExpression="Referral_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblrefMemberName" runat="server" Text='<%# Eval("Referral Name") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                            <%-- <asp:TemplateField HeaderText="Investment" SortExpression="BV">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBV" runat="server" Text='<%# Eval("BV") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                            <%-- <asp:TemplateField HeaderText="Slab" SortExpression="Slab">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBV" runat="server" Text='<%# Eval("Slab") %>'></asp:Label><br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Daily CashBack" SortExpression="Comm">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIncome" runat="server" Text='<%# Eval("Comm") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

