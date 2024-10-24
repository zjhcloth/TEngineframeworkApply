using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{


    public class MessengerKey : BaseMessengerKey
    {

        public const string Dir = "Dir";
        public const string Die = "Die";
        public const string Attack = "Attack";
        public const string AddPlayer = "AddPlayer";
        public const string RemovePlayer = "RemovePlayer";
        
        public const string OnPVEFly = "OnPVEFly";
        public const string OnPVEDrop = "OnPVEDrop";
        public static MessengerEvent<int, Vector3> PVEDropEvent = new MessengerEvent<int, Vector3>(OnPVEDrop);
        public static MessengerEvent<Transform> PVEFlyEvent = new MessengerEvent<Transform>(OnPVEFly);
        public static MessengerEvent<float, float> DirEvent = new MessengerEvent<float, float>(Dir); //人物选择角度 上下 左右
        public static MessengerEvent<int, int> DieEvent = new MessengerEvent<int, int>(Die);
        public static MessengerEvent<PlayerProperty> AddPlayerEvent = new MessengerEvent<PlayerProperty>(AddPlayer);

        public static MessengerEvent<PlayerProperty> RemovePlayerEvent =
            new MessengerEvent<PlayerProperty>(RemovePlayer);
        
    }
}