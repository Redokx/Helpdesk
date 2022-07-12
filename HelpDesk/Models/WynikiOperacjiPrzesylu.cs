using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class WynikiOperacjiPrzesylu
    {
        public bool Sukces { get; set; }
        public string NazwaPliku { get; set; }
        public string Blad { get; set; }
    }
}