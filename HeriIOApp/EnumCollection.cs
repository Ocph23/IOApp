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


    public enum UserType
    {
        Administrator,Company,Customer
    }
    public enum StatusPesanan
    {
        Baru,Pelaksanaan,Selesai,Batal
    }

    public enum VerifikasiPembayaran
    {
       None,MenungguVerifikasi,Panjar,Lunas,Batal
    }

    public enum JenisPembayaran
    {
        Panjar, Pelunasan
    }


    public enum StatusAntarKembali
    {
        Belum,Sudah
    }
}
