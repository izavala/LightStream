using System;
using Xunit;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using LightStream;
using System.IO;
using static LightStream.Messages;

namespace LightStreamTest
{
    public class UnitTest1 : TestKit
    {
        public static string DIRECTORY = "C:/Users/user/Documents/GitHub/LightStream/LightStream/LightStreamTest/bin/Debug/netcoreapp2.2/TestFile.txt";
         [Fact]
        public void Send_File_Test()
        {
            int xx; 
            ActorSystem TestSystem;
            TestSystem = ActorSystem.Create("TestSystem");
            var sender = TestSystem.ActorOf(Props.Create(() => new FileSendActor(DIRECTORY,TestActor)));
            

        }
    }
}
