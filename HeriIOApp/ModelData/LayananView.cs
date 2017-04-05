using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeriIOApp.ModelData
{
    public class LayananView:layanan
    {
        private int _jumlah;
        private double _biaya;

        public int LayananId { get; set; }

        public int Jumlah
        {
            get { return _jumlah; }
            set
            {
                _jumlah = value;
                Biaya = value * Harga;
                OnPropertyChange("Jumlah");
                
            }
        }

        public double Biaya
        {
            get { return _biaya; }
            set
            {
                _biaya = value;
                OnPropertyChange("Biaya");
            }
        }


        public bool Diantar { get; set; }
        public string Penerima { get; set; }
        public bool Kembali { get; set; }
        public perusahaan Perusahaan { get;  set; }
    }
}
