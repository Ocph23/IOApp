using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
{ 
     [TableName("layanan")]
    public class layanan:BaseNotifyProperty
    {
        [PrimaryKey("Id")]
        [DbColumn("Id")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        [DbColumn("Nama")]
        public string Nama
        {
            get { return _nama; }
            set
            {
                _nama = value;
            }
        }

        [DbColumn("IdKategori")]
        public int IdKategori
        {
            get { return _idkategori; }
            set
            {
                _idkategori = value;
            }
        }

        [DbColumn("IdPerusahaan")]
        public int IdPerusahaan
        {
            get { return _idperusahaan; }
            set
            {
                _idperusahaan = value;
            }
        }

       

        [DbColumn("Stok")]
        public int Stok
        {
            get { return _stok; }
            set
            {
                _stok = value;
            }
        }

        [DbColumn("Unit")]
        public double Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
            }
        }

        [DbColumn("Harga")]
        public double Harga
        {
            get { return _harga; }
            set
            {
                _harga = value;
            }
        }

     

        [DbColumn("Keterangan")]
        public string Keterangan
        {
            get { return _keterangan; }
            set
            {
                _keterangan = value;
            }
        }

        [DbColumn("Aktif")]
        public bool Aktif
        {
            get { return _aktif; }
            set
            {
                _aktif = value;
            }
        }




        [DbColumn("Photo")]
        public byte[] Photo
        {
            get { return _photo; }
            set { _photo = value;
            }
        }

        public string KategoriName { get; internal set; }

        private int _id;
        private string _nama;
        private int _idkategori;
        private int _idperusahaan;
        private int _stok;
        private double _unit;
        private double _harga;
        private double _hargapengiriman;
        private string _keterangan;
        private bool _aktif;
        private byte[] _photo;
    }

}


