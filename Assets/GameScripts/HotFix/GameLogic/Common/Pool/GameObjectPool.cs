//=====================================================
// - FileName: GameObjectPool.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/18 11:32:41
// - Description:  GameObject对象池
//======================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 激活GameObject的操作方式 根据优化经验和测试，OutSide方式激活的消耗最小
    /// </summary>
    public enum emGameObjectActivePlan
    {
        Active, // 显示隐藏
        Scale, // 缩放为0
        OutSide, // 移至屏幕外
    }

    public class GameObjectPool : IDisposable
    {
        /// <summary>
        /// 回收对象根节点名
        /// </summary>
        static readonly string COLLECT_PARENT_NAME = "GameObjectPoolRoot";

        /// <summary>
        /// OutSide类型父节点坐标
        /// </summary>
        public static readonly Vector3 OUT_SIDE_POS = new Vector3(10000, 10000, 10000);

        /// <summary>
        /// OutSide类型时回收对象的父节点
        /// </summary>
        GameObject mCollectParent;

        /// <summary>
        ///  父对象
        /// </summary>
        public Transform parent { get; private set; }

        /// <summary>
        ///  容量
        /// </summary>
        public int capacity { get; private set; }

        /// <summary>
        /// 激活对象的方式
        /// </summary>
        public emGameObjectActivePlan plan { get; private set; }

        /// <summary>
        /// 总对象数量
        /// </summary>
        public int count => mWorks.Count + mCaches.Count;

        /// <summary>
        ///  使用的
        /// </summary>
        private List<GameObject> mWorks;

        /// <summary>
        ///  缓存的
        /// </summary>
        private List<GameObject> mCaches;

        /// <summary>
        ///   创建方法
        /// </summary>
        private System.Func<Transform, GameObject> mCreateFunc;

        /// <summary>
        /// 删除方法
        /// </summary>
        private System.Action<GameObject> mDeleteAction;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GameObjectPool(Transform parent, int capacity,
            System.Func<Transform, GameObject> create = null,
            System.Action<GameObject> delete = null,
            emGameObjectActivePlan plan = emGameObjectActivePlan.OutSide)
        {
            this.parent = parent;
            this.capacity = capacity;
            this.plan = plan;
            mWorks = new List<GameObject>();
            mCaches = new List<GameObject>();
            mCreateFunc = create;
            mDeleteAction = delete;

            CreateOutSideRoot();
        }

        /// <summary>
        /// 分配一个对象
        /// </summary>
        public GameObject Allocate()
        {
            if (mCaches.Count <= 0)
                Collect();

            GameObject ele = null;
            if (mCaches.Count > 0)
            {
                ele = mCaches[mCaches.Count - 1];
                mCaches.RemoveAt(mCaches.Count - 1);
                /// 分配到空的，重新分配
                if (null == ele)
                {
                    return Allocate();
                }
            }
            else
            {
                ele = Create();
            }

            if (ele != null)
                mWorks.Add(ele);

            if (ele != null && ele.gameObject != null)
            {
                SetActive(ele, true);
            }

            return ele;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Collect()
        {
            for (int i = mWorks.Count - 1; i >= 0; i--)
            {
                if (!IsActive(mWorks[i]))
                {
                    mCaches.Add(mWorks[i]);
                    mWorks.Remove(mWorks[i]);
                }
            }
        }

        /// <summary>
        /// 放入缓存池
        /// </summary>
        public bool Free(GameObject obj)
        {
            SetActive(obj, false);
            if (mWorks.Remove(obj))
            {
                mCaches.Add(obj);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取缓存列表
        /// </summary>
        public List<GameObject> GetCaches()
        {
            return mCaches;
        }

        /// <summary>
        /// 从缓存队列中移除一个对象
        /// </summary>
        public void RemoveFromCaches(GameObject go)
        {
            if (null != mCaches && mCaches.Contains(go))
            {
                mCaches.Remove(go);
                Delete(go);
            }
        }

        /// <summary>
        /// 是否为激活的
        /// </summary>
        public bool IsActive(GameObject obj)
        {
            if (null == obj)
            {
                return false;
            }

            if (plan == emGameObjectActivePlan.Active)
            {
                return obj.activeSelf;
            }
            else if (plan == emGameObjectActivePlan.Scale)
            {
                return obj.transform.localScale != Vector3.zero;
            }
            else if (plan == emGameObjectActivePlan.OutSide)
            {
                return obj.transform.parent != mCollectParent;
            }

            return true;
        }

        /// <summary>
        /// 设置是否闲置
        /// </summary>
        public void SetActive(GameObject obj, bool active)
        {
            if (active)
            {
                if (plan == emGameObjectActivePlan.Active)
                {
                    obj.SetActive(active);
                }
                else if (plan == emGameObjectActivePlan.Scale)
                {
                    obj.transform.localScale = Vector3.one;
                }
                else if (plan == emGameObjectActivePlan.OutSide)
                {
                    obj.transform.SetParent(parent, false);
                }
            }
            else
            {
                if (plan == emGameObjectActivePlan.Active)
                {
                    obj.SetActive(active);
                }
                else if (plan == emGameObjectActivePlan.Scale)
                {
                    obj.transform.localScale = Vector3.zero;
                }
                else if (plan == emGameObjectActivePlan.OutSide)
                {
                    obj.transform.SetParent(mCollectParent.transform, false);
                }
            }
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            foreach (var node in mWorks)
            {
                Delete(node);
            }

            mWorks.Clear();
            foreach (var node in mCaches)
            {
                Delete(node);
            }

            mCaches.Clear();
        }

        /// <summary>
        /// Create
        /// </summary>
        private GameObject Create()
        {
            if (count >= capacity)
            {
                Debug.LogWarning("BufferPool out capacity = " + capacity);
                capacity *= 2;
            }

            if (mCreateFunc != null)
            {
                return mCreateFunc(parent);
            }
            else
            {
                GameObject go = new GameObject();
                if (parent != null)
                {
                    Transform t = go.transform;
                    t.parent = parent;
                    t.localPosition = Vector3.zero;
                    t.localRotation = Quaternion.identity;
                    t.localScale = Vector3.one;
                }

                return go;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete(GameObject ele)
        {
            if (mDeleteAction != null)
            {
                mDeleteAction(ele);
            }
            else
            {
                if (ele.gameObject != null)
                {
                    if (ele.gameObject is GameObject)
                    {
                        GameObject go = ele.gameObject;
                        go.transform.parent = null;
                    }

                    UnityEngine.Object.DestroyImmediate(ele.gameObject);
                }
            }
        }

        /// <summary>
        /// 创建一个OutSide的挂点
        /// </summary>
        void CreateOutSideRoot()
        {
            if (plan == emGameObjectActivePlan.OutSide)
            {
                var tf = parent.Find(COLLECT_PARENT_NAME);
                if (tf == null)
                {
                    mCollectParent = new GameObject(COLLECT_PARENT_NAME);
                    mCollectParent.transform.SetParent(parent, false);
                    mCollectParent.transform.localPosition = OUT_SIDE_POS;
                }
                else
                {
                    mCollectParent = tf.gameObject;
                }
            }
        }

        #region Dispos

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (null != mCollectParent)
            {
                GameObject.Destroy(mCollectParent);
            }

            ((IDisposable)this).Dispose();
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear();
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~GameObjectPool()
        {
            Dispose(false);
        }

        #endregion
    }
}
