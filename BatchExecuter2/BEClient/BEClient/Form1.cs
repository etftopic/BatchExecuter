using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Collections;

namespace BEClient
{
    public partial class Form1 : Form
    {
        private static ArrayList threads = new ArrayList();
        private static TcpListener listener = null;
        public static Configurator Config = new Configurator("Alien", "blabla", "www.orf.at", true, false, false, false);

        public Form1()
        {
            InitializeComponent();
            Config.ReadConfigFile();
            listener = new TcpListener(4711);
            listener.Start();
            Thread MainThread = new Thread(new ThreadStart(MainThreadMethod));
            MainThread.Start();
        }

        public static void MainThreadMethod()
        {
            while (true)
            {
                // Wartet auf eingehenden Verbindungswunsch
                TcpClient c = listener.AcceptTcpClient();
                // Initialisiert und startet einen Server-Thread
                // und fügt ihn zur Liste der Server-Threads hinzu
                threads.Add(new ServerThread(c, Config));
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            IEnumerator threadenum;

            for (threadenum = threads.GetEnumerator(); threadenum.MoveNext();)
            {
                // Nächsten Server-Thread holen
                ServerThread st = (ServerThread)threadenum.Current;
                // und stoppen
                st.stop = true;
                while (st.running)
                    Thread.Sleep(1000);
            }

            listener.Stop();
        }




    }
}
