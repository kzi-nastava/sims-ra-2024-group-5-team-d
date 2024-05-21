using BookingApp.Appl.UseCases;
using BookingApp.Domain.Models;
using BookingApp.WPF.ViewModels;
using BookingApp.WPF.ViewModels.OwnerViewModels;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BookingApp.WPF.Views.Utils
{
    public class PDFGenerator
    {

        public string CreateOwnerPdf(ObservableCollection<OwnerRatingViewModel> ratings)
        {
            AccommodationRatingService accommodationRatingService = new AccommodationRatingService();
            Document document = new Document();

            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);

            string labelText = "RATING REPORT\n";
            Label label = new Label(labelText, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(label);
            string date = DateTime.Now.ToString("dd.M.yyyy HH:mm");
            Label label2 = new Label(date, 0, 50, 504, 100, Font.Helvetica, 12, TextAlign.Right);
            page.Elements.Add(label2);
            List<string> accommodatioNames = new List<string>();
            accommodatioNames = ratings.Select(rating => rating.AccommodationName)
                                               .Distinct()
                                               .ToList();
            accommodatioNames.ForEach(a => Debug.WriteLine(a));
            for (int i = 0; i < accommodatioNames.Count; i++)
            {
                double correctnes = (double)ratings.Where(rating => rating.AccommodationName == accommodatioNames[i]).Sum(r => r.CorrectnessRating) / ratings.Where(rating => rating.AccommodationName == accommodatioNames[i]).Count();
                double cleanliness = (double)ratings.Where(rating => rating.AccommodationName == accommodatioNames[i]).Sum(r => r.CleanlinessRating) / ratings.Where(rating => rating.AccommodationName == accommodatioNames[i]).Count();
                string text = i + 1 + ".   " + "Accommodation: " + accommodatioNames[i] + "\n" + "\n" +
                    "   Category 1: " + "Cleanliness" + "\n" +
                    "       Average rating: " + cleanliness + "\n" +
                    "   Category 2: " + "Correctness" + "\n" +
                    "       Average rating: " + correctnes + "\n" + "\n" + "\n";
                Label label1 = new Label(text, 0, 100 + i * 100, 504, 100, Font.Helvetica, 12, TextAlign.Left);
                page.Elements.Add(label1);
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = System.IO.Path.Combine(desktopPath, "ReviewReport.pdf");
            document.Draw(filename);

            return filename;

        }

        public string CreateGuestActiveReservationsPdf(ObservableCollection<UserReservationsViewModel> activeReservations)
        {
            Document document = new Document();

            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);

            string labelText = "ACTIVE RESERVATIONS\n";
            Label label = new Label(labelText, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(label);
            string date = DateTime.Now.ToString("dd.M.yyyy HH:mm");
            Label label2 = new Label(date, 0, 50, 504, 100, Font.Helvetica, 12, TextAlign.Right);
            page.Elements.Add(label2);

            List<string> accommodatioNames = new List<string>();
            accommodatioNames = activeReservations.Select(reservation => reservation.Name).Distinct().ToList();
            for (int i = 0; i < accommodatioNames.Count; i++)
            {
                string reservedFrom = activeReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).ReservedFrom.ToString("dd/MM/yyyy");
                string reservedTo = activeReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).ReservedTo.ToString("dd/MM/yyyy");
                Location location = activeReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).Location;
                int capacity = activeReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).Capacity;
                string text = i + 1 + ".   " + "Accommodation: " + accommodatioNames[i] + "\n" + "\n" +
                    "   Reserved from: " + reservedFrom +"\n"+
                    "   Reserved to: "+ reservedTo +"\n" +
                    "   Location : "+ location + "\n" +
                    "   Number of people: "+ capacity + "\n" + "\n" + "\n";
                Label label1 = new Label(text, 0, 100 + i * 100, 504, 100, Font.Helvetica, 12, TextAlign.Left);
                page.Elements.Add(label1);
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = System.IO.Path.Combine(desktopPath, "ActiveReservations.pdf");
            document.Draw(filename);

            return filename;
        }

        public string CreateGuestCancelledReservationsPdf(ObservableCollection<UserReservationsViewModel> cancelledReservations)
        {
            Document document = new Document();

            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 54.0f);
            document.Pages.Add(page);

            string labelText = "CANCELLED RESERVATIONS\n";
            Label label = new Label(labelText, 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(label);
            string date = DateTime.Now.ToString("dd.M.yyyy HH:mm");
            Label label2 = new Label(date, 0, 50, 504, 100, Font.Helvetica, 12, TextAlign.Right);
            page.Elements.Add(label2);

            List<string> accommodatioNames = new List<string>();
            accommodatioNames = cancelledReservations.Select(reservation => reservation.Name).Distinct().ToList();
            for (int i = 0; i < accommodatioNames.Count; i++)
            {
                string reservedFrom = cancelledReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).ReservedFrom.ToString("dd/MM/yyyy");
                string reservedTo = cancelledReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).ReservedTo.ToString("dd/MM/yyyy");
                Location location = cancelledReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).Location;
                int capacity = cancelledReservations.FirstOrDefault(reservation => reservation.Name == accommodatioNames[i]).Capacity;
                string text = i + 1 + ".   " + "Accommodation: " + accommodatioNames[i] + "\n" + "\n" +
                    "   Reserved from: " + reservedFrom + "\n" +
                    "   Reserved to: " + reservedTo + "\n" +
                    "   Location : " + location + "\n" +
                    "   Number of people: " + capacity + "\n" + "\n" + "\n";
                Label label1 = new Label(text, 0, 100 + i * 100, 504, 100, Font.Helvetica, 12, TextAlign.Left);
                page.Elements.Add(label1);
            }



            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = System.IO.Path.Combine(desktopPath, "CancelledReservations.pdf");
            document.Draw(filename);

            return filename;
        }
    }
}
