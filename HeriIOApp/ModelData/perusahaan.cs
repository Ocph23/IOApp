using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
{ 
     [TableName("perusahaan")] 
     public class perusahaan : BaseNotifyProperty
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

          [DbColumn("UserId")] 
          public string UserId 
          { 
               get{return _userid;} 
               set{ 
                      _userid=value; 
                     OnPropertyChange("UserId");
                     }
          } 

          [DbColumn("Nama")] 
          public string Nama 
          { 
               get{return _nama;} 
               set{ 
                      _nama=value; 
                     OnPropertyChange("Nama");
                     }
          } 

          [DbColumn("Pemilik")] 
          public string Pemilik 
          { 
               get{return _pemilik;} 
               set{ 
                      _pemilik=value; 
                     OnPropertyChange("Pemilik");
                     }
          } 

          [DbColumn("Telepon")] 
          public string Telepon 
          { 
               get{return _telepon;} 
               set{ 
                      _telepon=value; 
                     OnPropertyChange("Telepon");
                     }
          } 

          [DbColumn("Email")] 
          public string Email 
          { 
               get{return _email;} 
               set{ 
                      _email=value; 
                     OnPropertyChange("Email");
                     }
          } 

          [DbColumn("Alamat")] 
          public string Alamat 
          { 
               get{return _alamat;} 
               set{ 
                      _alamat=value; 
                     OnPropertyChange("Alamat");
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



        [DbColumn("Terverifikasi")]
        public bool Terverifikasi
        {
            get { return _terverifikasi; }
            set
            {
               _terverifikasi=value;
                OnPropertyChange("Terverifikasi");
            }
        }


          private int  _id;
           private string  _userid;
           private string  _nama;
           private string  _pemilik;
           private string  _telepon;
           private string  _email;
           private string  _alamat;
           private DateTime  _tanggal;
        private bool _terverifikasi;
    }
}


