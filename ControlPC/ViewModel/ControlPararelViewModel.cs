using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Management;
using System.Windows;
using ControlPC.Entity;
using ControlPC.Command;
using ControlPC.WMI;
using ControlPC.Method;

namespace ControlPC.ViewModel
{
    public class ControlPararelViewModel : ViewModelBase
    {
        List<string> ListVLAN = new List<string>()
        {
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

        private List<PCWithUSB> listNPC = new List<PCWithUSB>();
        public List<PCWithUSB> ListNPC
        {
            get { return listNPC; }
            set
            {
                base.RaisePropertyChangingEvent("ListNPC");
                listNPC = value;
                base.RaisePropertyChangedEvent("ListNPC");
            }
        }

        private List<PCWithUSB> listServer = new List<PCWithUSB>();
        public List<PCWithUSB> ListServer
        {
            get { return listServer; }
            set
            {
                base.RaisePropertyChangingEvent("ListServer");
                listServer = value;
                base.RaisePropertyChangedEvent("ListServer");
            }
        }


        private List<ComputerInfo> listAD = new List<ComputerInfo>();
        public List<ComputerInfo> ListAD
        {
            get { return listAD; }
            set
            {
                base.RaisePropertyChangingEvent("ListAD");
                listAD = value;
                base.RaisePropertyChangedEvent("ListAD");
            }
        }

        private List<ComputerInfo> listADOld = new List<ComputerInfo>();
        public List<ComputerInfo> ListADOld
        {
            get { return listADOld; }
            set
            {
                base.RaisePropertyChangingEvent("ListADOld");
                listADOld = value;
                base.RaisePropertyChangedEvent("ListADOld");
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

        private int itemCount;
        public int ItemCount
        {
            get { return itemCount; }
            set
            {
                base.RaisePropertyChangingEvent("ItemCount");
                itemCount = value;
                base.RaisePropertyChangedEvent("ItemCount");
            }
        }

        string domain = "cvn.canon.co.jp";
        string user = "v438475";
        string password = "nG0M0unt@!";

        public ICommand GetIP { get; set; }
        public ICommand GetID { get; set; }
        public ICommand Register { get; set; }
        public ICommand Uninstall { get; set; }
        public ICommand Ping80C { get; set; }
        public ICommand Ping23C { get; set; }
        public ICommand CheckPC { get; set; }
        public ICommand ChangePass { get; set; }

        public ControlPararelViewModel()
        {
            ListVLAN = File.ReadAllLines("IP.txt").ToList();

            for (int i = 0; i < ListVLAN.Count; i++)
            {
                for (int j = 1; j < 255; j++)
                {
                    PCInfor pc = new PCInfor();
                    pc.IPAddress = ListVLAN[i] + "." + j;
                    pc.No = i * 254 + j;

                    listPC.Add(pc);
                }
            }

            // For ChangePass
            /*for (int i = 0; i < ListVLAN.Count; i++)
            {
                PCInfor pc = new PCInfor();
                pc.IPAddress = ListVLAN[i];
                pc.No = i;

                listPC.Add(pc);
            }*/

            List<string> pcs = File.ReadAllLines(@".\Office PC.csv").ToList();
            for (int i = 1; i < pcs.Count; i++)
            {
                PCWithUSB pc = new PCWithUSB();
                string[] aa = pcs[i].Split(',');
                if (aa.Count() == 2)
                {
                    pc.No = i;
                    pc.Dept = aa[0].Trim();
                    pc.Name = aa[1].Trim();

                    listOffice.Add(pc);
                }
            }

            List<string> npcs = File.ReadAllLines(@".\NPC.csv").ToList();
            for (int i = 1; i < npcs.Count; i++)
            {
                PCWithUSB pc = new PCWithUSB();
                string[] aa = npcs[i].Split(',');
                if (aa.Count() == 2)
                {
                    pc.No = i;
                    pc.Dept = aa[0].Trim();
                    pc.Name = aa[1].Trim();

                    listNPC.Add(pc);
                }
            }

            List<string> servers = File.ReadAllLines(@".\Server.csv").ToList();
            for (int i = 1; i < servers.Count; i++)
            {
                PCWithUSB pc = new PCWithUSB();
                string[] aa = servers[i].Split(',');
                if (aa.Count() == 2)
                {
                    pc.No = i;
                    pc.Dept = aa[0].Trim();
                    pc.Name = aa[1].Trim();
                    listServer.Add(pc);
                }
            }

            GetIP = new RelayCommand(PingAll);
            GetID = new RelayCommand(GetPcInfo, GetIDCanExe);
            Register = new RelayCommand(RegisterDNS);
            Uninstall = new RelayCommand(UninstallSMC);
            Ping80C = new RelayCommand(Ping80Exe);
            Ping23C = new RelayCommand(Ping23Exe);
            CheckPC = new RelayCommand(NeedName);
            ChangePass = new RelayCommand(ChangePassExe);
            //GetPCInAD();
            //CheckInAD();
            //CheckListOld();
        }

        string ps = "$NewPassword = ConvertTo-SecureString \"aD$3rSvZ0\" -AsPlainText -Force \r\n Set-LocalUser -Name Administrator -Password $NewPassword";

        private async void ChangePassExe()
        {
            DateTime start = DateTime.Now;

            Parallel.ForEach(listPC.FindAll(x => x.TTL != 0), async item =>
            {
                await RunScriptRegister(item, ps);
                IP = item.IPAddress;
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });
        }

        private async void Ping80Exe()
        {
            DateTime start = DateTime.Now;

            Parallel.ForEach(listPC.FindAll(x=>x.TTL != 0), async item =>
            {
                await Ping80(item);
                IP = item.IPAddress;
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });
        }

        private async void Ping23Exe()
        {
            DateTime start = DateTime.Now;

            Parallel.ForEach(listPC.FindAll(x => x.TTL != 0), async item =>
            {
                await Ping23(item);
                IP = item.IPAddress;
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });
        }

        private async void UninstallSMC()
        {
            ItemCount = 0;
            DateTime start = DateTime.Now;
            Parallel.ForEach(listNPC.FindAll(x => x.HasSEP), async item =>
            {
                await UninstallWMI(item);
                IP = item.IPAddress;
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });
        }

        private async Task UninstallWMI(PCWithUSB pc)
        {
            try
            {
                await Task.Run(async () =>
                {
                    ConnectionOptions options = new ConnectionOptions();
                    options.Username = domain + "\\" + user;
                    options.Password = password;
                    ManagementScope scope = new ManagementScope("\\\\" + pc.Name + "\\root\\cimv2", options);
                    scope.Connect();

                    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Product WHERE Name = 'Symantec Endpoint Protection'");
                    ManagementObjectSearcher mos = new ManagementObjectSearcher(scope, query);

                    foreach (ManagementObject mo in mos.Get())
                    {
                        if (mo["Name"].ToString() == "Symantec Endpoint Protection")
                        {
                            object hr = mo.InvokeMethod("Uninstall", null);
                            //pc.Exception = hr.ToString();
                        }
                    }
                });
            }
            catch //(Exception ex)
            {
                //pc.Exception = ex.Message;
            }
        }

        private async void RegisterDNS()
        {
            ItemCount = 0;
            DateTime start = DateTime.Now;
            Parallel.ForEach(listPC.FindAll(x => x.Name != null ? x.Name.Contains("No such") : false), async item =>
            {
                await RunScriptRegister(item);
                IP = item.IPAddress;
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });
        }


        private void CheckInAD()
        {
            foreach (var item in listOffice)
            {
                if (listAD.Any(x => x.Name == item.Name))
                {
                    ComputerInfo ci = listAD.Find(x => x.Name == item.Name);
                    ci.PCName = item.Name;
                }
            }

            foreach (var item in listNPC)
            {
                if (listAD.Any(x => x.Name == item.Name))
                {
                    ComputerInfo ci = listAD.Find(x => x.Name == item.Name);
                    ci.PCName = item.Name;
                }
            }
        }

        private void CheckListOld()
        {
            foreach (var item in listAD)
            {
                if (item.Name != item.PCName)
                {
                    listADOld.Add(item);
                }
            }

            /*foreach (var item in listADOld)
            {
                ChangeOU1(item);
            }*/
        }



        private void GetPCInAD()
        {
            List<string> ouList = new List<string>();
            ouList.Add("LDAP://OU=QV PCs,DC=cvn,DC=canon,DC=co,DC=jp");
            ouList.Add("LDAP://OU=IT QV,DC=cvn,DC=canon,DC=co,DC=jp");
            //ouList.Add("LDAP://OU=QV NPCs,DC=cvn,DC=canon,DC=co,DC=jp");
            //ouList.Add("LDAP://OU=TL PCs,DC=cvn,DC=canon,DC=co,DC=jp");

            foreach (var ou in ouList)
            {
                using (DirectoryEntry entry = new DirectoryEntry(ou, user, password))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = "(objectClass=computer)";
                        foreach (SearchResult res in searcher.FindAll())
                        {
                            if (res.Properties["name"].Count > 0)
                            {
                                ComputerInfo ci = new ComputerInfo();
                                ci.Name = (string)res.Properties["name"][0];

                                foreach (System.Collections.DictionaryEntry item in res.Properties)
                                {
                                    if (item.Key.ToString() == "operatingsystemversion")
                                    {
                                        ResultPropertyValueCollection rpvc = (ResultPropertyValueCollection)item.Value;
                                        ci.Version = rpvc[0].ToString();
                                    }
                                    if (item.Key.ToString() == "operatingsystem")
                                    {
                                        ResultPropertyValueCollection rpvc = (ResultPropertyValueCollection)item.Value;
                                        ci.VerName = rpvc[0].ToString();
                                    }
                                    if (item.Key.ToString().Contains("lastlogon"))
                                    {
                                        ResultPropertyValueCollection rpvc = (ResultPropertyValueCollection)item.Value;
                                        ci.lastLogon = new DateTime((Int64)rpvc[0]);
                                    }
                                }
                                listAD.Add(ci);
                            }
                        }
                    }
                }
            }
        }

        private async void PingAll()
        {
            ItemCount = 0;
            DateTime start = DateTime.Now;
            Parallel.ForEach(listPC, async item =>
                {
                    await GetName2(item);
                    Time = (DateTime.Now - start).ToString();
                    ItemCount++;
                });
        }


        private void GetMonitorSerial()
        {
            /*string scriptText = "ForEach ($Monitor in Get-WmiObject WmiMonitorID -Namespace root\\wmi) \n"
            + "{ \n $Serial = Decode $Monitor.SerialNumberID -notmatch 0 "
            + "\n echo \"$Serial\" }";*/
        }

        /// <summary>
        /// Runs the given powershell script and returns the script output.
        /// </summary>
        /// <param name="scriptText">the powershell script text to run</param>
        /// <returns>output of the script</returns>
        private async Task RunScriptToList(PCWithUSB pc, 
            string scriptText = "Get-Pnpdevice -class \"WPD\" | SELECT FRIENDLYNAME, INSTANCEID, STATUS")
        {
            try
            {
                await Task.Run(async () =>
                    {
                        System.Security.SecureString ss = new NetworkCredential("", password).SecurePassword;

                        string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
                        PSCredential remoteCredential = new PSCredential(user, ss);
                        WSManConnectionInfo connectionInfo = new WSManConnectionInfo(false, pc.Name, 5985,
                            "/wsman", shellUri, remoteCredential, 1000);

                        // create Powershell runspace
                        Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo);

                        // open it
                        runspace.Open();

                        // create a pipeline and feed it the script text
                        Pipeline pipeline = runspace.CreatePipeline();
                        pipeline.Commands.AddScript(scriptText);

                        // add an extra command to transform the script output objects into nicely formatted strings
                        // remove this line to get the actual objects that the script returns. For example, the script
                        // "Get-Process" returns a collection of System.Diagnostics.Process instances.
                        pipeline.Commands.Add("Out-String");

                        // execute the script
                        Collection<PSObject> results = pipeline.Invoke();

                        // close the runspace
                        runspace.Close();

                        List<string> re = results[0].ToString().Split(new char[2] { '\r', '\n' }).ToList();

                        for (int i = 0; i < re.Count; i++)
                        {
                            if (string.IsNullOrEmpty(re[i]))
                            {
                                re.Remove(re[i]);
                                i--;
                            }
                        }

                        // convert the script result into a single string
                        List<string> stringBuilder = new List<string>();

                        int p1 = re[0].ToString().ToUpper().IndexOf("INSTANCEID");

                        int p2 = re[0].ToString().ToUpper().IndexOf("STATUS");

                        for (int i = 2; i < re.Count; i++)
                        {
                            if (!re[i].ToString().ToLower().Contains("canon"))
                            //if (re[i].ToString().ToLower().Contains(",ok"))
                            {
                                string res = re[i].ToString().Substring(0, p1).Trim() + "," + re[i].ToString().Substring(p1, p2 - p1).Trim() + ","
                                + re[i].ToString().Substring(p2, re[i].ToString().Length - p2).Trim();
                                if (res.ToLower().Contains(",ok"))
                                {
                                    stringBuilder.Add(res);
                                }
                            }
                        }

                        pc.WPD = string.Join(";", stringBuilder);
                    });
            }
            catch
            {
            }
        }

        private async Task RunScriptRegister(PCInfor pc, string scriptText = "ipconfig /registerdns")
        {
            try
            {
                await Task.Run(async () =>
                {
                    System.Security.SecureString ss = new NetworkCredential("", password).SecurePassword;

                    //string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
                    PSCredential remoteCredential = new PSCredential(user, ss);

                    WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri("http://" + pc.IPAddress + ":5985/wsman"),
                        "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", remoteCredential);
                    connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Negotiate;

                    // create Powershell runspace
                    Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo);

                    // open it
                    runspace.Open();

                    // create a pipeline and feed it the script text
                    Pipeline pipeline = runspace.CreatePipeline();
                    pipeline.Commands.AddScript(scriptText);

                    // add an extra command to transform the script output objects into nicely formatted strings
                    // remove this line to get the actual objects that the script returns. For example, the script
                    // "Get-Process" returns a collection of System.Diagnostics.Process instances.
                    pipeline.Commands.Add("Out-String");

                    // execute the script
                    Collection<PSObject> results = pipeline.Invoke();

                    //pc.Name = results[0].

                    // close the runspace
                    runspace.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool GetIDCanExe(object p)
        {
            bool res = true;

            if (!listOffice.Any(x => x.ThreadID == -1))
                res = true;
            else if (!listOffice.Any(x => x.ThreadID != -1))
                res = true;
            else
                res = false;

            return res;
        }

        private async Task RegisterName(PCInfor pc)
        {
            try
            {
                await Task.Run(async () =>
                    {

                    });
            }
            catch
            {
            }
        }

        private async Task GetName2(PCInfor pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        IP = pc.IPAddress;
                        pc.ThreadID = Thread.CurrentThread.ManagedThreadId;

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
            catch
            {
            }
        }

        private async Task Ping80(PCInfor pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        TcpClient tcp = new TcpClient();
                        await tcp.ConnectAsync(IPAddress.Parse(pc.IPAddress), 22);

                        if (tcp.Connected)
                        {
                            pc.P80 = true;
                        }
                    });
            }
            catch
            {

            }
        }

        private async Task ChangePassSv(PCInfor pc)
        {
            try
            {
                await Task.Run(async () =>
                {
                    TcpClient tcp = new TcpClient();
                    await tcp.ConnectAsync(IPAddress.Parse(pc.IPAddress), 80);

                    if (tcp.Connected)
                    {
                        pc.P80 = true;
                    }
                });
            }
            catch
            {

            }
        }

        private async Task Ping23(PCInfor pc)
        {
            try
            {
                await Task.Run(async () =>
                {
                    TcpClient tcp = new TcpClient();
                    await tcp.ConnectAsync(IPAddress.Parse(pc.IPAddress), 902);

                    if (tcp.Connected)
                    {
                        pc.P902 = true;
                    }
                });
            }
            catch
            {

            }
        }

        private async void GetPcInfo()
        {
            await GetIPOffice();
            await GetIPNPC();
            await GetIPServer();
            //await PingPort();
            DateTime start = DateTime.Now;

            ItemCount = 0;

            Parallel.ForEach(listOffice.FindAll(x => !string.IsNullOrEmpty(x.IPAddress)), async item =>
            //Parallel.ForEach(listOffice, async item =>
                {
                    await GetUSB(item);
                    await GetOU(item);
                    //await ChangeOU(item);
                    await Task.Delay(100);

                    //await RunScriptToList(item);
                    //await Task.Delay(100);
                    Time = (DateTime.Now - start).ToString();
                    ItemCount++;
                });

            Parallel.ForEach(listNPC.FindAll(x => !string.IsNullOrEmpty(x.IPAddress)), async item =>
            {
                await GetUSB(item);
                await GetOU(item);
                //await ChangeOU(item);
                await Task.Delay(100);
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });

            Parallel.ForEach(listServer.FindAll(x => !string.IsNullOrEmpty(x.IPAddress)), async item =>
            {
                await GetUSB(item);
                await GetOU(item);
                //await ChangeOU(item);
                await Task.Delay(100);
                Time = (DateTime.Now - start).ToString();
                ItemCount++;
            });
        }

        /*private async Task PingPort()
        {
            try
            {
                Parallel.ForEach(listPC.FindAll(x => x.TTL != 0), async item =>
                    {
                        TcpClient tcp = new TcpClient();
                        await tcp.ConnectAsync(IPAddress.Parse(item.IPAddress), 80);

                        if (tcp.Connected)
                        {
                            item.P80 = true;
                        }
                    });
            }
            catch
            {

            }
        }*/

        private async Task GetIPOffice()
        {
            try
            {
                Parallel.ForEach(listPC.FindAll(x => !string.IsNullOrEmpty(x.Name)), async item =>
                    {
                        if (listOffice.Any(x => item.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim()))
                        {
                            PCWithUSB pu = listOffice.Find(x => item.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim());
                            pu.IPAddress = item.IPAddress;
                            item.Exist = true;
                        }
                    });
            }
            catch
            {

            }
        }

        private async Task GetIPNPC()
        {
            try
            {
                Parallel.ForEach(listPC.FindAll(x => !string.IsNullOrEmpty(x.Name)), async item =>
                {
                    if (listNPC.Any(x => item.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim()))
                    {
                        PCWithUSB pu = listNPC.Find(x => item.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim());
                        pu.IPAddress = item.IPAddress;
                        item.Exist = true;
                    }
                });
            }
            catch
            {

            }
        }

        private async Task GetIPServer()
        {
            try
            {
                Parallel.ForEach(listPC.FindAll(x => !string.IsNullOrEmpty(x.Name)), async item =>
                {
                    if (listServer.Any(x => item.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim()))
                    {
                        PCWithUSB pu = listServer.Find(x => item.Name.Replace("." + domain, "").ToLower().Trim() == x.Name.ToLower().Trim());
                        pu.IPAddress = item.IPAddress;
                        item.Exist = true;
                    }
                });
            }
            catch
            {

            }
        }

        private async void NeedName()
        {
            try
            {
                Parallel.ForEach(listPC.FindAll(x => !string.IsNullOrEmpty(x.Name)), async item =>
                {
                    if (!string.IsNullOrEmpty(item.Name) && item.TTL > 120 && item.TTL < 129 && !item.Exist)
                    {
                        item.None = true;
                    }
                });
            }
            catch
            {
            }
        }

        private void ChangeOU1(ComputerInfo pc)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, user, password);

            // find the computer in question
            ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, pc.Name);

            if (computer != null)
            {
                DirectoryEntry eLocation = new DirectoryEntry("LDAP://cvn.canon.co.jp/" + computer.DistinguishedName, user, password);
                DirectoryEntry nLocation = 
                    new DirectoryEntry("LDAP://cvn.canon.co.jp/OU=Dept PC,OU=QV PCs,DC=cvn,DC=canon,DC=co,DC=jp", user, password);
                string newName = eLocation.Name;
                eLocation.MoveTo(nLocation, newName);
                nLocation.Close();
                eLocation.Close();
            }
        }

        private async Task ChangeOU(PCWithUSB pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, user, password);

                        // find the computer in question
                        ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, pc.Name);

                        if (computer != null)
                        {
                            DirectoryEntry eLocation = new DirectoryEntry("LDAP://cvn.canon.co.jp/" + computer.DistinguishedName, user, password);
                            DirectoryEntry nLocation = 
                                new DirectoryEntry("LDAP://cvn.canon.co.jp/OU=PC Office PCs,OU=QV PCs,DC=cvn,DC=canon,DC=co,DC=jp", user, password);
                            string newName = eLocation.Name;
                            eLocation.MoveTo(nLocation, newName);
                            nLocation.Close();
                            eLocation.Close();
                        }
                    });
            }
            catch
            {
            }
        }

        private async Task GetOU(PCWithUSB pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain, user, password);

                        // find the computer in question
                        ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, pc.Name);

                        if (computer != null)
                        {
                            DirectoryEntry eLocation = new DirectoryEntry("LDAP://cvn.canon.co.jp/" + computer.DistinguishedName, user, password);
                            pc.Path = eLocation.Path.Replace("LDAP://cvn.canon.co.jp/CN=" + pc.Name + ",", "")
                                .Replace(",DC=cvn,DC=canon,DC=co,DC=jp","");
                        }

                        pc.ThreadID = Thread.CurrentThread.ManagedThreadId;
                    });
                
            }
            catch
            {
            }
        }

        private async Task GetUSB(PCWithUSB pc)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        IP = pc.Name + "-" + pc.IPAddress;

                        Connection wmiConnection = new Connection(user,
                                                      password,
                                                      domain,
                                                      pc.Name);

                        Win32_Keyboard m = new Win32_Keyboard(wmiConnection);
                        Win32_PointingDevice ll = new Win32_PointingDevice(wmiConnection);
                        Win32_USBControllerDevice p = new Win32_USBControllerDevice(wmiConnection);
                        Win32_PhysicalMemory r = new Win32_PhysicalMemory(wmiConnection);
                        Win32_NetworkAdapter n = new Win32_NetworkAdapter(wmiConnection);
                        Win32_DiskDrive dd = new Win32_DiskDrive(wmiConnection);
                        Win32_BaseBoard a = new Win32_BaseBoard(wmiConnection);
                        //Win32_BIOS b = new Win32_BIOS(wmiConnection);
                        Win32_Product pd = new Win32_Product(wmiConnection);
                        Win32_OperatingSystem wo = new Win32_OperatingSystem(wmiConnection);
                        Win32_Processor pr = new Win32_Processor(wmiConnection);
                        WmiMonitorID wp = new WmiMonitorID(wmiConnection);
                        Win32_PnPEntity ww = new Win32_PnPEntity(wmiConnection);

                        foreach (string property in m.GetPropertyValues())
                        {

                            pc.KeyboardID += (!string.IsNullOrEmpty(pc.KeyboardID) ? ", " : "") + property.Replace("DeviceID: ", "").Trim();
                        }

                        foreach (string property in ll.GetPropertyValues())
                        {
                            pc.MouseID += (!string.IsNullOrEmpty(pc.MouseID) ? ", " : "") + property.Replace("DeviceID: ", "").Trim();
                        }

                        foreach (string property in p.GetPropertyValues())
                        {
                            if (property.Contains("USBPRINT"))
                            {
                                pc.PrinterID += (!string.IsNullOrEmpty(pc.PrinterID) ? ", " : "") + property.Trim();
                            }
                        }

                        foreach (var item in wp.GetPropertyValues())
                        {
                            pc.MonitorId += (!string.IsNullOrEmpty(pc.MonitorId) ? ", " : "") + item.Trim();
                        }

                        foreach (var item in ww.GetPropertyValues())
                        {
                            if (item.Contains("DISPLAY") && !item.Contains("ROOT"))
                            {
                                pc.WPD += (!string.IsNullOrEmpty(pc.WPD) ? ", " : "") + item.Replace("DeviceID: ", "").Trim();
                            }
                        }

                        foreach (string property in r.GetPropertyValues())
                        {
                            if (property.Contains("SerialNumber"))
                            {
                                pc.RamSeri += (!string.IsNullOrEmpty(pc.RamSeri) ? ", " : "") + property.Replace("SerialNumber: ", "").Trim();
                            }

                            if (property.Contains("Capacity"))
                            {
                                pc.RamCapa += (!string.IsNullOrEmpty(pc.RamCapa) ? ", " : "")
                                    + SizeSuffix(double.Parse(property.Replace("Capacity: ", "").Trim()));
                            }

                            if (property.Contains("Speed"))
                            {
                                pc.RamSpeed += (!string.IsNullOrEmpty(pc.RamSpeed) ? ", " : "") + property.Replace("Speed: ", "").Trim();
                            }
                        }

                        foreach (string property in n.GetPropertyValues())
                        {
                            if (property.Contains("MACAddress"))
                            {
                                pc.MAC += (!string.IsNullOrEmpty(pc.MAC) ? ", " : "") + property.Replace("MACAddress: ", "").Trim();
                            }
                        }

                        foreach (string property in dd.GetPropertyValues())
                        {
                            if (property.Contains("SerialNumber"))
                            {
                                pc.DiskSeri += (!string.IsNullOrEmpty(pc.DiskSeri) ? ", " : "") + property.Replace("SerialNumber: ", "").Trim();
                            }

                            if (property.Contains("Size"))
                            {
                                pc.DiskSize += (!string.IsNullOrEmpty(pc.DiskSize) ? ", " : "")
                                    + SizeSuffix(double.Parse(property.Replace("Size: ", "").Trim()));
                            }
                        }

                        foreach (string property in a.GetPropertyValues())
                        {
                            if (property.Contains("Manufacturer"))
                            {
                                pc.MainName += (!string.IsNullOrEmpty(pc.MainName) ? ", " : "") + property.Replace("Manufacturer: ", "").Trim();
                            }

                            if (property.Contains("Product"))
                            {
                                pc.MainProduct += (!string.IsNullOrEmpty(pc.MainProduct) ? ", " : "") + property.Replace("Product: ", "").Trim();
                            }
                        }

                        foreach (var pro in pd.GetPropertyValues())
                        {
                            if (!string.IsNullOrEmpty(pro))
                            {
                                pc.HasSEP = true;
                            }
                        }

                        foreach (var item in wo.GetPropertyValues())
                        {
                            if (item.Contains("BuildNumber"))
                            {
                                pc.WinBuild = (!string.IsNullOrEmpty(pc.WinBuild) ? ", " : "") + item.Replace("BuildNumber: ", "").Trim();
                            }

                            if (item.Contains("Caption"))
                            {
                                pc.WinCap = (!string.IsNullOrEmpty(pc.WinCap) ? ", " : "") + item.Replace("Caption: ", "").Trim();
                            }
                        }

                        foreach (var item in pr.GetPropertyValues())
                        {
                            if (item.Contains("Name"))
                            {
                                pc.ProName = (!string.IsNullOrEmpty(pc.ProName) ? ", " : "") + item.Replace("Name: ", "").Trim();
                            }

                            if (item.Contains("NumberOfCores"))
                            {
                                pc.ProCore = (!string.IsNullOrEmpty(pc.ProCore) ? ", " : "") + item.Replace("NumberOfCores: ", "").Trim();
                            }

                            if (item.Contains("NumberOfLogicalProcessors"))
                            {
                                pc.ProThread = (!string.IsNullOrEmpty(pc.ProThread) ? ", " : "") 
                                    + item.Replace("NumberOfLogicalProcessors: ", "").Trim();
                            }
                        }

                        /*foreach (string property in b.GetPropertyValues())
                        {
                            if (property.Contains("Name"))
                            {
                                pc.BiosName += (!string.IsNullOrEmpty(pc.BiosName) ? ", " : "") + property.Replace("Name: ", "").Trim();
                            }

                            if (property.Contains("Version"))
                            {
                                pc.BiosVersion += (!string.IsNullOrEmpty(pc.BiosVersion) ? ", " : "") + property.Replace("Version: ", "").Trim();
                            }
                        }*/



                        await Task.Delay(100);

                        string[] mac = pc.MAC.Split(',');

                        foreach (var item in mac)
                        {
                            Win32_NetworkAdapterConfiguration nac = new Win32_NetworkAdapterConfiguration(wmiConnection, item.Trim());

                            foreach (string property in nac.GetPropertyValues())
                            {
                                if (property.Contains("DHCPEnabled"))
                                {
                                    pc.DHCP += (!string.IsNullOrEmpty(pc.DHCP) ? ", " : "") + property.Replace("DHCPEnabled: ", "").Trim();
                                }
                            }
                        }
                    });
            }
            catch
            {
                
            }
        }//End

        /// <summary>
        /// Runs the given powershell script and returns the script output.
        /// </summary>
        /// <param name="scriptText">the powershell script text to run</param>
        /// <returns>output of the script</returns>
        /*private string RunScript(string scriptText)
        {


            string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
            PSCredential remoteCredential = new PSCredential("userID", StringToSecureString("Password"));
            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(false, "Ip Address of server", 5985, 
                "/wsman", shellUri, remoteCredential, 1 * 60 * 1000);

            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // add an extra command to transform the script output objects into nicely formatted strings
            // remove this line to get the actual objects that the script returns. For example, the script
            // "Get-Process" returns a collection of System.Diagnostics.Process instances.
            pipeline.Commands.Add("Out-String");

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }*/

        private readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private string SizeSuffix(double value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}
