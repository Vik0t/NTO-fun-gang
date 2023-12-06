using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorButton : MonoBehaviour
{
    public Sprite pressedSprite;
    public Sprite releasedSprite;
    public List<string> activatedBy;

    public UnityEvent onPress;
    public UnityEvent onRelease;

    public SpriteRenderer indicator;
    private int counter;

    void Awake() {
        indicator.sprite = releasedSprite;
        counter = 0;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (activatedBy.Contains(coll.tag)) {
            if (counter == 0) {
                gameObject.GetComponent<AnimatorBooleanTransition>().SetState(true);
                indicator.sprite = pressedSprite;
                onPress.Invoke();
            }
            counter++;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (activatedBy.Contains(coll.tag)) {
            counter--;
            if (counter == 0) {
                gameObject.GetComponent<AnimatorBooleanTransition>().SetState(false);
                indicator.sprite = releasedSprite;
                onRelease.Invoke();
            }
        }
    }
}