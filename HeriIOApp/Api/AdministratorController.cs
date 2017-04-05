using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HeriIOApp.Api
{
    public class AdministratorController : ApiController
    {
        // GET: api/administrator
        public IEnumerable<ModelData.perusahaan> GetCompanies()
        {
            Helpers.CheckWaktubooking();
            using (var db = new OcphDbContext())
            {
                var result = db.Companies.Select();
                return result;
            }
        }



        public IEnumerable<ModelData.pelanggan> GetCustomers()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Customers.Select();
                return result;
            }
        }


        public IEnumerable<dynamic> GetPembayaran()
        {
            using (var db = new OcphDbContext())
            {
                var result = from a in db.Pembayaran.Select()
                             join b in db.Pemesanan.Select() on a.IdPemesanan equals b.Id
                             select new { Pembayaran=a, Pemesanan=b };
                return result;
            }
        }

        public IEnumerable<ModelData.pemesanan> GetPesanan()
        {

            using (var db = new OcphDbContext())
            {
                try
                {

                    var de = from a in db.Layanan.Select()
                             join b in db.PemesananDetail.Select() on a.Id equals b.IdLayanan
                             join c in db.Pemesanan.Select() on b.IdPemesanan equals c.Id
                             select c;

                    foreach (var item in de)
                    {

                        item.Layanans = (from a in db.PemesananDetail.Where(O => O.IdPemesanan == item.Id)
                                         join b in db.Layanan.Select() on a.IdLayanan equals b.Id
                                         join p in db.Companies.Select() on b.IdPerusahaan equals p.Id
                                         select new ModelData.LayananView
                                         {
                                             Id = a.Id,
                                             LayananId = b.Id,
                                             Nama = b.Nama,
                                             Harga = b.Harga,
                                             HargaPengiriman = b.HargaPengiriman,
                                             Diantar = a.Diantar,
                                             Penerima = a.Penerima,
                                             Kembali = a.Kembali, 
                                             Perusahaan=p
                                         }).ToList();
                        item.Pelanggan = db.Customers.Where(O => O.Id == item.IdPelanggan).FirstOrDefault();


                    }

                    var result = de.GroupBy(O => O.KodePemesanan).ToList();
                    List<ModelData.pemesanan> list = new List<ModelData.pemesanan>();
                    foreach (var item in result)
                    {
                        list.Add(item.Select(O => O).FirstOrDefault());
                    }
                    return list;
                }
                catch (Exception ex)
                {

                    throw new SystemException(ex.Message);
                }


            }
        }

        public IEnumerable<ModelData.reportfee> GetPenjualan()
        {
            using (var db = new OcphDbContext())
            {
                return db.ReportFees.Where(O=>O.VerifikasiPembayaran== VerifikasiPembayaran.Lunas).ToList().OrderBy(O=>O.KodePemesanan);
            }
        }
        public async Task<HttpResponseMessage> ValidatePayment()
        {
            var item = await Request.Content.ReadAsAsync<ModelData.pemesanan>();

            if (item.VerifikasiPembayaran == VerifikasiPembayaran.Lunas && item.StatusPesanan== StatusPesanan.Menunggu)
            {
                using (var db = new OcphDbContext())
                {
                    var tr = db.Connection.BeginTransaction();
                    try
                    {
                        item.StatusPesanan = StatusPesanan.Pelaksanaan;
                        if (db.Pemesanan.Update(O => new {O.StatusPesanan, O.VerifikasiPembayaran }, item, O => O.Id == item.Id))
                        {
                            Dictionary<string, ModelData.pesan> datas = new Dictionary<string, ModelData.pesan>();
                            item.Details = db.PemesananDetail.Where(O => O.IdPemesanan == item.Id).ToList();
                            //Send Message To Companu
                            foreach (var detail in item.Details)
                            {
                                var message = "Anda Mendapatkan Orderan Dengan KodePesanan " + item.KodePemesanan + " Lihat Pesanan Untuk Melihat Detail";
                                var layanan = db.Layanan.Where(O => O.Id == detail.IdLayanan).FirstOrDefault();
                                if (layanan != null)
                                {
                                    var company = db.Companies.Where(O => O.Id == layanan.IdPerusahaan).FirstOrDefault();
                                    if (!datas.ContainsKey(company.UserId))
                                        datas.Add(company.UserId, new ModelData.pesan { Judul = "Orderan Baru", Pengirim = "Administrator", Pesan = message, Tanggal = DateTime.Now, IdUser = company.UserId });
                                }
                            }

                            foreach (var pesan in datas)
                            {
                                db.Inbox.Insert(pesan.Value);

                            }


                            tr.Commit();
                            return Request.CreateErrorResponse(HttpStatusCode.OK, "Pembayaran Berhasil Diverifikasi");

                        }
                        else
                        {
                            throw new SystemException("Pesanan Gagal Diverifikasi");
                        }
                    }

                    catch (Exception)
                    {
                        tr.Rollback();

                        throw new SystemException("Pesanan Gagal Diverifikasi");
                    }
                }
            }else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Anda Tidak Dapat Membatalkan Verivikasi Pembayaran");

            }
        }



        public async Task<HttpResponseMessage> ValidateCompany()
        {
            var item = await Request.Content.ReadAsAsync<ModelData.perusahaan>();

            using (var db = new OcphDbContext())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Perusahaan Anda Telah ");
                    

                    var isUpdated = db.Companies.Update(O => new { O.Terverifikasi }, item, O => O.Id == item.Id);
                    string message = "";
                    if(isUpdated)
                    {
                        if (isUpdated && item.Terverifikasi)
                        {
                            sb.Append("DIAKTIFKAN ");
                            message = "Perusahaan Telah Diaktifkan";
                        }
                        else if (isUpdated && !item.Terverifikasi)
                        {
                            sb.Append("DINONAKTIFKAN ");
                            message = "Perusahaan Telah Dinonaktifkan";
                        }


                        var pesan = new ModelData.pesan { Judul = "Aktivasi Peerusahaan", Pengirim = "Administrator", Pesan = sb.ToString(), Tanggal = DateTime.Now, IdUser = item.UserId };
                        db.Inbox.Insert(pesan);

                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Mengubah Status");
                    }
                }
                catch (Exception e)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Mengubah Status");
                }
            }
        }


    }
}
