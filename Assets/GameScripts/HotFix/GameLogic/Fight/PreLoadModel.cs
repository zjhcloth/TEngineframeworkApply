using System;
using System.Collections.Generic;
using GameBase;
using TEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 预加载资源
/// </summary>
namespace GameLogic
{
    public class PreLoadModel : Singleton<PreLoadModel>
    {
        private static Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();
        private Material uiGrayMat;
        
        public void Clear()
        {
            objDic.Clear();
        }

        /// <summary>
        /// 移除在内存中的资源
        /// </summary>
        /// <param name="name"></param>
        public void RemoveObjByName(string name)
        {
            if (objDic.ContainsKey(name))
            {
                GameModule.Resource.UnloadAsset(objDic[name]);
                objDic.Remove(name);
            }
        }

        /// <summary>
        /// 获取预制体
        /// </summary>
        /// <param name="name"></param>
        /// <param name="act"></param>
        public void GetPrefab(string name, UnityAction<GameObject> act)
        {
            GameObject obj;
            objDic.TryGetValue(name, out obj);
            if (obj == null)
                LoadObj(name, act);
            else
                act?.Invoke(obj);
        }

        /// <summary>
        /// 实例化预制体
        /// </summary>
        /// <param name="name"></param>
        /// <param name="act"></param>
        /// <param name="parent"></param>
        /// <param name="IsResetPos"></param>
        public void InstantiatePrefab(string name, UnityAction<GameObject> act, Transform parent = null,
            bool IsResetPos = false)
        {
            GameObject obj;
            GameObject InstObject;
            //Debug.Log("load obj" + name);
            objDic.TryGetValue(name, out obj);
            if (obj == null)
                LoadObj(name, (obje) =>
                {
                    InstObject = GameObject.Instantiate(obje, parent, IsResetPos);
                    act?.Invoke(InstObject);
                });
            else
            {
                InstObject = GameObject.Instantiate(obj, parent, IsResetPos);
                act?.Invoke(InstObject);
            }
        }

        /// <summary>
        /// 加载物件
        /// </summary>
        /// <param name="name">物件的key</param>
        /// <param name="act">回调</param>
        private async void LoadObj(string name, UnityAction<GameObject> act)
        {
            if (objDic.ContainsKey(name))
            {
                act?.Invoke(objDic[name]);
                return;
            }

            var result = await GameModule.Resource.LoadAssetAsync<GameObject>(name);

            if (result != null)
            {
                AddObjToDict(name, result);
                act?.Invoke(result);
            }
        }

        /// <summary>
        /// 把加载出来的物件加入缓存中
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        private void AddObjToDict(string name, GameObject obj)
        {
            if (!objDic.ContainsKey(name) && obj != null)
            {
                //Debug.Log("AddObjToDict: " + name);
                objDic.Add(name, obj);
            }
        }


        /// <summary>
        /// 设置让UI的Image变成黑白
        /// </summary>
        /// <param name="img">需要变色的图</param>
        /// <param name="gray">是否黑白</param>
        public async void SetUIGrayMaterial(Image img, bool gray)
        {
            if (uiGrayMat != null)
            {
                img.material = gray ? uiGrayMat : null;
                return;
            }

            uiGrayMat = await GameModule.Resource.LoadAssetAsync<Material>("ToGray");
            if (uiGrayMat != null)
            {
                img.material = gray ? uiGrayMat : null;
            }
        }


        /// <summary>
        /// 设置UI的流光特效
        /// </summary>
        /// <param name="img">Image图</param>
        /// <param name="liuguangIndex">第几个流光,不同流光表现不一样,不要调mat</param>
        /// <param name="show">是否展示</param>
        public async void SetUILiuguangMaterial(Image img, int liuguangIndex, bool show = true)
        {
            if (!show)
            {
                img.material = null;
                return;
            }

            var result = await GameModule.Resource.LoadAssetAsync<Material>("liuguang" + liuguangIndex);
            if (result != null)
            {
                img.material = result;
            }

            GameModule.Resource.UnloadAsset(result);
        }
    }
}