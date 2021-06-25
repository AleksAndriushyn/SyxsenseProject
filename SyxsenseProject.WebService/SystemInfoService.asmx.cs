using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace SyxsenseProject.WebService
{
    /// <summary>
    /// Summary description for SystemInfoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SystemInfoService : System.Web.Services.WebService
    {

        [WebMethod]
        public bool IsMachineUp(string hostName)
        {
            bool retVal = false;
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply previousReply = pingSender.Send(hostName, timeout, buffer, options);
                if (previousReply.Status == IPStatus.Success)
                {
                    PingReply currentReply = pingSender.Send(hostName, timeout, buffer, options);
                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromMinutes(5);

                    var timer = new Timer((e) =>
                    {
                        if (currentReply != previousReply)
                        {

                        }
                    }, null, startTimeSpan, periodTimeSpan);
                    
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                retVal = false;
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }
    }
}
