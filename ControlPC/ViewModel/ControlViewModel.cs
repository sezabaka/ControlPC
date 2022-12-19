using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows.Threading;
using System.Threading;
using ControlPC.Entity;
using ControlPC.WMI;
using ControlPC.Method;
using ControlPC.Command;

namespace ControlPC.ViewModel
{
    public class ControlViewModel : ViewModelBase
    {
        List<string> ListVLAN = new List<string>()
        {
            "10.16.7",
            "10.16.8",
            "10.16.9",
            "10.16.15",
            "192.168.58",
            "192.168.59",
            "192.168.60",
            "192.168.61",
            "192.168.62",
            "192.168.63",
            "192.168.64",
            "192.168.65",
            "192.168.66"
        };

        private List<PCInfor> listPC = new List<PCInfor>();
        public List<PCInfor> ListPC
        {
            get { return listPC; }
            set
            {
                base.RaisePropertyChangingEvent("ListPC");
                listPC = value;
                base.RaisePropertyChangedEvent("ListPC");
            }
        }

        private List<PCInfor> listOther = new List<PCInfor>();
        public List<PCInfor> ListOther
        {
            get { return listOther; }
            set
            {
                base.RaisePropertyChangingEvent("ListOther");
                listOther = value;
                base.RaisePropertyChangedEvent("ListOther");
            }
        }

        private List<PCWithUSB> listOffice = new List<PCWithUSB>();
        public List<PCWithUSB> ListOffice
        {
            get { return listOffice; }
            set
            {
                base.RaisePropertyChangingEvent("ListOffice");
                listOffice = value;
                base.RaisePropertyChangedEvent("ListOffice");
            }
        }

        private string ip;
        public string IP
        {
            get { return ip; }
            set
            {
                base.RaisePropertyChangingEvent("IP");
                ip = value;
                base.RaisePropertyChangedEvent("IP");
            }
        }

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                base.RaisePropertyChangingEvent("Time");
                time = value;
                base.RaisePropertyChangedEvent("Time");
            }
        }


        List<PCInfor> list = new List<PCInfor>();
        Queue<PCInfor> que = new Queue<PCInfor>();
        string domain = "cvn.canon.co.jp";

        public ICommand GetIP { get; set; }
        public ICommand GetID { get; set; }

        public ControlViewModel()
        {
            for (int i = 0; i < ListVLAN.Count; i++)
            {
                for (int j = 1; j < 255; j++)
                {
                    PCInfor pc = new PCInfor();
                    pc.IPAddress = ListVLAN[i] + "." + j;
                    pc.No = i * 254 + j;

                    //list.Add(pc);
                    listPC.Add(pc);
                }
            }

            List<string> pcs = File.ReadAllLines(@".\Office PC.csv").ToList();
            for (int i = 1; i < pcs.Count; i++)
            {
                PCWithUSB pc = new PCWithUSB();
                string[] aa = pcs[i].Split(',');
                if (aa.Count() == 2)
                {
                    pc.No = i;
                    pc.Dept = aa[0];
                    pc.Name = aa[1];

                    listOffice.Add(pc);
                }
            }

            GetIP = new AsyncCommand(GetInfor);
            GetID = new AsyncCommand(GetUSBID);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.IsEnabled = true;
            timer.Start();
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (que.Count > 0)
            {
                PCInfor pc = que.Dequeue();

                if (listOffice.Any(x => pc.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim()))
                {
                    PCWithUSB pu = listOffice.Find(x => pc.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim());
                    pu.IPAddress = pc.IPAddress;
                }
            }
        }

        private async Task GetInfor2(object p)
        {
            DateTime start = DateTime.Now;
            Time = (DateTime.Now - start).ToString();
        }

        private async Task GetInfor(object p)
        {
            DateTime start = DateTime.Now;

            /*await Task.Run(async () =>
                {
                    foreach (var item in list)
                    {
                        PCInfor pc = await GetName(item);

                        if (pc != null)
                        {
                            if (!string.IsNullOrEmpty(pc.Name))
                            {
                                listPC.Add(pc);

                                que.Enqueue(pc);
                            }
                            else
                            {
                                listOther.Add(pc);
                            }
                        }
                    }
                });

            Time = (DateTime.Now - start).ToString();*/
            Parallel.ForEach(listPC, async item =>
                {
                    await GetName2(item);
                    Time = (DateTime.Now - start).ToString();
                });
        }

        private async Task GetName2(PCInfor pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        IP = pc.IPAddress;

                        //string to hold our return messge
                        string returnMessage = string.Empty;

                        //IPAddress instance for holding the returned host
                        IPAddress address = IPAddress.Parse(pc.IPAddress);

                        //set the ping options, TTL 128
                        PingOptions pingOptions = new PingOptions(128, true);

                        //create a new ping instance
                        Ping ping = new Ping();

                        //32 byte buffer (create empty)
                        byte[] buffer = new byte[32];

                        PingReply pingReply = await ping.SendPingAsync(address, 10, buffer, pingOptions);

                        if (!(pingReply == null))
                        {
                            if (pingReply.Status == IPStatus.Success)
                            {
                                pc.TTL = (byte)pingReply.Options.Ttl;

                                IPHostEntry hostEntry = await Dns.GetHostEntryAsync(pc.IPAddress);
                                if (hostEntry != null)
                                {
                                    pc.Name = hostEntry.HostName;
                                }
                                else
                                {
                                    throw new Exception("");
                                }
                            }
                            else
                            {
                                throw new Exception("");
                            }
                        }

                        await Task.Delay(10);
                    });
            }
            catch (Exception ex)
            {
            }
        }


        private async Task GetUSBID(object p)
        {
            DateTime start = DateTime.Now;

            foreach (var item in listOffice.FindAll(x => !string.IsNullOrEmpty(x.IPAddress)))
            {
                await GetUSB(item);
            }
             
            Time = (DateTime.Now - start).ToString();
             

            /*Parallel.ForEach(listOffice.FindAll(x => !string.IsNullOrEmpty(x.IPAddress)), async item =>
                {
                    await GetUSB(item);
                    await Task.Delay(100);
                    Time = (DateTime.Now - start).ToString();
                });*/
        }

        private async Task GetUSB(PCWithUSB pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        IP = pc.Name + "-" + pc.IPAddress;

                        Connection wmiConnection = new Connection("admv458574",
                                                      "8574&Hly",
                                                      "cvn.canon.co.jp",
                                                      pc.Name);

                        Win32_Keyboard m = new Win32_Keyboard(wmiConnection);
                        Win32_PointingDevice ll = new Win32_PointingDevice(wmiConnection);
                        Win32_USBControllerDevice p = new Win32_USBControllerDevice(wmiConnection);

                        foreach (string property in m.GetPropertyValues())
                        {
                            if (property.Contains("DeviceID: USB"))
                            {
                                pc.KeyboardID = property;
                            }
                        }

                        foreach (string property in ll.GetPropertyValues())
                        {
                            if (property.Contains("DeviceID: USB"))
                            {
                                pc.MouseID = property;
                            }
                        }

                        foreach (string property in p.GetPropertyValues())
                        {
                            if (property.Contains("USBPRINT"))
                            {
                                pc.PrinterID = property;
                            }
                        }

                        await Task.Delay(100);
                    });
            }
            catch
            {
            }
        }

        private async Task<PCInfor> GetName(PCInfor pc)
        {
            try
            {
                PCInfor result = new PCInfor();
                result.IPAddress = pc.IPAddress;
                result.No = pc.No;
                result.ThreadID = Thread.CurrentThread.ManagedThreadId;
                IP = pc.IPAddress;

                //string to hold our return messge
                string returnMessage = string.Empty;

                //IPAddress instance for holding the returned host
                IPAddress address = IPAddress.Parse(pc.IPAddress);

                //set the ping options, TTL 128
                PingOptions pingOptions = new PingOptions(128, true);

                //create a new ping instance
                Ping ping = new Ping();

                //32 byte buffer (create empty)
                byte[] buffer = new byte[32];

                PingReply pingReply = await ping.SendPingAsync(address, 10, buffer, pingOptions);

                if (!(pingReply == null))
                {
                    if (pingReply.Status == IPStatus.Success)
                    {
                        result.TTL = (byte)pingReply.Options.Ttl;
                        pc.TTL = result.TTL;

                        IPHostEntry hostEntry = await Dns.GetHostEntryAsync(pc.IPAddress);
                        if (hostEntry != null)
                        {
                            result.Name = hostEntry.HostName;
                        }
                        else
                        {
                            throw new Exception("");
                        }

                        /*var timeout = TimeSpan.FromMilliseconds(50);
                        var hostEntry = ResolveAsync(pc.IPAddress);

                        if (!hostEntry.Wait(timeout))
                        {
                            listOther.Add(result);
                            throw new TimeoutException();
                        }
                        
                        result.Name = hostEntry.Result.HostName;*/

                    }
                    else
                    {
                        throw new Exception("");
                    }
                }

                await Task.Delay(10);

                return result;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                return new PCInfor()
                {
                    IPAddress = pc.IPAddress,
                    TTL = pc.TTL
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<IPHostEntry> ResolveAsync(string ipAddress)
        {
            IPHostEntry entry = await Dns.GetHostEntryAsync(ipAddress);
            return entry;
        }
    }
}
