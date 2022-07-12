using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class Plik
    {
        public WynikiOperacjiPrzesylu PrzeslijPlik(HttpPostedFileBase plik)
        {
            WynikiOperacjiPrzesylu wynikPlik = new WynikiOperacjiPrzesylu
            {
                Sukces = true,
                Blad = null
            };
            string rozszerzenie = Path.GetFileName(plik.FileName);
            string nowaNazwa = Guid.NewGuid().ToString() +"_"+ rozszerzenie;
            var sciezkaPliku = Path.Combine(HttpContext.Current.Request.MapPath("~/Zalaczniki/"), nowaNazwa);
            // przesyłamy oryginalny obrazek na serwer i tworzymy obiekt typu Image, który go przechowuje
            try
            {
                wynikPlik.NazwaPliku = nowaNazwa;
                plik.SaveAs(sciezkaPliku);
            }
            catch
            {
                wynikPlik.Sukces = false;
                wynikPlik.Blad = "Błąd w trakcie przesyłania pliku";
            }
           
            return wynikPlik;
        }
        
    }
}