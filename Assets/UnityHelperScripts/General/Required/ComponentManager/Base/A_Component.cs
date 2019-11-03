using UnityEngine;

public abstract class A_Component : MonoBehaviour, I_QuickComponent {

    [Header("Component Settings")]
    [SerializeField] protected SO_ComponentManager m_componentManager;
    [SerializeField] protected E_SubscriptionType m_subscriptionType = E_SubscriptionType.AUTO_ENABLE_DISABLE;

    protected virtual void OnEnable()
    {
        switch (m_subscriptionType)
        {
            case E_SubscriptionType.AUTO_ENABLE_DISABLE: RegisterToManager(); break;
            case E_SubscriptionType.ENABLE_ONLY: RegisterToManager(); break;
        }
    }

    protected virtual void OnDisable()
    {
        switch (m_subscriptionType)
        {
            case E_SubscriptionType.AUTO_ENABLE_DISABLE: UnRegisterFromManager(); break;
        }
    }

    public abstract void Execute();


    public virtual void RegisterToManager()
    {
        m_componentManager.AddComponent(this);
    }

    public virtual void UnRegisterFromManager()
    {
        m_componentManager.RemoveComponent(this);
    }
}

public enum E_SubscriptionType
{
    AUTO_ENABLE_DISABLE,
    ENABLE_ONLY,
    MANUAL
}