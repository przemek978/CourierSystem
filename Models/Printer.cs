using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;


namespace CourierSystem.Models
{
    public static class Printer
    {

        public static string GenerujListPrzewozowy(List<Shipment> ships, User user, bool IsCourier = false)
        {
            Document document = new Document();
            string name = @"Lista Przesyłek " + DateTime.Today.ToString("dd-MM-yyyy");
            string FileName = @"C:\Users\przem\Desktop\" + name + ".pdf";
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(FileName, FileMode.Create));
                document.SetPageSize(PageSize.A4);
                document.Open();

                //Czcionka z polskimi znakami
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1257, BaseFont.EMBEDDED);
                BaseFont baseFontBold =
                    BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.EMBEDDED);

                // Dodaj nagłówek dokumentu
                Font headerFont = new Font(baseFontBold, 48, Font.BOLD);
                Paragraph header = new Paragraph(name, headerFont);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                // Dodaj odstęp między nagłówkiem a danymi firmy
                Paragraph spacing = new Paragraph();
                spacing.SpacingAfter = 20f; // Ustaw dowolną wartość, aby zwiększyć odstęp
                document.Add(spacing);

                // Dodaj informacje o firmie
                Font companyFont = new Font(baseFont, 12);
                Paragraph companyInfo = new Paragraph();
                companyInfo.Alignment = Element.ALIGN_CENTER;
                companyInfo.Add(new Chunk("Szybkie shipy", new Font(baseFont, 16, Font.BOLD)));
                companyInfo.Add(Chunk.NEWLINE);
                companyInfo.Add(new Chunk("Adres: ul. Przykładowa 166, 17-200 Hajnówka", companyFont));
                companyInfo.Add(Chunk.NEWLINE);
                companyInfo.Add(new Chunk("Telefon: 123-456-789", companyFont));
                companyInfo.Add(Chunk.NEWLINE);
                document.Add(companyInfo);

                // Dodaj odstęp pomiędzy nagłówkiem a tabelą
                document.Add(new Paragraph(" "));

                // Dodaj tabelę z danymi przesyłek
                PdfPTable dataTable = new PdfPTable(IsCourier == true ? 4 : 5);

                // Utwórz nagłówki tabeli
                Font headtableFont = new Font(baseFont, 14, Font.BOLD);
                PdfPCell headerCell1 = new PdfPCell(new Phrase("Numer przesyłki", headtableFont));
                PdfPCell headerCell2 = new PdfPCell(new Phrase("Adresat", headtableFont));
                PdfPCell headerCell3 = new PdfPCell(new Phrase("Nr Telefonu", headtableFont));
                if (!IsCourier)
                {
                    PdfPCell headerCell4 = new PdfPCell(new Phrase("Kurier", headtableFont));
                    headerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                    headerCell4.BackgroundColor = BaseColor.ORANGE;
                    dataTable.AddCell(headerCell4);
                }

                PdfPCell headerCellD = new PdfPCell(new Phrase("Dostarczono", headtableFont));

                // Ustaw styl czcionki dla nagłówków
                headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerCell1.BackgroundColor = BaseColor.ORANGE;

                headerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerCell2.BackgroundColor = BaseColor.ORANGE;

                headerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerCell3.BackgroundColor = BaseColor.ORANGE;

                headerCellD.BackgroundColor = BaseColor.ORANGE;
                headerCellD.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCellD.VerticalAlignment = Element.ALIGN_MIDDLE;

                // Dodaj nagłówki do tabeli
                dataTable.AddCell(headerCell1);
                dataTable.AddCell(headerCell2);
                dataTable.AddCell(headerCell3);
                dataTable.AddCell(headerCellD);

                // Dodaj dane do tabeli
                for (int i = 0; i < ships.Count; i++)
                {
                    PdfPCell shipmentNumberCell =
                        new PdfPCell(new Phrase(ships[i].ShipmentNumber.ToString(), companyFont));
                    shipmentNumberCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    shipmentNumberCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PdfPCell recipientCell = new PdfPCell(new Phrase(
                        ships[i].Recipient.FirstName + " " + ships[i].Recipient.LastName + "\n" +
                        ships[i].Recipient.Address + "\n", companyFont));
                    recipientCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PdfPCell phonenumberCell =
                        new PdfPCell(new Phrase(ships[i].Recipient.PhoneNumber.ToString(), companyFont));
                    phonenumberCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    phonenumberCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    dataTable.AddCell(shipmentNumberCell);
                    dataTable.AddCell(recipientCell);
                    dataTable.AddCell(phonenumberCell);

                    if (!IsCourier)
                    {
                        PdfPCell courierCell = new PdfPCell(new Phrase(ships[i].Courier.Name.ToString(), companyFont));
                        courierCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        courierCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        dataTable.AddCell(courierCell);
                    }

                    PdfPCell shipCell = new PdfPCell(new Phrase("", companyFont));
                    shipCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    shipCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    dataTable.AddCell(shipCell);
                }

                document.Add(dataTable);

                Font footFont = new Font(baseFont, 14, Font.BOLD);
                PdfContentByte contentByte = writer.DirectContent;
                float X = document.PageSize.Width - document.RightMargin;
                float Y = document.BottomMargin + footFont.GetCalculatedLeading(1.5f);
                if (IsCourier)
                {
                    ColumnText.ShowTextAligned(contentByte, Element.ALIGN_RIGHT,
                        new Phrase("Kurier: " + ((Courier)user).Name, footFont), X, Y, 0);
                }
                else
                {
                    ColumnText.ShowTextAligned(contentByte, Element.ALIGN_RIGHT,
                        new Phrase("Wystawił: " + user.Username, footFont), X, Y, 0);
                }

            }
            catch (IOException)
            {
                MessageBox.Show("Dokument jest otwarty");
                throw;
            }
            catch
            {
                MessageBox.Show("Coś poszlo nie tak");
                throw;
            }
            finally
            {
                document.Close();
            }

            return FileName;
        }

        public static string GenerujEtykiete(string ShipNumber)
        {
            Document document = new Document();
            string name = @"Etykieta "+ ShipNumber;
            string FileName = @"C:\Users\przem\Desktop\" + name + ".pdf";
            try
            {
                // Utwórz obiekt writer do zapisu dokumentu do pliku
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(FileName, FileMode.Create));
                document.SetPageSize(PageSize.A4.Rotate());
                document.Open();

                // Utwórz obiekt PdfContentByte
                PdfContentByte contentByte = writer.DirectContent;

                //Czcionka z polskimi znakami
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1257, BaseFont.EMBEDDED);
                BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.EMBEDDED);

                // Dodaj nagłówek dokumentu
                Font headerFont = new Font(baseFontBold, 48, Font.BOLD);
                Paragraph header = new Paragraph("Paczka", headerFont);
                header.Alignment = Element.ALIGN_CENTER;
                document.Add(header);

                // Dodaj odstęp między nagłówkiem a danymi firmy
                Paragraph spacing = new Paragraph();
                spacing.SpacingAfter = 20f; // Ustaw dowolną wartość, aby zwiększyć odstęp
                document.Add(spacing);

                // Dodaj informacje o firmie
                Font companyFont = new Font(baseFont, 12);
                Paragraph companyInfo = new Paragraph();
                companyInfo.Alignment = Element.ALIGN_CENTER;
                companyInfo.Add(new Chunk("Szybkie shipy", new Font(baseFont, 16, Font.BOLD)));
                companyInfo.Add(Chunk.NEWLINE);
                companyInfo.Add(new Chunk("Adres: ul. Przykładowa 166, 17-200 Hajnówka", companyFont));
                companyInfo.Add(Chunk.NEWLINE);
                companyInfo.Add(new Chunk("Telefon: 123-456-789", companyFont));
                companyInfo.Add(Chunk.NEWLINE);
                document.Add(companyInfo);

                // Dodaj odstęp pomiędzy nagłówkiem a tabelą
                document.Add(new Paragraph(" "));

                // Ustaw pozycję etykiety
                float pageWidth = document.PageSize.Width;
                float pageHeight = document.PageSize.Height;
                float labelWidth = 1000; // Szerokość etykiety
                float labelHeight = 200; // Wysokość etykiety
                float posX = (pageWidth - labelWidth) / 2; // Oblicz pozycję X dla wyśrodkowania etykiety
                float posY = (pageHeight - labelHeight) / 2; // Oblicz pozycję Y dla wyśrodkowania etykiety


                // Wygeneruj kod kreskowy z numerem przesyłki
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_39,
                    Options = new EncodingOptions
                    {
                        Height = 200,
                        Width = 1000
                    }
                };
                Bitmap barcodeBitmap = barcodeWriter.Write(ShipNumber);

                // Konwertuj obrazek kodu kreskowego na Image
                Image barcodeImage = Image.GetInstance(barcodeBitmap, BaseColor.BLACK);
                barcodeImage.ScalePercent(80);

                // Wyśrodkuj kod kreskowy na etykiecie
                float barcodeX = posX + (labelWidth - barcodeImage.ScaledWidth) / 2;
                float barcodeY = posY + (labelHeight - barcodeImage.ScaledHeight) / 2;
                barcodeImage.SetAbsolutePosition(barcodeX, barcodeY);
                document.Add(barcodeImage);

                // Wyświetl numer przesyłki na etykiecie
                //posY -= 50;
                //contentByte.BeginText();
                //contentByte.SetTextMatrix(posX, posY);
                //contentByte.SetFontAndSize(baseFont, 12);
                //contentByte.ShowText("Numer przesyłki:");
                //contentByte.EndText();

                //posY -= 20;

                //contentByte.BeginText();
                //contentByte.SetTextMatrix(posX, posY);
                //contentByte.SetFontAndSize(baseFont, 12);
                //contentByte.ShowText(ShipNumber);
                //contentByte.EndText();
            }
            catch (IOException)
            {
                MessageBox.Show("Dokument jest otwarty");
                throw;
            }
            catch
            {
                MessageBox.Show("Coś poszlo nie tak");
                throw;
            }
            finally
            {
                document.Close();
            }
            return FileName;
        }
    }
}
