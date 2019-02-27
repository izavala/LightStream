using System;
using Xunit;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using LightStream;
using System.IO;

namespace LightStreamTest
{
    public class UnitTest1 : TestKit
    {
        public static string DIRECTORY = "TestFile.txt";
        [Fact]
        public void Send_File_Test()
        {
            ActorSystem TestSystem;
            TestSystem = ActorSystem.Create("TestSystem");

            var sender = TestSystem.ActorOf(Props.Create(() => new FileSendActor(DIRECTORY,TestActor)));
            ExpectMsg<Messages.StartSream>();
            ExpectMsg<Messages.SendBytes>();

        }
    }
}
