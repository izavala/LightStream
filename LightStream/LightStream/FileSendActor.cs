using Akka.Actor;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using static LightStream.Messages;

namespace LightStream
{
    public class FileSendActor : ReceiveActor , IDisposable
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly string _filePath;
        private readonly string _fileName;
        private readonly int MAXMESSAGESIZE = 12000;
        private readonly ICanTell _buddy;
        public FileSendActor(string FilePath, string FileName, ICanTell buddy)
        {
            _filePath = FilePath;
            _fileName = FileName;
            _buddy = buddy;
            StartToSend();
        }
        
        private void StartToSend()
        {
            Receive<SendBytes>(b=>
            {
                int loops = b._len/ MAXMESSAGESIZE;
                int remainder = b._len % MAXMESSAGESIZE;
                int _pt = 0;
                _log.Info("Sart");
                _buddy.Tell(new StartSream(_fileName, b._len),Self);
                for (var i=0; i< loops; i++)
                {
                    byte[] tempBuffer = new byte[MAXMESSAGESIZE];

                    for (var ii = 0; ii< MAXMESSAGESIZE; ii++)
                    {

                        tempBuffer[ii] = b._bytes[_pt++];
                        
                    }
                    _log.Info("package{0} of {1} sent.", i, loops);
                    _buddy.Tell(new SendBytes(tempBuffer, MAXMESSAGESIZE, i),Self);
                }
                if(remainder!=0)
                {
                    byte[] tempBuffer = new byte[MAXMESSAGESIZE];

                    for (var i = 0; i < remainder; i++)
                    {
                        tempBuffer[i] = b._bytes[_pt++];
                    }
                    _buddy.Tell(new SendBytes(tempBuffer, remainder,0),Self);
                }
                
                _buddy.Tell(new StopStream { },Self);

                Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(1.5), Self, PoisonPill.Instance, ActorRefs.NoSender);
                
            });

        }

        protected override void PreStart()
        {
            //var fileToBytes = File.ReadAllBytes(Path.GetFullPath(_filePath));
            //var directory = Direc
            _log.Info("Beginning to read {0}", _fileName);
            var fileToBytes = File.ReadAllBytes(_filePath);
            var len = fileToBytes.Length;
            Self.Tell(new SendBytes(fileToBytes, len,0));
        }

        public void Dispose()
        {
            //nothing to dispose.
        }
    }
}
