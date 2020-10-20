using UnityEngine;

namespace SUHScripts.ReactiveFPS
{
    public class Singleton<T>
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

        public virtual void OnReset()
        {

        }
        public void Reset()
        {
            if (!DidSucceed) return;
            if (IsReset) return;

            IsReset = true;
            Instance = null;
            OnReset();
        }

    }
}

