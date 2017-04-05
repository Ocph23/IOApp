using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;namespace HeriIOApp.ModelData 
{ 
     [TableName("pemesanandetail")] 
     public class pemesanandetail : BaseNotifyProperty
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

          [DbColumn("IdLayanan")] 
          public int IdLayanan
          { 
               get{return _idlayanan;} 
               set{ 
                      _idlayanan=value; 
                     OnPropertyChange("IdLayananDetail");
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



        [DbColumn("Diantar")]

        public bool Diantar
        {
            get
            {
                return _tgldiantar;
            }
            set
            {
                _tgldiantar = value;
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
            get { return _tglkembali; }
            set
            {
                _tglkembali = value;
                OnPropertyChange("Kembali");
            }
        }

        private int  _id;
        private int  _idpemesanan;
        private int  _idlayanan;
        private bool _tgldiantar;
        private string _penerima;
        private bool _tglkembali;
        private int _jumlah;
    }
}


