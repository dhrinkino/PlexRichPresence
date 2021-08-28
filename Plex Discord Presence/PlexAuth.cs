using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

namespace Plex_Discord_Presence
{
    class PlexAuth
    {

        public static string Client_Identifier = null; // this is uniq ID of your application, you can write pretty much any number 
        public static string X_Plex_Product = null; // Name of your application, also you will see this name in your plex account
        private static string code = null;
        private static string pin = null;
        private static string token = null;

        public bool LoadPin(string filename)
        {
            try
            {
                string[] credentials = File.ReadAllLines(filename);
                code = credentials[0];
                pin = credentials[1];
                token = credentials[2];
            }
            catch
            {
                return false;
            }

            return true;
        }
        private void genUrl(int countdown)
        {
            // open web browser
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://app.plex.tv/auth#?clientID=" + Client_Identifier + "&code=" + code + "&context%5Bdevice%5D%5Bproduct%5D=" + X_Plex_Product + "",
                UseShellExecute = true
            });
            Thread.Sleep(countdown * 1000);
        }

        public void Set(string id, string name)
        {
            Client_Identifier = id;
            X_Plex_Product = name;
        }

        public bool SavePin(string filename)
        {
            try
            {
                File.WriteAllLines(filename, new string[3] { code, pin, token });
            }
            catch
            {
                return false;
            }
            return true;
        }
        public string Token()
        {
            string tkn;
            string response = null;
            string url = null;
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.Headers["accept"] = "application/json";
                    url = "https://plex.tv/api/v2/pins/" + pin + "/?code=" + code + "&X-Plex-Client-Identifier=" + Client_Identifier;
                    response = webclient.DownloadString("https://plex.tv/api/v2/pins/" + pin + "/?code=" + code + "&X-Plex-Client-Identifier=" + Client_Identifier);
                }
            }
            catch
            {
                return null;
            }
            dynamic obj_plex_pin = JsonConvert.DeserializeObject(response);
            tkn = obj_plex_pin["authToken"];
            token = tkn;
            return token;
        }

        public void Generate()
        {
            string response = null;
            using (WebClient webclient = new WebClient())
            {
                var data = new System.Collections.Specialized.NameValueCollection();
                data.Add("strong", "true");
                data.Add("X-Plex-Product", X_Plex_Product);
                data.Add("X-Plex-Client-Identifier", Client_Identifier);
                webclient.Headers["accept"] = "application/json";
                byte[] resp_bytes = webclient.UploadValues("https://plex.tv/api/v2/pins/", "POST", data);
                response = Encoding.UTF8.GetString(resp_bytes);
            }
            dynamic obj_plex_pin = JsonConvert.DeserializeObject(response);
            code = obj_plex_pin["code"];
            pin = obj_plex_pin["id"];
            genUrl(15);
        }
        public string CurrentToken()
        {
            return token;
        }
        public bool ValidToken()
        {
            string response;
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.Headers["accept"] = "application/json";
                    string url = "https://plex.tv/api/v2/user?X-Plex-Product=" + X_Plex_Product + "&X-Plex-Client-Identifier=" + Client_Identifier + "&X-Plex-Token=" + token;
                    response = webclient.DownloadString(url);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetServer()
        {
            dynamic obj_srv = null;
            try
            {
                using (WebClient webclient = new WebClient())
                {
                    webclient.Headers["accept"] = "application/json";
                    string url = "https://plex.tv/api/resources?X-Plex-Token=" + token;
                    string response = webclient.DownloadString(url);
                    XmlDocument PlexXml = new XmlDocument();
                    PlexXml.LoadXml(response);
                    string jsonText = JsonConvert.SerializeXmlNode(PlexXml);
                    obj_srv = JsonConvert.DeserializeObject(jsonText);
                }
            }
            catch
            {
                return null;
            }

            int count = obj_srv["MediaContainer"]["@size"];
            if (count > 1)
            {
                for (int i = 0; i < count; i++)
                {
                    if (obj_srv["MediaContainer"]["Device"][i]["@provides"] == "server")
                    {
                        return obj_srv["MediaContainer"]["Device"][i]["Connection"]["@uri"];
                    }
                }
            }
            else if (count == 1)
            {
                if (obj_srv["MediaContainer"]["Device"]["@provides"] == "server")
                {
                    return obj_srv["MediaContainer"]["Device"]["Connection"]["@uri"];
                }
            }
            return null;
        }


    }
}
