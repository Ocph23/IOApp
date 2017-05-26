using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; using DAL;

namespace HeriIOApp.ModelData
{
    [TableName("ReportFeeEvent")]
    public class ReportFeeEvent : BaseNotifyProperty
    {
        [DbColumn("KodePemesanan")]
        public string KodePemesanan
        {
            get { return _kodepemesanan; }
            set
            {
                _kodepemesanan = value;
                OnPropertyChange("KodePemesanan");
            }
        }

        [DbColumn("StatusPesanan")]
        public string StatusPesanan
        {
            get { return _statuspesanan; }
            set
            {
                _statuspesanan = value;
                OnPropertyChange("StatusPesanan");
            }
        }

        [DbColumn("Tanggal")]
        public DateTime Tanggal
        {
            get { return _tanggal; }
            set
            {
                _tanggal = value;
                OnPropertyChange("Tanggal");
            }
        }

        [DbColumn("VerifikasiPembayaran")]
        public VerifikasiPembayaran VerifikasiPembayaran
        {
            get { return _verifikasipembayaran; }
            set
            {
                _verifikasipembayaran = value;
                OnPropertyChange("VerifikasiPembayaran");
            }
        }

        [DbColumn("Jumlah")]
        public int Jumlah
        {
            get { return _jumlah; }
            set
            {
                _jumlah = value;
                OnPropertyChange("Jumlah");
            }
        }

        [DbColumn("Unit")]
        public double Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                OnPropertyChange("Unit");
            }
        }

        [DbColumn("Perusahaan")]
        public string Perusahaan
        {
            get { return _perusahaan; }
            set
            {
                _perusahaan = value;
                OnPropertyChange("Perusahaan");
            }
        }

        [DbColumn("Pelanggan")]
        public string Pelanggan
        {
            get { return _pelanggan; }
            set
            {
                _pelanggan = value;
                OnPropertyChange("Pelanggan");
            }
        }

        [DbColumn("biaya")]
        public double biaya
        {
            get { return _biaya; }
            set
            {
                _biaya = value;
                fee = value * 0.1;
                OnPropertyChange("biaya");
            }
        }

     //   [DbColumn("fee")]
        public double fee
        {
            get {
                return _fee;
            }
            set
            {
                _fee = value;
                OnPropertyChange("fee");
            }
           
        }

        [DbColumn("NamaLayanan")]
        public string NamaLayanan
        {
            get { return _namalayanan; }
            set
            {
                _namalayanan = value;
                OnPropertyChange("NamaLayanan");
            }
        }

       
       
        [DbColumn("CancelByUser")]
        public bool CancelByUser
        {
            get { return _cancelByUser; }
            set { _cancelByUser = value;
                OnPropertyChange("CancelByUser"); }
        }


        [DbColumn("JenisEvent")]
        public string JenisEvent
        {
            get { return _jenisEvent; }
            set
            {
                _jenisEvent = value;
                OnPropertyChange("JenisEvent");
            }
        }

        private bool _cancelByUser;
        private string _kodepemesanan;
        private string _statuspesanan;
        private DateTime _tanggal;
        private VerifikasiPembayaran _verifikasipembayaran;
        private int _jumlah;
        private double _unit;
        private string _perusahaan;
        private string _pelanggan;
        private double _biaya;
        private double _fee;
        private string _namalayanan;
        private string _jenisEvent;
    }
}
