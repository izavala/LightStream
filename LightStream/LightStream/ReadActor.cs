using System;
using Akka.Actor;
using System.Collections.Generic;
using System.Text;

namespace LightStream
{
    public class ReadActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        IActorRef _validationActor;

        public ReadActor(IActorRef validationActor)
        {
            _validationActor = validationActor;               
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                Console.WriteLine("Please provide the URI of a log file on disk.\n");
            }
            GetAndValidateInput();
        }

        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) &&
            String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }
            _validationActor.Tell(message);

            
        }
    }
}
