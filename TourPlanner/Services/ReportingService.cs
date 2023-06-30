using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Jsoup.Nodes;
using TourPlanner.Models;
using Document = iText.Layout.Document;

namespace TourPlanner.Services {
    public class ReportingService {

        private static readonly PdfFont Font = PdfFontFactory.CreateFont(StandardFonts.COURIER);
        private static readonly PdfFont Bold = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);

        public static void GenerateReport(string fp, Tour tour) {
            var writer = new PdfWriter(fp);

            var pdf = new PdfDocument(writer);

            var document = new Document(pdf);

            document.Add(new Paragraph(tour.Name)
                .SetFont(Bold)
                .SetFontSize(24)
                .SetTextAlignment(TextAlignment.CENTER)
            );

            document.Add(new Paragraph(tour.Description)
                .SetFont(Font)
                .SetFontSize(16)
                .SetTextAlignment(TextAlignment.JUSTIFIED)
            );

            document.Add(new Paragraph($"From: {tour.From}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"To: {tour.To}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"Mode of Transportation: {Enum.GetName(tour.TransportType)}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"Distance: {tour.Distance}km")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"Time/Duration: {tour.FormatedTime}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"Average Time/Duration: {tour.FormatedAverageTime}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"Popularity: {Enum.GetName(tour.Popularity)}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            document.Add(new Paragraph($"Difficulty: {Enum.GetName(tour.AverageLogDifficulty())}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            var cf = tour.ChildFriendly ? "Yes" : "no";
            document.Add(new Paragraph($"Child Friendly: {cf}")
                .SetFont(Font)
                .SetFontSize(16)
            );

            if (tour.Logs.Count > 0) {
                document.Add(new Paragraph("LOGS")
                    .SetFont(Bold)
                    .SetFontSize(20)
                );


                float[] cellWidths = { 100f, 150f, 100f, 100f, 80f };
                var table = new Table(cellWidths);

                string[] columns = { "Date", "Comment", "Difficulty", "Rating", "Time" };
                foreach (var s in columns) {
                    table.AddCell(new Cell().Add(new Paragraph(s).SetFont(Bold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER)) );
                }


                foreach (var log in tour.Logs) {
                    table.AddCell( new Cell().Add( new Paragraph(log.Date.ToShortDateString()).SetFont(Font).SetFontSize(12) ) );
                    table.AddCell( new Cell().Add( new Paragraph(log.Comment).SetFont(Font).SetFontSize(12) ) );
                    table.AddCell( new Cell().Add( new Paragraph(Enum.GetName(log.Difficulty)).SetFont(Font).SetFontSize(12) ) );
                    table.AddCell( new Cell().Add( new Paragraph(Enum.GetName(log.Rating)).SetFont(Font).SetFontSize(12) ) );
                    table.AddCell( new Cell().Add( new Paragraph(log.FormatedTime).SetFont(Font).SetFontSize(12) ) );
                }

                document.Add(table);
            }

            document.Add(new Paragraph("Tour Image:")
                .SetFont(Bold)
                .SetFontSize(20)
            );

            document.Add(new Image(ImageDataFactory.Create(tour.PicturePath)).ScaleToFit(500, 280));

            document.Close();

            pdf.Close();

            writer.Close();
        }

        public static void GenerateSummarizeReport(string fp, IEnumerable<Tour> tours) {
            var writer = new PdfWriter(fp);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            foreach (var tour in tours) {
                document.Add(new Paragraph(tour.Name)
                    .SetFont(Bold)
                    .SetFontSize(24)
                    .SetTextAlignment(TextAlignment.CENTER)
                );

                document.Add(new Paragraph($"Average Time/Duration: {tour.FormatedAverageTime}")
                .SetFont(Font)
                .SetFontSize(16)
            );

                document.Add(new Paragraph($"Popularity: {Enum.GetName(tour.Popularity)}")
                    .SetFont(Font)
                    .SetFontSize(16)
                );

                document.Add(new Paragraph($"Difficulty: {Enum.GetName(tour.AverageLogDifficulty())}")
                    .SetFont(Font)
                    .SetFontSize(16)
                );

                var cf = tour.ChildFriendly ? "Yes" : "no";
                document.Add(new Paragraph($"Child Friendly: {cf}")
                    .SetFont(Font)
                    .SetFontSize(16)
                );

                if (tour.Logs.Count > 0) {
                    document.Add(new Paragraph("LOGS")
                        .SetFont(Bold)
                        .SetFontSize(20)
                    );


                    float[] cellWidths = { 100f, 150f, 100f, 100f, 80f };
                    var table = new Table(cellWidths);

                    string[] columns = { "Date", "Comment", "Difficulty", "Rating", "Time" };
                    foreach (var s in columns) {
                        table.AddCell(new Cell().Add(new Paragraph(s).SetFont(Bold).SetFontSize(16).SetTextAlignment(TextAlignment.CENTER)));
                    }


                    foreach (var log in tour.Logs) {
                        table.AddCell(new Cell().Add(new Paragraph(log.Date.ToShortDateString()).SetFont(Font).SetFontSize(12)));
                        table.AddCell(new Cell().Add(new Paragraph(log.Comment).SetFont(Font).SetFontSize(12)));
                        table.AddCell(new Cell().Add(new Paragraph(Enum.GetName(log.Difficulty)).SetFont(Font).SetFontSize(12)));
                        table.AddCell(new Cell().Add(new Paragraph(Enum.GetName(log.Rating)).SetFont(Font).SetFontSize(12)));
                        table.AddCell(new Cell().Add(new Paragraph(log.FormatedTime).SetFont(Font).SetFontSize(12)));
                    }

                    document.Add(table);
                }



            }


            document.Close();
            pdf.Close();
            writer.Close();
        }
    }
}
