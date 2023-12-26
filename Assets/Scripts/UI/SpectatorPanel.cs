using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SpectatorPanel : MonoBehaviour
{
    private CinemachineVirtualCamera cvm;
    [SerializeField] private ApplyCommands ac;
    private int camIndex;
    private bool isPlayerSearch = true;
    public Sprite playerIcon;
    public Image img;
    private Transform player;
    private List<int> avaliableIndex = new List<int>();

    public void ApplyCam() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        avaliableIndex = ac.foundedDrones;
        ChangeImage();
    }

    public void NextSpec() {
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
        ChangeImage();
    }

    private void ChangeImage() {
        if (isPlayerSearch) {
            img.sprite = playerIcon;
            cvm.Follow = player;
        }
        else {
            img.sprite = ac.bots[avaliableIndex[camIndex]].img;
            Transform currBot = GameObject.Find(ac.bots[avaliableIndex[camIndex]].name).transform;
            cvm.Follow = currBot;
        }
    }
}