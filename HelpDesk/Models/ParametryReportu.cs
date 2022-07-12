using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class ParametryReportu
    {
        public string RaportNazwa {get;set;}
        public string RaportTemat { get; set; }
        public string RaportTyp { get; set; }
        public DataTable DataSource { get; set; }
        public bool CzyMaParametr { get; set; }
        public string DataSetNazwa { get; set; }
    }
}