using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using log4net;
using System.Web;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Configuration;


namespace ManageRobot.Helpers
{
    class Get_Token
    {

        public static string getToken()
        {
            string URL = NiceRestHelpers.fetchFQDNfromConfig() + @"/openam/json/authenticate";
            if (ManageRobot.Main.logArchitect.IsDebugEnabled) ManageRobot.Main.logArchitect.Debug("Connecting to " + URL);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.PreAuthenticate = true;
            request.AllowAutoRedirect = true;
            request.Headers.Add("X-OpenAM-Username", "anonymous");
            request.Headers.Add("X-OpenAM-Password", "anonymous");
            try
            {
                //if (ManageRobot.Main.logArchitect.IsDebugEnabled)
                //ManageRobot.Main.logArchitect.Error("Common Ground- sending URI: " + request.RequestUri.ToString());
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    returnedTokenData obj = JsonConvert.DeserializeObject<returnedTokenData>(response);
                    return obj.tokenId;
                }
            }
            catch (Exception e)
            {
                if (ManageRobot.Main.logArchitect.IsDebugEnabled)
                    ManageRobot.Main.logArchitect.Error("Common Ground - Could not get Token.\nReason:\n" + e.Message);
            }
            return "Could not get token";
        }

        public class returnedTokenData
        {
            public string tokenId { get; set; }
            public string successUrl { get; set; }
        }
    }

}
