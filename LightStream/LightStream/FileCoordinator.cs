using Akka.Actor;
using Akka.Event;
using static LightStream.Messages;

namespace LightStream
{
    public class FileCoordinator : ReceiveActor, IWithUnboundedStash
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private readonly string _fileDirectory;
        ICanTell _buddy;

        public IStash Stash { get; set; }

        public FileCoordinator(ICanTell buddy, string fileDirectory)
        {
            _fileDirectory = fileDirectory;
            _buddy = buddy;
            _buddy.Tell(new BeginConnection { }, Self);
            _log.Info("Starting to connection with Buddy.");
            WaitForCommand();

        }

    

        private void WaitForCommand()
        {

            Receive<SendingFile>(rf =>
            {
                _log.Info("Coordinating Receive: {0}", rf._fileName);
                var actorName = ReceiverName(rf._fileName);
                var receive = Context.Child(actorName);
                if (receive.IsNobody())
                {
                    _log.Info("Receive Actor created for: {0}", rf._fileName);
                    receive = Context.ActorOf(Props.Create(
                    () => new FileReceive(_fileDirectory)), actorName);
                     _buddy.Tell(new ReceivingFile(receive, rf._filePath, rf._fileName), Self);
                }
                else
                {
                    _log.Info("A receive process for transferring file {0} is already running", rf._fileName);
                }
                
               
            });

            Receive<SendFile>(sf =>
            {
                _log.Info("Coordinating transfer of {0}", sf._fileName);
                _buddy.Tell(new SendingFile(sf._fileName , sf._filePath), Self);
            });
            Receive<ReceivingFile>(rec =>
            {
                _log.Info("Coordinating Send of {0}", rec._fileName);
                var actorName = SenderName(rec._fileName);
                var send = Context.Child(actorName);
                if(send.IsNobody())
                {
                    _log.Info("Send Actor created for: {0}", rec._fileName);
                    send = Context.ActorOf(Props.Create(
                    () => new FileSendActor(rec._filePath, rec._fileName, rec._sender)), actorName);
                }
                else
                {
                    _log.Info("A process for transferring file {0} is already running", rec._sender);
                }
                
            });


        }

        public static string SenderName(string fileName) {

        return $"recv-{System.Uri.EscapeUriString(fileName)}";
            }
        public static string ReceiverName(string fileName)
        {
            return $"recv-{System.Uri.EscapeUriString(fileName)}";
        }
    }
}