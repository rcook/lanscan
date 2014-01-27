//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Lanscan.Networking.Interop;

    public sealed class DhcpInfo
    {
        private readonly IPAddress m_gatewayAddress;
        private readonly IPAddress m_dnsServerAddress;
        private readonly string m_domainName;

        public DhcpInfo(IPAddress gatewayAddress, IPAddress dnsServerAddress, string domainName)
        {
            if (gatewayAddress == null)
            {
                throw new ArgumentNullException("gatewayAddress");
            }
            if (dnsServerAddress == null)
            {
                throw new ArgumentNullException("dnsServerAddress");
            }

            m_gatewayAddress = gatewayAddress;
            m_dnsServerAddress = dnsServerAddress;
            m_domainName = domainName;
        }

        public IPAddress GatewayAddress
        {
            get { return m_gatewayAddress; }
        }

        public IPAddress DnsServerAddress
        {
            get { return m_dnsServerAddress; }
        }

        public string DomainName
        {
            get { return m_domainName; }
        }

        public static DhcpInfo GetDhcpInfo(Guid networkAdapterId)
        {
            using (var wrapper = new DhcpWrapper())
            {
                DHCPCAPI_PARAMS outParameters;

                if (!wrapper.TryExecuteRequest(networkAdapterId, OPTION.ROUTER_ADDRESS, out outParameters) || outParameters.nBytesData != 4u)
                {
                    return null;
                }
                var gatewayAddress = GetAddress(outParameters.Data);

                if (!wrapper.TryExecuteRequest(networkAdapterId, OPTION.DOMAIN_NAME_SERVERS, out outParameters) || !IsValidAddressByteCount((int)outParameters.nBytesData))
                {
                    return null;
                }
                var dnsServerAddress = GetAddresses(outParameters.Data, (int)outParameters.nBytesData / 4).First();

                if (!wrapper.TryExecuteRequest(networkAdapterId, OPTION.DOMAIN_NAME, out outParameters))
                {
                    return null;
                }
                var domainName = GetDomainName(outParameters.Data, (int)outParameters.nBytesData);

                var result = new DhcpInfo(gatewayAddress, dnsServerAddress, domainName);
                return result;
            }
        }

        private static bool IsValidAddressByteCount(int count)
        {
            var result = count > 0 && count % 4 == 0;
            return result;
        }

        private static IPAddress[] GetAddresses(IntPtr data, int addressCount)
        {
            var addresses = new IPAddress[addressCount];
            for (int i = 0, offset = 0; i < addressCount; ++i, offset += 4)
            {
                addresses[i] = GetAddress(data, offset);
            }
            return addresses;
        }

        private static IPAddress GetAddress(IntPtr data, int offset = 0)
        {
            var addressBytes = (DHCPCAPI_IP_ADDRESS)Marshal.PtrToStructure(data + offset, typeof(DHCPCAPI_IP_ADDRESS));
            var result = new IPAddress(new[]
            {
                addressBytes.Value0,
                addressBytes.Value1,
                addressBytes.Value2,
                addressBytes.Value3
            });
            return result;
        }

        private static string GetDomainName(IntPtr data, int count)
        {
            if (data == IntPtr.Zero || count == 0)
            {
                return null;
            }

            var result = Marshal.PtrToStringAnsi(data, count);
            return result;
        }

        private sealed class DhcpWrapper : IDisposable
        {
            private const int BufferLength = 2000;

            private bool m_disposed;
            private SafeCoTaskMemoryHandle m_sendHandle = new SafeCoTaskMemoryHandle(Marshal.SizeOf(typeof(DHCPCAPI_PARAMS)));
            private SafeCoTaskMemoryHandle m_receiveHandle = new SafeCoTaskMemoryHandle(Marshal.SizeOf(typeof(DHCPCAPI_PARAMS)));
            private SafeGlobalMemoryHandle m_bufferHandle = new SafeGlobalMemoryHandle(BufferLength);

            public DhcpWrapper()
            {
                uint version;
                var errorCode = NativeMethods.DhcpCApiInitialize(out version);
                if (errorCode != 0u)
                {
                    throw new InvalidOperationException("DhcpCApiInitialize failed");
                }
            }

            ~DhcpWrapper()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public bool TryExecuteRequest(Guid networkAdapterId, OPTION optionId, out DHCPCAPI_PARAMS result)
            {
                result = default(DHCPCAPI_PARAMS);

                var sendPtr = m_sendHandle.DangerousGetHandle();
                var receivePtr = m_receiveHandle.DangerousGetHandle();
                var bufferPtr = m_bufferHandle.DangerousGetHandle();

                var sendParameter = default(DHCPCAPI_PARAMS);
                sendParameter.Flags = 0u;
                sendParameter.OptionId = OPTION.PAD;
                sendParameter.IsVendor = false;
                sendParameter.Data = IntPtr.Zero;
                sendParameter.nBytesData = 0u;
                Marshal.StructureToPtr(sendParameter, sendPtr, false);

                var sendParameters = default(DHCPCAPI_PARAMS_ARRAY);
                sendParameters.nParams = 1u;
                sendParameters.Params = sendPtr;

                var receiveParameter = default(DHCPCAPI_PARAMS);
                receiveParameter.Flags = 0u;
                receiveParameter.OptionId = optionId;
                receiveParameter.IsVendor = false;
                receiveParameter.Data = IntPtr.Zero;
                receiveParameter.nBytesData = 0u;
                Marshal.StructureToPtr(receiveParameter, receivePtr, false);

                var receiveParameters = default(DHCPCAPI_PARAMS_ARRAY);
                receiveParameters.nParams = 1u;
                receiveParameters.Params = receivePtr;

                var byteCount = (uint)BufferLength;
                var errorCode = NativeMethods.DhcpRequestParams(
                    DHCPCAPI_REQUEST.SYNCHRONOUS,
                    IntPtr.Zero,
                    networkAdapterId.ToString("b"),
                    IntPtr.Zero,
                    sendParameters,
                    receiveParameters,
                    bufferPtr,
                    ref byteCount,
                    null);
                if (errorCode != 0u)
                {
                    return false;
                }

                result = (DHCPCAPI_PARAMS)Marshal.PtrToStructure(receiveParameters.Params, typeof(DHCPCAPI_PARAMS));
                return true;
            }

            private void Dispose(bool disposing)
            {
                if (!m_bufferHandle.IsInvalid)
                {
                    m_bufferHandle.Dispose();
                    m_bufferHandle = null;
                }
                if (!m_receiveHandle.IsInvalid)
                {
                    m_receiveHandle.Dispose();
                    m_receiveHandle = null;
                }
                if (!m_sendHandle.IsInvalid)
                {
                    m_sendHandle.Dispose();
                    m_sendHandle = null;
                }
                if (!m_disposed)
                {
                    NativeMethods.DhcpCApiCleanup();
                    m_disposed = true;
                }
            }

            private static class NativeMethods
            {
                [DllImport("dhcpcsvc.dll")]
                public static extern uint DhcpCApiInitialize(out uint Version);

                [DllImport("dhcpcsvc.dll")]
                public static extern void DhcpCApiCleanup();

                [DllImport("dhcpcsvc.dll", CharSet = CharSet.Unicode)]
                public static extern uint DhcpRequestParams(
                    DHCPCAPI_REQUEST Flags,
                    IntPtr Reserved,
                    string AdapterName,
                    IntPtr ClassId,
                    DHCPCAPI_PARAMS_ARRAY SendParams,
                    DHCPCAPI_PARAMS_ARRAY RecdParams,
                    IntPtr Buffer,
                    ref uint pSize,
                    string RequestIdStr);
            }
        }
    }
}
