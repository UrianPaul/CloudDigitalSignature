using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using New_site.Models;
using New_site.Models;
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using New_site.Utils;
using System.Diagnostics;
using System.Threading;

namespace New_site.Controllers
{
    public class DefaultApiController : ApiController
    {
        private ApplicationSignInManager _signManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signManager ?? Request.GetOwinContext().GetUserManager<ApplicationSignInManager>();
            }
            private set
            {
                _signManager = value;
            }
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        [HttpGet]
        [Route("api/takeValues/{a}/{b}")]
        public int TakeMyValues(int a, int b)
        {
            //DigitalSignatureClient ws = new DigitalSignatureClient();
            //Int32 x = Convert.ToInt32(a);
            //Int32 y = Convert.ToInt32(b);
            //Int32 sum = ws.Add(x, y);
            Int32 sum = 2 + 3;

            return sum;
        }

        [HttpGet]
        [Route("test")]
        public void test()
        {

            var asd = new ApplicationDbContext();

            var dsa = new Model();

            asd.Models.Add(dsa);

            asd.SaveChanges();
        }

        [HttpGet]
        [Route("api/loginVerify")]
        public bool loginVerify(string email, string password)
        {
            var context = new ApplicationDbContext();

            var user = context.Users.FirstOrDefault(s => s.Email == email);

            bool logged = false;

            if (user != null)
            {
                logged = SignInManager.PasswordSignInAsync(user.UserName, password, false, false).Result == Microsoft.AspNet.Identity.Owin.SignInStatus.Success;
            }


            return logged;
        }

        [HttpGet]
        [Route("api/TestVerify")]
        public bool TestVerify()
        {
            return true;
        }

        [HttpGet]
        [Route("api/register")]
        public bool RegisterApi(string email, string password, string confirmPassword)
        {
            var errorMessage = string.Empty;

            Process make_alias = new Process();
            make_alias.StartInfo.FileName = "cmd.exe";
            make_alias.StartInfo.RedirectStandardInput = true;
            make_alias.StartInfo.RedirectStandardOutput = true;
            make_alias.StartInfo.CreateNoWindow = true;
            make_alias.StartInfo.UseShellExecute = false;
            make_alias.StartInfo.WorkingDirectory = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list";

            if (ModelState.IsValid && confirmPassword == password)
            {
                var user = new ApplicationUser() { UserName = email, Email = email, EmailConfirmed = true };
                var result = UserManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    string alias = "md " + email;
                    make_alias.Start();
                    make_alias.StandardInput.WriteLine(alias);
                    make_alias.StandardInput.Flush();
                    make_alias.StandardInput.Close();
                    make_alias.WaitForExit();
                    //return new ApiResponse(true, "Userul a fost creat", "The user was created", ApiResponseSeverity.Success, true, true);
                    return true;
                }

                if (result.Errors.Any())
                {
                    foreach (var item in result.Errors)
                    {
                        errorMessage += item + " ";
                    }
                }
            }
            else
            {
                errorMessage = "Password doesn't match!";
            }
            //return new ApiResponse(true, "Userul nu a fost creat", $"The user was not created: {errorMessage}", ApiResponseSeverity.Error, true, false);
            return false;
        }

        [HttpGet]
        [Route("api/generateCertificate")]
        //localhost/api/generateCertificate?countryShort=RO&country=Romania&locality=Bucharest&organisation=Informatics&department=Crypto&domain=myemailtest&email=myemail@gmail.com&password=paul1994
        public bool generateCertificate(string countryShort,string country,string locality,string organisation,string department,
                                        string domain, string email, string password)
        {
            string secretpasswd = "secretpassword";
            Generare_cheiePrivata(password, email);
            Thread.Sleep(5000);
            Generare_CSR(password, email, countryShort, country, locality, organisation, department, domain);
            Thread.Sleep(8000);
            Get_certificate(secretpasswd, email);
            Thread.Sleep(8000);

            return true;
        }

        public static void Generare_cheiePrivata(string passwd, string email)
        {
            Process cmd1 = new Process();

            cmd1.StartInfo.FileName = "cmd.exe";
            cmd1.StartInfo.RedirectStandardInput = true;
            cmd1.StartInfo.RedirectStandardOutput = true;
            cmd1.StartInfo.CreateNoWindow = true;
            cmd1.StartInfo.UseShellExecute = false;
            cmd1.StartInfo.WorkingDirectory = @"C:\usr\local\ssl\bin";

            cmd1.Start();
            string key_directory = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\" + email + @"\" + email + ".key.pem";
            string key_passwd = passwd;

            using (StreamWriter sw = cmd1.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("openssl genrsa -aes256 -passout pass:" + key_passwd + " -out " + key_directory + " 2048");
                }

            }

            cmd1.Close();

        }

        public static void Generare_CSR(string passwd, string email, string country_name, string province_name, string locality_name, string org_name, string unit_name, string common_name)
        {
            Process cmd2 = new Process();

            cmd2.StartInfo.FileName = "cmd.exe";
            cmd2.StartInfo.RedirectStandardInput = true;
            cmd2.StartInfo.RedirectStandardOutput = true;
            cmd2.StartInfo.CreateNoWindow = true;
            cmd2.StartInfo.UseShellExecute = false;
            cmd2.StartInfo.WorkingDirectory = @"C:\usr\local\ssl\bin";

            string cfg_location = @"C:\root\ca\intermediate\openssl.cnf";
            string key_location = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\" + email + @"\" + email + ".key.pem";
            string csr_location = @"C:\root\ca\intermediate\csr\" + email + ".csr.pem";
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

        public static void Get_certificate(string passwd, string email)
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
            string csr_location = @"C:\root\ca\intermediate\csr\" + email + ".csr.pem";
            string cert_location = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list\" + email + @"\" + email + ".cert.crt";
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

            //Console.WriteLine("We're done! You got the certificate!");
            cmd3.Close();
        }


    }
}
