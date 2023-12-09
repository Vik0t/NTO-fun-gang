using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanging : MonoBehaviour
{
    public Sprite colorDefault;
    public Sprite colorCondition;
    public void ChangeColor(bool isCondition) {
        Image img = gameObject.GetComponent<Image>();

        if (isCondition) img.sprite = colorCondition;
        else img.sprite = colorDefault;
    }
}
