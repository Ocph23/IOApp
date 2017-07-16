using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HeriIOApp.ModelData 
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

        [DbColumn("JenisEventId")]
        public int JenisEventId
        {
            get { return _JenisEventId; }
            set
            {
                _JenisEventId= value;
                OnPropertyChange("JenisEventId");
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
        [DbColumn("TanggalSelesai")]
        public DateTime? TanggalSelesai
        {
            get { return _tanggalselesai; }
            set
            {
                _tanggalselesai = value;
                OnPropertyChange("TanggalSelesai");
            }
        }


        [DbColumn("Alamat")]
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

        [JsonConverter(typeof(StringEnumConverter))]
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

        [JsonConverter(typeof(StringEnumConverter))]
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
        [DbColumn("IsEvent")]
        public bool IsEvent
        {
            get { return _isEvent; }
            set{
                _isEvent = value;
                OnPropertyChange("IsEvent");
            }
        }

        [DbColumn("KodeValidasi")]
        public int KodeValidasi
        {
            get { return _codeValidate; }
            set
            {
                _codeValidate = value;
                OnPropertyChange("CodeValidate");
            }
        }


        [DbColumn("JumlahUndangan")]
        public int JumlahUndangan
        {
            get { return _jumlahUndangan; }
            set
            {
                _jumlahUndangan = value;
                OnPropertyChange("JumlahUndangan");
            }
        }

      
        [DbColumn("CancelByUser")]
        public bool CancelByUser
        {
            get { return _CancelByUser; }
            set
            {
                _CancelByUser = value;
                OnPropertyChange("CancelByUser");
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
        public pembayaran Panjar { get;  set; }
        public jenisevent JenisEvent { get; internal set; }

        private int  _id;
           private int  _idpelanggan;
           private string  _kodepemesanan;
           private DateTime  _tanggalacara;
           private string  _catatan;
           private StatusPesanan  _statuspembayaran;
           private DateTime  _tanggal;
        private string _alamat;
        private VerifikasiPembayaran _VerifikasiPembayaran;
        private bool _isEvent;
        private int _codeValidate;
        private int _jumlahUndangan;
        private int _JenisEventId; private bool _CancelByUser;
        private DateTime? _tanggalselesai;

        public pemesanan(pemesanan p)
        {
            this.Alamat = p.Alamat;
            this.Catatan = p.Catatan;
            this.Details = p.Details;
            this.Id = p.Id;
            this.IdPelanggan = p.Id;
            this.IsEvent = p.IsEvent;
            this.JumlahUndangan = p.JumlahUndangan;
            this.KodePemesanan = p.KodePemesanan;
            this.KodeValidasi = p.KodeValidasi;
            this.Layanans = p.Layanans;
            this.Pelanggan = p.Pelanggan;
            this.StatusPesanan = p.StatusPesanan;
            this.Tanggal = p.Tanggal;
            this.TanggalAcara = p.TanggalAcara;
            this.VerifikasiPembayaran = p.VerifikasiPembayaran;
            this.TanggalSelesai = p.TanggalSelesai;
        }

        public pemesanan()
        {

        }
    }
}


