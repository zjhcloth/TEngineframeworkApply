//=====================================================
// - FileName: FightMemoryManager.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/18 11:32:41
// - Description:  添加一个战斗资源内存分配缓存和释放管理
//* （目前仅管理了怪物资源的内存，后续可以把其他角色，特效等放入统一管理）
//* 资源部分
//1.音效，2.粒子特效，3.角色预制体，4.UI图标，5.序列化配置（技能，AI)
//======================================================

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class FightMemoryManager : MonoSingleton<FightMemoryManager>
    {
        /// <summary>
        /// 单个缓存池最大容量
        /// </summary>
        const int BUFFER_POOL_CAPACITY = 128;

        /// <summary>
        /// 缓存池映射表
        /// </summary>
        Dictionary<string, GameObjectPool> mPoolMap = new Dictionary<string, GameObjectPool>();

        /// <summary>
        /// 资源缓存
        /// </summary>
        private Dictionary<string, Object> mAssetCache = new Dictionary<string, Object>();

        /// <summary>
        /// 通用的资源名(备用，保存那些通用的，战斗后不需要卸载的)
        /// </summary>
        private List<string> mCommonAssets = new List<string>();

        /// <summary>
        /// 本场战斗所需的资源
        /// </summary>
        private List<string> mAssets = new List<string>();

        /// <summary>
        /// 场景加载完后加载资源（调用了场景的灯光组件所以要等到场景加载完）
        /// </summary>
        /// <param name="room">房间信息,包含角色信息</param>
        /// <param name="act">加载完毕回调,继续跑Loading,一般是直接关闭</param>
        public async UniTask LoadAsync()
        {
           
            //await UniTask.Delay(2000);
            // 未等待CreatePlayer所以会执行在角色创建之后
            await LoadResByGameMode();
            PlayerManager.CreatePlayer();
            await UniTask.DelayFrame(1);
            // GameObject go = Instance.Allocate("Joystick", false);
            //
            // // 创建Canvas
            // Canvas canvas = new GameObject("Canvas").AddComponent<Canvas>();
            // canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //
            // // 创建CanvasScaler
            // CanvasScaler canvasScaler = new GameObject("CanvasScaler").AddComponent<CanvasScaler>();
            // canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            // canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            // canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //
            // // 创建GraphicRaycaster
            // GraphicRaycaster graphicRaycaster = new GameObject("GraphicRaycaster").AddComponent<GraphicRaycaster>();
            //
            // // 设置层级关系
            // canvasScaler.transform.SetParent(canvas.transform);
            // graphicRaycaster.transform.SetParent(canvas.transform);
            //
            // go.transform.parent = canvas.transform;
            MainFightCtrl.StartFight();

        }

        /// <summary>
        /// 目前没有做预加载管理，统一在游戏开始前加载一遍会出现的怪物预制体，放到预制体缓存里,后续需要做预实例化避免游戏中刷兵产生的GC
        /// </summary>
        private async UniTask LoadBattleResAsync()
        {
            foreach (var item in mAssets)
            {
                await LoadAssetCache(item);
            }
        }

        /// <summary>
        /// 根据模式加载资源
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async UniTask LoadResByGameMode()
        {
            mAssets.Add("card");
            mAssets.Add("Joystick");
            mAssets.Add("PopupNumber");
            await LoadBattleResAsync();
        }

        /// <summary>
        /// 根据玩家加载对应的资源（技能、武器的特效
        /// </summary>
        /// <returns></returns>
        public async UniTask LoadResByPlayers()
        {
            // 预加载技能特效和武器
            foreach (BasePlayerProperty pro in BaseFightData.mPlayerDataList.Values)
            {
               
            }

            // 预加载通用资源 TODO:这类资源可以用一张配置表收集下，手写加载比较不好管理
            // await LoadAssetCache(EffectConstant.EFF_NAME_ChuMo);
            // await LoadAssetCache(EffectConstant.EFF_NAME_RuShui);
            // await LoadAssetCache(EffectConstant.EFF_NAME_MoyuRuShui);
            // await LoadAssetCache(EffectConstant.EFF_NAME_InMoFootEffect);
            // await LoadAssetCache(EffectConstant.EFF_NAME_RenXingJump);
            // await LoadAssetCache(EffectConstant.EFF_NAME_KongzhongShuaimo);
            // await LoadAssetCache(EffectConstant.EFF_NAME_Dead);
            // await LoadAssetCache(EffectConstant.EFF_NAME_Shenji);
            // await LoadAssetCache(EffectConstant.EFF_NAME_BeAttack);
            // await LoadAssetCache("Eff_DiePaoPao");
        }

        /// <summary>
        /// 实例化资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async UniTask<GameObject> InstantiateAsync(string name)
        {
            await LoadAssetCache(name);
            GameObject go = null;
            var asset = GetAsset(name);
            if (asset != null)
            {
                go = (GameObject)Instantiate(asset);
            }

            return go;
        }

        private async UniTask LoadPlayerSkill(int skillId, int skillLv = 1)
        {
            string prefabName;
            // var conf = ItemSkillConfig.Instance.FindItemSkillById(skillId);
            // if (conf != null && conf.type != (int)SkillType.被动)
            // {
            //     prefabName = GameCacheTools.GetArtResourceById(conf.artResourceId);
            //
            //     var skillPrefab = SkillFactory.GetSkill(prefabName);
            //     if (skillPrefab != null)
            //     {
            //         var skillBase = skillPrefab.SkillMono(skillLv);
            //         if (skillBase != null)
            //         {
            //             await LoadAssetCache(skillBase.mStartEffect);
            //             await LoadAssetCache(skillBase.mIngEffect);
            //             await LoadAssetCache(skillBase.mIngEffect);
            //         }
            //     }
            //     else
            //     {
            //         Debug.LogError("技能找不到预制体: " + prefabName);
            //     }
            // }
        }

        /// <summary>
        /// 加载资源并Cache
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async UniTask LoadAssetCache(string item)
        {
            if (!string.IsNullOrEmpty(item) && !mAssetCache.ContainsKey(item))
            {
                try
                {
                    var asset = await GameModule.Resource.LoadAssetAsync<Object>(item);
                    if (!mAssetCache.ContainsKey(item)) //避免外部同步调用引起的key重复
                        mAssetCache.Add(item, asset);
                }
                catch
                {
                    Debug.LogError($"找不到资源 {item}");
                }
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Free(string prefabName, GameObject obj, bool cache = true)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                return;
            }

            if (cache && mPoolMap.ContainsKey(prefabName))
            {
                mPoolMap[prefabName].Free(obj);
            }
            else if (!cache)
            {
                DestroyImmediate(obj);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarningFormat("can't find CachePool which prefabName is :{0}", prefabName);
#endif
                // 不在缓存池的直接删掉
                Destroy(obj);
            }
        }

        /// <summary>
        /// 删除某个资源的所有对象缓存
        /// </summary>
        /// <param name="perfabName"></param>
        public void DeletePrefabByName(string prefabName)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                return;
            }

            if (mPoolMap.ContainsKey(prefabName))
            {
                mPoolMap[prefabName].Clear();
                mPoolMap.Remove(prefabName);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            if (null != mPoolMap)
            {
                foreach (var item in mPoolMap)
                {
                    if (null != item.Value && null != item.Value)
                    {
                        item.Value.Clear();
                    }
                }

                mPoolMap.Clear();
            }

            if (null != mAssetCache)
            {
                foreach (var item in mAssetCache)
                {
                    if (null != item.Value && null != item.Value)
                    {
                        GameModule.Resource.UnloadAsset(item.Value);
                    }
                }

                mAssetCache.Clear();
            }
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        /// <param name="prefabName"></param>
        /// <param name="cache">缓存的</param>
        /// <returns></returns>
        public GameObject Allocate(string prefabName, bool cache = true)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                return null;
            }

            mAssetCache.TryGetValue(prefabName, out Object asset);
            if (null == asset)
            {
                Debug.LogError($"Asset is null {prefabName}");
                return null;
            }

            if (cache && !mPoolMap.ContainsKey(prefabName))
            {
                /// 设置池
                var pool = new GameObjectPool(transform, BUFFER_POOL_CAPACITY,
                    (root) => { return CreateObjInPool(asset, root); });

                /// 添加到映射中
                mPoolMap.Add(prefabName, pool);
            }

            /// 申请对象
            GameObject go = cache ? mPoolMap[prefabName].Allocate() : CreateObjInPool(asset, transform);
            return go;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public Object GetAsset(string prefabName)
        {
            mAssetCache.TryGetValue(prefabName, out Object asset);
            return asset;
        }

        /// <summary>
        /// 创建池子中对象
        /// </summary>
        GameObject CreateObjInPool(Object asset, Transform root)
        {
            GameObject go = (GameObject)Instantiate(asset);
            go.transform.parent = root;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
            return go;
        }
    }
}