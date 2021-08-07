using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrainReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainReservationController : ControllerBase
    {
        [HttpPost]
        public TrainReservationResponseModel TrainReservationRequest(TrainReservationRequestModel trainReservationRequestModel)
         {
            TrainReservationResponseModel trainReservationResponseModel = new TrainReservationResponseModel();
            List<YerlesimAyrinti> yerlesimAyrintiList = new List<YerlesimAyrinti>();
            if (trainReservationRequestModel != null)
            {
                bool Available = IsAvailable(trainReservationRequestModel);

                if (Available == true)
                {
                    if (trainReservationRequestModel.KisilerFarkliVagonlaraYerlestirilebilir)
                    {
                        int i = 0;
                        while (i < trainReservationRequestModel.RezervasyonYapilacakKisiSayisi)
                        {
                            Random random = new Random();
                            int vagonRandom = random.Next(trainReservationRequestModel.Tren.Vagonlar.Count);

                            var selectedCar = trainReservationRequestModel.Tren.Vagonlar[vagonRandom];
                            var ratio = Convert.ToDouble((selectedCar.DoluKoltukAdet + 1)) / Convert.ToDouble(selectedCar.Kapasite);
                            if (ratio < 0.7)
                            {
                                if (yerlesimAyrintiList.Count != 0)
                                {
                                    int v = 0;
                                    foreach (var vagondolumu in yerlesimAyrintiList)
                                    {
                                        if (selectedCar.Ad == vagondolumu.VagonAdi)
                                        {
                                            vagondolumu.KisiSayisi += 1;
                                            v++;
                                        }
                                    }
                                    if (v == 0)
                                    {
                                        yerlesimAyrintiList.Add(new YerlesimAyrinti
                                        {
                                            VagonAdi = selectedCar.Ad,
                                            KisiSayisi = 1
                                        });
                                    }
                                }
                                else
                                {
                                    yerlesimAyrintiList.Add(new YerlesimAyrinti
                                    {
                                        VagonAdi = selectedCar.Ad,
                                        KisiSayisi = 1
                                    });
                                }
                                i++;
                            }
                        }
                    }
                    else
                    {
                        bool theEndv = true;
                        while (theEndv)
                        {
                            Random random = new Random();
                            int vagonRandom = random.Next(trainReservationRequestModel.Tren.Vagonlar.Count);
                            var selectedCar = trainReservationRequestModel.Tren.Vagonlar[vagonRandom];
                            var ratio = Convert.ToDouble((selectedCar.DoluKoltukAdet + trainReservationRequestModel.RezervasyonYapilacakKisiSayisi)) / Convert.ToDouble(selectedCar.Kapasite);
                            if (ratio < 0.7)
                            {
                                yerlesimAyrintiList.Add(new YerlesimAyrinti
                                {
                                    VagonAdi = selectedCar.Ad,
                                    KisiSayisi = trainReservationRequestModel.RezervasyonYapilacakKisiSayisi
                                });
                                theEndv = false;
                            }
                        }
                    }
                    trainReservationResponseModel.RezervasyonYapilabilir = true;
                }
                else
                    trainReservationResponseModel.RezervasyonYapilabilir = false;
            }

            trainReservationResponseModel.YerlesimAyrinti = yerlesimAyrintiList;
            return trainReservationResponseModel;
        }
        public bool IsAvailable(TrainReservationRequestModel trainReservationRequestModel)
        {
            if (trainReservationRequestModel.KisilerFarkliVagonlaraYerlestirilebilir == false)
            {
                foreach (var item in trainReservationRequestModel.Tren.Vagonlar)
                {
                    var ratio = Convert.ToDouble((item.DoluKoltukAdet + trainReservationRequestModel.RezervasyonYapilacakKisiSayisi)) / Convert.ToDouble(item.Kapasite);
                    if (ratio < 0.7)
                        return true;
                }
                return false;
            }
            else
            {
                var reservationPeople = trainReservationRequestModel.RezervasyonYapilacakKisiSayisi;
                foreach (var item in trainReservationRequestModel.Tren.Vagonlar)
                {
                    var doluKoltuk = item.DoluKoltukAdet;
                    while (Convert.ToDouble(doluKoltuk) / Convert.ToDouble(item.Kapasite) < 0.7)
                    {
                        doluKoltuk++;
                        reservationPeople--;
                    }
                }
                if (reservationPeople <= 0)
                    return true;
            }
            return false;
        }
    }
}
