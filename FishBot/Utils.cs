using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Anilist4Net.Enums;
using Discord;

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

            IPAddress address = dnsServers[0];

            //set the ping options, TTL 128
            PingOptions pingOptions = new PingOptions(128, true);

            int numping = 4;

            //create a new ping instance
            Ping ping = new Ping();

            //32 byte buffer (create empty)
            byte[] buffer = new byte[32];

            long?[] pings = new long?[numping];

            //here we will ping the host numping times (standard)
            for (int i = 0; i < numping; i++)
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

            for (int i = 0; i < numping; i++)
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

    }