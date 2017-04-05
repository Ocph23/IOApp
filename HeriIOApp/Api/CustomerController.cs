using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace HeriIOApp.Api
{
    public class CustomerController : ApiController
    {
        // GET: api/Customers
        public IEnumerable<dynamic> GetLayanan()
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var data = from a in db.Layanan.Where(O => O.Aktif == true)
                               join b in db.Categories.Select() on a.IdKategori equals b.Id
                               join c in db.Companies.Where(O => O.Terverifikasi == true) on a.IdPerusahaan equals c.Id
                               select new { a.Photo, a.Aktif, a.Harga, a.HargaPengiriman, a.Id, a.IdKategori, a.IdPerusahaan, a.Nama, a.Keterangan, a.Stok, a.Unit, Kategori = b.Nama, Perusahaan = c };

                    return data;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        // GET: api/Customers/5

        [HttpGet]
        public async Task<HttpResponseMessage> GetPesanan(string Code)
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {

                    var pesanan = db.Pemesanan.Where(O => O.KodePemesanan == Code).FirstOrDefault();
                    if (pesanan != null)
                    {
                        var details = db.PemesananDetail.Where(O => O.IdPemesanan == pesanan.Id);
                        var Items = new List<ModelData.layanan>();
                        //var data = db.Categories.InsertAndGetLastID(item);
                        foreach (var item in details)
                        {
                            Items.Add(db.Layanan.Where(O => O.Id == item.IdLayanan).FirstOrDefault());
                        }

                        var data = new { pesanan, Items };
                        if (Items.Count > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, data);
                        }

                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Data Tidak Ditemukan");
                        }
                    }
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Data Tidak Ditemukan");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetChartView()
        {
            var items = await Request.Content.ReadAsAsync<List<ModelData.pemesanandetail>>();
            try
            {
                using (var db = new OcphDbContext())
                {

                    var newItems = new List<ModelData.LayananView>();
                    //var data = db.Categories.InsertAndGetLastID(item);
                    foreach (var item in items)
                    {
                        var l = db.Layanan.Where(O => O.Id == item.Id).FirstOrDefault();
                        newItems.Add(new ModelData.LayananView
                        {
                            Harga = l.Harga,
                            Nama = l.Nama,
                            Unit = l.Unit,
                            HargaPengiriman = l.HargaPengiriman,
                            Id = l.Id,
                            IdKategori = l.IdKategori,
                            Jumlah = item.Jumlah,
                            IdPerusahaan = l.IdPerusahaan,
                            Aktif = l.Aktif
                        });
                    }


                    if (newItems.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, newItems);
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Tidak Dapat Menyimpan Data");
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<HttpResponseMessage> InsertPesanan()
        {
            var item = await Request.Content.ReadAsAsync<ModelData.pemesanan>();
            using (var db = new OcphDbContext())
            {
                var tr = db.Connection.BeginTransaction();
                try
                {
                    var userId = User.Identity.GetUserId();
                    var customer = db.Customers.Where(O => O.UserId == userId).FirstOrDefault();
                    item.IdPelanggan = customer.Id;
                    if (item.Alamat == null)
                    {
                        item.Alamat = customer.Alamat;
                    }

                    item.StatusPesanan = StatusPesanan.Baru;
                    item.Tanggal = DateTime.Now;
                    item.KodePemesanan = Helpers.KodePemesanan();
                    var id = db.Pemesanan.InsertAndGetLastID(item);

                    if (id > 0)
                    {
                        var message = "Terimakash Telah Melakukan Pemesan. Kode Pemesanan Anda adalah " + item.KodePemesanan + " Silahkan Lakukan Pembayaran untuk Proses Selanjutnya ";
                        var pesan = new ModelData.pesan { IdUser = userId, Judul = "Pemesanan", Pengirim = "Admin", Pesan = message, Tanggal = DateTime.Now };
                        db.Inbox.Insert(pesan);
                        foreach (var detail in item.Details)
                        {
                            detail.IdPemesanan = id;
                            detail.Id = db.PemesananDetail.InsertAndGetLastID(detail);
                        }

                        tr.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, item);
                    }

                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Tidak Dapat Menyimpan Data");
                    }
                }
                catch (Exception ex)
                {

                    tr.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
                }

            }



        }

        public async Task<HttpResponseMessage> PaymentConfirm()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                "This request is not properly formatted"));
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var file = new ModelData.pembayaran();

            foreach (HttpContent ctnt in provider.Contents)
            {
                var name = ctnt.Headers.ContentDisposition.Name;
                var field = name.Substring(1, name.Length - 2);
                if (field == "file")
                {
                    var f = ctnt.Headers.ContentDisposition.FileName;
                    file.NamaFile = f.Substring(1, f.Length - 2);

                    //now read individual part into STREAM
                    var stream = await ctnt.ReadAsStreamAsync();

                    byte[] data = new byte[stream.Length];


                    if (stream.Length != 0)
                    {
                        await stream.ReadAsync(data, 0, (int)stream.Length);
                        file.data = data;
                    }
                }
                else if (field == "IdPemesanan")
                {
                    var id = await ctnt.ReadAsStringAsync();
                    file.IdPemesanan = Convert.ToInt32(id);
                }
                else if (field == "Bank")
                {
                    file.bank = await ctnt.ReadAsStringAsync();
                }
                else if (field == "NamaPengirim")
                {
                    file.NamaPengirim = await ctnt.ReadAsStringAsync();
                }
                else if (field == "Pesan")
                {
                    file.Pesan = await ctnt.ReadAsStringAsync();
                }
                else if (field == "TipeFile")
                {
                    file.TipeFile = await ctnt.ReadAsStringAsync();
                }
            }

            using (var db = new OcphDbContext())
            {
                var tr = db.Connection.BeginTransaction();
                try
                {


                    var save = db.Pembayaran.Insert(file);
                    if (save)
                    {
                        var isUpdated = db.Pemesanan.Update(O => new { O.StatusPesanan }, new ModelData.pemesanan { StatusPesanan = StatusPesanan.Menunggu }, O => O.Id == file.IdPemesanan);
                        if (isUpdated)
                        {
                            tr.Commit();
                            return Request.CreateResponse(HttpStatusCode.Created);
                        }
                        else
                        {
                            throw new SystemException("Konfirmasi Pembayaran Gagal");
                        }

                    }

                    else
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
                }
                catch (Exception ex)
                {
                    tr.Rollback();

                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }

            }
        }
    }
}