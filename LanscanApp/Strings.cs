//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace LanscanApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Windows.ApplicationModel.Resources;

    public static class Strings
    {
        private static readonly ResourceLoader ResourceLoader = new ResourceLoader();

        public static string ActionBarControl_SelectFirstNUsableAddressesFormat
        {
            get { return ResourceLoader.GetString("Strings_ActionBarControl_SelectFirstNUsableAddressesFormat"); }
        }

        public static string ActionBarControl_SelectWholeNetwork
        {
            get { return ResourceLoader.GetString("Strings_ActionBarControl_SelectWholeNetwork"); }
        }

        public static string ProtocolToStringConverter_Tcp
        {
            get { return ResourceLoader.GetString("Strings_ProtocolToStringConverter_Tcp"); }
        }

        public static string ProtocolToStringConverter_Udp
        {
            get { return ResourceLoader.GetString("Strings_ProtocolToStringConverter_Udp"); }
        }

        public static string Status_GetConnectionInfoFailed
        {
            get { return ResourceLoader.GetString("Strings_Status_GetConnectionInfoFailed"); }
        }

        public static string Status_GetHostInfoFailed
        {
            get { return ResourceLoader.GetString("Strings_Status_GetHostInfoFailed"); }
        }

        public static string Status_GetDhcpInfoFailed
        {
            get { return ResourceLoader.GetString("Strings_Status_GetDhcpInfoFailed"); }
        }

        public static string Status_GetExternalAddressFailed
        {
            get { return ResourceLoader.GetString("Strings_Status_GetExternalAddressFailed"); }
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Cancelled")]
        public static string Status_ScanCancelled
        {
            get { return ResourceLoader.GetString("Strings_Status_ScanCancelled"); }
        }

        public static string Status_ScanCompletedFormat
        {
            get { return ResourceLoader.GetString("Strings_Status_ScanCompletedFormat"); }
        }

        public static string Status_ScanStarted
        {
            get { return ResourceLoader.GetString("Strings_Status_ScanStarted"); }
        }

        public static string Format_ActionBarControl_SelectFirstNUsableAddressesFormat(int count)
        {
            var result = String.Format(
                CultureInfo.CurrentCulture,
                ActionBarControl_SelectFirstNUsableAddressesFormat,
                count);
            return result;
        }

        public static string Format_Status_ScanCompleted(TimeSpan timeSpan)
        {
            var result = String.Format(
                CultureInfo.CurrentCulture,
                Status_ScanCompletedFormat,
                timeSpan);
            return result;
        }
    }
}
