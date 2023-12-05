using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visuals;

public class TransitionTest : MonoBehaviour
{
    public bool state = true;

    public AnimatorBooleanTransition trans;

    void OnValidate () {
        if (trans != null) {
            trans.SetState (state);
        }
    }
}
