using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LightStream
{
    class FileValidatorActor : UntypedActor
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
            if(string.IsNullOrEmpty(msg))
            {
                _consoleWrite.Tell(new Messages.NullInputError("Input provided was blank, please try again. \n"));
            }
            else
            {
                if(File.Exists(msg))
                {
                    _consoleWrite.Tell(new Messages.InputSuccess("Starting to send File"));
                    _fileCoordinator.Tell(new FileSend(msg));
                }
                else
                {
                    _consoleWrite.Tell(new Messages.InputError("File not found"));

                }
            }
        }
    }


}
