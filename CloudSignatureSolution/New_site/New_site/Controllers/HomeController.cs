using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using New_site.Models;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using certInstDLL;
using System.Security.Cryptography.X509Certificates;

namespace New_site.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //instalare certificat.

        public ActionResult Instalare_certificat()
        {
            var userId = User.Identity.GetUserId();

            var existingModel = db.Models.FirstOrDefault(s => s.UserID == userId);
            var user = db.Users.FirstOrDefault(s => s.Id == userId);

            if (existingModel == null || existingModel.Domeniu == null)
            {
                return RedirectToAction("Create", "UserDetails");
            }

            string email = user.Email;
            certInstall(email);
            Thread.Sleep(3000);

            string crtSubj = email;
            string keyName = email + ".key.pem";
            string keystProv = "SampleKSP";

            if(CertificateUtils.InstallCertificate(crtSubj, keyName, keystProv))
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Create", "UserDetails");
        }

       

        public ActionResult Generare_certificat()
        {
            var userId = User.Identity.GetUserId();

            var existingModel = db.Models.FirstOrDefault(s => s.UserID == userId);
            var user = db.Users.FirstOrDefault(s => s.Id == userId);

            if (existingModel == null || existingModel.Domeniu==null)
            {
                return RedirectToAction("Create", "UserDetails");
            }

            //generam cheia privata pe baza parolei introduse in tabel
            string passwd = existingModel.Private_password;
            string email = user.Email;
            string secretpasswd = "secretpassword";
            Generare_cheiePrivata(passwd,email);
            Thread.Sleep(3000);
            Generare_CSR(passwd, email, existingModel.Tara_forma_scurta, existingModel.Tara, existingModel.Localitate, existingModel.Organizatie, existingModel.Departament, existingModel.Domeniu);
            Thread.Sleep(3000);
            Get_certificate(secretpasswd, email);
            Thread.Sleep(3000);


            return RedirectToAction("Index", "Home");
        }
       
        public void certInstall(string email)
        {
            string cert_location = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\" + email + @"\" + email + ".cert.crt";
            X509Certificate2 certificate = new X509Certificate2(cert_location);
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();

        }

        public static void Generare_cheiePrivata(string passwd,string email)
        {
            Process cmd1 = new Process();

            cmd1.StartInfo.FileName = "cmd.exe";
            cmd1.StartInfo.RedirectStandardInput = true;
            cmd1.StartInfo.RedirectStandardOutput = true;
            cmd1.StartInfo.CreateNoWindow = true;
            cmd1.StartInfo.UseShellExecute = false;
            cmd1.StartInfo.WorkingDirectory = @"C:\usr\local\ssl\bin";

            cmd1.Start();
            string key_directory = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\"+email+@"\"+email+ ".key.pem";
            string key_passwd = passwd;

            using (StreamWriter sw = cmd1.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("openssl genrsa -aes256 -passout pass:" + key_passwd + " -out "+key_directory+ " 2048");
                }
                
            }

            cmd1.Close();

        }

        public static void  Generare_CSR(string passwd,string email,string country_name,string province_name,string locality_name,string org_name,string unit_name,string common_name)
        {
            Process cmd2 = new Process();

            cmd2.StartInfo.FileName = "cmd.exe";
            cmd2.StartInfo.RedirectStandardInput = true;
            cmd2.StartInfo.RedirectStandardOutput = true;
            cmd2.StartInfo.CreateNoWindow = true;
            cmd2.StartInfo.UseShellExecute = false;
            cmd2.StartInfo.WorkingDirectory = @"C:\usr\local\ssl\bin";

            string cfg_location = @"C:\root\ca\intermediate\openssl.cnf";
            string key_location = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\"+email+@"\"+email+ ".key.pem";
            string csr_location = @"C:\root\ca\intermediate\csr\"+email+".csr.pem";
            string secret_passwd = passwd;
            string csr_cmd = "openssl req -config " + cfg_location + " -key " + key_location + " -new -sha256 -out " + csr_location + " -passin pass:" + secret_passwd;
            cmd2.Start();

            using (StreamWriter sw = cmd2.StandardInput)
            {

                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(csr_cmd);

                    //completare detalii
          
                    sw.WriteLine(country_name);
                    sw.WriteLine(province_name);
                    sw.WriteLine(locality_name);
                    sw.WriteLine(org_name);
                    sw.WriteLine(unit_name);
                    sw.WriteLine(common_name);
                    sw.WriteLine(email);

                }

            }

            cmd2.Close();
        }

        public static void Get_certificate(string passwd,string email)
        {
            Process cmd3 = new Process();

            cmd3.StartInfo.FileName = "cmd.exe";
            cmd3.StartInfo.RedirectStandardInput = true;
            cmd3.StartInfo.RedirectStandardOutput = true;
            cmd3.StartInfo.CreateNoWindow = true;
            cmd3.StartInfo.UseShellExecute = false;
            cmd3.StartInfo.WorkingDirectory = @"C:\usr\local\ssl\bin";

            string interm_passwd = "secretpassword";
            string cfg_location = @"C:\root\ca\intermediate\openssl.cnf";
            string csr_location = @"C:\root\ca\intermediate\csr\"+email+".csr.pem";
            string cert_location = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\"+email+@"\"+email+".cert.crt";
            string cert_cmd = "openssl ca -config " + cfg_location + " -extensions usr_cert -days 375 -notext -md sha256 -in " + csr_location + " -out " + cert_location + " -passin pass:" + interm_passwd;

            cmd3.Start();

            using (StreamWriter sw = cmd3.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(cert_cmd);
                    sw.WriteLine('y');
                    sw.WriteLine('y');
                }


            }

            Console.WriteLine("We're done! You got the certificate!");
            cmd3.Close();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}