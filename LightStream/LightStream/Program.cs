using System;
using Akka.Actor;
using Akka.Remote;

namespace LightStream
{
    class Program
    {
        public static ActorSystem FileSystem;
        public static string DIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/LightStream/ReceivedData";
        static void Main(string[] args)
        {
           FileSystem = ActorSystem.Create("FileSystem");
            IActorRef receiver = FileSystem.ActorOf(Props.Create<FileReceive>(), "Receiver");
            IActorRef coordinator = FileSystem.ActorOf(Props.Create(()=> new FileCoordinator(receiver, DIRECTORY)), "Coordinator");
            IActorRef writer = FileSystem.ActorOf(Props.Create<ConsoleWrite>(),"Writer");
            IActorRef validator = FileSystem.ActorOf(Props.Create(()=>new FileValidatorActor(writer,coordinator)));
            IActorRef reader = FileSystem.ActorOf(Props.Create<ReadActor>(validator), "Reader");
            
            reader.Tell(ReadActor.StartCommand);
            FileSystem.WhenTerminated.Wait();
        }
    }
}
