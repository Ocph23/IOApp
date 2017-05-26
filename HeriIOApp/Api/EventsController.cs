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


    public class EventsController : ApiController
    {
        // GET: api/Categories
        public IEnumerable<jenisevent> Get()
        {
            using (var db = new OcphDbContext())
            {
                var result = db.Events.Select();
                return result;
            }
        }

        // GET: api/Categories/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Categories
        [Authorize(Roles = "Administrator")]
        public async Task<HttpResponseMessage> Post()
        {
            var item = await Request.Content.ReadAsAsync<jenisevent>();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var id = db.Events.InsertAndGetLastID(item);
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
        [Authorize(Roles = "Administrator")]
        // PUT: api/Categories/5
        public async Task<HttpResponseMessage> Put()
        {
            var item = await Request.Content.ReadAsAsync<jenisevent>();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var isUpdated = db.Events.Update(O => new { O.Nama, O.Keterangan }, item, O => O.Id == item.Id);
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
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<HttpResponseMessage> DeleteEvent()
        {
            var item = await Request.Content.ReadAsAsync<jenisevent>();
            try
            {
                using (var db = new OcphDbContext())
                {
                    var isDeleted = db.Events.Delete(O => O.Id == item.Id);
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
