using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static LightStream.Messages;

namespace NightWatch
{
    class Watcher : IDisposable
    {
        private readonly IActorRef _report;
        private FileSystemWatcher _watcher;
        private readonly string _fileDir;

        public Watcher(IActorRef ReportActor, string absoluteFilePath)
        {
            _report = ReportActor;
            _fileDir = absoluteFilePath;
        }

        /// <summary>
        /// Begin monitoring file.
        /// </summary>
        public void Start()
        {
            // Need this for Mono 3.12.0 workaround
            // uncomment next line if you're running on Mono!
            // Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");

            // make watcher to observe our specific file
            _watcher = new FileSystemWatcher(_fileDir);

            // watch our file for changes to the file name,
            // or new messages being written to file
            _watcher.NotifyFilter = NotifyFilters.LastWrite;

            // assign callbacks for event types
            _watcher.Changed += OnFileChanged;
            _watcher.Error += OnFileError;

            // start watching
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stop monitoring file.
        /// </summary>
        public void Dispose()
        {
            _watcher.Dispose();
        }

        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file error events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFileError(object sender, ErrorEventArgs e)
        {
            //_report.Tell(new ReportACtor.FileError(_fileNameOnly,
            //    e.GetException().Message),
            //    ActorRefs.NoSender);
        }

        /// <summary>
        /// Callback for <see cref="FileSystemWatcher"/> file change events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                // here we use a special ActorRefs.NoSender
                // since this event can happen many times,
                // this is a little microoptimization
                var fullPath = e.FullPath.Replace("\\", "/");
                var fileName = e.Name;
                _report.Tell(new SendFile(fullPath,fileName), ActorRefs.NoSender);
            }
        }
    }
}