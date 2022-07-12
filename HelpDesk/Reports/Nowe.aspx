<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nowe.aspx.cs" Inherits="Helpdesk.Reports.WebForm1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> <title></title></head>
<body>
    <form id="form1" runat="server"> 
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" Width="100%" Height="1060px" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204"               InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226">
                <LocalReport ReportPath="Reports\rpt\Nowe.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource Name="DataSet1" DataSourceId="SqlDataSource1"></rsweb:ReportDataSource>
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:HelpdeskConnection %>' SelectCommand="SELECT Zgloszenie.IdZgloszenia AS Numerzgłoszenia, 
                Zgloszenie.Temat AS Tytułzgłoszenia, Kategoria.NazwaKategorii AS Kategoriazgłoszenia, Priorytet.NazwaPriorytetu, 
                Zgloszenie.DataDodania AS Datautworzeniazgłoszenia, 
                Zgloszenie.DataZakonczenia AS Datazakończeniazgłoszenia, Status.NazwaStatusu AS Status, 
                DATEDIFF(hh, Zgloszenie.DataDodania, Zgloszenie.DataZakonczenia) AS Czaswykonaniazgłoszeniawgodzinach, 
                DATEDIFF(mi, Zgloszenie.DataDodania, Zgloszenie.DataZakonczenia) AS Czaswykonaniazgłoszeniawminutach, Uzytkownik.Imie, Uzytkownik.Nazwisko
                FROM  Zgloszenie INNER JOIN
                         Status ON Zgloszenie.IdStatusu = Status.IdStatusu INNER JOIN
                         Kategoria ON Zgloszenie.IdKategorii = Kategoria.IdKategorii INNER JOIN
                         Priorytet ON Zgloszenie.IdPriorytet = Priorytet.IdPriorytetu RIGHT OUTER JOIN
                         Uzytkownik ON Zgloszenie.PracownikId = Uzytkownik.Id
                WHERE        (Zgloszenie.IdStatusu = 1)">

            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>