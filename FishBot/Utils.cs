using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Anilist4Net;
using Anilist4Net.Enums;
using Discord;
using FishBot.Logging;

namespace FishBot;

public static class Utils
    {
        public static async Task<long?> PingDnsAsync()
        {
            List<NetworkInterface> Interfaces = new List<NetworkInterface>();
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    Interfaces.Add(nic);
                }
            }


            NetworkInterface result = null;
            foreach (NetworkInterface nic in Interfaces)
            {
                if (result == null)
                {
                    result = nic;
                }
                else
                {
                    if (nic.GetIPProperties().GetIPv4Properties() != null)
                    {
                        if (nic.GetIPProperties().GetIPv4Properties().Index < result.GetIPProperties().GetIPv4Properties().Index)
                            result = nic;
                    }
                }
            }
            if (result == null)
            {
                return null;
            }
            IPInterfaceProperties adapterProperties = result.GetIPProperties();
            IPAddressCollection dnsServers = adapterProperties.DnsAddresses;

            return await dnsServers[0].PingAsync();
        }

        public static async Task<long?> PingAsync(string address, int count = 4)
        {
            PingOptions pingOptions = new PingOptions(128, true);

            //create a new ping instance
            Ping ping = new Ping();

            //32 byte buffer (create empty)
            byte[] buffer = new byte[32];

            long?[] pings = new long?[count];

            //here we will ping the host numping times (standard)
            for (int i = 0; i < count; i++)
            {
                try
                {
                    //send the ping numping times to the host and record the returned data.
                    //The Send() method expects numping items:
                    //1) The IPAddress we are pinging
                    //2) The timeout value
                    //3) A buffer (our byte array)
                    //numping) PingOptions
                    PingReply pingReply = await ping.SendPingAsync(address, 1000, buffer, pingOptions);

                    //make sure we dont have a null reply
                    if (!(pingReply == null))
                    {
                        switch (pingReply.Status)
                        {
                            case IPStatus.Success:
                                pings[i] = pingReply.RoundtripTime;
                                break;
                            case IPStatus.TimedOut:
                                pings[i] = null;
                                break;
                            default:
                                pings[i] = null;
                                break;
                        }
                    }
                    else
                    {
                        pings[i] = null;
                    }

                }
                catch (PingException ex)
                {
                    pings[i] = null;
                }
                catch (SocketException ex)
                {
                    pings[i] = null;
                }
            }
            int notnull = 0;
            long? returnValue = null;

            for (int i = 0; i < count; i++)
            {
                if (pings[i].HasValue)
                {
                    notnull++;
                    if (returnValue.HasValue)
                    {
                        returnValue += pings[i].Value;
                    }
                    else
                    {
                        returnValue = pings[i].Value;
                    }
                }
            }
            returnValue /= notnull;

            //return the message
            return returnValue;
        }

        public static async Task<long?> PingAsync(this IPAddress address, int count = 4)
        {
            PingOptions pingOptions = new PingOptions(128, true);

            //create a new ping instance
            Ping ping = new Ping();

            //32 byte buffer (create empty)
            byte[] buffer = new byte[32];

            long?[] pings = new long?[count];

            //here we will ping the host numping times (standard)
            for (int i = 0; i < count; i++)
            {
                try
                {
                    //send the ping numping times to the host and record the returned data.
                    //The Send() method expects numping items:
                    //1) The IPAddress we are pinging
                    //2) The timeout value
                    //3) A buffer (our byte array)
                    //numping) PingOptions
                    PingReply pingReply = await ping.SendPingAsync(address, 1000, buffer, pingOptions);

                    //make sure we dont have a null reply
                    if (!(pingReply == null))
                    {
                        switch (pingReply.Status)
                        {
                            case IPStatus.Success:
                                pings[i] = pingReply.RoundtripTime;
                                break;
                            case IPStatus.TimedOut:
                                pings[i] = null;
                                break;
                            default:
                                pings[i] = null;
                                break;
                        }
                    }
                    else
                    {
                        pings[i] = null;
                    }

                }
                catch (PingException ex)
                {
                    pings[i] = null;
                }
                catch (SocketException ex)
                {
                    pings[i] = null;
                }
            }
            int notnull = 0;
            long? returnValue = null;

            for (int i = 0; i < count; i++)
            {
                if (pings[i].HasValue)
                {
                    notnull++;
                    if (returnValue.HasValue)
                    {
                        returnValue += pings[i].Value;
                    }
                    else
                    {
                        returnValue = pings[i].Value;
                    }
                }
            }
            returnValue /= notnull;

            //return the message
            return returnValue;
        }

        public static Color ToColor(this UserProfileColours color) =>
            color switch
            {
                UserProfileColours.blue => Color.Blue,

                UserProfileColours.gray => Color.DarkGrey,

                UserProfileColours.green => Color.Green,

                UserProfileColours.orange => Color.Orange,

                UserProfileColours.pink => Color.Default,

                UserProfileColours.purple => Color.Purple,

                UserProfileColours.red => Color.Red,

                _ => Color.Default
            };

        public static readonly Dictionary<string, string> Replace = new Dictionary<string, string>
        {
            {"<br>\n", "\n"},
            {"<br>", "\n"},
            {"<i>", "*"},
            {"</i>", "*"},
            {"<b>", "**"},
            {"</b>", "**"}
        };

        private static StringBuilder builder = new StringBuilder();
        public static string FormatMarkdown(string md)
        {
            builder.Clear();
            builder.Append(md);
            foreach (var pair in Replace)
            {
                builder.Replace(pair.Key, pair.Value);
            }
            return builder.ToString();
        }

        public static string FormatMediaRelations(MediaRelation[] relations)
        {
            string formatted = "";
            formatted += $"[{relations[0].Media.RomajiTitle}]({relations[0].Media.SiteUrl}), ";
            formatted += $"[{relations[1].Media.RomajiTitle}]({relations[1].Media.SiteUrl}), ";
            formatted += $"[{relations[2].Media.RomajiTitle}]({relations[2].Media.SiteUrl}), ";
            formatted += $"[{relations[3].Media.RomajiTitle}]({relations[3].Media.SiteUrl}), ";
            formatted += $"[{relations[4].Media.RomajiTitle}]({relations[4].Media.SiteUrl})";

            return formatted;
        }

        public static async Task LogAsync(this Exception e, LogType type = LogType.Commands)
        {
            await Program.Logger.LogAsync(new Logging.LogMessage(e.Message, LogType.Commands, LogLevel.Error));
            await Program.Logger.LogAsync(new Logging.LogMessage(e.StackTrace, LogType.Commands, LogLevel.Trace));
        }
        
        private static readonly Dictionary<LogLevel, string> SeverityColors = new Dictionary<LogLevel, string>()
        {
            {LogLevel.Trace, "\u001b[0m"},//grey
            {LogLevel.Debug, "\u001b[37m"},//white
            {LogLevel.Info, "\u001b[36m"},//cyan
            {LogLevel.Warning, "\u001b[33m"},//yellow
            {LogLevel.Error, "\u001b[31m"},//red
            {LogLevel.Fatal, "\u001b[35m"}//magenta
        };
        
        public static string GetColor(this LogLevel level)
        {
            SeverityColors.TryGetValue(level, out string code);
            return code;
        }
        
        private static readonly Dictionary<LogType, string> TypeColors = new Dictionary<LogType, string>()
        {
            {LogType.Runtime, "\u001b[37m"},//white
            {LogType.Commands, "\u001b[36m"},//cyan
            {LogType.Api, "\u001b[33m"},//yellow
            {LogType.Network, "\u001b[35m"},//red
        };
        
        public static string GetColor(this LogType type)
        {
            TypeColors.TryGetValue(type, out string code);
            return code;
        }
    }