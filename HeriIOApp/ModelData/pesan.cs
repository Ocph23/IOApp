using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
{ 
     [TableName("pesan")] 
     public class pesan : BaseNotifyProperty
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

          [DbColumn("IdUser")] 
          public string IdUser 
          { 
               get{return _iduser;} 
               set{ 
                      _iduser=value; 
                     OnPropertyChange("IdUser");
                     }
          } 

          [DbColumn("Pengirim")] 
          public string Pengirim 
          { 
               get{return _pengirim;} 
               set{ 
                      _pengirim=value; 
                     OnPropertyChange("Pengirim");
                     }
          } 

          [DbColumn("Judul")] 
          public string Judul 
          { 
               get{return _judul;} 
               set{ 
                      _judul=value; 
                     OnPropertyChange("Judul");
                     }
          } 

          [DbColumn("Tanggal")] 
          public DateTime Tanggal 
          { 
               get{return _tanggal;} 
               set{ 
                      _tanggal=value; 
                     OnPropertyChange("Tanggal");
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

        [DbColumn("Terbaca")]
        public bool Terbaca {

            get { return _terbaca; }
            set
            {
                _terbaca = value;
                OnPropertyChange("Terbaca");
            }
        }

        private int  _id;
           private string  _iduser;
           private string  _pengirim;
           private string  _judul;
           private DateTime  _tanggal;
           private string  _pesan;
        private bool _terbaca;
    }
}


