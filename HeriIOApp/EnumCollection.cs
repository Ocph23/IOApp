using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeriIOApp
{
    class EnumCollection
    {
    }

    public enum StatusPesanan
    {
        Baru,Menunggu,Pelaksanaan,Selesai,Batal
    }

    public enum VerifikasiPembayaran
    {
        Tunda,Lunas,Batal
    }


    public enum StatusAntarKembali
    {
        Belum,Sudah
    }
}
