using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.Remote;

namespace LightStream
{
    class Program
    {
        public static ActorSystem FileSystem;
        public static string DIRECTORY;
        static void Main(string[] args)
        {
            DIRECTORY = args.Length > 0 ? args[0] : "C:/Users/user/Documents/GitHub/LightStream/LightStream/Reported";
            Console.Title = "LightStream";
            var config = HoconLoader.ParseConfig("Stream.hocon");
            FileSystem = ActorSystem.Create("FileSystem", config);
            
            IActorRef coordinator = FileSystem.ActorOf(Props.Create(()=> new FileCoordinatorLS( DIRECTORY)), "Coordinator");
            IActorRef writer = FileSystem.ActorOf(Props.Create<ConsoleWrite>(),"Writer");
            IActorRef validator = FileSystem.ActorOf(Props.Create(()=>new FileValidatorActor(writer,coordinator)));
            IActorRef reader = FileSystem.ActorOf(Props.Create<ReadActor>(validator), "Reader");
            
            reader.Tell(ReadActor.StartCommand);
            FileSystem.WhenTerminated.Wait();
        }
    }
}
