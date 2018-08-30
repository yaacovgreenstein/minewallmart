<%@ Page Title="Home Page" EnableEventValidation="true" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Wallmart Product Mining</h1>
        <p>
            <asp:Button ID="ButtonDownloadPage" runat="server" OnClick="ButtonDownloadPage_Click" Text="Mine Product Information" />
            <asp:TextBox ID="TextBoxProductUrl" runat="server"  ToolTip="Enter Url Here" Columns="100" TextMode="MultiLine"></asp:TextBox>
        </p>
        <p>
            <asp:Label ID="LabelProductInfo" runat="server" ></asp:Label>
        </p>
        <p>
            Seek in Reviews
            <asp:TextBox ID="TextBoxSeek" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonSeek" runat="server" OnClick="Button1_Click" Text="Seek" />
            <asp:Button ID="ButtonClear" runat="server" OnClick="ButtonClear_Click" Text="Clear" />
        </p>
        <p>
            <asp:GridView ID="GridView1" AllowPaging="true" PageSize="200" OnPageIndexChanging="GridView1_PageIndexChanging"  
            runat="server"></asp:GridView> 
        </p>
        <p>
            <asp:Literal ID="TextBoxPageContent" runat="server"   ></asp:Literal>
        </p>
    </div>

    
</asp:Content>
