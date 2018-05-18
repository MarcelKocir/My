using Impulsovi;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace WindowsFormsApplication
{
    public partial class FormBrowser : Form
    {
        public FormBrowser()
        {
            InitializeComponent();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            NavigateDirectly("http://impuls.mopa.cz/form2/index.php");
        }

        private void bCaptureImage_Click(object sender, EventArgs e)
        {
            CaptureImage();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            lInfo.Text = "Started!!!";
            new Starter().Start(this.Navigate, this.CaptureImage, webBrowser);           
        }

        private string NavigateDirectly(string p_UrlRequest)
        {
            return Navigate(p_UrlRequest, null).Item1;
        }

        /// <summary>
        /// Navigate url
        /// </summary>
        /// <param name="p_UrlRequest"></param>
        /// <returns></returns>
        public Tuple<string, string> Navigate(string p_UrlRequest, string p_Referer)
        {            
            return Navigate(this, p_UrlRequest, p_Referer);
        }

        private delegate Tuple<string, string> DispatchNavigateHandler(FormBrowser formBrowser, string p_UrlRequest, string p_Referer);
        private Tuple<string, string> Navigate(FormBrowser formBrowser, string p_UrlRequest, string p_Referer)
        {
            if (formBrowser.InvokeRequired)
            {
                DispatchNavigateHandler handler = new DispatchNavigateHandler(NavigateProcessing);
                return (Tuple<string, string>)this.Invoke(handler, formBrowser, p_UrlRequest, p_Referer);
            }
            else
            {
                return NavigateProcessing(formBrowser, p_UrlRequest, p_Referer);
            }
        }

        public Tuple<string, string> NavigateProcessing(FormBrowser formBrowser, string p_UrlRequest, string p_Referer)
        {
            if (!string.IsNullOrEmpty(p_Referer))
            {
                lInfo.Text = p_UrlRequest;
                formBrowser.webBrowser.Navigate(p_UrlRequest, "_self", null, "Referer: " + p_Referer);
            }
            else
            {
                formBrowser.webBrowser.Navigate(p_UrlRequest);
            }

            while (formBrowser.webBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();

            return new Tuple<string, string>(formBrowser.webBrowser.DocumentText, formBrowser.webBrowser.Document.Cookie);
        }

        public string CaptureImage()
        {
            return CaptureImage(this);
        }

        private delegate string DispatchCaptureImageHandler(FormBrowser formBrowser);
        private string CaptureImage(FormBrowser formBrowser)
        {
            if (formBrowser.InvokeRequired)
            {
                DispatchCaptureImageHandler handler = new DispatchCaptureImageHandler(CaptureImageProcessing);
                return this.Invoke(handler, formBrowser).ToString();
            }
            else
            {
                return CaptureImageProcessing(formBrowser);
            }
        }

        /// <summary>
        /// Ulozeni obrazku
        /// </summary>
        /// <returns></returns>
        private string CaptureImageProcessing(FormBrowser formBrowser)
        {
            string result = null;
            IHTMLDocument2 doc = (IHTMLDocument2)formBrowser.webBrowser.Document.DomDocument;
            IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();

            foreach (IHTMLImgElement img in doc.images)
            {
                imgRange.add((IHTMLControlElement)img);

                imgRange.execCommand("Copy", false, null);

                using (Bitmap bmp = (Bitmap)Clipboard.GetDataObject().GetData(DataFormats.Bitmap))
                {
                    result = Environment.CurrentDirectory + "\\captcha.bmp";
                    bmp.Save(result);
                }

            }

            return result;
        }

    }
}
