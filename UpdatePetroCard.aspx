<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdatePetroCard.aspx.cs" Inherits="UpdatePetroCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript" language="javascript">
     function isNumberKey(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;

         return true;
     }
</script>

   <%-- <script>
        document.addEventListener("DOMContentLoaded", function () {
            var txtCourierDate = document.getElementById('<%= txtcourierdate.ClientID %>');
        var today = new Date().toISOString().split('T')[0]; // Get today's date in YYYY-MM-DD format
        txtCourierDate.setAttribute("min", today);
    });
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Update Petro Card Status</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div align="center">
                                            <span id="lblt" class="text-danger"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            Id No :
                                            <asp:TextBox class="form-control" ID="txtidno" runat="server" ReadOnly="true" ></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Member name :
    <asp:TextBox class="form-control" ID="txtmembername" runat="server" ReadOnly="true" ></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Card SR. No :
                                            <asp:TextBox class="form-control" ID="txtcardno" runat="server" OnTextChanged="txtcardno_TextChanged" AutoPostBack="true" onkeypress="return isNumberKey(event);" ></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Docket No :
                                            <asp:TextBox class="form-control" ID="txtdocketno" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Courier Name :
                                            <asp:TextBox class="form-control" ID="txtcouriername" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Dispatch Date :
                                        <asp:TextBox class="form-control" ID="txtcourierdate" runat="server" type="date" onkeypress="return false;"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Remarks :
                                    <asp:TextBox class="form-control" ID="txtRemarks" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Status :
                                    <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="true" Text="Approve" Value ="Y">Dispatch</asp:ListItem>
                                        <asp:ListItem Text="Pending" Value ="P">Pending</asp:ListItem>
                                    </asp:RadioButtonList>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtKitId" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" OnClick="BtnSave_Click" />
                                        </div>
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

