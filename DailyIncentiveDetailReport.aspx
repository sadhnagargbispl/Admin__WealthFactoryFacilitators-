<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DailyIncentiveDetailReport.aspx.cs" Inherits="DailyIncentiveDetailReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function openPopup(element) {
            var url = element.href;
            hs.htmlExpand(element, {
                objectType: 'iframe',
                width: 620,
                height: 450,
                marginTop: 0
            });
            return false;
        }
    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">DailyIncentive Detail  Report</h3>
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
                                       <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control"  OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="True">
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
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GvData_PageIndexChanging">

                                                        <Columns>
                                                            <%--<asp:BoundField DataField="SNo" HeaderText="SNo." SortExpression="SNo" />--%>
                                                            <asp:TemplateField HeaderText="SNo.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payout Date" SortExpression="payoutdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="StartDate" runat="server" Text='<%# Eval("payoutdate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member ID" SortExpression="Member ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member Name" SortExpression="Member Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("mem_Name") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Daily Stacking Bonus" SortExpression="Daily Stacking Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewSelfIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("PairIncome") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Team Matching Bonus">
                                                                            <ItemTemplate>
                                                                                <%#Eval("PairIncome")%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Direct Referral Bonus" SortExpression="Direct Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewdirectIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Black" Text='<%# Eval("PairIncentive") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField HeaderText="Level Bonus" SortExpression="Level Bonus">
                                                                <ItemTemplate>
                                                                    <a href='<%# "ViewLevelIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text='<%# Eval("LevelIncome") %>'></asp:Label>
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Award/Reward" SortExpression="Rank Bonus">
                                                                <ItemTemplate>
                                                                   <%-- <a href='<%# "ViewRankIncome.aspx?formno=" + Eval("FormNo") + "&SessId=" + Eval("SessId") %>'
                                                                        onclick="return openPopup(this)">--%>
                                                                        <asp:Label ID="Label12" runat="server" ForeColor="Black" Text='<%# Eval("RewardInc") %>'></asp:Label>
                                                                   <%-- </a>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Royalty Bonus" SortExpression="Royalty Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("RoyaltyIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                          <%-- <asp:TemplateField HeaderText="Gross Income">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblnetIncome" runat="server"  Text='<%# Eval("NetIncome") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>--%>
                                                                        <asp:TemplateField HeaderText="Admin Charge">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbladmincharge" runat="server"  Text='<%# Eval("AdminCharge") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="TDS Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltdsamount" runat="server"  Text='<%# Eval("TDSAmount") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Net Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblchqamt" runat="server"  Text='<%# Eval("chqamt") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Token Value" SortExpression="Token Value">
                                                                <ItemTemplate>
                                                                    <%#Eval("TokenVal")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
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

