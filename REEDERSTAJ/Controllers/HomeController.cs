using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using REEDERSTAJ.Models;

namespace REEDERSTAJ.Controllers
{
    public class HomeController : Controller
    {

        private REEDER2Entities db = new REEDER2Entities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Telefon()
        {
            var urunler = db.URUNLER.Include(u => u.KATEGORILER);
            urunler = urunler.OrderBy(a => a.ADI);
            return View(urunler.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Telefon(Siralama siralama)
        {

            var urunler = db.URUNLER.Include(u => u.KATEGORILER);

            if (siralama.SIRA == "En Düşük Fiyat")
            {
                urunler = urunler.OrderBy(a => a.FIYAT);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Yüksek Fiyat")
            {
                urunler = urunler.OrderByDescending(a => a.FIYAT);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Yeni")
            {
                urunler = urunler.OrderByDescending(a => a.ID);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Eski")
            {
                urunler = urunler.OrderBy(a => a.ID);
                return View(urunler.ToList());
            }

            urunler = urunler.OrderBy(a => a.ADI);
            return View(urunler.ToList());
        }


        public ActionResult Tablet()
        {
            var urunler = db.URUNLER.Include(u => u.KATEGORILER);
            urunler = urunler.OrderBy(a => a.ADI);

            return View(urunler.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tablet(Siralama siralama)
        {

            var urunler = db.URUNLER.Include(u => u.KATEGORILER);

            if (siralama.SIRA == "En Düşük Fiyat")
            {
                urunler = urunler.OrderBy(a => a.FIYAT);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Yüksek Fiyat")
            {
                urunler = urunler.OrderByDescending(a => a.FIYAT);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Yeni")
            {
                urunler = urunler.OrderByDescending(a => a.ID);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Eski")
            {
                urunler = urunler.OrderBy(a => a.ID);
                return View(urunler.ToList());
            }

            urunler = urunler.OrderBy(a => a.ADI);
            return View(urunler.ToList());
        }


        public ActionResult AkilliSaat()
        {
            var urunler = db.URUNLER.Include(u => u.KATEGORILER);
            urunler = urunler.OrderBy(a => a.ADI);

            return View(urunler.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AkilliSaat(Siralama siralama)
        {

            var urunler = db.URUNLER.Include(u => u.KATEGORILER);

            if (siralama.SIRA == "En Düşük Fiyat")
            {
                urunler = urunler.OrderBy(a => a.FIYAT);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Yüksek Fiyat")
            {
                urunler = urunler.OrderByDescending(a => a.FIYAT);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Yeni")
            {
                urunler = urunler.OrderByDescending(a => a.ID);
                return View(urunler.ToList());
            }
            else if (siralama.SIRA == "En Eski")
            {
                urunler = urunler.OrderBy(a => a.ID);
                return View(urunler.ToList());
            }

            urunler = urunler.OrderBy(a => a.ADI);
            return View(urunler.ToList());
        }


        public ActionResult Profil(int? id)
        {
            MUSTERI mUSTERI = db.MUSTERI.Find(id);

            if (Session["Musteri"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != Convert.ToInt32(Session["Musteri"]))
            {
                return RedirectToAction("Profil/" + Convert.ToInt32(Session["Musteri"]));
            }

            

            ViewBag.YASADIGISEHIRID = new SelectList(db.SEHIRLER, "ID", "ACIKLAMA", mUSTERI.YASADIGISEHIRID);
            return View(mUSTERI);
        }

        public ActionResult Sifre()
        {
            if (Session["Musteri"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sifre([Bind(Include = "MEVCUT,YSIFRE,OSIFRE")] SIFRE sIFRE)
        {
            int musID = Convert.ToInt32(Session["Musteri"]);
            var mus = db.MUSTERI.Where(a => a.ID == musID ).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if ( mus.SIFRE == sIFRE.MEVCUT)
                {
                    if (sIFRE.YSIFRE == sIFRE.OSIFRE)
                    {
                        mus.SIFRE = sIFRE.YSIFRE;
                        db.Entry(mus).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["D"] = "D";
                        return RedirectToAction("Profil/" + Session["Musteri"]);
                    }
                    else
                    {
                        TempData["AD"] = "AD";
                        return RedirectToAction("Sifre");
                    }
                }
                else
                {
                    TempData["MSY"] = "MSY";
                    return RedirectToAction("Sifre");
                }
            }   
            return View();
        }



        public ActionResult UrunDetay(int? id)
        {
            if (Session["Musteri"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            URUNLER uRUNLER = db.URUNLER.Find(id);
            VMSepeteEkle vm = new VMSepeteEkle();
            vm.GetURUNLER = db.URUNLER.Find(id);
            vm.GetSEPETSATIR = db.SEPETSATIR.Find(id);
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunDetay([Bind(Include = "ID,SEPETID,URUNID,URUNADET,TARIH")] SEPETSATIR sEPETSATIR)
        {

            var sEPET = db.SEPET.Where(c => c.ID == sEPETSATIR.SEPETID).FirstOrDefault();
            var sTOK = db.URUNLER.Where(d => d.ID == sEPETSATIR.URUNID).FirstOrDefault();

            int temp = Convert.ToInt32(Session["SepetID"]);

            if (ModelState.IsValid)
            {
                
                if(sEPETSATIR.URUNADET < 1)
                {
                    /* Müşteriler 1 den daha az ürün Satın alamaz */
                    TempData["HATA"] = "HATALI ADET";
                    return RedirectToAction("UrunDetay/" + sEPETSATIR.URUNID);             
                }
                if(sEPETSATIR.URUNADET > sTOK.STOKMIKTAR)
                {
                    /* Müşteriler olmayan ürün satın alamaz */
                    TempData["SHATA"] = "HATALI ADET";
                    return RedirectToAction("UrunDetay/" + sEPETSATIR.URUNID);
                }

                var test = db.SEPETSATIR.Where(m => m.SEPETID == temp).ToList();


                /*Eğer sepete eklenilen ürün sepette varsa sepetteki ürünün adeti ile şimdi gönderilen
                 adet toplanır yani bir sepette aynı üründen bulunmaz fazla sayıda bulunur.*/
                foreach ( var i in test)
                {
                    if (sEPETSATIR.SEPETID == temp && sEPETSATIR.URUNID == i.URUNID)
                    {
                        i.URUNADET = i.URUNADET + sEPETSATIR.URUNADET;

                        db.Entry(i).State = EntityState.Modified;
                        db.SaveChanges();

                        if (sTOK.KATEGORIID == 1)
                        {
                            TempData["POPUP"] = "POPUP";
                            return RedirectToAction("Telefon");

                        }
                        else if (sTOK.KATEGORIID == 2)
                        {
                            TempData["POPUP"] = "POPUP";
                            return RedirectToAction("Tablet");
                        }
                        else
                        {
                            TempData["POPUP"] = "POPUP";
                            return RedirectToAction("AkilliSaat");
                        }
                    }
                }


                sEPETSATIR.TARIH = DateTime.Now;
                sEPETSATIR.URUNID = sEPETSATIR.ID;
                sEPET.KONTROL = true;
                
                /* sepete eklenen miktarda ürün stok tan düşer*/
                sTOK.STOKMIKTAR = sTOK.STOKMIKTAR - sEPETSATIR.URUNADET;

                db.SEPETSATIR.Add(sEPETSATIR);
                db.SaveChanges();   
                

                /*görünüm kısmındaki sepet ikonunun üstünde yazan sayı sepetteki farklı ürün sayısı*/
                Session["SepetS"] = Convert.ToInt32(Session["SepetS"]) + 1;

                if (sEPETSATIR.URUNLER.KATEGORIID == 1)
                {
                    TempData["POPUP"] = "POPUP";
                    return RedirectToAction("Telefon");

                }
                else if (sEPETSATIR.URUNLER.KATEGORIID == 2)
                {
                    TempData["POPUP"] = "POPUP";
                    return RedirectToAction("Tablet");
                }
                else
                {
                    TempData["POPUP"] = "POPUP";
                    return RedirectToAction("AkilliSaat");
                }
            }

            ViewBag.SEPETID = new SelectList(db.SEPET, "ID", "ID", sEPETSATIR.SEPETID);
            ViewBag.URUNID = new SelectList(db.URUNLER, "ID", "ADI", sEPETSATIR.URUNID);
            return View(sEPETSATIR);
        }

        public ActionResult Sepet(int? id)
        {
            if (Session["Musteri"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != Convert.ToInt32(Session["SepetID"]))
            {
                return RedirectToAction("Sepet/" + Convert.ToInt32(Session["SepetID"]));
            }

            var sEPET = db.SEPETSATIR.Include(s => s.SEPET);

            return View(sEPET);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sepet([Bind(Include = "ID,MUSTERIID,TARIH,ADRES")] SEPET sEPET)
        {
            if (ModelState.IsValid)
            {
                sEPET.KONTROL = true;
                db.Entry(sEPET).State = EntityState.Modified;
                db.SaveChanges();


                Random Personel = new Random();

                SAYAC sAYAC = db.SAYAC.Find(1);

                FIS fIS = db.FIS.Create();
                fIS.FISNO = sAYAC.FFIS ;
                sAYAC.FFIS = (Convert.ToInt32(sAYAC.FFIS) + 1).ToString();
                fIS.SEPETID = Convert.ToInt32(Session["SepetID"]);
                fIS.PERSONELID = Personel.Next(1,7);
                db.FIS.Add(fIS);
                db.SaveChanges();


                FATURA fATURA = db.FATURA.Create();
                fATURA.TOPLAM = Convert.ToDouble(Session["Toplam"]);
                fATURA.FISID = fIS.ID;
                db.FATURA.Add(fATURA);
                db.SaveChanges();


                FATURALAR fATURALAR = db.FATURALAR.Create();
                fATURALAR.FATURANO = sAYAC.FFATURA;
                sAYAC.FFATURA = (Convert.ToInt32(sAYAC.FFATURA) + 1).ToString();
                fATURALAR.TARIHID = sEPET.ID;
                fATURALAR.FATURAID = fATURA.ID;
                db.FATURALAR.Add(fATURALAR);

                Session["SepetS"] = 0;

                db.SaveChanges();

                SEPET sEPETtemp = new SEPET();

                sEPETtemp.MUSTERIID = Convert.ToInt32(Session["Musteri"]);
                sEPETtemp.TARIH = DateTime.Now;
                sEPETtemp.KONTROL = false;
                db.SEPET.Add(sEPETtemp);
                db.SaveChanges();

                Session["SepetID"] = sEPETtemp.ID;

                TempData["POPUP"] = "POPUP";
                return RedirectToAction("Index");
            }

            ViewBag.MUSTERIID = new SelectList(db.MUSTERI, "ID", "ADISOYADI", sEPET.MUSTERIID);
            return View(sEPET);
        }
        public ActionResult SepettenCikar(int id)
        {

            SEPETSATIR Urun = db.SEPETSATIR.Find(id);

            Urun.URUNLER.STOKMIKTAR = Urun.URUNLER.STOKMIKTAR + Urun.URUNADET;
            Session["SepetS"] = Convert.ToInt32(Session["SepetS"]) - 1;
            db.SEPETSATIR.Remove(Urun);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Faturalarim()
        {
            var faturalar = db.FATURALAR.Include(f => f.FATURA);
            return View(faturalar.ToList());
        }
        public ActionResult FaturaGoruntule(int? id)
        {
            

            return View();
        }
    }
}