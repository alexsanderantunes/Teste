using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TesteWeb.Models;

namespace TesteWeb.Controllers
{
    public class HomeController : Controller
    {
        private AccessAPI api = new AccessAPI();
        public ActionResult Index()
        {
            return View(api.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante  visitante = api.Get((int)id);
            if (visitante == null)
            {
                return HttpNotFound();
            }
            return View(visitante);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Mensagem,Endereco")]Visitante visitante)
        {
            Visitante retorno = null;
            try
            {
                if (ModelState.IsValid)
                {
                    retorno = api.Set(visitante);
                }
            }
            catch (Exception /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Aplicação de Teste MVC 5 com chamadas a um Web API que usa EF6.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Alexsander Antunes (alexsander.antunes.1974@gmail.com)";

            return View();
        }
    }

    public class AccessAPI
    {
        private string pathValue = "";
        private string googleKey = "";
        public AccessAPI()
        {
            pathValue = ConfigurationManager.AppSettings["urlAPI"];
            googleKey = ConfigurationManager.AppSettings["GoogleMapsKey"];
        }
        

        public List<Visitante> List()
        {
            return List(pathValue + "Visitantes/");
        }

        public Visitante Get(int id)
        {
            return Get(pathValue + "Visitantes/" + id.ToString());
        }

        public Visitante Set(Visitante visitante)
        {
            return Set(pathValue + "Visitantes/", visitante);
        }

        private Visitante Set(string requestUrl, Visitante visitante)
        {
            try
            {
                visitante.Localizacao = GetLocation(visitante.Endereco);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(visitante);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var jsonData = streamReader.ReadToEnd();
                    var responseObject = serializer.Deserialize<Visitante>(jsonData);
                    return responseObject;
                }
            }
            catch (Exception e)
            {
                // catch exception and log it
                throw new Exception("Erro ao acessar ao servidor API", e);
            }
        }

        private Visitante Get(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var jsonData = reader.ReadToEnd();
                        var responseObject = serializer.Deserialize<Visitante>(jsonData);
                        return responseObject;
                    }
 
                }
            }
            catch (Exception e)
            {
                // catch exception and log it
                throw new Exception("Erro ao acessar ao servidor API", e);
            }
        }
        private List<Visitante> List(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var jsonData = reader.ReadToEnd();
                        var responseObject = serializer.Deserialize<List<Visitante>>(jsonData);
                        return responseObject;
                    }

                }
            }
            catch (Exception e)
            {
                // catch exception and log it
                throw new Exception("Erro ao acessar ao servidor API", e);
            }

        }
        private string GetLocation(string endereco)
        {
            string path = "https://maps.googleapis.com/maps/api/geocode/json?address=" + HttpUtility.UrlEncode(endereco) + "&key=" + googleKey;
            return GetGoogleMapsLocation(path);
        }
        private string GetGoogleMapsLocation(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var jsonData = reader.ReadToEnd();
                        return jsonData;
                    }

                }
            }
            catch (Exception e)
            {
                // catch exception and log it
                throw new Exception("Erro ao acessar ao servidor API", e);
            }

        }
    }
}