using UnityEngine;

public abstract class A_StateEventListener : MonoBehaviour, I_StateBehaviour
{
    [SerializeField] SO_StateEvent m_stateEvent = default;
    [SerializeField] E_SubscriptionType m_subscriptionType = default;

    public string StateName => gameObject.name;

    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void ExecuteMain();

    protected void OnEnable()
    {
        switch (m_subscriptionType)
        {
            case E_SubscriptionType.ENABLE_ONLY: StartListening(); break;
            case E_SubscriptionType.AUTO_ENABLE_DISABLE: StartListening(); break;
        }
    }

    protected virtual void OnDisable()
    {
        switch (m_subscriptionType)
        {
            case E_SubscriptionType.AUTO_ENABLE_DISABLE: StopListening(); break;
        }
    }

    public virtual void StartListening()
    {
        m_stateEvent.AddStateBehaviour(this);
    }

    public virtual void StopListening()
    {
        m_stateEvent.RemoveStateBehaviour(this);
    }
}
