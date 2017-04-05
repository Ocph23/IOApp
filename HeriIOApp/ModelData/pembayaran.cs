using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
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

          [DbColumn("TipeFile")] 
          public string TipeFile 
          { 
               get{return _tipefile;} 
               set{ 
                      _tipefile=value; 
                     OnPropertyChange("TipeFile");
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
      }
}


