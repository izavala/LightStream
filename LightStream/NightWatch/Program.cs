using Akka.Actor;
using LightStream;
using System;

namespace NightWatch
{
    class Program
    {
        public static ActorSystem WatchSystem;
        public static string WDIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/Reported";
        public static string GDIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/WatchList";
        static void Main(string[] args)
        {
            var config = HoconLoader.ParseConfig("Watch.hocon");
            WatchSystem = ActorSystem.Create("BuddySystem", config);
            var client = WatchSystem.ActorSelection("akka.tcp://FileSystem@localhost:8080/user/Coordinator");

            IActorRef coordinator = WatchSystem.ActorOf(Props.Create(() => new FileCoordinator(client,GDIRECTORY)), "Coordinator");
            IActorRef writer = WatchSystem.ActorOf(Props.Create<ConsoleWrite>(), "Writer");
            var watcher = new Watcher(coordinator, GDIRECTORY);

            watcher.Start();
            WatchSystem.WhenTerminated.Wait();
        }
    }
}