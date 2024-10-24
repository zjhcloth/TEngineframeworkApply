//=====================================================
// - FileName: BaseProperty.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 14:46:09
// - Description: 属性基类
//======================================================

using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class BasePlayerProperty
    {
        public PlayerMessenger mPlayerMessenger;

        public const string UpdatePlayerPro = "UpdatePlayerPro";
        public const string UpdatePlayerProVector = "UpdatePlayerProVector";
        public const string UpdatePlayerProInt = "UpdatePlayerProInt";
        public const string UpdatePlayerProFloat = "UpdatePlayerProFloat";
        public const string UpdatePlayerProBool = "UpdatePlayerProBool";
        public PlayerMessengerEvent<int, string, object> UpdatePlayerProEvent;
        public PlayerMessengerEvent<int, string, int> UpdatePlayerProIntEvent;
        public PlayerMessengerEvent<int, string, float> UpdatePlayerProFloatEvent;
        public PlayerMessengerEvent<int, string, Vector3> UpdatePlayerProVectorEvent;
        public PlayerMessengerEvent<int, string, bool> UpdatePlayerProBoolEvent;

        public BasePlayerProperty(int loc)
        {
            _loc = loc;
            _campPosType = FightTools.GetCampPosTypeByIdx(loc); //布阵位置（1,2,3【前】、4,5,6【中】、7,8,9【后排】）
            mPlayerMessenger = new PlayerMessenger();
            UpdatePlayerProEvent = new PlayerMessengerEvent<int, string, object>(UpdatePlayerPro, mPlayerMessenger);
            UpdatePlayerProIntEvent = new PlayerMessengerEvent<int, string, int>(UpdatePlayerProInt, mPlayerMessenger);
            UpdatePlayerProFloatEvent = new PlayerMessengerEvent<int, string, float>(UpdatePlayerProFloat, mPlayerMessenger);
            UpdatePlayerProVectorEvent = new PlayerMessengerEvent<int, string, Vector3>(UpdatePlayerProVector, mPlayerMessenger);
            UpdatePlayerProBoolEvent = new PlayerMessengerEvent<int, string, bool>(UpdatePlayerProBool, mPlayerMessenger);
        }
        
        public float mTime = 0;//帧时间
        public Dictionary<EnumUtil.BuffType, Buff> Buffs;// buff列表
        public Skill Skill1;//普攻
        public Skill Skill2;//大招

        private Skill _currSkill; //当前技能
        public Skill CurrSkill//当前技能
        {
            get { return _currSkill; }
            set { _currSkill = value; }
        }
        private EnumUtil.HeroType _heroType; //职业（剑客 = 1,刀客 = 2,拳宗 = 3,万变 = 4,无招 = 5）
        public EnumUtil.HeroType HeroType
        {
            get { return _heroType; }
            set { _heroType = value; }
        }
        private EnumUtil.CampPosType _campPosType;//布阵位置（1,2,3【前】、4,5,6【中】、7,8,9【后排】）
        //基础属性
        private float _atk; //攻击

        public float Atk
        {
            get { return _atk; }
            set { _atk = value; }
        }
        private float _def; //防御
        public float Def
        {
            get { return _def; }
            set { _def = value; }
        }
        private float _hp; //气血
        public float Hp
        {
            get { return _hp; }
            set
            {
                _hp = value;
                if (value <= 0) IsDie = true;
            }
        }
        private float _maxHp; //最大气血
        public float MaxHp
        {
            get { return _maxHp; }
            set
            {
                _maxHp = value;
            }
        }
        private float _maxMp; //最大内力
        public float MaxMp
        {
            get { return _maxMp; }
            set { _maxMp = value; }
        }
        private float _mp; //内力
        public float Mp
        {
            get { return _mp; }
            set { _mp = value; }
        }

        //特殊属性
        private float _energy; //怒气
        public float Energy
        {
            get { return _energy; }
            set { _energy = value; }
        }
        private float _shield; //护体罡气
        public float Shield
        {
            get { return _shield; }
            set { _shield = value; }
        }
        //进阶属性
        private float _hurt; //伤害
        public float Hurt
        {
            get { return _hurt; }
            set { _hurt = value; }
        }
        private float _hurtDef; //伤害抗性
        public float HurtDef
        {
            get { return _hurtDef; }
            set { _hurtDef = value; }
        }
        private float _energyRate; //怒气效率
        public float EnergyRate
        {
            get { return _energyRate; }
            set { _energyRate = value; }
        }
        private float _avo; //闪避率
        public float Avo
        {
            get { return _avo; }
            set { _avo = value; }
        }
        private float _avoDis; //忽视闪避率
        public float AvoDis
        {
            get { return _avoDis; }
            set { _avoDis = value; }
        }
        private float _critRate; //暴击率
        public float CritRate
        {
            get { return _critRate; }
            set { _critRate = value; }
        }
        private float _critRateDis; //忽视暴击率
        public float CritRateDis
        {
            get { return _critRateDis; }
            set { _critRateDis = value; }
        }
        private float _critHurt; //暴伤
        public float CritHurt
        {
            get { return _critHurt; }
            set { _critHurt = value; }
        }
        private float _critHurtDis; //忽视爆伤
        public float CritHurtDis
        {
            get { return _critHurtDis; }
            set { _critHurtDis = value; }
        }
        private float _atkSpd; //攻速
        public float AtkSpd
        {
            get { return _atkSpd; }
            set { _atkSpd = value; }
        }
        private float _speed; //移动速度
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        private float _stimeSpeed; //逻辑帧移动速度
        public float StimeSpeed
        {
            get { return _stimeSpeed; }
            set { _stimeSpeed = value; }
        }
        private float _norHurt; //普攻伤害
        public float NorHurt
        {
            get { return _norHurt; }
            set { _norHurt = value; }
        }
        private float _norHurtDef; //普攻抗性
        public float NorHurtDef
        {
            get { return _norHurtDef; }
            set { _norHurtDef = value; }
        }
        private float _skillHurt; //技能伤害
        public float SkillHurt
        {
            get { return _skillHurt; }
            set { _skillHurt = value; }
        }
        private float _skillHurtDef; //技能伤害抗性
        public float SkillHurtDef
        {
            get { return _skillHurtDef; }
            set { _skillHurtDef = value; }
        }
        private float _hurtDur; //持续伤害
        public float HurtDur
        {
            get { return _hurtDur; }
            set { _hurtDur = value; }
        }
        private float _hurtDurDef; //持续伤害抗性
        public float HurtDurDef
        {
            get { return _hurtDurDef; }
            set { _hurtDurDef = value; }
        }

        //表现属性
        private Vector3 _position; //位置
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                bool change = _position != value;
                _position = value; 
                if (change && UpdatePlayerProVectorEvent != null)
                {
                    Debug.Log($"------------{_loc}---UpdatePlayerProVectorEvent--{value}");
                    UpdatePlayerProVectorEvent.Broadcast(_loc, PlayerProEnumKey.position, value, PlayerProEnumKey.position);
                }
            }
        }
        private int _loc; //占位位置（1-9）
        public int Loc
        {
            get { return _loc; }
            set { _loc = value; }
        }
        /// <summary>
        /// 是否死亡
        /// </summary>
        private bool _isDie;
        public bool IsDie
        {
            get { return _isDie; }
            set
            {
                bool change = _isDie != value;
                _isDie = value;
                // if (change && UpdatePlayerProBoolEvent != null)
                // {
                //     UpdatePlayerProBoolEvent.Broadcast(mLoc, PlayerProEnumKey.isDie, value, PlayerProEnumKey.isDie);
                // }
            }
        }

        /// <summary>
        /// 当前目标
        /// </summary>
        private BasePlayerProperty _target;
        public BasePlayerProperty Target
        {
            get { return _target; }
            set
            {
                _target = value;
            }
        }
        
        /// <summary>
        /// 攻击范围
        /// </summary>
        private float _atkRange;
        public float AtkRange
        {
            get { return _atkRange; }
            set
            {
                _atkRange = value;
            }
        }
        
        /// <summary>
        /// 感知范围
        /// </summary>
        private float _feelRange;
        public float FeelRange
        {
            get { return _feelRange; }
            set
            {
                _feelRange = value;
            }
        }

        /// <summary>
        /// 移动状态
        /// </summary>
        private EnumUtil.MoveStatus _moveStatus;
        public EnumUtil.MoveStatus MoveStatus
        {
            get { return _moveStatus; }
            set
            {
                bool change = _moveStatus != value;
                _moveStatus = value;
                if (change && UpdatePlayerProEvent != null)
                {
                    UpdatePlayerProEvent.Broadcast(_loc, PlayerProEnumKey.moveStatus, value, PlayerProEnumKey.moveStatus);
                }
            }
        }
        
        /// <summary>
        /// 是否队长
        /// </summary>
        private bool _isLeader;
        public bool IsLeader
        {
            get { return _isLeader; }
            set
            {
                bool change = _isLeader != value;
                _isLeader = value;
            }
        }
        
        /// <summary>
        /// 移动方向
        /// </summary>
        private Vector3 _moveDir;

        public Vector3 MoveDir
        {
            get { return _moveDir; }
            set
            {
                bool change = _moveDir != value;
                _moveDir = value;
            }
        }
        
        /// <summary>
        /// 添加buff
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Buff AddBuff(EnumUtil.BuffType type)
        {
            Buff buff;
            bool hasBuff = Buffs.ContainsKey(type);
            if (hasBuff)
            {
                buff = Buffs[type];
                if (buff.Count < buff.MaxCount) //叠加层数
                    buff.Count++;
                buff.EndTime = mTime + buff.KeepTime;//刷新时间
            }
            else
            {
                buff = new Buff();
                buff.Type = type;
                buff.EndTime = mTime + buff.KeepTime;
                Buffs.Add(type, buff);
            }
            return buff;
        }
        
        public void RemoveBuff(EnumUtil.BuffType type)
        {
            Buff buff;
            bool hasBuff = Buffs.ContainsKey(type);
            if (hasBuff)
            {
                buff = Buffs[type];
                buff.OnFinish?.Invoke();
                Buffs.Remove(type);
            }
        }

    }
    public class PlayerProEnumKey
    {
    

        public const string weaponId = "weaponId";
        public const string skinId = "skinId";
    
        public const string position = "position";
   
        public const string moveDir = "moveDir";//  移动朝向Int
   
        public const string speed = "speed";
        public const string isDie = "isDie";
        public const string isRealDie = "isRealDie";
    
        public const string skill2 = "skill2";
        public const string skill1 = "skill1";

        public const string curSkill = "curSkill"; //  当前技能id
        public const string useSkill = "useSkill"; //  是否在使用技能
    

        public const string exp = "exp";
        public const string level = "level";
        public const string hp = "hp";
        public const string maxHp = "maxHp";
        public const string mp = "mp";
        public const string maxMp = "maxMp";
        public const string hudun = "hudun";
        public const string maxHuDun = "maxHuDun";


        public const string hurt = "hurt";//4个方向的被击效果
        public const string buffs = "buffs";
        public const string kill = "kill";
        public const string die = "die";//死亡次数
        
        public const string moveStatus = "moveStatus";//移动状态

    }
}
