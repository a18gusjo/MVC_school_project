
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelViewController.Models;
using System.Configuration;
using System.Data;

namespace ModelViewController.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {

            _configuration = configuration;
            SlädeModel slädeModel = new SlädeModel(_configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RenView()
        {
            RenModel renModel = new RenModel(_configuration);
            ViewBag.RenTable = renModel.GetAllRen();
            StankModel stankModel = new StankModel(_configuration);
            ViewBag.StankTable = stankModel.GetAllStank();
            UnderartModel underartModel = new UnderartModel(_configuration);
            ViewBag.UnderartTable = underartModel.GetAllUnderart();
            TjänstModel tjänstModel = new TjänstModel(_configuration);
            ViewBag.TjänstTable = tjänstModel.GetAllTjänst();
            PensioneradModel pensioneradModel = new PensioneradModel(_configuration);
            ViewBag.PensioneradTable = pensioneradModel.GetAllPensionerad();

            return View();
        }
        public IActionResult SlädeView()
        {
            SlädeModel slädeModel = new SlädeModel(_configuration);
            ViewBag.SlädeTable = slädeModel.GetAllSläde();
            SlädeModel regModel = new SlädeModel(_configuration);
            ViewBag.RegTable = regModel.GetAllReg();
            SlädeModel expressModel = new SlädeModel(_configuration);
            ViewBag.ExpressTable = expressModel.GetAllExpress();
            SlädeModel lastModel = new SlädeModel(_configuration);
            ViewBag.LastTable = lastModel.GetAllLast();
            return View();
        }
        public IActionResult RenspannView()
        {
            RenspannModel renspannModel = new RenspannModel(_configuration);
            ViewBag.RenspannTable = renspannModel.GetAllRenSpann();
            return View();
        }

        public IActionResult InsertRen(string nr, string klan, string underart, long stank)
        {
            RenModel renModel = new RenModel(_configuration);
            renModel.InsertRen(nr, klan, underart, stank);
            return RedirectToAction("RenView");
        }

        public IActionResult PensioneraRen(string nr, string tjänststatus)
        {
            if (tjänststatus == "itjänst")
            {
                RenModel renModel = new RenModel(_configuration);
                renModel.PensioneraRen(nr);
            }
            return RedirectToAction("RenView");
        }
        public IActionResult UppdateraPensionerad(string smak, string fabriknamn, string polsaburknr, string ren_nummer)
        {

            PensioneradModel pensioneradModel = new PensioneradModel(_configuration);
            pensioneradModel.UppdateraPensionerad(smak, fabriknamn, polsaburknr, ren_nummer);

            return RedirectToAction("RenView");
        }


        public IActionResult UppdateraTjänst(string lon, string ren_nr)
        {

            TjänstModel tjänstModel = new TjänstModel(_configuration);
            tjänstModel.UppdateraTjänst(lon, ren_nr);
            return RedirectToAction("RenView");
        }

        public IActionResult InsertRenspann(string namn, int kapacitet)
        {
            RenspannModel renspannModel = new RenspannModel(_configuration);
            renspannModel.InsertRenspann(namn, kapacitet);
            return RedirectToAction("RenspannView");
        }

        public IActionResult DeleteRenspann(string namn)
        {
            RenspannModel renspannModel = new RenspannModel(_configuration);
            renspannModel.DeleteRenspann(namn);
            return RedirectToAction("RenspannView");
        }
        public IActionResult SlädeResultat(string namn)
        {

            SlädeModel slädeModel = new SlädeModel(_configuration);
            ViewBag.SlädeResult = slädeModel.SökSläde(namn);

            return View();

        }

        public IActionResult FåKapacitet(string släde_nr)
        {

            SlädeModel slädeModel = new SlädeModel(_configuration);
            ViewBag.Kapacitet = slädeModel.SökSläde(släde_nr);

            return View();

        }


        public IActionResult UppdateraSläde(string tillvärkare, string steglängd, int reg_typ, string nr)
        {
            SlädeModel slädeModel = new SlädeModel(_configuration);
            slädeModel.UppdateraSläde(tillvärkare, steglängd, reg_typ, nr);
            return RedirectToAction("SlädeView");
        }

        public IActionResult InsertLast(string släde_nr, double extrakapacitet, string klimattyp)
        {
            SlädeModel slädeModel = new SlädeModel(_configuration);
            slädeModel.InsertLast(släde_nr, extrakapacitet, klimattyp);
            return RedirectToAction("SlädeView");
        }

        public IActionResult InsertExpress(string släde_nr, string raketantal, string bromsverkan)
        {
            SlädeModel slädeModel = new SlädeModel(_configuration);
            slädeModel.InsertExpress(släde_nr, raketantal, bromsverkan);
            return RedirectToAction("SlädeView");
        }

        public IActionResult DeleteSläde(string nr)
        {
            SlädeModel slädeModel = new SlädeModel(_configuration);
            slädeModel.DeleteSläde(nr);
            return RedirectToAction("SlädeView");
        }

            public IActionResult InsertSläde(string nr, string namn)
        {
            SlädeModel slädeModel = new SlädeModel(_configuration);
            slädeModel.InsertSläde(nr, namn);
            return RedirectToAction("SlädeView");
        }
    }
}
