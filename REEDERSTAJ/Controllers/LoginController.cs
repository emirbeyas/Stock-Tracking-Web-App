using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using REEDERSTAJ.Models;
using System.Web.Security;

namespace REEDERSTAJ.Controllers
{
    public class LoginController : Controller
    {
        private REEDER2Entities db = new REEDER2Entities();
        // GET: Login

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(MUSTERI m)
        {
            if (m.TCKIMLIKNO == "Admin" && m.SIFRE == "123")
            {
                Session["ADMIN"] = m.TCKIMLIKNO;

                return RedirectToAction("Index", "Yonetim");
            }

            var mus = db.MUSTERI.FirstOrDefault(x => x.TCKIMLIKNO == m.TCKIMLIKNO && x.SIFRE == m.SIFRE && x.DURUM != false );

            if (mus != null)
            {
                Session["Musteri"] = mus.ID;
                Session["MusteriAd"] = mus.ADISOYADI;

                var s = db.SEPET.ToList();
                var ss = db.SEPETSATIR.ToList();
                
                foreach (var b in ss)
                {
                    if ((b.SEPET.ADRES == null && b.SEPET.MUSTERI.ADISOYADI == mus.ADISOYADI))
                    {
                        db.SEPETSATIR.Remove(b);
                        db.SaveChanges();
                    }
                }

                foreach (var a in s)
                {
                    if ((a.KONTROL == false && a.MUSTERI.ADISOYADI == mus.ADISOYADI) 
                        || (a.KONTROL == null && a.MUSTERI.ADISOYADI == mus.ADISOYADI) 
                        || (a.ADRES == null && a.MUSTERI.ADISOYADI == mus.ADISOYADI) )
                    {
                        db.SEPET.Remove(a);
                        db.SaveChanges();
                    }
                }

                SEPET sEPET = new SEPET();

                sEPET.MUSTERIID = mus.ID;
                sEPET.TARIH = DateTime.Now;
                sEPET.KONTROL = false;
                db.SEPET.Add(sEPET);
                db.SaveChanges();

                Session["SepetS"] = 0;
                Session["SepetID"] = sEPET.ID;

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult SignUp()
        {
            ViewBag.YASADIGISEHIRID = new SelectList(db.SEHIRLER, "ID", "ACIKLAMA");
            return View();
        }
        [HttpPost]
        public ActionResult SignUp([Bind(Include = "ID,ADISOYADI,TCKIMLIKNO,DOGUMTARIHI,SIFRE,YASADIGISEHIRID")] MUSTERI mUSTERI)
        {
            if (ModelState.IsValid)
            {
                if ((mUSTERI.TCKIMLIKNO).Length != 11)
                {
                    ViewBag.YASADIGISEHIRID = new SelectList(db.SEHIRLER, "ID", "ACIKLAMA", mUSTERI.YASADIGISEHIRID);

                    return View();
                }

                var musteriler = db.MUSTERI.ToList();

                foreach ( var a in musteriler)
                {

                    if (mUSTERI.TCKIMLIKNO == a.TCKIMLIKNO)
                    {
                        TempData["HATA"] = "TC KİMLİK NUMARANIZA KAYITLI BİR KULLANICI VAR";
                        return RedirectToAction("SignUp");
                    }

                }

                db.MUSTERI.Add(mUSTERI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.YASADIGISEHIRID = new SelectList(db.SEHIRLER, "ID", "ACIKLAMA", mUSTERI.YASADIGISEHIRID);
            return View(mUSTERI);
        }

    }
}