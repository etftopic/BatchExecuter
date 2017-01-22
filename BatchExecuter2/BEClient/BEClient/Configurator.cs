using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BEClient
{
    public class Configurator
    {
        private string _name;
        private string _ip;
        private string _idle;
        private bool _print;
        private bool _x86;
        private bool _kiosk;
        private bool _incognito;


        public Configurator (string Name, string Ip, string Idle, bool Print, bool x86, bool Kiosk, bool Incognito)
        {
            this._name = Name;
            this._ip = Ip;
            this._idle = Idle;
            this._print = Print;
            this._x86 = x86;
            this._kiosk = Kiosk;
            this._incognito = Incognito;
        }

        public void ReadConfigFile()
        {
            XmlDocument doc = new XmlDocument();
            string path = @"BEClient.exe.config";
            doc.Load(path);

            IEnumerator ie = doc.SelectNodes("/configuration/appSettings/add").GetEnumerator();

            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == "name")
                {
                    this._name = (ie.Current as XmlNode).Attributes["value"].Value;
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "ip")
                {
                    this._ip = (ie.Current as XmlNode).Attributes["value"].Value;
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "idle")
                {
                    this._idle = (ie.Current as XmlNode).Attributes["value"].Value;
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "print")
                {
                    this._print = bool.Parse((ie.Current as XmlNode).Attributes["value"].Value);
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "x86")
                {
                    this._x86 = bool.Parse((ie.Current as XmlNode).Attributes["value"].Value);
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "kiosk")
                {
                    this._kiosk = bool.Parse((ie.Current as XmlNode).Attributes["value"].Value);
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "incognito")
                {
                    this._incognito = bool.Parse((ie.Current as XmlNode).Attributes["value"].Value);
                }

            }

            
        }

        public void WriteConfigFile()
        {
            XmlDocument doc = new XmlDocument();
            string path = @"BEClient.exe.config";
            doc.Load(path);

            IEnumerator ie = doc.SelectNodes("/configuration/appSettings/add").GetEnumerator();

            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == "name")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._name;
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "ip")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._ip;
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "idle")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._idle;
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "print")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._print.ToString();
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "x86")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._x86.ToString();
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "kiosk")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._kiosk.ToString();
                }
                else if ((ie.Current as XmlNode).Attributes["key"].Value == "incognito")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = this._incognito.ToString();
                }

            }

            doc.Save(path);
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Ip
        {
            get
            {
                return _ip;
            }

            set
            {
                _ip = value;
            }
        }

        public string Idle
        {
            get
            {
                return _idle;
            }

            set
            {
                _idle = value;
            }
        }

        public bool Print
        {
            get
            {
                return _print;
            }

            set
            {
                _print = value;
            }
        }

        public bool x86
        {
            get
            {
                return _x86;
            }

            set
            {
                _x86 = value;
            }
        }

        public bool Kiosk
        {
            get
            {
                return _kiosk;
            }

            set
            {
                _kiosk = value;
            }
        }

        public bool Incognito
        {
            get
            {
                return _incognito;
            }

            set
            {
                _incognito = value;
            }
        }
    }
}
