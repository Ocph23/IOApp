using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public static void CheckWaktubooking()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Pemesanan.Where(O => O.StatusPesanan == StatusPesanan.Baru && O.VerifikasiPembayaran == VerifikasiPembayaran.Tunda);
                foreach (var item in result)
                {
                    var sekarang = DateTime.Now;
                    if (sekarang.Subtract(item.Tanggal).TotalMinutes > 30)
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
    }
}
