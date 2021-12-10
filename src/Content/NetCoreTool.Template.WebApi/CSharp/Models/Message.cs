using System;

namespace Company.WebApplication.CS.Models
{
    [Serializable]
    public class Message
    {
        public string Value { get; }

        public Message(string value)
        {
            Value = value;
        }
    }
}
