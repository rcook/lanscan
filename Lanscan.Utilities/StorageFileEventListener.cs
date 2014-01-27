//////////////////////////////////////////////////////////////////////
//
// Lanscan
// Copyright (C) 2013, Richard Cook. All rights reserved.
//
//////////////////////////////////////////////////////////////////////

namespace Lanscan.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Windows.Storage;

    public sealed class StorageFileEventListener : EventListener
    {
        private const string MessageFormat = "{0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff}\tType: {1}\tId: {2}\tMessage: '{3}'";

        private readonly string m_name;
        private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1);
        private StorageFile m_storageFile;

        public StorageFileEventListener(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name", "name");
            }

            m_name = name;
            AssignLocalFile();
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData == null)
            {
                throw new ArgumentNullException("eventData");
            }

            if (m_storageFile == null)
            {
                return;
            }

            var lines = new List<string>();
            var line = String.Format(
                CultureInfo.InvariantCulture,
                MessageFormat,
                DateTime.Now,
                eventData.Level,
                eventData.EventId,
                eventData.Payload[0]);
            lines.Add(line);
            WriteToFile(lines);
        }

        private async void AssignLocalFile()
        {
            m_storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                m_name.Replace(" ", "_") + ".log",
                CreationCollisionOption.OpenIfExists);
        }

        private async void WriteToFile(IEnumerable<string> lines)
        {
            await m_semaphore.WaitAsync();

            await Task.Run(async () =>
            {
                try
                {
                    await FileIO.AppendLinesAsync(m_storageFile, lines);
                }
                catch (Exception)
                {
                }
                finally
                {
                    m_semaphore.Release();
                }
            });
        }
    }
}
