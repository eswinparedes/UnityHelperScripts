using UnityEngine;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;

namespace SUHScripts.Tests
{
    using static SUHScripts.PhysicsCasting;

    public class TEST_Selectable : MonoBehaviour
    {
        [SerializeField] Transform m_raycastIdentity = default;
        [SerializeField] KeyCode m_selectKeycode = KeyCode.Space;

        Option<I_Focusable> m_currentFocusable = (Option<I_Focusable>) NONE;
        Option<I_Selectable> m_currentSelectable = (Option<I_Selectable>)NONE;

        void Update()
        {
            var hitData = Raycast(m_raycastIdentity.GetRayFromTransform());

            m_currentFocusable = 
                m_currentFocusable
                .ForNewEntryUpdate(
                    hitData.GetComponentOption<I_Focusable>(),
                    focusable => focusable.OnFocusStart(),
                    focusable => focusable.OnFocusEnd());

            m_currentSelectable =  
                m_currentSelectable
                .ForToggleUpdate(
                    hitData.GetComponentOption<I_Selectable>(),
                    Input.GetKeyDown(m_selectKeycode),
                    Input.GetKeyUp(m_selectKeycode),
                    sel => sel.OnSelectStart(),
                    sel => sel.OnSelectEnd());
        }
    }
}
