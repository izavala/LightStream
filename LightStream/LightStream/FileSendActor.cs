using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static LightStream.Messages;

namespace LightStream
{
    class FileSendActor : ReceiveActor , IDisposable
    {
        private readonly string _filePath;
        private readonly int MAXMESSAGESIZE = 12000;
        private readonly ICanTell _buddy;
        public FileSendActor(string FilePath, ICanTell buddy)
        {
            _filePath = FilePath;
            _buddy = buddy;
            StartToSend();
        }
        
        private void StartToSend()
        {
            Receive<SendBytes>(b=>
            {
                int loops = (int)b._len/ MAXMESSAGESIZE;
                int remainder = b._len % MAXMESSAGESIZE;
                byte[] tempBuffer = new byte[MAXMESSAGESIZE];
                int _pt = 0;

                _buddy.Tell(new Messages.StartSream(_filePath, b._len),Self);
                for (var i=0; i< loops; i++)
                {
                    for(var ii = 0; ii< MAXMESSAGESIZE; ii++)
                    {
                        tempBuffer[ii] = b._bytes[_pt];
                        _pt++;
                    }
                    _buddy.Tell(new Messages.SendBytes(tempBuffer, MAXMESSAGESIZE),Self);
                    Array.Clear(tempBuffer, 0, tempBuffer.Length);
                }
                if(remainder!=0)
                {
                    for (var i = 0; i < remainder; i++)
                    {
                        tempBuffer[i] = b._bytes[_pt];
                        _pt++;
                    }
                    _buddy.Tell(new Messages.SendBytes(tempBuffer, remainder),Self);
                    Array.Clear(tempBuffer, 0, tempBuffer.Length);
                }
                _buddy.Tell(new Messages.StopStream { },Self);
            });

        }

        protected override void PreStart()
        {
            var fileToBytes = File.ReadAllBytes(Path.GetFullPath(_filePath));
            var len = fileToBytes.Length;
            Self.Tell(new SendBytes(fileToBytes, len));
        }

        public void Dispose()
        {
            //nothing to dispose.
        }
    }
}
