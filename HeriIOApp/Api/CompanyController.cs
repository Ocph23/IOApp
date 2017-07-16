using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HeriIOApp.Api
{
    public class CompanyController : ApiController
    {
        // GET: api/Company

        [Authorize(Roles = "Company")]
        public IEnumerable<dynamic> GetLayanan()
        {
            Helpers.CheckWaktubooking();
            try
            {
                var userId = User.Identity.GetUserId();
                using (var db = new OcphDbContext())
                {
                    var perusahaan = db.Companies.Where(O => O.UserId == userId).FirstOrDefault();
                    if (perusahaan != null)
                    {
                        //  var result = db.Layanan.Where(O => O.IdPerusahaan == perusahaan.Id);

                        var data = from a in db.Layanan.Where(O => O.IdPerusahaan == perusahaan.Id)
                                   join b in db.Categories.Select() on a.IdKategori equals b.Id
                                   select new {a.Id,a.IdKategori, a.Aktif, a.Harga, a.Nama, a.Keterangan, a.Stok,
                                        Kategori = b.Nama };

                        return data.OrderByDescending(O=>O.Aktif);
                    }
                    else
                    {
                        throw new Exception("Lengkapi Profile Perusahaan Anda");
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost]

        public async Task<HttpResponseMessage> PostLayanan()
        {
            if (Helpers.CompanyIsActive(User.Identity.GetUserId()))
            {
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        if (!Request.Content.IsMimeMultipartContent())
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                            "This request is not properly formatted"));
                        var provider = new MultipartMemoryStreamProvider();
                        await Request.Content.ReadAsMultipartAsync(provider);

                        var layanan = new ModelData.layanan { Aktif = true, Id = 0 };

                        foreach (HttpContent ctnt in provider.Contents)
                        {
                            var name = ctnt.Headers.ContentDisposition.Name;
                            var field = name.Substring(1, name.Length - 2);
                            if (field == "file")
                            {
                                //now read individual part into STREAM
                                var stream = await ctnt.ReadAsStreamAsync();

                                byte[] data = new byte[stream.Length];


                                if (stream.Length != 0)
                                {
                                    await stream.ReadAsync(data, 0, (int)stream.Length);
                                    layanan.Photo = data;
                                }
                            }
                            else if (field == "Nama")
                            {
                                layanan.Nama = await ctnt.ReadAsStringAsync();
                            }
                            else if (field == "IdPerusahaan")
                            {
                                var idPerus = await ctnt.ReadAsStringAsync();
                                layanan.IdPerusahaan = Convert.ToInt32(idPerus);
                            }
                            else if (field == "IdKategori")
                            {
                                var idkat = await ctnt.ReadAsStringAsync();
                                layanan.IdKategori = Convert.ToInt32(idkat);
                            }
                            else if (field == "Stok")
                            {
                                var stok = await ctnt.ReadAsStringAsync();
                                layanan.Stok = Convert.ToInt32(stok);
                            }
                           

                            else if (field == "Harga")
                            {
                                var harga = await ctnt.ReadAsStringAsync();
                                layanan.Harga = Convert.ToDouble(harga);
                            }
                            
                            else if (field == "Keterangan")
                            {
                                layanan.Keterangan = await ctnt.ReadAsStringAsync();
                            }
                        }

                        var userid = User.Identity.GetUserId();
                        var idperus = db.Companies.Where(O => O.UserId == userid).FirstOrDefault().Id;
                        layanan.IdPerusahaan = idperus;
                        var id = db.Layanan.InsertAndGetLastID(layanan);
                        if (id > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.Created, id);

                        }

                        else
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed);

                    }
                    catch (Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                    }
                }
            }else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Perusahaan Anda Tinda Aktif");
            }
        }

        public async Task<HttpResponseMessage> UpdateLayanan()
        {
            if (Helpers.CompanyIsActive(User.Identity.GetUserId()))
            {
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        if (!Request.Content.IsMimeMultipartContent())
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                            "This request is not properly formatted"));
                        var provider = new MultipartMemoryStreamProvider();
                        await Request.Content.ReadAsMultipartAsync(provider);

                        var layanan = new ModelData.layanan { Aktif = true, Id = 0 };

                        foreach (HttpContent ctnt in provider.Contents)
                        {
                            var name = ctnt.Headers.ContentDisposition.Name;
                            var field = name.Substring(1, name.Length - 2);
                             if (field == "Id")
                            {
                                var id= await ctnt.ReadAsStringAsync();
                                layanan.Id = Convert.ToInt32(id);
                            }
                           else if (field == "Nama")
                            {
                                layanan.Nama = await ctnt.ReadAsStringAsync();
                            }
                            else if (field == "IdPerusahaan")
                            {
                                var idPerus = await ctnt.ReadAsStringAsync();
                                layanan.IdPerusahaan = Convert.ToInt32(idPerus);
                            }
                          
                            else if (field == "Stok")
                            {
                                var stok = await ctnt.ReadAsStringAsync();
                                layanan.Stok = Convert.ToInt32(stok);
                            }
                            else if (field == "Harga")
                            {
                                var harga = await ctnt.ReadAsStringAsync();
                                layanan.Harga = Convert.ToDouble(harga);
                            }

                            else if (field == "Keterangan")
                            {
                                layanan.Keterangan = await ctnt.ReadAsStringAsync();
                            }
                        }

                      
                        if (db.Layanan.Update(O=> new { O.Nama,O.Stok,O.Harga,O.Keterangan},layanan,O=>O.Id==layanan.Id))
                        {
                            return Request.CreateResponse(HttpStatusCode.Created, "Data Berhasil Disimpan");

                        }

                        throw new SystemException("Data Gagal Diubah");

                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                    }
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Perusahaan Anda Tinda Aktif");
            }
        }



        public async Task<HttpResponseMessage> RemoveLayanan(int Id,bool actived)
        {
            using (var db = new OcphDbContext())
            {
                try
                {
                    var isUpdate=db.Layanan.Update(O =>new { O.Aktif },new ModelData.layanan {  Aktif=actived},O=>O.Id==Id);
                    if(isUpdate)
                    {
                      return  Request.CreateResponse(HttpStatusCode.Accepted);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Tidak Dapat Menghapus Data");
                    }

                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.NotImplemented,ex.Message);

                }
            }



        }

        [Authorize(Roles ="Company")]
        public IEnumerable<dynamic> GetPesanan()
        {

            using (var db = new OcphDbContext())
            {
                try
                {
                    var UserId = User.Identity.GetUserId();
                    var company = db.Companies.Where(O => O.UserId == UserId).FirstOrDefault();


                    var de = from a in db.Layanan.Where(O => O.IdPerusahaan == company.Id)
                             join b in db.PemesananDetail.Select() on a.Id equals b.IdLayanan
                             join c in db.Pemesanan.Where(O => O.VerifikasiPembayaran == VerifikasiPembayaran.Lunas || O.VerifikasiPembayaran == VerifikasiPembayaran.Panjar) on b.IdPemesanan equals c.Id
                             select c;

                    foreach (var item in de)
                    {

                        item.Layanans = (from a in db.PemesananDetail.Where(O => O.IdPemesanan == item.Id)
                                         join b in db.Layanan.Where(O => O.IdPerusahaan == company.Id) on a.IdLayanan equals b.Id
                                         select new ModelData.LayananView
                                         {
                                             Id = a.Id,
                                             LayananId=b.Id,
                                             Nama = b.Nama,
                                             Harga = b.Harga,
                                             Diantar = a.Diantar,
                                             Penerima = a.Penerima,
                                             Jumlah =a.Jumlah,
                                             Kembali = a.Kembali
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

        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<HttpResponseMessage>GetMyPenawaran()
        {

            using (var db = new OcphDbContext())
            {
                try
                {
                    var ui = User.Identity.GetUserId();
                    var cop = db.Companies.Where(O => O.UserId == ui).FirstOrDefault();
                    var Penawarans = db.Penawarans.Where(O => O.IdPerusahaan == cop.Id).ToList();
                    foreach (var pe in Penawarans)
                    {
                        var pesanan = db.Pemesanan.Where(O => O.Id == pe.IdPemesanan).FirstOrDefault();
                        pe.Pesanan = pesanan;
                        pe.Pesanan.JenisEvent = db.Events.Where(O => O.Id == pesanan.JenisEventId).FirstOrDefault();

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, Penawarans.OrderByDescending(O=>O.Tanggal).ToList());

                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }
        }

        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetPesananById(int Id)
        {

            using (var db = new OcphDbContext())
            {
                try
                {
                    var ui = User.Identity.GetUserId();
                    var cop = db.Companies.Where(O => O.UserId == ui).FirstOrDefault();
                    var Pemesanan = db.Pemesanan.Where(O => O.Id == Id).FirstOrDefault();
                    if (Pemesanan != null)
                    {
                        var JenisEvent = db.Events.Where(O => O.Id == Pemesanan.JenisEventId).FirstOrDefault();
                        var customer = db.Customers.Where(O => O.Id == Pemesanan.IdPelanggan).FirstOrDefault();
                        var Bid = db.Penawarans.Where(O => O.IdPemesanan == Pemesanan.Id && O.IdPerusahaan == cop.Id).FirstOrDefault();
                        return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(new { Pemesanan, JenisEvent, customer,Bid }));
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Pesanan Tidak Ditemukan");
                    }

                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }
        }

        
        [HttpPost]
        public async Task<HttpResponseMessage> PostPenawaran()
        {
            var item = await Request.Content.ReadAsAsync<ModelData.penawaran>();
            using (var db = new OcphDbContext())
            {
                var ui = User.Identity.GetUserId();
                var cop = db.Companies.Where(O => O.UserId == ui).FirstOrDefault();
                item.Penerima = string.Empty;
                item.Diantar = false;
                item.Kembali = false;
                item.Dipilih = false;
                item.Tanggal = DateTime.Now;
                item.IdPerusahaan = cop.Id;
                try
                {
                    var id = db.Penawarans.InsertAndGetLastID(item);
                    if (id>0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK,id);
                    }else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Mengajukan Penawaran");
                    }

                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,e.Message);
                }
            }
        }
        [HttpPost]

        public async Task<HttpResponseMessage> ChangeProgress()
        {

            var item = await Request.Content.ReadAsAsync<ModelData.pemesanandetail>();
            using (var db = new OcphDbContext())
            {
                try
                {
                    if (db.PemesananDetail.Update(O => new { O.Diantar, O.Penerima, O.Kembali }, item, O => O.Id == item.Id))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Diperbaharui");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Diperbaharui");
        }

        public async Task<HttpResponseMessage> ChangeProgressPenawaran()
        {

            var item = await Request.Content.ReadAsAsync<ModelData.penawaran>();
            using (var db = new OcphDbContext())
            {
                try
                {
                    if (db.Penawarans.Update(O => new { O.Diantar, O.Penerima, O.Kembali }, item, O => O.Id == item.Id))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Diperbaharui");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Diperbaharui");
        }


        public async Task<HttpResponseMessage> ChangeLayananActived()
        {

            if (Helpers.CompanyIsActive(User.Identity.GetUserId()))
            {
                var item = await Request.Content.ReadAsAsync<ModelData.layanan>();
                using (var db = new OcphDbContext())
                {
                    try
                    {
                        if (db.Layanan.Update(O => new { O.Aktif}, item, O => O.Id == item.Id))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }

                    }
                    catch (Exception e)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Diperbaharui");
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Gagal Diperbaharui");
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Perusahaan Anda Tidak Aktif");
        }

    }
}
