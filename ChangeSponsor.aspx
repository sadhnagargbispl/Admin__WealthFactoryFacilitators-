<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangeSponsor.aspx.cs" Inherits="ChangeSponsor" %>

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
                                <h3 class="card-title">Change Sponsor</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"
                                            Visible="false"></asp:Label>
                                        <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;"></span>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                            Enter Member ID :
                                        </div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="TxtIDNo_TextChanged">
                                            </asp:TextBox>
                                            <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                                ControlToValidate="TxtIDNo" ValidationGroup="Save" ForeColor="red"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                            <strong>Current Sponsor:</strong>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="LblOldSponsor" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                            <strong>New Sponsor ID :</strong>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="TxtSpIDNo" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="TxtSpIDNo_TextChanged"></asp:TextBox>
                                                    <asp:Label ID="LblSponserName" runat="server" CssClass="label-text"></asp:Label>
                                                    <asp:TextBox ID="TxtFormNos" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="TxtSpIDNo" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Sponser ID."
                                                ControlToValidate="TxtSpIDNo" ValidationGroup="Save" ForeColor="red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                    </div>
                                    <div class="col-md-12" style="margin-bottom: 1%">
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Button ID="BtnLegShift" runat="server" Text="Change Sponsor"
                                                class="btn btn-primary" ValidationGroup="Save" OnClick="BtnLegShift_Click" />
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </section>
    </div>

</asp:Content>

