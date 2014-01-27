//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
//
// Based on Alphons van der Heijden's code presented at
// http://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C
//
// Licensed under Code Project Open License (CPOL) 1.02
// http://www.codeproject.com/info/cpol10.aspx
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Networking.Dns.Records
{
    using System;
    using System.Globalization;
    using System.Text;

    public sealed class LOCRecord : Record
    {
        private readonly byte m_version;
        private readonly byte m_size;
        private readonly byte m_horizPre;
        private readonly byte m_vertPre;
        private readonly uint m_latitude;
        private readonly uint m_longitude;
        private readonly uint m_altitude;

        public LOCRecord(RR rr, RecordReader recordReader)
            : base(rr)
        {
            if (recordReader == null)
            {
                throw new ArgumentNullException("recordReader");
            }

            m_version = recordReader.ReadByte(); // must be 0!
            m_size = recordReader.ReadByte();
            m_horizPre = recordReader.ReadByte();
            m_vertPre = recordReader.ReadByte();
            m_latitude = recordReader.ReadUInt32();
            m_longitude = recordReader.ReadUInt32();
            m_altitude = recordReader.ReadUInt32();
        }

        public byte VERSION
        {
            get { return m_version; }
        }

        public byte SIZE
        {
            get { return m_size; }
        }

        public byte HORIZPRE
        {
            get { return m_horizPre; }
        }

        public byte VERTPRE
        {
            get { return m_vertPre; }
        }

        public uint LATITUDE
        {
            get { return m_latitude; }
        }

        public uint LONGITUDE
        {
            get { return m_longitude; }
        }

        public uint ALTITUDE
        {
            get { return m_altitude; }
        }

        public override string ToString()
        {
            var result = String.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5}",
                ToTime(LATITUDE, 'S', 'N'),
                ToTime(LONGITUDE, 'W', 'E'),
                ToAlt(ALTITUDE),
                SizeToString(SIZE),
                SizeToString(HORIZPRE),
                SizeToString(VERTPRE));
            return result;
        }

        private static string SizeToString(byte s)
        {
            var units = "cm";
            var baseValue = s >> 4;
            var power = s & 0x0f;
            if (power >= 2)
            {
                power -= 2;
                units = "m";
            }
            /*
            if (power >= 3)
            {
                power -= 3;
                units = "km";
            }
            */
            var output = new StringBuilder();
            output.AppendFormat("{0}", baseValue);
            for (; power > 0; --power)
            {
                output.Append('0');
            }
            output.Append(units);

            var result = output.ToString();
            return result;
        }

        private static string LonToTime(uint r)
        {
            var mid = 2147483648u; // 2^31
            var dir = 'E';
            if (r > mid)
            {
                dir = 'W';
                r -= mid;
            }
            var h = (double)r / (360000.0d * 10.0d);
            var m = 60.0d * (h - (int)h);
            var s = 60.0d * (m - (int)m);
            var result = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2:0.000} {3}", (int)h, (int)m, s, dir);
            return result;
        }

        private static string ToTime(uint r, char below, char above)
        {
            var mid = 2147483648u; // 2^31
            var dir = '?';
            if (r > mid)
            {
                dir = above;
                r -= mid;
            }
            else
            {
                dir = below;
                r = mid - r;
            }
            var h = (double)r / (360000.0d * 10.0d);
            var m = 60.0d * (h - (int)h);
            var s = 60.0d * (m - (int)m);
            var result = String.Format(CultureInfo.InvariantCulture, "{0} {1} {2:0.000} {3}", (int)h, (int)m, s, dir);
            return result;
        }

        private static string ToAlt(uint a)
        {
            var alt = (a / 100.0) - 100000.00;
            var result = String.Format(CultureInfo.InvariantCulture, "{0:0.00}m", alt);
            return result;
        }
    }
}
