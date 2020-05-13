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
using Direct;
using Direct.Shared;

namespace ManageRobot.Helpers
{
    class Solution_Dproj_Mapping
    {
        #region Functions
        public static string getDprojIdGeneral(string projectName)
        {
            string URL = NiceRestHelpers.fetchFQDNfromConfig() + @"/RTServer/rest/nice/rti/core/solution-assignments/solutions?active=true";
            if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Debug("Connecting to " + URL);
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsErrorEnabled) Main.logArchitect.Error("Failed to create request with error : " + e.ToString());
                MessageBox.Show("Could not parse URL string. Please make sure FQDN under AppSettings is configured in the client's root folder");
                throw new Exception("Failed to create request");
            }
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Headers.Add("Cookie", "iPlanetDirectoryPro=" + Get_Token.getToken());
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    ReturnedSolutionsList returnedSolutionsList = JsonConvert.DeserializeObject<ReturnedSolutionsList>(response);
                    foreach (Solution solution in returnedSolutionsList.solutions)
                    {
                        if (solution.name == projectName)
                            return solution.solutionId;
                    }
                    throw new Exception("Solution Not Found");
                }
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled)
                    Main.logArchitect.Error("Manage Robot- Could not get workflows.\nReason:\n" + e.Message);
            }
            throw new Exception("Solution Not Found");
        }

        public static string getDprojId(string projectName)
        {
            string URL = null;
            try
            {
                URL = NiceRestHelpers.fetchFQDNfromConfig() + @"/RTServer/rest/nice/rti/core/solution-assignments/" +
                Environment_Functions.TeamID.Replace(@"\", "$$$");
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Error("Could not get TeamID from Server.\nError: " + e.ToString());
                throw new Exception(e.ToString());
            }
            if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Debug("Connecting to " + URL);
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsErrorEnabled) Main.logArchitect.Error("Failed to create request with error : " + e.ToString());
                MessageBox.Show("Could not parse URL string. Please make sure FQDN under AppSettings is configured in the client's root folder");
                throw new Exception("Failed to create request");
            }
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Headers.Add("Cookie", "iPlanetDirectoryPro=" + Get_Token.getToken());
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    ReturnedAssignmentData assignedSolution = JsonConvert.DeserializeObject<ReturnedAssignmentData>(response);
                    foreach (SelfSolution selfSolution in assignedSolution.selfSolutions)
                    {
                        if (selfSolution.solution.display == projectName)
                            return selfSolution.solution.solutionId;
                    }
                    throw new Exception("Solution Not Found");
                }
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled)
                    Main.logArchitect.Error("Manage Robot- Could not get workflows.\nReason:\n" + e.Message);
            }
            throw new Exception("Solution Not Found");
        }

        public static SelfSolution getDprojSolution(string projectName)
        {
            string URL = null;
            try
            {
                URL = NiceRestHelpers.fetchFQDNfromConfig() + @"/RTServer/rest/nice/rti/core/solution-assignments/" +
                Environment_Functions.TeamID.Replace(@"\", "$$$");
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Error("Could not get TeamID from Server.\nError: " + e.ToString());
                throw new Exception(e.ToString());
            }
            if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Debug("Connecting to " + URL);
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsErrorEnabled) Main.logArchitect.Error("Failed to create request with error : " + e.ToString());
                MessageBox.Show("Could not parse URL string. Please make sure FQDN under AppSettings is configured in the client's root folder");
                throw new Exception("Failed to create request");
            }
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Headers.Add("Cookie", "iPlanetDirectoryPro=" + Get_Token.getToken());
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    ReturnedAssignmentData assignedSolution = JsonConvert.DeserializeObject<ReturnedAssignmentData>(response);
                    foreach (SelfSolution selfSolution in assignedSolution.selfSolutions)
                    {
                        if (selfSolution.solution.display == projectName)
                            return selfSolution;
                    }
                    throw new Exception("Solution Not Found");
                }
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled)
                    Main.logArchitect.Error("Manage Robot - Could not get workflows.\nReason:\n" + e.Message);
            }
            throw new Exception("Solution Not Found");
        }

        public static List<SelfSolution> getAssignedVersion()
        {
            string URL = null;
            try
            {
                URL = NiceRestHelpers.fetchFQDNfromConfig() + @"/RTServer/rest/nice/rti/core/solution-assignments/" +
                Direct.Shared.Environment_Functions.TeamID.Replace(@"\", "$$$");
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Error("Could not get TeamID from Server.\nError: " + e.ToString());
                return new List<SelfSolution>();
            }
            if (Main.logArchitect.IsDebugEnabled) Main.logArchitect.Debug("Connecting to " + URL);
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(URL);
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsErrorEnabled) Main.logArchitect.Error("Failed to create request with error : " + e.ToString());
                MessageBox.Show("Could not parse URL string. Please make sure FQDN under AppSettings is configured in the client's root folder");
                throw new Exception("Failed to create request");
            }
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Headers.Add("Cookie", "iPlanetDirectoryPro=" + Get_Token.getToken());
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    ReturnedAssignmentData returnedData = JsonConvert.DeserializeObject<ReturnedAssignmentData>(response);
                    return returnedData.selfSolutions;
                }
            }
            catch (Exception e)
            {
                if (Main.logArchitect.IsDebugEnabled)
                    Main.logArchitect.Error("Manage Robot - Could not get assigned solutions.\nReason:\n" + e.Message);
            }
            return new List<SelfSolution>();
        }
        #endregion

        public class Version
        {
            public int versionNumber { get; set; }
            public string description { get; set; }
        }

        public class Solution
        {
            public string solutionId { get; set; }
            public string name { get; set; }
            public List<Version> versions { get; set; }
        }

        public class ReturnedSolutionsList
        {
            public List<Solution> solutions { get; set; }
        }
    }

    public class Solution
    {
        public string solutionId { get; set; }
        public int version { get; set; }
        public string display { get; set; }
        public string notes { get; set; }
    }

    public class SelfSolution
    {
        public string teamId { get; set; }
        public bool loadOnStartup { get; set; }
        public object assignedOn { get; set; }
        public string assignedBy { get; set; }
        public Solution solution { get; set; }
    }

    public class ReturnedAssignmentData
    {
        public List<SelfSolution> selfSolutions { get; set; }
        public List<object> parentSolutions { get; set; }
        public bool viewable { get; set; }
        public bool editable { get; set; }
    }

}
