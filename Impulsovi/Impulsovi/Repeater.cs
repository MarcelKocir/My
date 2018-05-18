using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using DeathByCaptcha;

namespace Impulsovi
{
    class Repeater
    {
        /// <summary>
        /// Cookie kontejner
        /// </summary>
        private CookieContainer _CookieJar;

        /// <summary>
        /// Pattern pro kompletni parametry dotazu
        /// </summary>
        private string _FullParamsPattern;

        /// <summary>
        /// Hlavni url pro registraci
        /// </summary>
        private const string _UrlRequest = "http://impuls.mopa.cz/form2/index.php";

        /// <summary>
        /// Metoda navigate do webbrowseru
        /// </summary>
        private Func<string, string, Tuple<string, string>> _NavigateMethod;

        /// <summary>
        /// Metoda ulozeni obrazku captcha na disk
        /// </summary>
        private Func<string> _CaptureImageMethod;

        /// <summary>
        /// Klient do DeathByCaptcha s credentials
        /// </summary>
        private Client _DeathByCaptchaClient = (Client)new SocketClient("koka333", "kokinka");

        /// <summary>
        /// WebBrowser
        /// </summary>
        private System.Windows.Forms.WebBrowser _WebBrowser;   

        public Repeater(string p_FullParamsPattern, Func<string, string, Tuple<string, string>> p_NavigateMethod, Func<string> p_CaptureImageMethod, 
            System.Windows.Forms.WebBrowser p_WebBrowser)
        {
            _FullParamsPattern = p_FullParamsPattern;
            _CookieJar = new CookieContainer();
            _NavigateMethod = p_NavigateMethod;
            _CaptureImageMethod = p_CaptureImageMethod;
            _WebBrowser = p_WebBrowser;
        }

        /// <summary>
        /// Hlavni vstupni metoda - registrace
        /// </summary>
        public bool DoRegistration(StringBuilder p_StringBuilder, ref int p_ErrCounter)
        {
            try
            {
                //NACTENI STRANKY
                Tuple<string, string> refreshResult = _NavigateMethod(_UrlRequest, System.Configuration.ConfigurationSettings.AppSettings.Get("BaseUrl"));
                string code = refreshResult.Item1;
                string cookie = refreshResult.Item2;

                //ADD COOKIE
                var cookies = cookie.Split(';');
                foreach(var ci in cookies)
                {
                    var cookieHelper = ci.Split('=');
                    _CookieJar.Add(new Cookie(cookieHelper[0], cookieHelper[1], "/", "impuls.mopa.cz"));
                }
                
                //NACTENI CAPTCHA
                Captcha captcha = GetCaptcha();
                string captchaText = captcha.Text;

                //NACTENI SKRYTYCH PARAMTERU ZE STRANKY
                string spmchk = GetSpmchkParam(code);
                string tel = GetTelParam(code);

                //SAMOTNA REGISTRACE
                string respond = Register(spmchk, tel, captchaText);

                //_WebBrowser.DocumentText = respond;
                p_StringBuilder.Append($"{ DateTime.Now}:");
                p_StringBuilder.AppendLine(_FullParamsPattern);

                if (respond.Contains("kód z obrázku"))
                {
                    p_StringBuilder.AppendLine("BAD CAPTCHA!");
                    //SPATNE ROZKODOVANY KOD CAPTCHA -> REKLAMACE
                    _DeathByCaptchaClient.Report(captcha);
                    return false;
                }
                else if (respond.Contains("Registrace do hry byla úspěšně provedena"))
                {
                    p_StringBuilder.AppendLine("REGISTERED SUCCESSFULLY");
                }
                else if (respond.Contains(" pouze jednou za pět minut"))
                {
                    p_StringBuilder.AppendLine("REGISTRACE PŘED UPLYNUTÍM 5-TI MINUT");
                }
                else
                {
                    p_StringBuilder.AppendLine("UNKNOWN RESPOND:");
                    p_StringBuilder.AppendLine(respond.ToString());
                    p_ErrCounter++;
                }
            }
            catch (System.Exception ex)
            {
                p_StringBuilder.AppendLine("EXCEPTION:");
                p_StringBuilder.AppendLine(ex.ToString());
                p_ErrCounter++;
            }
            p_StringBuilder.AppendLine();
                
            return true;
        }

        /// <summary>
        /// Inicializacni nacteni (TimeTable)
        /// </summary>
        /// <returns></returns>
        private string TimeTableRequest()
        {
            var encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(ParamPatternsBI.TimeTableRequestParams);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_UrlRequest);
            request.Method = "POST";
            request.CookieContainer = _CookieJar;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = data.Length;

            var sp = request.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);

            request.KeepAlive = true;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
            request.Referer = "http://www.impuls.cz/souteze/hra-halo-tady-impulsovi-se-vraci-novym-podzimni-kolem/";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "cs,en-US;q=0.7,en;q=0.3");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            WebResponse response = request.GetResponse();
            String result;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            response.Close();

            return result;
        }

        [Obsolete]
        /// <summary>
        /// Nacteni hlavni stranky pro ziskani skrytych parametru
        /// </summary>
        /// <returns></returns>
        private String GetCode()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_UrlRequest);
            request.Method = "GET";
            request.CookieContainer = _CookieJar;

            var sp = request.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);

            request.KeepAlive = true;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "cs,en-US;q=0.7,en;q=0.3");

            WebResponse response = request.GetResponse();
            String result;
            using (var sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }            
            response.Close();


            return result;
        }

        /// <summary>
        /// Registrace
        /// </summary>
        /// <param name="p_Spmchk"></param>
        /// <param name="p_Tel"></param>
        /// <param name="p_CaptchaText"></param>
        /// <returns></returns>
        private string Register(string p_Spmchk, string p_Tel, string p_CaptchaText)
        {
            var encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(GetPostParams(p_Spmchk, p_Tel, p_CaptchaText));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_UrlRequest);
            request.Method = "POST";
            request.CookieContainer = _CookieJar;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;


            var sp = request.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);

            request.KeepAlive = true;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
            request.Referer = "http://impuls.mopa.cz/form2/index.php";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "cs,en-US;q=0.7,en;q=0.3");
            request.ServicePoint.Expect100Continue = false;


            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            WebResponse response = request.GetResponse();
            String result;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            response.Close();

            return result;
        }

        /// <summary>
        /// Nacteni captcha
        /// </summary>
        /// <returns></returns>
        private Captcha GetCaptcha()
        {
            Captcha result = null;

            string fileName = _CaptureImageMethod();

            // Put your CAPTCHA file name, stream, or vector of bytes,
            // and desired timeout (in seconds) here:
            Captcha captcha = _DeathByCaptchaClient.Decode(fileName, 30);
            if (captcha.Solved && captcha.Correct)
            {
                result = captcha;
            }

            return result;
        }

        /// <summary>
        /// Ziskani paramteru Spmchk
        /// </summary>
        /// <param name="p_Code"></param>
        /// <returns></returns>
        private string GetSpmchkParam(string p_Code)
        {
            int indexStart = p_Code.IndexOf(@"<input type=""hidden"" value=");
            int indexEnd = p_Code.IndexOf(@" name=""spmchk"" />");
            string result = p_Code.Substring(indexStart, indexEnd - indexStart);
            result = result.Replace(@"<input type=""hidden"" value=", String.Empty);
            result = result.Replace(@"""", String.Empty);

            return result;
        }

        /// <summary>
        /// Ziskani paramteru Tel
        /// </summary>
        /// <param name="p_Code"></param>
        /// <returns></returns>
        private string GetTelParam (string p_Code)
        {
            int indexStart = p_Code.IndexOf(@"<input type=""hidden"" value=""1"" name=");
            int indexEnd = p_Code.IndexOf(@" />", indexStart);
            string result = p_Code.Substring(indexStart, indexEnd - indexStart);
            result = result.Replace(@"<input type=""hidden"" value=""1"" name=", String.Empty);
            result = result.Replace(@"""", String.Empty);

            return result;
        }

        /// <summary>
        /// Konecny format vsech parametru
        /// </summary>
        /// <param name="p_Spmchk"></param>
        /// <param name="p_Tel"></param>
        /// <returns></returns>
        private string GetPostParams(string p_Spmchk, string p_Tel, string p_CaptchaText)
        {
            string result = String.Format(_FullParamsPattern, p_Spmchk, p_Tel, p_CaptchaText);            

            return result;
        }


    }
}
