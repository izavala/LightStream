using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightStream
{
    class FileReceive : ReceiveActor
    {
        private string _filePath;

        public FileReceive(string FilePath)
        {
            _filePath = FilePath;
            StartToRead();
        }

        public void StartToRead()
        {
           
        }
    }
}
