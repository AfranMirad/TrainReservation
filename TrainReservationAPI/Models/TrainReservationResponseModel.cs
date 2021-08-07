using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainReservationAPI
{
    public class TrainReservationResponseModel
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<YerlesimAyrinti> YerlesimAyrinti { get; set; }
    }
}
