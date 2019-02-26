using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LightStream
{
    class FileSend : ReceiveActor
    {
        private string _filePath;

        public FileSend(string FilePath)
        {
            _filePath = FilePath;
            StartToSend();
        }
        
        private void StartToSend()
        {

        }

        protected override void PreStart()
        {
            var fileStream = new FileStream(Path.GetFullPath(_filePath),
                                FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileStreamReader = new StreamReader(fileStream, Encoding.UTF8);
            var text = fileStreamReader.ReadToEndAsync();
        }
    }
}
