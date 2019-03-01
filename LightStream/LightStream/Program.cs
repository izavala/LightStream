using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.Remote;

namespace LightStream
{
    class Program
    {
        public static ActorSystem FileSystem;
        //public static string DIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/LightStream/ReceivedData";
        public static string DIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/Reported";
        static void Main(string[] args)
        {
            var config = HoconLoader.ParseConfig("Stream.hocon");
            FileSystem = ActorSystem.Create("FileSystem", config);
            
            var Buddy = FileSystem.ActorSelection("akka.tcp://BuddySystem@localhost:8081/user/Coordinator");
            IActorRef coordinator = FileSystem.ActorOf(Props.Create(()=> new FileCoordinator(Buddy, DIRECTORY)), "Coordinator");
            IActorRef writer = FileSystem.ActorOf(Props.Create<ConsoleWrite>(),"Writer");
            IActorRef validator = FileSystem.ActorOf(Props.Create(()=>new FileValidatorActor(writer,coordinator)));
            IActorRef reader = FileSystem.ActorOf(Props.Create<ReadActor>(validator), "Reader");
            
            reader.Tell(ReadActor.StartCommand);
            FileSystem.WhenTerminated.Wait();
        }
    }
}
