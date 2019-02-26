using System;
using Akka.Actor;
using Akka.Remote;

namespace LightStream
{
    class Program
    {
        public static ActorSystem FileSystem;
        static void Main(string[] args)
        {
           FileSystem = ActorSystem.Create("FileSystem");

        }
    }
}
