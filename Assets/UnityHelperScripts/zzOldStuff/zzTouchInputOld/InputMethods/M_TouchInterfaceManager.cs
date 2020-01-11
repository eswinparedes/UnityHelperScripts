using UnityEngine;

namespace SUHScripts
{
    public class M_TouchInterfaceManager : A_Component{

        [Header("Touch Data")]
        [SerializeField] SO_TouchInterfaceData m_touchInterfaceData = default;

        C_TouchInterfaceInput m_currentInput;

        C_TouchInput m_touchInput;
        C_MouseInput m_mouseInput;

        C_TouchInterfaceInput m_currentInterface;
        #region Monobehavoiur
        private void Awake()
        {
            #if UNITY_EDITOR
                    m_currentInput = new C_MouseInput(m_touchInterfaceData);

            #elif UNITY_STANDALONE
                    m_currentInput = new C_MouseInput(m_touchInterfaceData);
            #elif UNITY_ANDROID
                    m_currentInput = new C_TouchInput(m_touchInterfaceData);
            #endif
        }
    #endregion

    #region implement interface
        public override void Execute()
        {
            m_currentInput.UpdateControls();
        }
    #endregion
    }

}
