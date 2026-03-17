using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Pooling
{
    public abstract class ObjectPool<T> where T : Component
    {
        private Transform _holder;
        private Queue<T> _objectPool = new Queue<T>();
        private T _prefab;

        public ObjectPool(T prefab, int initialSize, Transform holder)
        {
            _prefab = prefab;
            _holder = holder;
            for (int i = 0; i < initialSize; i++)
            {
                var obj = GameObject.Instantiate(_prefab, _holder, false);
                obj.transform.localPosition = Vector3.zero;
                obj.gameObject.SetActive(false);
                _objectPool.Enqueue(obj);
            }
        }

        public T GetObject()
        {
            T obj;
            if (_objectPool.Count > 0)
            {
                obj = _objectPool.Dequeue();
                obj.transform.SetParent(null, true);
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = GameObject.Instantiate(_prefab);
            }
            return obj;
        }
        public T GetObject(Transform parent)
        {
            T obj;
            if (_objectPool.Count > 0)
            {
                obj = _objectPool.Dequeue();
                obj.transform.SetParent(parent);
                obj.transform.localPosition = Vector3.zero;
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = GameObject.Instantiate(_prefab, parent);
            }
            return obj;
        }

        public virtual void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_holder, false);
            obj.transform.localPosition = Vector3.zero;
            _objectPool.Enqueue(obj);
        }
    }
}