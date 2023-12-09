using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public string animName;
    public void Play() => gameObject.GetComponent<Animator>().Play(animName);
}
