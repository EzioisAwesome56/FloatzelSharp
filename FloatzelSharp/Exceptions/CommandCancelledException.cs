using System;
using System.Runtime.Serialization;

namespace FloatzelSharp.commands {
    [Serializable]
    internal class CommandCancelledException : Exception {
        public CommandCancelledException() {
        }

        public CommandCancelledException(string message) : base(message) {
        }

        public CommandCancelledException(string message, Exception innerException) : base(message, innerException) {
        }

        protected CommandCancelledException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}