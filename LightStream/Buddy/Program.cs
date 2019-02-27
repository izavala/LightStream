using System;
using Akka.Actor;
using Akka.Remote;

namespace LightStream
{
    class Program
    {
        public static ActorSystem BuddySystem;
        public static string DIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/Buddy/Data";
        static void Main(string[] args)
        {
            var config = HoconLoader.ParseConfig("Client.hocon");
            BuddySystem = ActorSystem.Create("BuddySystem", config);
            var client = BuddySystem.ActorSelection("akka.tcp://FileSystem@localhost:8080/user/Coordinator");
            IActorRef coordinator = BuddySystem.ActorOf(Props.Create(() => new FileCoordinator(client, DIRECTORY)), "Coordinator");
            IActorRef writer = BuddySystem.ActorOf(Props.Create<ConsoleWrite>(), "Writer");
            IActorRef validator = BuddySystem.ActorOf(Props.Create(() => new FileValidatorActor(writer, coordinator)));
            IActorRef reader = BuddySystem.ActorOf(Props.Create<ReadActor>(validator), "Reader");

            reader.Tell(ReadActor.StartCommand);
            BuddySystem.WhenTerminated.Wait();
        }
    }
}