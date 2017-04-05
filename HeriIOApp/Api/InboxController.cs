using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HeriIOApp.ModelData;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace HeriIOApp.Api
{
    public class InboxController : ApiController
    {
        // GET: api/Inbox
        public IEnumerable<pesan> GetInbox()
        {
            var userid = User.Identity.GetUserId();
            using (var db = new OcphDbContext())
            {
                return db.Inbox.Where(O => O.IdUser == userid).OrderByDescending(O => O.Tanggal);
            }
            
        }

        // GET: api/Inbox/5
        public int GetInboxCount()
        {
            using (var db = new OcphDbContext())
            {
                var userId = User.Identity.GetUserId();
                return db.Inbox.Where(O => O.IdUser == userId && O.Terbaca==false).Count();
            }
        }


        [HttpGet]
        public async Task<HttpResponseMessage> ReadMessage(int Id)
        {
            try
            {
                using (var db = new OcphDbContext())
                {
                    var isUpdated = db.Inbox.Update(O => new { O.Terbaca }, new pesan { Terbaca = true }, O => O.Id == Id);
                    if (isUpdated)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Can't Updated");
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
