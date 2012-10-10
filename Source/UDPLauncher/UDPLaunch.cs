namespace UDPLauncher
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using UDPLauncher.Properties;

    internal class UDPLaunch
    {
        private bool IntermiddetError;
        private TextWriter Logger = new StreamWriter(Environment.CurrentDirectory + @"\log.txt");

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public bool IsProcessOpen(string CheckProgram)
        {
            ManagementObjectCollection objects = new ManagementObjectSearcher("SELECT * FROM WIN32_Process").Get();
            if (objects != null)
            {
                foreach (ManagementObject obj in objects)
                {
                    if (((string) obj["ExecutablePath"]) != null)
                    {
                        string str = (string) obj["ExecutablePath"];
                        if (str.Contains(CheckProgram))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsProcessOpen(string CheckProgram, out uint RunningID)
        {
            ManagementObjectCollection objects = new ManagementObjectSearcher("SELECT ProcessID,ExecutablePath FROM WIN32_Process").Get();
            if (objects != null)
            {
                foreach (ManagementObject obj2 in objects)
                {
                    if (((string) obj2["ExecutablePath"]) != null)
                    {
                        string str = (string) obj2["ExecutablePath"];
                        if (str.Contains(CheckProgram))
                        {
                            RunningID = (uint) obj2["ProcessID"];
                            return true;
                        }
                    }
                }
            }
            RunningID = 0;
            return false;
        }

        private static int Main(string[] args)
        {
            Console.Title = "UDPLauncher_Silent";
            IntPtr hWnd = FindWindow(null, "UDPLauncher_Silent");
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
            TextWriter writer = new StreamWriter(Environment.CurrentDirectory + @"\log.txt");
            writer.WriteLine("------   UDPLauncher Version 1.0   ------");
            writer.WriteLine("Listening on port: " + Settings.Default.PortToListenTo);
            writer.WriteLine("Launching Program: " + Settings.Default.ProgramToLaunch + "\n");
            writer.Close();
            UDPLaunch launch = new UDPLaunch();
            if (((Settings.Default.PortToListenTo != 0) && (Settings.Default.PortToListenTo <= 0xffff)) && ((Settings.Default.ProgramToLaunch != null) && (Settings.Default.ProgramToLaunch != "")))
            {
                if (!Settings.Default.UseXBMCEvent)
                {
                    launch.UDPListener(Settings.Default.PortToListenTo, Settings.Default.ProgramToLaunch, Settings.Default.ExitIfOpen);
                }
                else
                {
                    launch.UDPListener(Settings.Default.PortToListenTo, Settings.Default.ProgramToLaunch, Settings.Default.ExitIfOpen, Settings.Default.UseXBMCEvent, Settings.Default.XBMCEvent, Settings.Default.XBMC_Server, Settings.Default.XBMC_Port, Settings.Default.XBMC_Username, Settings.Default.XBMC_Password);
                }
            }
            else
            {
                writer.WriteLine("You specified uncorrect settings. I have to terminate, I cannot figure out what you want...terminating");
                launch.IntermiddetError = true;
            }
            if (launch.IntermiddetError)
            {
                return 10;
            }
            return 0;
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public void UDPListener(int Port, string Program, bool ExitIfOpen)
        {
            bool flag = false;
            try
            {
                UdpClient client = new UdpClient(Port);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, Port);
                Process process = new Process {
                    StartInfo = { FileName = Program }
                };
                while (!flag)
                {
                    this.Logger.WriteLine("Waiting for UDP-Packet...");
                    if (client.Receive(ref remoteEP) != null)
                    {
                        this.Logger.WriteLine("We just received a Packet.");
                        this.Logger.WriteLine("Checking for: " + Program);
                        if (ExitIfOpen)
                        {
                            uint num;
                            if (this.IsProcessOpen(Program, out num))
                            {
                                this.Logger.WriteLine("We have a match, Program is running");
                                this.Logger.WriteLine("Now we have to kill it, the ID is: " + num);
                                Process.GetProcessById(Convert.ToInt32(num)).Kill();
                                this.Logger.WriteLine("Done!");
                            }
                            else
                            {
                                this.Logger.WriteLine("Program is not running...launching!");
                                process.Start();
                            }
                        }
                        else if (this.IsProcessOpen(Program))
                        {
                            this.Logger.WriteLine("Program already running...doing nothing!");
                        }
                        else
                        {
                            this.Logger.WriteLine("Program is not running...launching!");
                            process.Start();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.IntermiddetError = true;
                flag = true;
                this.Logger.WriteLine(exception.ToString() + "\n");
                this.Logger.WriteLine("ERROR! Terminating!");
            }
            if (flag)
            {
                this.Logger.WriteLine("\nI received an error at some point...I have to terminate.");
            }
        }

        public void UDPListener(int Port, string Program, bool ExitIfOpen, bool UseXBMCEvents, string XBMCEvent, string XBMC_Server, int XBMC_Port, string XBMC_Username, string XBMC_Password)
        {
            bool flag = false;
            try
            {
                UdpClient client = new UdpClient(Port);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, Port);
                Process process = new Process {
                    StartInfo = { FileName = Program }
                };
                while (!flag)
                {
                    this.Logger.WriteLine("Waiting for message...");
                    if (client.Receive(ref remoteEP) != null)
                    {
                        uint num;
                        this.Logger.WriteLine("We just received a Message.");
                        this.Logger.WriteLine("Checking for: " + Program);
                        if (this.IsProcessOpen(Program, out num))
                        {
                            if (ExitIfOpen)
                            {
                                this.Logger.WriteLine("We have a match, Program is running!");
                                this.Logger.WriteLine("Now we have to kill it, the ID is: " + num);
                                Process.GetProcessById(Convert.ToInt32(num)).Kill();
                                this.Logger.WriteLine("Done!");
                            }
                            if (UseXBMCEvents)
                            {
                                this.Logger.WriteLine("We have a match, Program is running!");
                                this.Logger.WriteLine(string.Concat(new object[] { "Firing ", XBMCEvent, " to the XBMC at ", XBMC_Server, ":", XBMC_Port }));
                                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Concat(new object[] { "http://", XBMC_Server, ":", XBMC_Port, "/xbmcCmds/xbmcHttp?command=", XBMCEvent }));
                                if ((XBMC_Username != "") && (XBMC_Password != ""))
                                {
                                    request.Credentials = new NetworkCredential(XBMC_Username, XBMC_Password);
                                }
                                try
                                {
                                    request.GetResponse();
                                }
                                catch (WebException exception)
                                {
                                    this.Logger.WriteLine("The Webserver returned an error. Please check your settings!\n" + exception.Message);
                                    this.IntermiddetError = true;
                                    flag = true;
                                }
                            }
                        }
                        else
                        {
                            this.Logger.WriteLine("Program is not running...launching!");
                            process.Start();
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                this.IntermiddetError = true;
                flag = true;
                this.Logger.WriteLine(exception2.ToString() + "\n");
                this.Logger.WriteLine("ERROR! Terminating!");
            }
        }
    }
}

