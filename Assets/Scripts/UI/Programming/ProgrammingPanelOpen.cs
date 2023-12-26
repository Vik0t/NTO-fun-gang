using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

public class ProgrammingPanelOpen : MonoBehaviour
{
    public GameObject panel;
    public Button firstButton;

    
    // Spectator
    private CinemachineVirtualCamera cvm;
    private int camIndex;
    private bool isPlayerSearch = true;
    private Transform player;
    private List<int> avaliableIndex = new List<int>();


    public void OpenProgrammingPanel(InputAction.CallbackContext value) {
        if (Controller.control) {
            Controller.control = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panel.SetActive(true);
            firstButton.Select();
        }
    }

    public void CloseProgrammingPanel() {
        Controller.control = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        panel.SetActive(false);
    }


    public void ApplyCam() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        avaliableIndex = gameObject.GetComponent<ApplyCommands>().foundedDrones;
        ChangePoint();
    }

    public void NextSpec(InputAction.CallbackContext value) {
        if (isPlayerSearch) {
            isPlayerSearch = false;
            camIndex = 0;
        }
        else {
            camIndex++;
            if (camIndex >= avaliableIndex.Count) {
                isPlayerSearch = true;
            }
        }
        ChangePoint();
    }

    private void ChangePoint() {
        if (isPlayerSearch) cvm.Follow = player;
        else {
            Transform currBot = GameObject.Find(gameObject.GetComponent<ApplyCommands>().bots[avaliableIndex[camIndex]].name).transform;
            cvm.Follow = currBot;
        }
    }
}