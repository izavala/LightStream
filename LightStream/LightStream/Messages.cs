using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightStream
{
    public class Messages
    {

        public class SendingFile {
            //public IActorRef _sender;

            //public SendingFile(IActorRef sender )
            //{
            //    _sender = sender;
            //}
        }

        public class ReceivingFile
        {
            public IActorRef _sender;

            public ReceivingFile(IActorRef sender)
            {
                _sender = sender;
            }
        }

        public class SendFile
        {
            public string _filePath;

            public SendFile(string FilePath)
            {
                _filePath = FilePath;
            }
        }
        public class SendBytes
        {
            public byte[] _bytes;
            public int _len;
            public int packetNumber;
            public SendBytes(byte[] by, int len, int package)
            {
                _len = len;
                _bytes = by;
                packetNumber = package;
            }
        }

        public class StartSream
        {
            public string _path;
            public int _len;
            public StartSream(string path, int len)
            {
                _path = path;
                _len = len;
            }
        }

        public class StopStream { };

        public class BeginConnection { };
        public class ConnectionEstablished { };
        public class InputSuccess
        {
            public InputSuccess(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }

        public class InputError
        {
            public InputError(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason) { }
        }

        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason) { }
        }
    }
}
