using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BEClient
{
    class ServerThread
    {
        public Configurator Config;
        // Stop-Flag
        public bool stop = false;
        // Flag für "Thread läuft"
        public bool running = false;
        // Die Verbindung zum Client
        private TcpClient connection = null;
        // Speichert die Verbindung zum Client und startet den Thread
        public ServerThread(TcpClient connection, Configurator Config)
        {
            this.Config = Config;
            // Speichert die Verbindung zu Client,
            // um sie später schließen zu können
            this.connection = connection;
            // Initialisiert und startet den Thread
            new Thread(new ThreadStart(Run)).Start();
        }

        public void Run()
        {
            // Setze Flag für "Thread läuft"
            this.running = true;
            // Hole den Stream für's schreiben
            Stream networkStream = this.connection.GetStream();
            byte[] bytesFrom = new byte[connection.ReceiveBufferSize];
            networkStream.Read(bytesFrom, 0, connection.ReceiveBufferSize);
            string ServerResponse;
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

            if (dataFromClient == "restart")
            {
                ServerResponse = dataFromClient + "$";
                System.Diagnostics.Process.Start("shutdown.exe", "-f -r -t 0");
            }
            else if (dataFromClient.Contains("chrome"))
            {
                ServerResponse = HandleWebSite(dataFromClient);
            }
            else if (dataFromClient=="status")
            {
                ServerResponse = HandleStatus(dataFromClient);
            }
            else if(dataFromClient.Contains("config"))
            {
                ServerResponse = HandleConfig(dataFromClient);
            }


            // Schließe die Verbindung zum Client
            this.connection.Close();
            // Setze das Flag "Thread läuft" zurück
            this.running = false;
        }

        private string HandleConfig(string dataFromClient)
        {
            string ServerResponse = null;

            string[] parameters = dataFromClient.Split(';');

            Config = new Configurator(parameters[1], parameters[2], parameters[3], bool.Parse(parameters[4]), bool.Parse(parameters[5]), bool.Parse(parameters[6]), bool.Parse(parameters[7]));
            Config.WriteConfigFile();

            ServerResponse = dataFromClient + "$";

            return ServerResponse;
        }

        private string HandleStatus(string dataFromClient)
        {
            string ServerResponse = null;

            try
            {
                List<string> TabList = ChromeHelper.GetAllWindows().Select(ChromeHelper.GetTitle).Where(x => x.Contains("Google Chrome")).ToList();
                ServerResponse = TabList[0].Substring(0, TabList[0].IndexOf("-"));
            }
            catch (Exception)
            {

                ServerResponse = "closed";
            }


            return ServerResponse;
        }

        private string HandleWebSite(string dataFromClient)
        {
            string ServerResponse = null;

            Process[] localByName = Process.GetProcessesByName("chrome");
            for (int i = 0; i < localByName.Length; i++)
            {
                localByName[i].Kill();
            }

            string param = "";
            if (Config.Kiosk) param = param + " --kiosk";
            if (Config.Incognito) param = param + " --incognito";
            if (Config.Print) param = param + " --kiosk-printing";

            Process.Start(@"chrome.exe", @param + " " + @dataFromClient);

            ServerResponse = dataFromClient + "$";
            return ServerResponse;
        }
    }


}
