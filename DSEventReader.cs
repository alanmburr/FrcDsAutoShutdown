using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrcDsAutoShutdown
{
    // https://github.com/orangelight/DSLOG-Reader/tree/master/DSLOG-Reader%202/DSLOG-Reader-Library
    internal class DSEventReader : Reader
    {
        public readonly List<DSEventsEntry> Entries;

        private Regex FMSMatchRegex = new Regex(@"(FMS Connected:)|(FMS Event Name:)|(FMS Disconnect)|(FMS-GOOD FRC)", RegexOptions.Compiled);
        private Regex FMSEventRegex = new Regex(@"(FMS Connected:)|(FMS Event Name:)", RegexOptions.Compiled);
        private bool ReadOnlyFms = false;

        public DSEventReader(string path) : base(path)
        {
            Entries = new List<DSEventsEntry>();
        }

        public override void Read()
        {
            if (ReadFile())
            {
                reader.Close();
            }
        }

        public void ReadForFMS()
        {
            ReadOnlyFms = true;
            if (ReadFile())
            {
                reader.Close();
            }
        }

        protected override bool ReadEntries()
        {
            bool isFMSMatch = false;
            bool haveName = false;
            bool haveNum = false;
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                if (!ReadOnlyFms)
                {
                    Entries.Add(ReadEntry());
                }
                else
                {

                    var entry = ReadEntry();
                    if (!isFMSMatch)
                    {
                        if (FMSMatchRegex.Match(entry.Data).Success)
                        {
                            isFMSMatch = true;
                            if (entry.Data.Contains("FMS Connected:")) haveNum = true;
                            if (entry.Data.Contains("FMS Event Name:")) haveName = true;
                            Entries.Add(entry);
                        }
                    }
                    else
                    {
                        if (FMSEventRegex.Match(entry.Data).Success)
                        {
                            if (entry.Data.Contains("FMS Connected:")) haveNum = true;
                            if (entry.Data.Contains("FMS Event Name:")) haveName = true;
                            Entries.Add(entry);
                            if (haveName && haveNum) break;
                        }
                    }
                }

            }

            return true;
        }

        protected DSEventsEntry ReadEntry()
        {
            DateTime time = Util.FromLVTime(reader.ReadInt64(), reader.ReadUInt64());
            Int32 l = reader.ReadInt32();
            string s = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(l));
            return new DSEventsEntry(time, s);
        }
    }

    public class DSEventsEntry
    {
        public readonly DateTime Time;
        public readonly String Data;
        public readonly String TimeData;

        public DSEventsEntry(DateTime time, string s)
        {
            Time = time;
            Data = s;
            TimeData = time.ToString("h:mm:ss.fff tt");
        }
    }

    public static class Util
    {
        public static byte[] Reverse(this byte[] b)
        {
            Array.Reverse(b);
            return b;
        }

        public static DateTime FromLVTime(long unixTime, UInt64 offset)
        {
            var epoch = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            epoch = epoch.AddSeconds(unixTime);
            epoch = TimeZoneInfo.ConvertTimeFromUtc(epoch, TimeZoneInfo.Local);

            return epoch.AddSeconds((double)offset / UInt64.MaxValue);
        }

        public static uint ReadUInt32Little(this BigEndianBinaryReader reader)
        {
            if (!BitConverter.IsLittleEndian) return reader.ReadUInt32();
            return BitConverter.ToUInt32(reader.ReadBytes(sizeof(UInt32)), 0);
        }
    }
    public class BigEndianBinaryReader : BinaryReader
    {
        private bool Reverse = true;
        public BigEndianBinaryReader(Stream stream) : base(stream)
        {
            Reverse = BitConverter.IsLittleEndian;
        }

        public override int ReadInt32()
        {
            if (!Reverse) return base.ReadInt32();
            return BitConverter.ToInt32(base.ReadBytes(sizeof(Int32)).Reverse(), 0);
        }
        public override Int16 ReadInt16()
        {
            if (!Reverse) return base.ReadInt16();
            return BitConverter.ToInt16(base.ReadBytes(sizeof(Int16)).Reverse(), 0);
        }
        public override Int64 ReadInt64()
        {
            if (!Reverse) return base.ReadInt64();
            return BitConverter.ToInt64(base.ReadBytes(sizeof(Int64)).Reverse(), 0);
        }

        public override UInt64 ReadUInt64()
        {
            if (!Reverse) return base.ReadUInt64();
            return BitConverter.ToUInt64(base.ReadBytes(sizeof(UInt64)).Reverse(), 0);
        }
        public override UInt32 ReadUInt32()
        {
            if (!Reverse) return base.ReadUInt32();
            return BitConverter.ToUInt32(base.ReadBytes(sizeof(UInt32)).Reverse(), 0);
        }

        public override UInt16 ReadUInt16()
        {
            if (!Reverse) return base.ReadUInt16();
            return BitConverter.ToUInt16(base.ReadBytes(sizeof(UInt16)).Reverse(), 0);
        }
    }

    public abstract class Reader
    {
        protected BigEndianBinaryReader reader;
        public int Version { get; private set; }
        public DateTime StartTime { get; private set; }
        public string Path { get; private set; }

        public Reader(string path)
        {
            Path = path;
            Version = -1;
        }

        public abstract void Read();

        public void OnlyReadMetaData()
        {
            if (reader != null)
            {
                return;//Throw something
            }
            if (!File.Exists(Path))
            {
                return;//Throw something
            }
            reader = new BigEndianBinaryReader(File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            ReadMetadata();
            reader.Close();
        }

        protected virtual void ReadMetadata()
        {
            if (reader == null) return;//Throw something
            Version = reader.ReadInt32();
            StartTime = Util.FromLVTime(reader.ReadInt64(), reader.ReadUInt64());
        }

        protected bool ReadFile()
        {
            if (reader != null)
            {
                return false;//Throw something
            }
            if (!File.Exists(Path))
            {
                return false;//Throw something
            }
            reader = new BigEndianBinaryReader(File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            ReadMetadata();
            if (Version != 4) return false;
            return ReadEntries();
        }

        protected abstract bool ReadEntries();

    }
    public enum PDPType
    {
        Unknown,
        CTRE,
        REV,
        None,
    }
}
