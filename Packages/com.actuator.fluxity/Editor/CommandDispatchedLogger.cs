using System;
using System.IO;

namespace AIR.Fluxity.Editor
{
    internal sealed class CommandDispatchedLogger : IDisposable
    {
        private const string CommandDispatchedTimeFormat = "HH:mm:ss.fff";
        private string _filePath;
        private FileStream _fileStream;
        private StreamWriter _stringWriter;

        public CommandDispatchedLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Dispose()
        {
            _stringWriter?.Flush();
            _fileStream?.Flush();
            _stringWriter?.Dispose();
            _fileStream?.Dispose();
            _stringWriter = null;
            _fileStream = null;
        }

        internal void Log(DispatchData dat)
        {
            if (_fileStream == null)
            {
                _fileStream = File.Create(_filePath);
                _stringWriter = new StreamWriter(_fileStream);
            }

            _stringWriter.WriteLine($"{dat.TimeStamp.ToString(CommandDispatchedTimeFormat)} - {dat.DispatchName}");
        }
    }
}