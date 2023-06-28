using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
namespace fidder
{
    public partial class Form2 : Form
    {   
        public Form2()
        {
            InitializeComponent();
        }

        static string tmp = "-------------------------------------";

        private void btn_start_Click(object sender, EventArgs e)
        {
            startFiddler();
        }

        public void startFiddler()
        {
            if (!FiddlerApplication.IsStarted())
            {
                FiddlerApplication.AfterSessionComplete += FiddlerApplication_AfterSessionComplete;
                FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
                FiddlerApplication.Startup(8888, true, true, true);
            }
            else
            {
                appendtext("fiddler is running...");
            }
        }


        private void FiddlerApplication_BeforeRequest(Session oSession)
        {
            if (oSession.RequestMethod == "POST" || oSession.RequestMethod == "GET")
            {

                string url = oSession.fullUrl;
                string body = oSession.GetRequestBodyAsString();
                string header = oSession.RequestHeaders.ToString();
                appendtext(url);
                appendtext(header);
                appendtext(body);

            }
        }

        private void FiddlerApplication_AfterSessionComplete(Session oSession)
        {
            if (oSession.RequestMethod == "POST" || oSession.RequestMethod == "GET")
            {
                string body = oSession.GetResponseBodyAsString();
                appendtext(body);
                appendtext(tmp);
            }

        }

        public void stopFiddler()
        {
            if (FiddlerApplication.IsStarted())
            {
                FiddlerApplication.Shutdown();

            }
        }

        public void install()
        {
            if (!CertMaker.rootCertExists())
            {
                CertMaker.createRootCert();
                CertMaker.trustRootCert();
            }
        }

        public void remove()
        {
            if (CertMaker.rootCertExists())
            {
                CertMaker.removeFiddlerGeneratedCerts();
            }
        }

        public void appendtext(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(value + "\r\n");
                }));
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            stopFiddler();
        }

        private void btn_install_Click(object sender, EventArgs e)
        {
            install();
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            remove();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {



            stopFiddler();
            if (FiddlerApplication.oProxy != null)
            {
                if (FiddlerApplication.oProxy.IsAttached)
                    FiddlerApplication.oProxy.Detach();
            }
        }

        private void btn_stop_Click_1(object sender, EventArgs e)
        {

        }
    }
}