using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightStream
{
    public class ConsoleWrite : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is Messages.InputError)
            {
                var msg = message as Messages.InputError;
                Console.WriteLine(msg.Reason);
                
            }
            else if (message is Messages.InputSuccess)
            {
                var msg = message as Messages.InputSuccess;
                Console.WriteLine(msg.Reason);
                
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
