//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Lanscan.Networking;
    using Lanscan.Networking.Dns;

    public static class Scanner
    {
        private const int TaskCount = 20;

        public static void ScanNetwork(CancellationToken userToken, DnsResolverBase resolver, IPAddressRange addresses, INetworkServiceCore[] networkServices, IProgress<ScanNetworkBatch> progress)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }
            if (addresses == null)
            {
                throw new ArgumentNullException("addresses");
            }
            if (networkServices == null)
            {
                throw new ArgumentNullException("networkServices");
            }
            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            var nestedTasks = new List<Task>();
            var chunkSize = GetChunkSize(addresses);
            foreach (var addressChunk in addresses.Chunk(chunkSize))
            {
                userToken.ThrowIfCancellationRequested();

                var nestedTask = Task.Factory.StartNew(() =>
                {
                    foreach (var address in addressChunk)
                    {
                        var results = new List<ScanNetworkResult>();
                        foreach (var networkService in networkServices)
                        {
                            userToken.ThrowIfCancellationRequested();

                            var endpoint = new IPEndpoint(address, networkService.Port);
                            var result = ScanEndpoint(userToken, resolver, networkService.Protocol, endpoint);
                            results.Add(result);
                        }
                        progress.Report(new ScanNetworkBatch(results.ToArray()));
                    }
                });
                nestedTasks.Add(nestedTask);
            }
            Task.WaitAll(nestedTasks.ToArray(), userToken);
        }

        private static int GetChunkSize(IPAddressRange addresses)
        {
            var chunkSize = addresses.Count / TaskCount;
            var result = chunkSize == 0 ? addresses.Count : chunkSize;
            return result;
        }

        private static ScanNetworkResult ScanEndpoint(CancellationToken userToken, DnsResolverBase resolver, Protocol protocol, IPEndpoint endpoint)
        {
            var status = Ping.PerformPing(endpoint, protocol);
            var hostName = status ? GetHostName(resolver, endpoint) : null;
            var result = new ScanNetworkResult(protocol, endpoint, hostName, status);
            return result;
        }

        private static string GetHostName(DnsResolverBase resolver, IPEndpoint endpoint)
        {
            if (resolver == null)
            {
                return null;
            }

            var result = resolver.GetHostName(endpoint.Address);
#if SCREENSHOT
            if (result != null)
            {
                result = result.Replace("example.com", "my-domain-name");
            }
#endif
            return result;
        }
    }
}
