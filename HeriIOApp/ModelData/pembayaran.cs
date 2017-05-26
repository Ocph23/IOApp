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
     [TableName("pembayaran")] 
     public class pembayaran : BaseNotifyProperty
    {
          [PrimaryKey("Id")] 
          [DbColumn("Id")] 
          public int Id 
          { 
               get{return _id;} 
               set{ 
                      _id=value; 
                     OnPropertyChange("Id");
                     }
          } 

          [DbColumn("IdPemesanan")] 
          public int IdPemesanan 
          { 
               get{return _idpemesanan;} 
               set{ 
                      _idpemesanan=value; 
                     OnPropertyChange("IdPemesanan");
                     }
          } 

          [DbColumn("bank")] 
          public string bank 
          { 
               get{return _bank;} 
               set{ 
                      _bank=value; 
                     OnPropertyChange("bank");
                     }
          } 

          [DbColumn("NamaPengirim")] 
          public string NamaPengirim 
          { 
               get{return _namapengirim;} 
               set{ 
                      _namapengirim=value; 
                     OnPropertyChange("NamaPengirim");
                     }
          } 

          [DbColumn("Pesan")] 
          public string Pesan 
          { 
               get{return _pesan;} 
               set{ 
                      _pesan=value; 
                     OnPropertyChange("Pesan");
                     }
          } 

          [DbColumn("NamaFile")] 
          public string NamaFile 
          { 
               get{return _namafile;} 
               set{ 
                      _namafile=value; 
                     OnPropertyChange("NamaFile");
                     }
          }


        [JsonConverter(typeof(StringEnumConverter))]
        [DbColumn("JenisPembayaran")]
        public JenisPembayaran JenisPembayaran
        {
            get { return _jenispembayaran; }
            set
            {
                _jenispembayaran = value;
                OnPropertyChange("JenisPesanan");
            }
        }

        [DbColumn("JumlahBayar")]
      
        public double JumlahBayar
        {
            get { return _jumlahBayar; }
            set { _jumlahBayar = value;
                OnPropertyChange("JumlahBayar");
            }
        }


        [DbColumn("TipeFile")] 
          public string TipeFile 
          { 
               get{return _tipefile;} 
               set{ 
                      _tipefile=value; 
                     OnPropertyChange("TipeFile");
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

        [DbColumn("data")] 
          public byte[] data 
          { 
               get{return _data;} 
               set{ 
                      _data=value; 
                     OnPropertyChange("data");
                     }
          } 

          private int  _id;
           private int  _idpemesanan;
           private string  _bank;
           private string  _namapengirim;
           private string  _pesan;
           private string  _namafile;
           private string  _tipefile;
           private byte[]  _data;
        private JenisPembayaran _jenispembayaran;
        private double _jumlahBayar;
        private DateTime _tanggal;
    }
}


