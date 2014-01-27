//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class ExternalAddressHelper
    {
        private static readonly IExternalAddressProvider[] ExternalAddressProviders = new IExternalAddressProvider[]
        {
            new WhatIsMyIPAddressExternalAddressProvider(),
            new DynDnsExternalAddressProvider()
        };

        public static async Task<IPAddress> GetCurrentExternalAddressAsync()
        {
            foreach (var externalAddressProvider in ExternalAddressProviders)
            {
                using (var client = new HttpClient())
                {
                    if (externalAddressProvider.UserAgent != null)
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", externalAddressProvider.UserAgent);
                    }

                    var response = await GetResponseAsync(client, externalAddressProvider.Uri);
                    if (response == null)
                    {
                        continue;
                    }

                    var responseText = await response.Content.ReadAsStringAsync();

                    IPAddress address;
                    if (externalAddressProvider.TryParseResponseText(responseText, out address))
                    {
                        return address;
                    }
                }
            }
            return null;
        }

        private async static Task<HttpResponseMessage> GetResponseAsync(HttpClient client, Uri uri)
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(uri);
            }
            catch (HttpRequestException)
            {
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response;
        }

        private interface IExternalAddressProvider
        {
            string UserAgent { get; }
            Uri Uri { get; }
            bool TryParseResponseText(string text, out IPAddress address);
        }

        private sealed class WhatIsMyIPAddressExternalAddressProvider : IExternalAddressProvider
        {
            private static readonly Uri WhatIsMyIPAddressUri = new Uri("http://whatismyipaddress.com/");
            private static readonly Regex IPAddressRegex = new Regex(@"(?<addressText>[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)");

            public WhatIsMyIPAddressExternalAddressProvider()
            {
            }

            public string UserAgent
            {
                get { return "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0"; }
            }

            public Uri Uri
            {
                get { return WhatIsMyIPAddressUri; }
            }

            public bool TryParseResponseText(string text, out IPAddress address)
            {
                address = default(IPAddress);

                var match = IPAddressRegex.Match(text);
                if (!match.Success)
                {
                    return false;
                }

                var group = match.Groups["addressText"];

                IPAddress temp;
                if (!IPAddress.TryParse(group.Value, out temp))
                {
                    return false;
                }

                address = temp;
                return true;
            }
        }

        private sealed class DynDnsExternalAddressProvider : IExternalAddressProvider
        {
            private const string CurrentIPAddressTag = "Current IP Address: ";
            private const string EndBodyTag = "</body>";

            private static readonly Uri DynDnsUri = new Uri("http://checkip.dyndns.org/");

            public DynDnsExternalAddressProvider()
            {
            }

            public string UserAgent
            {
                get { return null; }
            }

            public Uri Uri
            {
                get { return DynDnsUri; }
            }

            public bool TryParseResponseText(string text, out IPAddress address)
            {
                address = default(IPAddress);

                if (String.IsNullOrWhiteSpace(text))
                {
                    return false;
                }

                var beginIndex = text.IndexOf(CurrentIPAddressTag) + CurrentIPAddressTag.Length;
                if (beginIndex == -1)
                {
                    return false;
                }

                var endIndex = text.IndexOf(EndBodyTag);
                if (endIndex == -1)
                {
                    return false;
                }

                var addressText = text.Substring(beginIndex, endIndex - beginIndex);

                IPAddress temp;
                if (!IPAddress.TryParse(addressText, out temp))
                {
                    return false;
                }

                address = temp;
                return true;
            }
        }
    }
}
