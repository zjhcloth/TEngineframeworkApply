using System;
using UnityEngine;
using zFrame.UI;
using Random = UnityEngine.Random;


namespace GameLogic
{
    public class MainFightCtrl : MonoBehaviour
    {
        public static MainFightCtrl Instance = null;
        public IBaseMode iModeManager;
        private bool start = false;
        public Joystick joystick;
        /// <summary>
        /// 主摄像机
        /// </summary>
        public Camera mainCamera;

        /// <summary>
        /// 正交摄像机
        /// </summary>
        public Camera imgCamera;
        
        private void Awake()
        {
            Instance = this;
            gameObject.AddComponent<FightMemoryManager>();
        }

        private void Start()
        {
            joystick.OnValueChanged.AddListener(v =>
            {
                Vector2 SendVector = Vector2.zero;
                if (v.magnitude != 0)
                {
                    SendVector = v;
                }
                CmdManager.Instance.CmdMove(SendVector);
            });
        }

        public static void StartFight()
        {
            if (Instance != null && !Instance.start)
            {
                Instance.start = true;
                Instance.iModeManager = Activator.CreateInstance(BaseFightData.mFightModeType) as IBaseMode;
                Instance.InitData();
            }
        }

        private void OnDestroy()
        {
            if (iModeManager != null)
                iModeManager.Clear();
        }

        void Update()
        {
            if (iModeManager != null)
            {
                iModeManager.OnUpdate();
            }
        }
        void FixedUpdate()
        {
            if (iModeManager != null)
            {
                if (iModeManager.IsStart())
                {
                    iModeManager.OnFixUpdate();
                }
            }
        }

        /// <summary>
        /// 初始化战斗模式的数据
        /// </summary>
        private void InitData()
        {
            if (iModeManager != null)
            {
                iModeManager.InitData();
                InitComplete();
            }
        }

        /// <summary>
        /// 初始化完成
        /// </summary>
        private void InitComplete()
        {
            iModeManager.CreateGameObject();
            iModeManager.InitComplete();
            iModeManager.ShowUI();
        }
    }
}
