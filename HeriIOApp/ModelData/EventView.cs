using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeriIOApp.ModelData
{
    public class EventView:ModelData.pemesanan
    {
        public EventView()
        {

        }

        public EventView(pemesanan p):base(p)
        {
            
        }

        public jenisevent JenisEvent { get; internal set; }
        public List<penawaran> Penawarans
        {
            get; set;
        }
    }
}