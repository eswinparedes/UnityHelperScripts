using UnityEngine;
using System;

namespace SUHScripts
{
    public class Singleton<T> : IDisposable
    {
        static Singleton<T> Instance;
        static Component attachedComponent;

        public bool DidSucceed { get; }
        public bool IsReset { get; private set; }
        public Singleton(Component component)
        {
            if (Instance != null)
            {
                Debug.LogError($"Instance already set by {attachedComponent.gameObject}");
                DidSucceed = false;
                return;
            }

            Instance = this;
            attachedComponent = component;
            DidSucceed = true;


        }

        protected virtual void OnReset()
        {

        }
        public void Dispose()
        {
            if (!DidSucceed) return;
            if (IsReset) return;

            IsReset = true;
            Instance = null;
            OnReset();
        }

    }
}

