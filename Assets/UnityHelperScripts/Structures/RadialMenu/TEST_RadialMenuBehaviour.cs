using UnityEngine;

namespace SUHScripts
{
    public class TEST_RadialMenuBehaviour : MonoBehaviour
    {
        [SerializeField] RadialMenuBehaviour m_radialMenu = default;
        [SerializeField] SO_A_Bool m_actionBehaviour = default;

        Vector2 input => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        void Start()
        {
            m_radialMenu.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            m_radialMenu.Update(new Vector2(input.x, input.y));
            if (m_actionBehaviour.IsTrue)
            {
                m_radialMenu.RequestActionOnSelectedRadialObject();
            }
        }
    }
}