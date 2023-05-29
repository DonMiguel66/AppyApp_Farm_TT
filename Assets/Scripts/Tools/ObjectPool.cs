using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools
{
    public class ObjectPool: IDisposable
    {
        private readonly Stack<GameObject> _stack = new Stack<GameObject>();
        private readonly ResourcePath _viewPath;
        private readonly Transform _root;

        public ObjectPool (ResourcePath viewPath)
        {
            _viewPath = viewPath;
            _root = new GameObject($"[{_viewPath.PathResource}]").transform;
        }

        public GameObject Pop()
        {
            GameObject go;
            if(_stack.Count == 0)
            {
                go = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
                //go.name = _viewPath.PathResource.Split("/"[1]).ToString();
            }
            else
            {
                go = _stack.Pop();
            }
            go.SetActive(true);
            go.transform.SetParent(null);
            return go;
        }

        public void InitialPush(GameObject go)
        {
            _stack.Push(go);
            go.transform.SetParent(_root);
        }
        
        public void Push(GameObject go)
        {
            _stack.Push(go);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.SetParent(_root);
            go.SetActive(false);
        }

        public void Dispose()
        {
            for(var i=0; i<_stack.Count; i++)
            {
                var gameObject = _stack.Pop();
                Object.Destroy(gameObject);
            }
            Object.Destroy(_root.gameObject);
        }
    }
}