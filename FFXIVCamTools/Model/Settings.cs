using System;
using System.Xml.Linq;

namespace FFXIVCamTools.Model
{
    class Settings
    {
        public int StructureAddress { get; set; }
        public int ZoomCurrent { get; set; }
        public int ZoomMax { get; set; }
        public int FovCurrent { get; set; }
        public int FovMax { get; set; }
        public string LastUpdate { get; set; }
        public float DesiredZoom { get; set; }
        public float DesiredFov { get; set; }

        public Settings() { }

        public static Settings Load(string location)
        {
            Settings settings = new Settings();

            try
            {
                XDocument xDocument = XDocument.Load(location);
                XElement xElement = xDocument.Element("Root");

                foreach (var element in xElement.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "Offset":
                            settings.StructureAddress = ConvertStringToHex(element.Element("StructureAddress").Value);
                            settings.ZoomCurrent = ConvertStringToHex(element.Element("ZoomCurrent").Value);
                            settings.ZoomMax = ConvertStringToHex(element.Element("ZoomMax").Value);
                            settings.FovCurrent = ConvertStringToHex(element.Element("FovCurrent").Value);
                            settings.FovMax = ConvertStringToHex(element.Element("FovMax").Value);
                            break;

                        case "Config":
                            settings.LastUpdate = element.Element("LastUpdate").Value;
                            settings.DesiredZoom = float.Parse(element.Element("DesiredZoom").Value);
                            settings.DesiredFov = float.Parse(element.Element("DesiredFov").Value);
                            break;
                    }
                }
            }
            catch
            {
                return null;
            }

            return settings;
        }

        public bool Save(string location)
        {
            try
            {
                XDocument xDocument = XDocument.Load(location);
                XElement xElement = xDocument.Element("Root");

                foreach (var element in xElement.Elements())
                {
                    switch (element.Name.LocalName)
                    {
                        case "Offset":
                            element.Element("StructureAddress").Value = StructureAddress.ToString("X");
                            element.Element("ZoomCurrent").Value = ZoomCurrent.ToString("X");
                            element.Element("ZoomMax").Value = ZoomMax.ToString("X");
                            element.Element("FovCurrent").Value = FovCurrent.ToString("X");
                            element.Element("FovMax").Value = FovMax.ToString("X");
                            break;

                        case "Config":
                            element.Element("LastUpdate").Value = LastUpdate;
                            element.Element("DesiredZoom").Value = DesiredZoom.ToString();
                            element.Element("DesiredFov").Value = DesiredFov.ToString();
                            break;
                    }
                }

                xElement.Save(location);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static int ConvertStringToHex(string s)
        {
            int hex;

            foreach (char c in s.ToCharArray())
            {
                if (!Uri.IsHexDigit(c))
                    return -1;
            }

            hex = int.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);

            return hex;
        }
    }
}
