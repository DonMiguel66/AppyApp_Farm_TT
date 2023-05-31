using System;
using System.Collections;
using System.Linq;
using Interfaces;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;


namespace Controllers
{
    public sealed class ListExecuteObject: IEnumerator, IEnumerable
    {
        private IExecute[] _interactiveObjects;
        private int _index = -1;
        private InteractiveObject _current;
        public ListExecuteObject()
        {
            var interactiveObjects = Object.FindObjectsByType<ClaimableObject>(FindObjectsSortMode.None);
            for (var i = 0; i < interactiveObjects.Length; i++)
            {
                if (interactiveObjects[i] is IExecute interactiveObject)
                {
                    //Debug.Log($"{interactiveObjects[i].name}");
                    AddExecuteObject(interactiveObject);
                }
            }
        }
        
        public void AddExecuteObject(IExecute execute)
        {
            if (_interactiveObjects == null)
            {
                _interactiveObjects = new[] {execute};
                return;
            }

            //if (_interactiveObjects.Any(ex => ex == execute)) return;
            Array.Resize(ref _interactiveObjects, Length + 1);
            _interactiveObjects[Length - 1] = execute;
        }

        public void RemoveExecuteObject(IExecute execute)
        {
            if(_interactiveObjects == null)
                return;
            if(_interactiveObjects.Any(ex => ex == execute)) return;
            Debug.Log("RR");
            var curIndex = Array.IndexOf(_interactiveObjects, execute);
            for (int i = curIndex; i < _interactiveObjects.Length-1; i++)
            {
                _interactiveObjects[i] = _interactiveObjects[i + 1];
            }
            Array.Resize(ref _interactiveObjects, Length-1);
        }
        
        public IExecute this [int index]
        {
            get => _interactiveObjects[index];
            private set => _interactiveObjects[index] = value;
        }
        public int Length => _interactiveObjects.Length;
        
        public bool MoveNext()
        {
            if (_index == _interactiveObjects.Length - 1)
            {
                Reset();
                return false;
            }
            _index++;
            return true;
        }
        public void Reset() => _index = -1;
        
        public object Current => _interactiveObjects[_index];
        
        public IEnumerator GetEnumerator()
        {
            return this;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}