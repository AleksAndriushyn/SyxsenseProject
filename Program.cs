using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SyxsenseProject.WebService;

namespace SyxsenseProject
{
    class Program
    {
        static void Main(string[] args)
        {
            GetInformation();
            Console.ReadKey();
        }

        public static void SendUDP(string hostNameOrAddress, int destinationPort, string data, int count)
        {
            IPAddress destination = Dns.GetHostAddresses(hostNameOrAddress)[0];
            IPEndPoint endPoint = new IPEndPoint(destination, destinationPort);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            for (int i = 0; i < count; i++)
            {
                socket.Connect(endPoint);
                if (socket.Connected)
                {
                    SystemInfoService systemInfoService = new SystemInfoService();
                    var response = systemInfoService.IsMachineUp(hostNameOrAddress);
                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromMinutes(5);

                    var timer = new Timer((e) =>
                    {
                        Console.WriteLine("Is machine online? {0}", response);
                    }, null, startTimeSpan, periodTimeSpan);
                }
            }
            socket.Close();
        }

        public static void GetInformation()
        {
            SelectQuery query = new SelectQuery(@"Select * from Win32_OperatingSystem");
            
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (ManagementObject process in searcher.Get())
                {
                    //print system info
                    Console.WriteLine("/*********Operating System Information ***************/");
                    Console.WriteLine("{0}{1}", "OS name: ", process["Caption"]);
                    Console.WriteLine("{0}{1}", "Time zone: ", process["CurrentTimeZone"]);
                    Console.WriteLine("{0}{1}", "Date time: ", process["LocalDateTime"]);
                    Console.WriteLine("{0}{1}", "Computer name: ", process["CSName"]);
                    //Console.WriteLine("{0}{1}", ".Net version:", process[".net version"]);
                    Int32 counter = 0;
                    SendUDP(process["CSName"].ToString(), 41181, counter.ToString(), counter.ToString().Length);
                }
            }
        }
    }
}
