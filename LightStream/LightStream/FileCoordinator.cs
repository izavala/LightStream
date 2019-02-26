using System;
using Akka.Actor;
using System.Collections.Generic;
using System.Text;
using static LightStream.Messages;

namespace LightStream
{
    class FileCoordinator : ReceiveActor
    {
       

        public FileCoordinator()
        {
            WaitForCommand();
        }
        
        private void WaitForCommand()
        {
            Receive<ReadFile>(rf =>
            {
                Context.ActorOf(Props.Create(
                    () => new FileReceive(rf._filePath)));
            });

            Receive<SendFile>(sf =>
            {
                Context.ActorOf(Props.Create(
                    () => new FileSend(sf._filePath)));
            });
        }


    }
}
