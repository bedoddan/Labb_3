using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Labb_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Labb_3.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InsertPerson()
        {
            int i = 0;
            string error = "";
           
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        [HttpPost]
        public IActionResult InsertPerson(IFormCollection fc)
        {

            Personer p1 = new Personer();
            PersonMetoder pm = new PersonMetoder();

            int i = 0;
            string error = "";


            p1.Persnr = Convert.ToInt64(fc["persnr"]);
            p1.Namn = fc["namn"];
            p1.Telnr = fc["telnr"];

            i = pm.InsertPerson(p1, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return RedirectToAction("VisaFiskar");
        }


        public IActionResult InsertFisk(IFormCollection fc)
        {

            Fiskar f1 = new Fiskar();
            PersonMetoder pm = new PersonMetoder();

            int i = 0;
            string error = "";

            f1.Art = fc["Art"];
            f1.Vikt = Convert.ToInt32(fc["Vikt"]);
            f1.Vatten = fc["Vatten"];
            f1.Persnr = Convert.ToInt64(fc["persnr"]);
            f1.Betenr = Convert.ToInt32(fc["Betenr"]);
            

            i = pm.InsertFisk(f1, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult InsertBete(IFormCollection fc)
        {

            Beten b1 = new Beten();
            PersonMetoder pm = new PersonMetoder();

            int i = 0;
            string error = "";


            b1.ID = Convert.ToInt32(fc["betenr"]);
            b1.Color = fc["color"];
            b1.Typ = fc["typ"];

            i = pm.InsertBete(b1, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult VisaFiskar ()
        {

            List<Totalus> Fisklista = new List<Totalus>();
            PersonMetoder pm = new PersonMetoder();
            string error = "";

            Fisklista = pm.GetFiskarWithDataSet(out error);

            ViewBag.error = error;

           

            return View(Fisklista);
        }

     

        public IActionResult VisaBeten ()
        {

            List<Beten> BetesLista = new List<Beten>();
            PersonMetoder pm = new PersonMetoder();

            string error = "";

            BetesLista = pm.GetBetenWithDataSet(out error);

            ViewBag.error = error;


            return View(BetesLista);

        }

        public IActionResult VisaPersoner()
        {

            List<Personer> PersonLista = new List<Personer>();
            PersonMetoder pm = new PersonMetoder();

            string error = "";

            PersonLista = pm.GetPersonerWithDataSet(out error);

            ViewBag.error = error;


            return View(PersonLista);
           
        }

        public IActionResult DeleteFisk(int id)
        {
            
            PersonMetoder pm = new PersonMetoder();

            

           int i = pm.DeleteFisk(id, out string error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult DeletePerson(int id)
        {

            PersonMetoder pm = new PersonMetoder();



            int i = pm.DeletePerson(id, out string error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult DeleteBete(int id)
        {

            PersonMetoder pm = new PersonMetoder();



            int i = pm.DeleteBete(id, out string error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult UpdatePersoner (IFormCollection fc, int id)
        {

            Personer b1 = new Personer();
            PersonMetoder pm = new PersonMetoder();

            int i = 0;
            string error = "";


            b1.Persnr = Convert.ToInt64(fc["Persnr"]);
            b1.Namn = fc["Namn"];
            b1.Telnr = fc["Telnr"];

            i = pm.UpdatePersoner(b1, id, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult UpdateFisk(IFormCollection fc, int id)
        {

            Fiskar b1 = new Fiskar();
            PersonMetoder pm = new PersonMetoder();

            int i = 0;
            string error = "";


            b1.ID = Convert.ToInt32(fc["FiskID"]);
            b1.Art = fc["Art"];
            b1.Vikt = Convert.ToInt32(fc["Vikt"]);
            b1.Vatten = fc["Vatten"];


            i = pm.UpdateFisk(b1, id, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        public IActionResult UpdateBeten(IFormCollection fc, int id)
        {

            Beten b1 = new Beten();
            PersonMetoder pm = new PersonMetoder();

            int i = 0;
            string error = "";


            b1.ID = Convert.ToInt32(fc["Betenr"]);
            b1.Typ = fc["Typ"];
            b1.Color = fc["Color"];

            i = pm.UpdateBeten(b1, id, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }

        [HttpGet]
        public ActionResult Filtrering ()
        {
            
            PersonMetoder pmTotalus = new PersonMetoder();
            

            FiltreringModell myModel = new FiltreringModell
            {
                TotalusFiltLista = pmTotalus.GetFiskarWithDataSet(out string errormsg)
            };

            ViewBag.error = errormsg;
            return View(myModel);
        }

        [HttpPost]
        public ActionResult Filtrering(string ValdArt)
        {
            int i = Convert.ToInt32(ValdArt);
            ViewData["ValdArt"] = i;

            PersonMetoder pmTotalus = new PersonMetoder();

            FiltreringModell myModel = new FiltreringModell
            {
                TotalusFiltLista = pmTotalus.GetFiskarWithDataSet(out string errormsg, i)

            };


            ViewData["ValdArt"] = i;
            ViewBag.error = errormsg;
            return View(myModel);
        }


        [HttpGet]
        public ActionResult Sortering()
        {

            PersonMetoder pmTotalus = new PersonMetoder();

            FiltreringModell myModel = new FiltreringModell
            {
                TotalusFiltLista = pmTotalus.GetFiskarWithDataSet(out string errormsg)
            };

            ViewBag.error = errormsg;

            return View(myModel);
        }

        [HttpPost]
        public ActionResult Sortering(string sort)
        {

            ViewData["sort"] = sort;
            PersonMetoder pmTotalus = new PersonMetoder();


            FiltreringModell myModel = new FiltreringModell
            {
                TotalusFiltLista = pmTotalus.SorteraNamn(out string errormsg, sort)
            };

            ViewBag.sort = sort;
            ViewBag.error = errormsg;
            
            return View(myModel);
        }

        [HttpGet]
        public ActionResult Sokning()
        {

            PersonMetoder pmTotalus = new PersonMetoder();

            FiltreringModell myModel = new FiltreringModell
            {
                TotalusFiltLista = pmTotalus.GetFiskarWithDataSet(out string errormsg)
            };

            ViewBag.error = errormsg;

            return View(myModel);
        }

        [HttpPost]
        public ActionResult Sokning(string SokString)
        {

            PersonMetoder pmTotalus = new PersonMetoder();

            FiltreringModell myModel = new FiltreringModell
            {

                TotalusFiltLista = pmTotalus.GetFiskarWithDataSet(out string errormsg, SokString)
            };

            ViewBag.error = errormsg;

            return View(myModel);
        }

    }

   

}

    

