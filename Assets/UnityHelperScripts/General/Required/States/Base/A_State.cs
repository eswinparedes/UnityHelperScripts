using UnityEngine;

public abstract class A_State : MonoBehaviour, I_StateBehaviour{

    public abstract void OnStateEnter();
    public abstract void OnStateExit();


    public string StateName { get { return gameObject.name; } }
}
