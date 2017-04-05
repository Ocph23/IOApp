using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
{ 
     [TableName("foto")] 
     public class foto : BaseNotifyProperty
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

          [DbColumn("IdLayananDetail")] 
          public int IdLayananDetail 
          { 
               get{return _idlayanandetail;} 
               set{ 
                      _idlayanandetail=value; 
                     OnPropertyChange("IdLayananDetail");
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
           private int  _idlayanandetail;
           private string  _namafile;
           private string  _tipefile;
           private byte[]  _data;
      }
}


