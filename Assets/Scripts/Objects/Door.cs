using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{

    private AnimatorBooleanTransition abt;
    void Start() => abt = GetComponent<AnimatorBooleanTransition>();
    public void Open () {
        abt.SetState(true);
    }

    public void Close () {
        abt.SetState(false);
    }  
}
