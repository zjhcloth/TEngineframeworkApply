using System;
namespace GameLogic
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PlayerMessengerAttribute : Attribute
    {
        public string key { get; set; }
        public string childType { get; set; }

        public PlayerMessengerAttribute(string key, string proEnum)
        {
            this.key = key;
            this.childType = proEnum;
        }
        public PlayerMessengerAttribute(string key)
        {
            this.key = key;
            this.childType = "";
        }
    }
}
