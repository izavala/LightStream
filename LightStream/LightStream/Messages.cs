using System;
using System.Collections.Generic;
using System.Text;

namespace LightStream
{
    class Messages
    {

        public class ReadFile
        {
            public string _filePath;

            public ReadFile(string FilePath)
            {
                _filePath = FilePath;
            }
        }

        public class SendFile
        {
            public string _filePath;

            public SendFile(string FilePath)
            {
                _filePath = FilePath;
            }
        }
        public class InputSuccess
        {
            public InputSuccess(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }

        public class InputError
        {
            public InputError(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; }
        }
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason) { }
        }

        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason) { }
        }
    }
}
