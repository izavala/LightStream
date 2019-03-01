using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static LightStream.Messages;

namespace LightStream
{
    public class FileReceive : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private string _fileName;
        private byte[] buffer;
        private int _pt;
        private readonly string _fileDirectory;

        public FileReceive(string fileDirectory)
        {
            _fileDirectory = fileDirectory;
            StartToRead();
        }

        public void StartToRead()
        {
            Receive<StartSream>(start =>
            {
                _log.Info("File exchange started for {0}. The total bytes of the file is: {1}", start._path,start._len);
                buffer = new byte[start._len];
                _fileName = start._path;
                _pt = 0;
            });

            Receive<SendBytes>(b =>
            {
                _log.Info("Packet received with with {0} bytes",b._len);
                _log.Info("package {0} received.", b.packetNumber);
                for (var i = 0; i<b._len;i++)
                {
                    buffer[_pt++] = b._bytes[i];
                }
            });
            Receive<StopStream>( b =>
            {
                _log.Info("Stream ended");
                _log.Info("Buffer has {0}", buffer.Length);
                SaveFile();
                Context.Stop(Self);
            });

        }

        private void SaveFile()
        {

            
            File.WriteAllBytesAsync(Path.Combine(_fileDirectory,_fileName), buffer);
            
        }
    }
}
