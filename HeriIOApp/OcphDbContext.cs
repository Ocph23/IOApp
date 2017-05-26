using DAL.DContext;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeriIOApp.ModelData;

namespace HeriIOApp
{
    public class OcphDbContext : IDbContext, IDisposable
    {
        private string connectionString = "Server=localhost;database=iodb;UID=root;password=;CharSet=utf8;Persist Security Info=True";
      //  private string connectionString = "Server=mysql3.gear.host;database=nyanyian;UID=nyanyian;password=Alpharian@77;CharSet=utf8;Persist Security Info=True";
        private IDbConnection _Connection = null;

        public OcphDbContext()
        {

        }
        public IRepository<kategori> Categories { get { return new Repository<kategori>(this); } }
        public IRepository<jenisevent> Events { get { return new Repository<jenisevent>(this); } }
        public IRepository<perusahaan> Companies{ get { return new Repository<perusahaan>(this); } }
        public IRepository<pelanggan> Customers { get { return new Repository<pelanggan>(this); } }
        public IRepository<layanan> Layanan{ get { return new Repository<layanan>(this); } }
        public IRepository<pemesanan> Pemesanan{ get { return new Repository<pemesanan>(this); } }
        public IRepository<pemesanandetail> PemesananDetail { get { return new Repository<pemesanandetail>(this); } }

        public IRepository<pembayaran> Pembayaran{ get { return new Repository<pembayaran>(this); } }
        public IRepository<pesan> Inbox{ get { return new Repository<pesan>(this); } }
        public IRepository<reportfee> ReportFees { get { return new Repository<reportfee>(this); } }
        public IRepository<penawaran> Penawarans { get { return new Repository<penawaran>(this); } }
        public IRepository<ReportFeeEvent> ReportFeeEvents { get { return new Repository<ReportFeeEvent>(this); } }

        public IRepository<profile> Profiles{ get { return new Repository<profile>(this); } }


        public IDbConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new MySqlDbContext(this.connectionString);
                    return _Connection;
                }
                else
                {
                    return _Connection;
                }
            }
        }

        public void Dispose()
        {
            if (_Connection != null)
            {
                if (this.Connection.State != ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
            }
        }

    }
}
