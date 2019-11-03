using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Maths/Float/Float Clamped")]
public class SO_FloatClamp : SO_A_FloatReadWrite {

    [SerializeField] float m_value = 0;
    [SerializeField] Vector2 MinMaxClamp = new Vector2(0, 1);

    public override float Value
    {
        get { return m_value; }
        set
        {
            float val = value;

            if(val < MinMaxClamp.x)
            {
                val = MinMaxClamp.x;
            }
            else if(val > MinMaxClamp.y)
            {
                val = MinMaxClamp.y;
            }

            m_value = val;
        }
    }
	
}
