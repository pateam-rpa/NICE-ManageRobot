using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ManageRobot.Helpers
{
    class SetRobot
    {
        public static string setContext(string cntx, string sol, bool ldstrp)
        {
            string URL = NiceRestHelpers.fetchFQDNfromConfig() + @"/RTServer/rest/nice/rti/core/solution-assignments/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Headers.Add("Cookie", "iPlanetDirectoryPro=" + Get_Token.getToken());


            solution Solution = new solution();
            SelfSolution getsolution = new SelfSolution();
            getsolution = Helpers.Solution_Dproj_Mapping.getDprojSolution(sol);
            Solution.solutionId = getsolution.solution.solutionId;
            Solution.version = getsolution.solution.version;
            Solution.display = getsolution.solution.display;

            BodyRoot bodyRoot = new BodyRoot();
            bodyRoot.context = cntx;
            bodyRoot.teamId = Direct.Shared.Environment_Functions.TeamID;
            bodyRoot.loadOnStartup = ldstrp;
            bodyRoot.solutions.Add(Solution);

            string DATA = JsonConvert.SerializeObject(bodyRoot);

            using (Stream webStream = request.GetRequestStream())

            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                requestWriter.Write(DATA);
            }

            try
            {
                if (Main.logArchitect.IsInfoEnabled)
                {
                    Main.logArchitect.Info("Trying to set context to: \"" + bodyRoot.context + "\"");
                    Main.logArchitect.Info("Request data was:\n" + JsonConvert.SerializeObject(bodyRoot, Formatting.Indented));
                }
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    if (Main.logArchitect.IsDebugEnabled)
                        Main.logArchitect.Debug("Response: " + response);
                    return response;
                }
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled)
                  Main.logArchitect.Error("Failed to set context for: \"" + bodyRoot.context + "\" \nReason:\n" + e.Message);
            }
            return "Failed to change context, check logs for more information";
        }

        protected class solution
        {
            public string solutionId { get; set; }
            public string display { get; set; }
            public int version { get; set; }
        }

        protected class BodyRoot
        {
            public string teamId { get; set; }
            public string context { get; set; }
            public bool loadOnStartup { get; set; }
            public List<solution> solutions { get; set; }
            
            public BodyRoot()
            {
                solutions = new List<solution>();
            }
        }
    }

    public class NiceRestHelpers
    {
        public static string fetchFQDNfromConfig()
        {
            string str1 = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Nice_Systems\\Real-Time\\RTClient.exe.config");
            string str2 = str1.Substring(str1.IndexOf("syncInvocationServiceUrl") + "syncInvocationServiceUrl=\"".Length);
            return str2.Substring(0, str2.IndexOf("/RTServer"));
        }
    }
}
