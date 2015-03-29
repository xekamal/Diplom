using System.Collections.Generic;

namespace Simulator.Utils.Infrastructure
{
    public class Logger
    {
        private static Logger _instance;
        private readonly IList<string> _messages;

        private Logger()
        {
            _messages = new List<string>();
        }

        public static Logger Instance
        {
            get { return _instance ?? (_instance = new Logger()); }
        }

        public IList<string> Messages
        {
            get { return _messages; }
        }

        public void WriteMessage(string message)
        {
            _messages.Add(message);
        }
    }
}