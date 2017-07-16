using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HeriIOApp.ModelData;

namespace HeriIOApp
{
    public class Helpers
    {
        public static Random random = new Random();
        public static string KodePemesanan()
        {
            int panjang = 5;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, panjang)
                .Select(s =>s[ random.Next(s.Length)]).ToArray());
        }

        public static int KodeValidasi()
        {
            int panjang = 3;
            const string chars = "123456789";
            var value= new string(Enumerable.Repeat(chars, panjang)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return Int32.Parse(value);
        }

        public static void SendEmail(string address, string subject, string message)
        {
            try
            {
                string email = "ocph23.test@gmail.com";
                string password = "Sony@7777";

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                msg.From = new MailAddress(email);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = true;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {

                throw new SystemException(ex.Message);
            }
         
        }

        public static void CheckWaktubooking()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Pemesanan.Where(O => O.StatusPesanan == StatusPesanan.Baru);
                foreach (var item in result)
                {
                    var sekarang = DateTime.Now;
                    if (sekarang.Subtract(item.Tanggal).TotalMinutes > 30 && !item.IsEvent)
                    {
                        db.Pemesanan.Update(O => new { O.StatusPesanan, O.VerifikasiPembayaran }, new ModelData.pemesanan { StatusPesanan = StatusPesanan.Batal, VerifikasiPembayaran = VerifikasiPembayaran.Batal }, O => O.Id == item.Id);
                    }
                    else if (sekarang.Subtract(item.Tanggal).TotalHours > 24 && item.IsEvent)
                    {
                        db.Pemesanan.Update(O => new { O.StatusPesanan, O.VerifikasiPembayaran }, new ModelData.pemesanan { StatusPesanan = StatusPesanan.Batal, VerifikasiPembayaran = VerifikasiPembayaran.Batal }, O => O.Id == item.Id);
                    }

                }
            }
        }

        internal static bool CompanyIsActive(string ui)
        {
            using (var db = new OcphDbContext())
            {
                return db.Companies.Where(O => O.UserId == ui).FirstOrDefault().Terverifikasi;
            }
        }

        internal static bool IsAvaliable(int idLayanan, DateTime tanggalAcara,int jumlah, ref layanan l)
        {

            using (var db = new OcphDbContext())
            {
                DateTime tgl = tanggalAcara.Add(TimeSpan.FromDays(1));
                DateTime tgl1 = tanggalAcara.Subtract(TimeSpan.FromDays(1));
                StatusPesanan status = StatusPesanan.Batal;
                StatusPesanan status1 = StatusPesanan.Selesai;
                var data = db.Pemesanan.Where(O => O.TanggalAcara >= tgl1 && O.TanggalAcara <=tgl && O.IsEvent == false && O.StatusPesanan != status && O.StatusPesanan != status1);
                var result = from a in data
                             join b in db.PemesananDetail.Select() on a.Id equals b.IdPemesanan
                             join c in db.Layanan.Where(O => O.Id == idLayanan) on b.IdLayanan equals c.Id
                             select new {Layanan=c, details=b };

                var r = result.FirstOrDefault();
              

                if (r != null)
                {
                    var readystok = r.Layanan.Stok- result.Sum(O => O.details.Jumlah);
                    l = r.Layanan;
                    l.Stok = readystok;
                    if (readystok - jumlah >= 0)
                    {
                       
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    l = db.Layanan.Where(O => O.Id == idLayanan).FirstOrDefault();
                    if (l.Stok - jumlah >= 0)
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
