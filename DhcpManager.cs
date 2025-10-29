using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Timers;

namespace FrcDsAutoShutdown
{
    internal class DhcpManager
    {
        private static Dictionary<string, AddressTuple> previousInterfaceAddresses;
        public static IPAddress TeamIPNetwork = IPAddress.None;
        private static Timer recheckIPTimer;

        internal DhcpManager()
        {
            previousInterfaceAddresses = GetInterfaceAddresses();
            ConfigureTimer();
        }

        private static void ConfigureTimer(ElapsedEventHandler elapsed = null)
        {
            recheckIPTimer = new Timer(10000);
            if (elapsed != null)
            {
                recheckIPTimer.Elapsed += elapsed;
            }
            recheckIPTimer.AutoReset = true;
        }

        public static void StartListening(ElapsedEventHandler elapsed = null)
        {
            if (recheckIPTimer == null)
            {
                ConfigureTimer(elapsed);
            }
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);
            recheckIPTimer.Enabled = true;
        }

        public static void StopListening()
        {
            NetworkChange.NetworkAddressChanged -= new NetworkAddressChangedEventHandler(AddressChangedCallback);
            if (recheckIPTimer != null)
            {
                recheckIPTimer.Enabled = false;
                recheckIPTimer.Stop();
                recheckIPTimer.Dispose();
            }
        }

        private static void AddressChangedCallback(object sender, EventArgs e)
        {
            var currentSnapshot = GetInterfaceAddresses();
            foreach (var ni in currentSnapshot)
            {
                var niId = ni.Key;
                var (networkInterface, currentAddresses) = ni.Value.ToTuple();
                AddressTuple previousAddresses;
                previousInterfaceAddresses.TryGetValue(niId, out previousAddresses);
                if (previousAddresses == null || !currentAddresses.SequenceEqual(previousAddresses.Addresses))
                {
                    if (currentAddresses.Any(ip => ip.Equals(IPAddress.None)))
                    {
                        // Assume the game is over and DHCP has been released (ip gone)
                        ExternalProcessManager.KillProcessesAndShutDownIfAppEnabled();
                    }
                }
            }
        }

        private static Dictionary<string, AddressTuple> GetInterfaceAddresses()
        {
            var result = new Dictionary<string, AddressTuple>();
            var _teamIPNetwork = TeamIPNetwork = ExternalProcessManager.GetTeamNetworkIPAddressFromDSTeamNumberIni();
            if (TeamIPNetwork.Equals(IPAddress.None))
            {
                _teamIPNetwork = IPAddress.Parse("10.0.0.0");
            }
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var ipProps = ni.GetIPProperties();
                var addresses = ipProps.UnicastAddresses
                    .Select(ua => ua.Address)
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6)
                    .Where(ip => IPAddressesAreInSameSubnet(ip, _teamIPNetwork) || ip.Equals(IPAddress.None) )
                    .ToList();

                result[ni.Id] = new AddressTuple(ni, addresses);
            }
            return result;
        }

        private static bool IPAddressesAreInSameSubnet(IPAddress ip1, IPAddress ip2)
        {
            byte[] ipBytes1 = ip1.GetAddressBytes();
            byte[] ipBytes2 = ip2.GetAddressBytes();
            byte[] maskBytes = IPAddress.Parse("255.255.255.0").GetAddressBytes();

            for (int i = 0; i < ipBytes1.Length; i++)
            {
                if ((ipBytes1[i] & maskBytes[i]) != (ipBytes2[i] & maskBytes[i]))
                    return false;
            }

            return true;
        }
    }

    internal class AddressTuple
    {
        public NetworkInterface NetIf { get; set; }
        public List<IPAddress> Addresses { get; set; }

        public AddressTuple(NetworkInterface netIf, List<IPAddress> addresses)
        {
            NetIf = netIf;
            Addresses = addresses;
        }

        public (NetworkInterface NetIf, List<IPAddress> Addresses) ToTuple()
        {
            return (NetIf, Addresses);
        }

        public static AddressTuple FromNamedTuple((NetworkInterface NetIf, List<IPAddress> Addresses) inputTuple)
        {
            return new AddressTuple(inputTuple.NetIf, inputTuple.Addresses);
        }

        public static AddressTuple FromTuple((NetworkInterface, List<IPAddress>) inputTuple)
        {
            return new AddressTuple(inputTuple.Item1, inputTuple.Item2);
        }
    }
}
