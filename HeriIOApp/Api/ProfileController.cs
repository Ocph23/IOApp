using HeriIOApp.ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.AspNet.Identity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace HeriIOApp.Api
{
    public class ProfileController : ApiController
    {
        // GET: api/Profile
       
        // GET: api/Profile/5
        [Authorize]
        public async Task<HttpResponseMessage> Get()
        {
            using (var db = new OcphDbContext())
            {
                var userid = User.Identity.GetUserId();
                var p = db.Profiles.Where(O => O.UserId == userid).FirstOrDefault();
                if(p==null)
                {
                    p = new profile { UserId = userid, Selogan="<p>Click Untuk Edit Quote</p>", Description= "<p>Click Untuk Edit Profil Perusahaan</p>" };
                    try
                    {
                        if (User.IsInRole("Company"))
                        {
                            p.UserType = UserType.Company;
                        }
                        else if (User.IsInRole("Customer"))
                        {
                            p.UserType = UserType.Customer;
                        }
                        else
                            p.UserType = UserType.Administrator;

                      p.Id=  db.Profiles.InsertAndGetLastID(p);
                        Request.CreateResponse(HttpStatusCode.OK, p);
                    }
                    catch (Exception ex)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    }
                }
              return  Request.CreateResponse(HttpStatusCode.OK, p);
            }
        }

        [Authorize(Roles ="Company")]
        public async Task<HttpResponseMessage> UpdateUserPhoto()
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

                        var p = new ModelData.profile ();

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
                                    p.UserPhoto= data;
                                }
                            }
                            else if (field == "UserId")
                            {
                                p.UserId= await ctnt.ReadAsStringAsync();
                            }
                            else if (field == "Id")
                            {
                                var id = await ctnt.ReadAsStringAsync();
                                p.Id = Convert.ToInt32(id);
                            }
                            else if (field == "UserType")
                            {
                                var idPerus = await ctnt.ReadAsStringAsync();
                                p.UserType = (UserType)Enum.Parse(typeof(UserType), idPerus);
                            }
                        }

                        if (db.Profiles.Update(O => new { O.UserPhoto }, p, O => O.Id == p.Id))
                        {
                            return Request.CreateResponse(HttpStatusCode.Created, true);

                        }

                        else
                            throw new SystemException("Data Tidak Dapat Diubah");

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
        [Authorize(Roles = "Company")]
        public async Task<HttpResponseMessage> UpdatePageImage()
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

                        var p = new ModelData.profile();

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
                                    p.PageImage = data;
                                }
                            }
                            else if (field == "UserId")
                            {
                                p.UserId = await ctnt.ReadAsStringAsync();
                            }
                            else if (field == "Id")
                            {
                                var id = await ctnt.ReadAsStringAsync();
                                p.Id = Convert.ToInt32(id);
                            }
                            else if (field == "UserType")
                            {
                                var idPerus = await ctnt.ReadAsStringAsync();
                                p.UserType = (UserType)Enum.Parse(typeof(UserType), idPerus);
                            }
                        }

                        if (db.Profiles.Update(O => new { O.PageImage }, p, O => O.Id == p.Id))
                        {
                            return Request.CreateResponse(HttpStatusCode.Created, true);

                        }

                        else
                            throw new SystemException("Data Tidak Dapat Diubah");

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


        public async Task<HttpResponseMessage> UpdateProfileText()
        {
            var item = await Request.Content.ReadAsAsync<ModelData.profile>();
            using (var db = new OcphDbContext())
            {
                try
                {
                  if(  db.Profiles.Update(O => new { O.Selogan, O.Description }, item, O => O.Id == item.Id))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Data berhasil diubah");
                    }
                    throw new SystemException("Data Gagal diubah");
                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }


        // POST: api/Profile
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Profile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Profile/5
        public void Delete(int id)
        {
        }
    }
}
