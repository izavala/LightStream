using Akka.Actor;
using LightStream;
using System;

namespace NightWatch
{
    class Program
    {
    
        public static ActorSystem WatchSystem;
        public static string GDIRECTORY = "C:/Users/user/Documents/WatchList";
        static void Main(string[] args)
        {
            Console.Title = "Night Watcher";
            var config = HoconLoader.ParseConfig("Watch.hocon");
            WatchSystem = ActorSystem.Create("BuddySystem", config);
            var client = WatchSystem.ActorSelection("akka.tcp://FileSystem@40.84.188.226:8080/user/Coordinator");
            IActorRef coordinator = WatchSystem.ActorOf(Props.Create(() => new FileCoordinator(client,GDIRECTORY)), "Coordinator");
            IActorRef writer = WatchSystem.ActorOf(Props.Create<ConsoleWrite>(), "Writer");
            var watcher = new Watcher(coordinator, GDIRECTORY);

            watcher.Start();
            WatchSystem.WhenTerminated.Wait();
        }
    }
}