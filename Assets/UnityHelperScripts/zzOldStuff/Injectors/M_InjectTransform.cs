using UnityEngine;

public class M_InjectTransform : MonoBehaviour
{
    [SerializeField] Transform m_transformToInject = default;
    [SerializeField] SO_A_Transform m_injectionTarget = default;
    [SerializeField] E_SubscriptionType m_subscriptionType = E_SubscriptionType.ENABLE_ONLY;

    private void OnEnable()
    {
        switch (m_subscriptionType)
        {
            case E_SubscriptionType.AUTO_ENABLE_DISABLE: m_injectionTarget.transform = m_transformToInject; break;
            case E_SubscriptionType.ENABLE_ONLY: m_injectionTarget.transform = m_transformToInject; break;
        }
    }

    private void OnDisable()
    {
        switch (m_subscriptionType)
        {
            case E_SubscriptionType.AUTO_ENABLE_DISABLE: m_injectionTarget.transform = null; break;
        }
    }

}
