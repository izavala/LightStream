using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LightStream
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWrite;
        private readonly IActorRef _fileCoordinator;

        public FileValidatorActor(IActorRef consoleWriteActor, IActorRef fileCoordinator)
        {
            _consoleWrite = consoleWriteActor;
            _fileCoordinator = fileCoordinator;
        }
        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWrite.Tell(new Messages.NullInputError("Input provided was blank, please try again. \n"));
                Sender.Tell(new Messages.StopStream { });
            }
            else
            {
                if(File.Exists(msg))
                {
                    _consoleWrite.Tell(new Messages.InputSuccess("Starting to send File"));
                    _fileCoordinator.Tell(new Messages.SendFile(msg));
                    Sender.Tell(new Messages.StopStream { });

                }
                else
                {
                    _consoleWrite.Tell(new Messages.InputError("File not found"));
                    Sender.Tell(new Messages.StopStream { });

                }
            }
        }
    }


}
