<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <meta name="viewport" />

    <link href="Login_Page/LoginCss.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">
        <div class="wrapper">
            <div id="formContent">
                <div class="fadeIn first">
                   <img src="images/logo.png" style="max-width:230px;" />
                </div>
                <div>
                    <asp:TextBox ID="TxtUID" runat="server" CssClass="fadeIn second" placeholder="User-ID"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ErrorMessage="Please Enter User-ID" ValidationGroup="LoginRR" ForeColor="Red" ControlToValidate="TxtUID"></asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="TxtPWD" runat="server" CssClass="fadeIn third" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="Please Enter Password" ValidationGroup="LoginRR" ForeColor="Red" ControlToValidate="TxtUID"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Button ID="BtnLogin" runat="server" Text="Log In" CssClass="fadeIn third" ValidationGroup="LoginRR" OnClick="BtnLogin_Click" />
                </div>
            </div>
        </div>
    </form>

</body>
</html>
