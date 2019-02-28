using System;
using Akka.Actor;
using System.Collections.Generic;
using System.Text;
using static LightStream.Messages;
using Akka.Event;

namespace LightStream
{
    public class FileCoordinator : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly string _fileDirectory;
        ICanTell _buddy;
        private string path;
        
        public FileCoordinator( ICanTell buddy, string fileDirectory)
        {
            _fileDirectory = fileDirectory;
            _buddy = buddy;
            _buddy.Tell(new BeginConnection { }, Self);
            _log.Info("Starting to connection with Buddy.");
            EstablishConnection();
            
        }

        private void EstablishConnection()
        {

            Receive<BeginConnection>(b=> {
                _buddy.Tell(new ConnectionEstablished { },Self);
            });

            Receive<ConnectionEstablished>(c=>{
                _log.Info("Connection established with Buddy.");
                Become(WaitForCommand);
            });
        }

        private void WaitForCommand()
        {
            
            Receive<SendingFile>(rf =>
            {
                _log.Info("Coordinating Receive");
                var receive = Context.ActorOf(Props.Create(
                    () => new FileReceive(_fileDirectory)));
                _buddy.Tell(new ReceivingFile(receive),Self);
            });

            Receive<SendFile>(sf =>
            {
                path = sf._filePath;
                _log.Info("Coordinating send");
                _buddy.Tell(new SendingFile { }, Self);
            });
            Receive<ReceivingFile>(rec =>
            {
                Context.ActorOf(Props.Create(
                    () => new FileSendActor(path, rec._sender)));
            });
        }


    }
}
