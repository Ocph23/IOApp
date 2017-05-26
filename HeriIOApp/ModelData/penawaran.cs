using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeriIOApp.ModelData
{
    [TableName("penawaran")]
    public class penawaran : BaseNotifyProperty
    {
        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChange("Id");
            }
        }

        [DbColumn("IdPemesanan")]
        public int IdPemesanan
        {
            get { return _idpemesanan; }
            set
            {
                _idpemesanan = value;
                OnPropertyChange("IdPemesanan");
            }
        }

        [DbColumn("IdPerusahaan")]
        public int IdPerusahaan
        {
            get { return _idperusahaan; }
            set
            {
                _idperusahaan = value;
                OnPropertyChange("IdPerusahaan");
            }
        }

        [DbColumn("Diantar")]
        public bool Diantar
        {
            get { return _diantar; }
            set
            {
                _diantar = value;
                OnPropertyChange("Diantar");
            }
        }

        [DbColumn("Penerima")]
        public string Penerima
        {
            get { return _penerima; }
            set
            {
                _penerima = value;
                OnPropertyChange("Penerima");
            }
        }

        [DbColumn("Kembali")]
        public bool Kembali
        {
            get { return _kembali; }
            set
            {
                _kembali = value;
                OnPropertyChange("Kembali");
            }
        }

        [DbColumn("Biaya")]
        public int Biaya
        {
            get { return _biaya; }
            set
            {
                _biaya = value;
                OnPropertyChange("Biaya");
            }
        }

        [DbColumn("DetailPenawaran")]
        public string DetailPenawaran
        {
            get { return _detailpenawaran; }
            set
            {
                _detailpenawaran = value;
                OnPropertyChange("DetailPenawaran");
            }
        }


        [DbColumn("Dipilih")]
        public bool Dipilih
        {
            get { return _dipilih; }
            set
            {
                _dipilih= value;
                OnPropertyChange("Dipilih");
            }
        }

        [DbColumn("Tanggal")]
      

        public DateTime Tanggal
        {
            get { return _tanggal; }
            set { _tanggal = value;
                OnPropertyChange("Tanggal");
            }
        }


        public pemesanan Pesanan { get; internal set; }
        public perusahaan Perusahaan { get; internal set; }

        private int _id;
        private int _idpemesanan;
        private int _idperusahaan;
        private bool _diantar;
        private string _penerima;
        private bool _kembali;
        private int _biaya;
        private string _detailpenawaran;
        private bool _dipilih; private DateTime _tanggal;
    }

}