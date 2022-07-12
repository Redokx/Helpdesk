using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class Grafika
    {
        public WynikOperacjiGraficznych PrzeslijPlik(HttpPostedFileBase obrazek)
        {
            WynikOperacjiGraficznych wynik = new WynikOperacjiGraficznych
            {
                Sukces = true,
                Blad = null
            };
            string rozszerzenie = Path.GetExtension(obrazek.FileName);
            if (!SprawdzTypPliku(rozszerzenie))
            {
                wynik.Sukces = false;
                wynik.Blad = "Niepoprawny typ pliku";
                return wynik;
            }
            string nowaNazwa = Guid.NewGuid().ToString() + rozszerzenie;
            var sciezkaObrazka = Path.Combine(HttpContext.Current.Request.MapPath("~/Grafika/"), nowaNazwa);
            var sciezkaMiniatury = Path.Combine(HttpContext.Current.Request.MapPath("~/Grafika/Mini"), nowaNazwa);
            // przesyłamy oryginalny obrazek na serwer i tworzymy obiekt typu Image, który go przechowuje
            obrazek.SaveAs(sciezkaObrazka);
            Image obrazekOryginalny = Image.FromFile(sciezkaObrazka);
            try
            {
                Image obrazekZmieniony = Skalowanie(obrazekOryginalny, 100);
                obrazekZmieniony.Save(sciezkaMiniatury);
                obrazekZmieniony.Dispose();
                obrazekZmieniony = Skalowanie(obrazekOryginalny, 600);
                obrazekOryginalny.Dispose();
                obrazekZmieniony.Save(sciezkaObrazka);
                obrazekZmieniony.Dispose();
                wynik.NazwaPliku = nowaNazwa;
            }
            catch
            {
                wynik.Sukces = false;
                wynik.Blad = "Błąd w trakcie przesyłania pliku";
            }
            return wynik;
        }
        // Metoda sprawdzająca typ pliku
        private bool SprawdzTypPliku(string rozszerzenie)
        {
            rozszerzenie = rozszerzenie.ToLower();
            switch (rozszerzenie)
            {
                case ".jpg": return true;
                case ".png": return true;
                case ".gif": return true;
                case ".jpeg": return true;
                default: return false;
            }
        }
        // Metoda Skalowania
        private Image Skalowanie(Image obrazek, int wielkosc)
        {
            int oryginalnaSzerokosc = obrazek.Width;
            int oryginalnaWysokosc = obrazek.Height;
            float szerokosc = oryginalnaSzerokosc;
            float wysokosc = oryginalnaWysokosc;
            bool zmiana = false;
            if (oryginalnaSzerokosc > oryginalnaWysokosc && oryginalnaSzerokosc > wielkosc)
            {
                szerokosc = wielkosc;
                wysokosc = wielkosc * oryginalnaWysokosc / oryginalnaSzerokosc;
                zmiana = true;
            }
            if (oryginalnaWysokosc > oryginalnaSzerokosc && oryginalnaWysokosc > wielkosc)
            {
                wysokosc = wielkosc;
                szerokosc = wielkosc * oryginalnaSzerokosc / oryginalnaWysokosc;
                zmiana = true;
            }
            if (zmiana)
            {
                Bitmap bmpObrazek = new Bitmap(System.Convert.ToInt32(szerokosc),
               System.Convert.ToInt32(wysokosc));
                bmpObrazek.SetResolution(obrazek.HorizontalResolution,
               obrazek.VerticalResolution);
                Graphics graphics = Graphics.FromImage(bmpObrazek);
                graphics.InterpolationMode =
               System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(obrazek, 0, 0, szerokosc, wysokosc);
                graphics.Dispose();
                return bmpObrazek;
            }
            return obrazek;
        }
    }
}