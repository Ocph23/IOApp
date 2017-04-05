using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
{
    [TableName("pemesanan")]
    public class pemesanan : BaseNotifyProperty
    {
        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int Id
        {
            get { return _id; }
            set {
                _id = value;
                OnPropertyChange("Id");
            }
        }

        [DbColumn("IdPelanggan")]
        public int IdPelanggan
        {
            get { return _idpelanggan; }
            set {
                _idpelanggan = value;
                OnPropertyChange("IdPelanggan");
            }
        }

        [DbColumn("KodePemesanan")]
        public string KodePemesanan
        {
            get { return _kodepemesanan; }
            set {
                _kodepemesanan = value;
                OnPropertyChange("KodePemesanan");
            }
        }

        [DbColumn("TanggalAcara")]
        public DateTime TanggalAcara
        {
            get { return _tanggalacara; }
            set {
                _tanggalacara = value;
                OnPropertyChange("TanggalAcara");
            }
        }



        public string Alamat
        {
            get { return _alamat; }
            set
            {
                _alamat = value;
                OnPropertyChange("Alamat");
            }
        }

        [DbColumn("Catatan")]
        public string Catatan
        {
            get { return _catatan; }
            set {
                _catatan = value;
                OnPropertyChange("Catatan");
            }
        }

        [DbColumn("StatusPesanan")]
        public StatusPesanan StatusPesanan
        {
            get { return _statuspembayaran; }
            set {
                _statuspembayaran = value;
                OnPropertyChange("StatusPesanan");
            }
        }

        [DbColumn("Tanggal")]
        public DateTime Tanggal
        {
            get { return _tanggal; }
            set {
                _tanggal = value;
                OnPropertyChange("Tanggal");
            }
        }


        [DbColumn("VerifikasiPembayaran")]
        public VerifikasiPembayaran VerifikasiPembayaran
        {
            get { return _VerifikasiPembayaran; }
            set
            {
                _VerifikasiPembayaran = value;
                OnPropertyChange("VerifikasiPembayaran");
            }
        }

        public pelanggan Pelanggan{get;set;}

        public List<pemesanandetail> Details
        {
            get;set;
        }

        public List<LayananView> Layanans
        {
            get; set;
        }

        private int  _id;
           private int  _idpelanggan;
           private string  _kodepemesanan;
           private DateTime  _tanggalacara;
           private string  _catatan;
           private StatusPesanan  _statuspembayaran;
           private DateTime  _tanggal;
        private string _alamat;
        private VerifikasiPembayaran _VerifikasiPembayaran;
    }
}


