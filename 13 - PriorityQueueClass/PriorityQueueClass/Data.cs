using System;

namespace PriorityQueueClass
{
    public class Data : IComparable<Data>
    {
        //* Private Properties
        private readonly DateTime _creationTime;

        //* Public Properties
        public int Priority { get; private set; }

        public string Message { get; private set; }

        public TimeSpan Age =>
            DateTime.UtcNow.Subtract(_creationTime);

        //* Constructors
        public Data(string message, int priority)
        {
            _creationTime = DateTime.UtcNow;
            Message = message;
            Priority = priority;
        }

        // Interface Implementations
        public int CompareTo(Data other)
        {
            int pri = Priority.CompareTo(other.Priority);
            if (pri > 0)
                pri = Age.CompareTo(other.Age);

            return pri;
        }

        // Overriden Methods
        public override string ToString() =>
            string.Format("[{0} : {1}] {2}", Priority, Age.Milliseconds,
                Message);
    }
}