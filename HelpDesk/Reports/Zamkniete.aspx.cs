using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Helpdesk.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load5(object sender, EventArgs e)
        {

        }


        //test raportowania
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if(!IsPostBack)
        //    {
        //        LoadReport();
        //    }
        //}
        //private void LoadReport()
        //{
        //    var parametryRaportu = (dynamic)HttpContext.Current.Session["ParametryReportu"];
        //    if(parametryRaportu !=null && !string.IsNullOrEmpty(parametryRaportu.RaportNazwa))
        //    {
        //        Page.Title = "Raport|" + parametryRaportu.RaportTemat;
        //        var dt = new DataTable();
        //        dt = parametryRaportu.DataSource;
        //        if (dt.Rows.Count >0)
        //        {
        //            GenerujDokumentRaportu(parametryRaportu,parametryRaportu.RaportTyp,dt);
        //        }
        //        else
        //        {
        //            ShowErrorMesseage();
        //        }

        //    }
        //}
        //public void GenerujDokumentRaportu(dynamic parametryRaportu,string raportTyp, DataTable data)
        //{
        //    string dsName = parametryRaportu.DataSetNazwa;
        //    ReportViewer1.LocalReport.DataSources.Clear();
        //    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(dsName, data));
        //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/" + "Reports//"+ parametryRaportu.RaportNazwa);
        //    ReportViewer1.DataBind();
        //    ReportViewer1.LocalReport.Refresh();
        //}

        //public void ShowErrorMesseage()
        //{
        //    ReportViewer1.LocalReport.DataSources.Clear();
        //    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("", new DataTable()));
        //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~" + "Reports//blank.rdlc");
        //    ReportViewer1.DataBind();
        //    ReportViewer1.LocalReport.Refresh();
        //}
    }
}