using HeriIOApp.ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HeriIOApp.Api
{
    public class CategoriesController : ApiController
    {
        // GET: api/Categories
        public IEnumerable<kategori> Get()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Categories.Select();
                return result;
            }
        }

        // GET: api/Categories/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Categories

        public async Task<HttpResponseMessage> Post()
        {
            var item = await Request.Content.ReadAsAsync<kategori>();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var id = db.Categories.InsertAndGetLastID(item);
                    if (id > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, id);
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

        // PUT: api/Categories/5
        public async Task<HttpResponseMessage> Put()
        {
            var item = await Request.Content.ReadAsAsync<kategori>();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var isUpdated= db.Categories.Update(O=>new { O.Nama,O.Keterangan},item,O=>O.Id==item.Id);
                    if (isUpdated)
                        return Request.CreateResponse(HttpStatusCode.OK);
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
        // DELETE: api/Categories/5

            [HttpPost]
        public async Task<HttpResponseMessage> DeleteCategory()
        {
            var item = await Request.Content.ReadAsAsync<kategori>();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var isDeleted= db.Categories.Delete(O => O.Id == item.Id);
                    if (isDeleted)
                        return Request.CreateResponse(HttpStatusCode.OK);
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Tidak Dapat Menghapus Data");
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, ex);
            }
        }
    }
}
