using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBooleanTransition : MonoBehaviour
{
    public string paramName = "Speed";
    private Animator anim;

    void Awake () {
        anim = GetComponent<Animator> ();
    }

    public void SetState (bool state) {
        anim.SetFloat (paramName, state ? 1 : -1);
        float stateTime = anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
        if (stateTime < 0 || stateTime > 1) {
            anim.Play (0, 0, Mathf.Clamp01 (stateTime));
        }
    }
}