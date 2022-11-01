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
    public class YonetimController : Controller
    {
        private REEDER2Entities db = new REEDER2Entities();
        // GET: Yonetim
        public ActionResult Index()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        
        }
        public ActionResult Kullanicilar()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var degerler = db.MUSTERI.ToList();

            return View(degerler);
        }

        public ActionResult KullaniciProfil(int? id)
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            MUSTERI mUSTERI = db.MUSTERI.Find(id);
            

            ViewBag.YASADIGISEHIRID = new SelectList(db.SEHIRLER, "ID", "ACIKLAMA", mUSTERI.YASADIGISEHIRID);
            return View(mUSTERI);
        }

        public ActionResult DurumDegistir(int? id)
        {
            
            MUSTERI mUSTERI = db.MUSTERI.Find(id);

            if (mUSTERI.DURUM == true || mUSTERI.DURUM == null)
            {
                mUSTERI.DURUM = false;
            }
            else
            {
                mUSTERI.DURUM = true;
            }


            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        
        }

    public ActionResult Faturalar()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var faturalar = db.FATURALAR.Include(f => f.FATURA);
            return View(faturalar.ToList());
        }
        public ActionResult FaturaGoruntule(int? id)
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            FATURALAR faturalar = db.FATURALAR.Find(id);
            VMFaturalar vm = new VMFaturalar();
            vm.GetFaturalars = db.FATURALAR.Find(id);
            vm.GetSiparis = db.SEPETSATIR.ToList();
            vm.GetFIYATDEGISTIR = db.FIYATDEGISTIR.ToList();
            
            return View(vm);
        }
        
        public ActionResult Urunler()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uRUNLER = db.URUNLER.Include(u => u.KATEGORILER);
            return View(uRUNLER.ToList());
        }

        public ActionResult URUNEKLE()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.KATEGORIID = new SelectList(db.KATEGORILER, "ID", "ACIKLAMA");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult URUNEKLE([Bind(Include = "ID,ADI,FIYAT,STOKMIKTAR,KATEGORIID,IMG,KODU")] URUNLER uRUNLER)
        {
            if (ModelState.IsValid)
            {
                db.URUNLER.Add(uRUNLER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
    
            ViewBag.KATEGORIID = new SelectList(db.KATEGORILER, "ID", "ACIKLAMA", uRUNLER.KATEGORIID);
            return View(uRUNLER);
        }
        public ActionResult UrunDuzenle(int? id)
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            URUNLER uRUNLER = db.URUNLER.Find(id);
            if (uRUNLER == null)
            {
                return HttpNotFound();
            }
            ViewBag.KATEGORIID = new SelectList(db.KATEGORILER, "ID", "ACIKLAMA", uRUNLER.KATEGORIID);
            return View(uRUNLER);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UrunDuzenle([Bind(Include = "ID,ADI,FIYAT,STOKMIKTAR,KATEGORIID,IMG,KALDIR,KODU")] URUNLER uRUNLER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uRUNLER).State = EntityState.Modified;
                db.SaveChanges();

                var f = db.FIYATDEGISTIR.Where(a => a.URUNID == uRUNLER.ID).OrderByDescending(a => a.ID).FirstOrDefault();

                if (f.EFIYAT != uRUNLER.FIYAT)
                {
                    FIYATDEGISTIR fIYATDEGISTIR = db.FIYATDEGISTIR.Create();
                    fIYATDEGISTIR.URUNID = uRUNLER.ID;
                    fIYATDEGISTIR.DTARIH = DateTime.Now;
                    fIYATDEGISTIR.EFIYAT = uRUNLER.FIYAT;
                    db.FIYATDEGISTIR.Add(fIYATDEGISTIR);
                    db.SaveChanges();
                }

                return RedirectToAction("Urunler");
            }

            ViewBag.KATEGORIID = new SelectList(db.KATEGORILER, "ID", "ACIKLAMA", uRUNLER.KATEGORIID);
            return View(uRUNLER);
        }
    }
}