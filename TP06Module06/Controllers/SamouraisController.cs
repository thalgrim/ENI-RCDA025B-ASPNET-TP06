using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using TP06Module06.Data;
using TP06Module06.Models;

namespace TP06Module06.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            ViewBag.Potentiel = samourai.Force+samourai.Arme.Degats;
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraisViewModel vm = new SamouraisViewModel();
            List<Arme> listeArme = db.Armes.ToList();
            List<Arme> listeArmeSansProprietaire = db.Armes.ToList();
            List<Samourai> listeSamourai = db.Samourais.ToList();
            foreach (Samourai samourai in listeSamourai)
            {
                if (samourai.Arme != null)
                {
                    listeArmeSansProprietaire.Remove(listeArme.FirstOrDefault(x => x.Id == samourai.Arme.Id));
                }
            }
            vm.Armes = listeArmeSansProprietaire;            
            vm.ArtMartials = db.ArtMartials.ToList();
            return View(vm);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraisViewModel vm)
        {

            Samourai samourai = new Samourai();
            samourai.Nom = vm.Samourai.Nom;
            samourai.Force = vm.Samourai.Force;

            if (ModelState.IsValid)
            {
                if (vm.IdSelectedArme.HasValue)
                {
                    samourai.Arme = db.Armes.FirstOrDefault(arme => arme.Id == vm.IdSelectedArme.Value);
                }

                if (vm.IdSelectedArtMartials.Count != 0)
                {
                    samourai.ArtMartials = db.ArtMartials.Where(a => vm.IdSelectedArtMartials.Contains(a.Id)).ToList();
                }

                db.Samourais.Add(samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            vm.Armes = db.Armes.ToList();
            vm.ArtMartials = db.ArtMartials.ToList();
            return View(vm);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraisViewModel vm = new SamouraisViewModel();
            vm.Armes = db.Armes.ToList();
            vm.ArtMartials = db.ArtMartials.ToList();
            vm.Samourai = samourai;
            return View(vm);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraisViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var samouraidb = db.Samourais.Find(vm.Samourai.Id);
                samouraidb.Force = vm.Samourai.Force;
                samouraidb.Nom = vm.Samourai.Nom;
                
                if (vm.IdSelectedArme.HasValue)
                {
                    samouraidb.Arme = db.Armes.FirstOrDefault(arme => arme.Id == vm.IdSelectedArme.Value);
                }
                else
                {
                    samouraidb.Arme = null;
                }

                if (vm.IdSelectedArtMartials.Count != 0)
                {
                    foreach (var item in vm.IdSelectedArtMartials)
                    {
                        samouraidb.ArtMartials.Add(db.ArtMartials.FirstOrDefault(artMartial => artMartial.Id == item));
                    }
                };

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            vm.Armes = db.Armes.ToList();
            vm.ArtMartials = db.ArtMartials.ToList();
            return View(vm);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
