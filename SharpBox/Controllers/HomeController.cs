using System;
using System.Web.Mvc;
using DropNet;
using SharpBox.Models;

namespace SharpBox.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET /

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to SharbBox!";
            ViewBag.OK = Configuration.Load();

            return View();
        }

        //
        // GET /Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST /Create

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ConnexionData cnx)
        {
            if (ModelState.IsValid)
            {
                string guid = Guid.NewGuid().ToString();
                Session[guid + "_cnx"] = cnx;

                var apidb = new DropNetClient(cnx.ApiKey, cnx.ApiSecret);
                apidb.UseSandbox = true;

                var token = apidb.GetToken();
                Session[guid + "_token"] = token;

                var url = apidb.BuildAuthorizeUrl(Url.Action("Confirm", "Home", new { id = guid }, "http"));
                return Redirect(url);
            }

            return View();
        }

        //
        // GET /Confirm

        public ActionResult Confirm(string id)
        {
            var guid = id;

            if (Session[guid + "_cnx"] != null)
            {
                var cnx = (ConnexionData)Session[guid + "_cnx"];
                var apidb = new DropNetClient(cnx.ApiKey, cnx.ApiSecret);
                apidb.UseSandbox = true;

                apidb.UserLogin = Session[guid + "_token"] as DropNet.Models.UserLogin;
                var token = apidb.GetAccessToken();

                cnx.UserToken = token.Token;
                cnx.UserSecret = token.Secret;

                Configuration.ConnexionData = cnx;
                Configuration.Save();

                return RedirectToAction("Index");
            }

            return View();
        }

        //
        // GET /About

        public ActionResult About()
        {
            return View();
        }
    }
}
