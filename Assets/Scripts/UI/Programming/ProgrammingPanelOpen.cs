using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ProgrammingPanelOpen : MonoBehaviour
{
    public GameObject panel;
    public Button firstButton;
    void Start() => Cursor.lockState = CursorLockMode.Locked;

    public void OpenProgrammingPanel(InputAction.CallbackContext value) {
        if (Controller.control) {
            Controller.control = false;
            panel.SetActive(true);
            firstButton.Select();
        }
    }

    public void CloseProgrammingPanel() {
        Controller.control = true;
        panel.SetActive(false);
    }
}