using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using HeriIOApp.ModelData;

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
                               select new { a.Photo, a.Aktif, a.Harga,  a.Id, a.IdKategori, a.IdPerusahaan, a.Nama, a.Keterangan, a.Stok, a.Unit, Kategori = b.Nama, Perusahaan = c };

                    return data;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

       
        // GET: api/Customers/5
        public async Task<HttpResponseMessage> GetCompanyProfile(int Id)
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var data = db.Companies.Where(O => O.Id == Id).FirstOrDefault();
                    data.Layanans = db.Layanan.Where(O => O.IdPerusahaan == Id).ToList();
                    var pro = db.Profiles.Where(O => O.UserId == data.UserId).FirstOrDefault();
                   object a = new { pro, data };
                    return Request.CreateResponse(HttpStatusCode.OK, a);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<HttpResponseMessage> GetLayananDetail(int Id)
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var lay = db.Layanan.Where(O => O.Id == Id).FirstOrDefault();
                    var data = db.Companies.Where(O => O.Id == lay.IdPerusahaan).FirstOrDefault();
                    data.Layanans = new List<layanan>();
                    data.Layanans.Add(lay);
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



        [HttpGet]
        public async Task<HttpResponseMessage> GetPesanan(string Code)
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {

                    var pesanan = db.Pemesanan.Where(O => O.KodePemesanan == Code).FirstOrDefault();
                    if(pesanan.VerifikasiPembayaran== VerifikasiPembayaran.Panjar)
                    {
                        var p = JenisPembayaran.Panjar;
                        pesanan.Panjar = db.Pembayaran.Where(O => O.IdPemesanan == pesanan.Id && O.JenisPembayaran == p).FirstOrDefault();
                    }
                    if (pesanan != null && pesanan.IsEvent==false)
                    {
                        var details = db.PemesananDetail.Where(O => O.IdPemesanan == pesanan.Id);
                        //var data = db.Categories.InsertAndGetLastID(item);
                        pesanan.Layanans = (from a in details
                                           join b in db.Layanan.Select() on a.IdLayanan equals b.Id
                                           select new ModelData.LayananView { Aktif=b.Aktif, Diantar=a.Diantar, Harga=b.Harga, 
                                            Id=b.Id, IdKategori=b.Id, IdPerusahaan=b.IdPerusahaan, Jumlah=a.Jumlah, Kembali=a.Kembali, Keterangan=b.Keterangan, LayananId=b.Id,
                                            Nama=b.Nama, Penerima=a.Penerima, Stok=b.Stok, Unit=b.Unit}).ToList();

                        if (pesanan.Layanans.Count > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, pesanan);
                        }

                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Data Tidak Ditemukan");
                        }
                    }else
                         if (pesanan != null && pesanan.IsEvent == true)
                    {
                        var ev = new ModelData.EventView(pesanan) { Panjar=pesanan.Panjar };
                        ev.Penawarans = db.Penawarans.Where(O => O.IdPemesanan == pesanan.Id && O.Dipilih == true).ToList();
                        foreach(var per in ev.Penawarans)
                        {
                            per.Perusahaan = db.Companies.Where(O => O.Id == per.IdPerusahaan).FirstOrDefault();
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, ev);
                        
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

        [Authorize(Roles = "Customer")]
        public async Task<HttpResponseMessage> GetMyEvents()
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var userId = User.Identity.GetUserId();
                    var customer = db.Customers.Where(O => O.UserId == userId).FirstOrDefault();
                    var pesanan = db.Pemesanan.Where(O => O.IdPelanggan == customer.Id && O.IsEvent==true);
                    List<ModelData.EventView> Events = new List<ModelData.EventView>();
                    foreach(var item in pesanan)
                    {
                        ModelData.EventView evt = new ModelData.EventView(item);
                        evt.JenisEvent = db.Events.Where(O => O.Id == item.JenisEventId).FirstOrDefault();
                        Events.Add(evt);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, Events);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
            }
        }

        [Authorize(Roles = "Customer")]
        public async Task<HttpResponseMessage> GetPenawarans(int id)
        {
            Helpers.CheckWaktubooking();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var pesanan = db.Pemesanan.Where(O => O.Id==id).FirstOrDefault();
                    var Penawarans = db.Penawarans.Where(O => O.IdPemesanan == pesanan.Id).ToList();
                    foreach(var item in Penawarans)
                    {
                        item.Perusahaan = db.Companies.Where(O => O.Id == item.IdPerusahaan).FirstOrDefault();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK,Penawarans);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SimpanPenawaran()
        {
            var items = await Request.Content.ReadAsAsync<List<ModelData.penawaran>>();
            using (var db = new OcphDbContext())
            {
                var trans = db.Connection.BeginTransaction();
                try
                {

                    bool onSelected = false;
                    foreach (var item in items)
                    {
                        if (item.Dipilih)
                        {
                            onSelected = true;
                            db.Penawarans.Update(O => new { O.Dipilih }, item, O => O.Id == item.Id);
                        }
                    }

                    if(onSelected)
                    {
                        trans.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }else
                    {
                        
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Tidak Dapat Menyimpan Data");
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Tidak Dapat Menyimpan Data");
                }
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
                    item.JenisEventId = 1;
                    item.KodeValidasi = Helpers.KodeValidasi();
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
                        Helpers.SendEmail(customer.Email, "Pemesanan", message);
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


        public async Task<HttpResponseMessage> InsertEvent()
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
                    item.KodeValidasi = Helpers.KodeValidasi();
                    item.IsEvent = true; 
                        item.KodePemesanan = Helpers.KodePemesanan();
                    var id = db.Pemesanan.InsertAndGetLastID(item);

                    if (id > 0)
                    {
                        var message = "Terimakash Telah Melakukan Pemesan. Kode Pemesanan Anda adalah " + item.KodePemesanan + " Silahkan Lakukan Pembayaran untuk Proses Selanjutnya ";
                        var pesan = new ModelData.pesan { IdUser = userId, Judul = "Pemesanan", Pengirim = "Admin", Pesan = message, Tanggal = DateTime.Now };


                        if (db.Inbox.Insert(pesan)) {
                            Helpers.SendEmail(User.Identity.GetUserName(), "Pemesanan", message);
                        }

                        var cmessage = "Ada Event Baru Dengan Kode Pesanan " + item.KodePemesanan + " An. " + customer.Nama + " Silahkan <button type = 'button' class='btn btn-primary' ng-click='Bid(" + id +
                            ")'>Ajukan Penawaran</button>";

                        var allPengusaha = db.Companies.Select();
                        foreach(var pengusaha in allPengusaha)
                        {
                            var cpesan = new ModelData.pesan { IdUser = pengusaha.UserId, Judul = "Event Baru", Pengirim = "Admin", Pesan = cmessage, Tanggal = DateTime.Now };

                            if (db.Inbox.Insert(cpesan))
                            {
                                var emailMessage = "Ada Event Baru Dengan Kode Pesanan " + item.KodePemesanan + " An. " + customer.Nama + " Silahkan Ajukan Penawaran";
                                Helpers.SendEmail(pengusaha.Email, "Event Baru", emailMessage);
                            }
                        }

                       
                        tr.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, item);
                    }

                    else
                    {
                        throw new SystemException( "Tidak Dapat Menyimpan Data");
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
                else if (field == "JumlahBayar")
                {
                    var j = await ctnt.ReadAsStringAsync();
                    file.JumlahBayar = Convert.ToDouble(j);
                }
                else if (field == "JenisPembayaran")
                {
                    var j = await ctnt.ReadAsStringAsync();
                    file.JenisPembayaran =(JenisPembayaran)Enum.Parse(typeof(JenisPembayaran),j);
                }
            }

            using (var db = new OcphDbContext())
            {
                var tr = db.Connection.BeginTransaction();
                try
                {
                    file.Tanggal = DateTime.Now;

                    var save = db.Pembayaran.Insert(file);
                    if (save)
                    {
                        var isUpdated = db.Pemesanan.Update(O => new { O.VerifikasiPembayaran }, new ModelData.pemesanan { VerifikasiPembayaran= VerifikasiPembayaran.MenungguVerifikasi }, O => O.Id == file.IdPemesanan);
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
                        throw new SystemException("Konfirmasi Pembayaran Gagal");
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> CancelAction()
        {
            var item = await Request.Content.ReadAsAsync<ModelData.pemesanan>();
            if(item.StatusPesanan== StatusPesanan.Baru)
            {
                Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Pesanan Belum bisa di Batalkan, Tunggu Verifikasi Pembayaran dari Admin");

            }else if(item.StatusPesanan== StatusPesanan.Pelaksanaan)
            {
                var today = DateTime.Now;
                var result = item.TanggalAcara.Subtract(today).TotalHours;
                if(result <= 72)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Anda Tidak Dapat Membatalkan Pesanan");
                }else
                {
                    using (var db = new OcphDbContext())
                    {
                        var trans = db.Connection.BeginTransaction();
                        try
                        {
                            item.CancelByUser = true;
                            item.StatusPesanan = StatusPesanan.Batal;
                            item.VerifikasiPembayaran = VerifikasiPembayaran.Batal;
                            db.Pemesanan.Update(O => new { O.StatusPesanan, O.VerifikasiPembayaran,O.CancelByUser }, item, O => O.Id == item.Id);
                            trans.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Pesanan Berhasil Dibatalkan");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message);

                        }
                    }
                }

            }
          return  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error From Server");

        }
    }
}