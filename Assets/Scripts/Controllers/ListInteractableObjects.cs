using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class ListInteractableObjects: IEnumerator, IEnumerable
    {
        private IInteractable[] _interactiveObjects;
        private int _index = -1;
        private InteractiveObject _current;
        
        public ListInteractableObjects()
        {
            var interactiveObjects = Object.FindObjectsByType<InteractiveObject>(FindObjectsSortMode.None);
            for (var i = 0; i < interactiveObjects.Length; i++)
            {
                if (interactiveObjects[i] is IInteractable interactiveObject)
                {
                    AddExecuteObject(interactiveObject);
                }
            }
        }
        
        public void AddExecuteObject(IInteractable execute)
        {
            if (_interactiveObjects == null)
            {
                _interactiveObjects = new[] {execute};
                return;
            }
            Array.Resize(ref _interactiveObjects, Length + 1);
            _interactiveObjects[Length-1] = execute;
        }
        
        public IInteractable this [int index]
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