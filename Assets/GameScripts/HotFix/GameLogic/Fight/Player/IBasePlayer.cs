using System.Collections.Generic;
using UnityEngine;
namespace GameLogic
{
    public interface IBasePlayer
    {
        void DoFixUpdate();
        void DoUpdate();
        Transform GetTransform();
        int GetLoc();
        void Clear();
    }
}