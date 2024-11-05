
namespace GameLogic
{
    public class BaseMessengerKey
    {
        public const string Update = "Update";
        public const string SFUpdate = "SFUpdate";
        public const string FixUpdate = "FixUpdate";
        public const string UpdateFightData = "UpdateFightData";

        public static MessengerEvent UpdateEvent = new MessengerEvent(Update);
        public static MessengerEvent SFUpdateEvent = new MessengerEvent(SFUpdate);
        public static MessengerEvent FixUpdateEvent = new MessengerEvent(FixUpdate);
        public static MessengerEvent<string, object> UpdateFightDataEvent = new MessengerEvent<string, object>(UpdateFightData);
    
    }
}


