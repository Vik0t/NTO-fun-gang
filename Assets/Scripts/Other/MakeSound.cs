using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    public GameObject soundsSelect;

    public void MakeTheSound() => Instantiate(soundsSelect);
}
