using System;
using Akka.Actor;
using System.Collections.Generic;
using System.Text;
using static LightStream.Messages;

namespace LightStream
{
    class FileCoordinator : ReceiveActor
    {
        private readonly string _fileDirectory;
        ICanTell _buddy;

        public FileCoordinator( ICanTell buddy, string fileDirectory)
        {
            _fileDirectory = fileDirectory;
            _buddy = buddy;
            WaitForCommand();
        }
        
        private void WaitForCommand()
        {
            Receive<ReadFile>(rf =>
            {
                Context.ActorOf(Props.Create(
                    () => new FileReceive(_fileDirectory)));
            });

            Receive<SendFile>(sf =>
            {
                Context.ActorOf(Props.Create(
                    () => new FileSendActor(sf._filePath,_buddy)));
            });
        }


    }
}
