using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BotCommands {
    Move,
    Rotate,
    Jump,
    Up,
    Down,
    Pick,
    Put,
    Attack,
    If
}

public class ApplyCommands : MonoBehaviour
{
    public List<int> cmds;
    private int currentBotInd;
    [SerializeField] private List<Blocks> blocks = new List<Blocks>();
    [SerializeField] private List<Bots> bots = new List<Bots>();
    public GameObject[] buttons;
    public GameObject[] operators;
    private Transform commandList;
    public Image botChangeButton;
    private GameObject groundBot;
    private GameObject flyingBot;
    private GameObject battleBot;
    private GameObject heavyBot;
    private GameObject shieldBot;
    public GameObject listPrefab;
    private int currentList;
    public GameObject mainPanel;
    public TMP_Text listIndexIndicator;
    public TMP_Text listCounter;

    private void Start() {
        UpdateAvaliableList();
        botChangeButton.sprite = bots[currentBotInd].img;  

        groundBot = GameObject.FindGameObjectWithTag(bots[0].name);
        flyingBot = GameObject.FindGameObjectWithTag(bots[1].name);
        battleBot = GameObject.FindGameObjectWithTag(bots[2].name);
        heavyBot  = GameObject.FindGameObjectWithTag(bots[3].name);
        shieldBot = GameObject.FindGameObjectWithTag(bots[4].name);

        commandList = GameObject.FindGameObjectWithTag("CommandList").transform;
        mainPanel.SetActive(false);
    }

    public void Apply() {
        switch (bots[currentBotInd].name) {
            case "GroundBot":
                groundBot.GetComponent<GroundBot>().StartDoCommands(bots[currentBotInd].chosenCommands);
                break;
            case "FlightBot":
                flyingBot.GetComponent<FlightBot>().StartDoCommands(bots[currentBotInd].chosenCommands);
                break;
            case "BattleBot":
                battleBot.GetComponent<FlightBot>().StartDoCommands(bots[currentBotInd].chosenCommands);
                break;
            case "HeavyBot":
                heavyBot.GetComponent<GroundBot>().StartDoCommands(bots[currentBotInd].chosenCommands);
                break;
            case "ShieldBot":
                shieldBot.GetComponent<GroundBot>().StartDoCommands(bots[currentBotInd].chosenCommands);
                break;
        }
    }

    public void Restart() =>  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ChangeBot() {
        bots[currentBotInd].chosenCommands = cmds;
        Delete(false);
        currentBotInd += 1;
        if (currentBotInd >= bots.Count) currentBotInd = 0;
        UpdateAvaliableList();
        botChangeButton.sprite = bots[currentBotInd].img;
        foreach (int i in bots[currentBotInd].chosenCommands) {
            AppedNewCommand(i);
        }
    }

    public void Delete(bool isAll) {
        if (isAll) bots[currentBotInd].chosenCommands = new List<int>();
        for (int list = 0; list < commandList.childCount; list++) {
            Transform listObj = commandList.GetChild(list);
            for (int itemNum = 0; itemNum < listObj.childCount; itemNum++) {
                
                Transform obj = listObj.GetChild(itemNum).transform;
                
                if (obj.childCount != 0) {
                    Transform child = obj.GetChild(0).transform;
                    child.parent = null;
                    Destroy(child.gameObject);
                }
            }
        }
        cmds = new List<int>();
    }

    private void AppedNewCommand(int i) {
        for (int list = 0; list < commandList.childCount; list++) {
            Transform listObj = commandList.GetChild(list);
            for (int itemNum = 0; itemNum < listObj.childCount; itemNum++) {
                
                Transform obj = listObj.GetChild(itemNum).transform;
                
                if (obj.childCount == 0) {
                    GameObject newObj = Instantiate(blocks[i].prefab);
                    newObj.transform.position = obj.position;
                    newObj.transform.parent = obj;
                    newObj.name = blocks[i].name;
                    newObj.transform.localScale = new Vector3(1,1,1);
                    cmds.Add(i);
                    return;
                }
            }
        }    
    }

    public void ButtonDistributor(string buttonName) {
        for (int i = 0; i < blocks.Count; i++) {
            if (blocks[i].name == buttonName) {
                AppedNewCommand(i);
            }
        }
    }

    private void UpdateAvaliableList() {
        for (int k = 0; k < buttons.Length; k++) {
            buttons[k].SetActive(false);
        }
        foreach (int index in bots[currentBotInd].avaliableCommands) {
            buttons[index].SetActive(true);
        }
    }

    public void AddList() {
        GameObject newList = Instantiate(listPrefab);
        newList.transform.parent = commandList;
        newList.transform.localScale = new Vector3(1,1,1);

        RectTransform rt = newList.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(0,-280,0);
        rt.sizeDelta = new Vector2(1,550);

        newList.GetComponent<Canvas>().enabled = false;
        listCounter.text = "At all: " + commandList.childCount.ToString();
    }

    public void ChangeListIndex(bool next) {
        if (next) {
            currentList++;
            if (currentList >= commandList.childCount) currentList = 0;
        }
        else {
            currentList--;
            if (currentList < 0) currentList = commandList.childCount-1;
        }
        listIndexIndicator.text = currentList.ToString();
        for (int i = 0; i < commandList.childCount; i++) {
            Transform child = commandList.GetChild(i);
            child.gameObject.GetComponent<Canvas>().enabled = false;
        }
        commandList.GetChild(currentList).GetComponent<Canvas>().enabled = true;
    }
}

[System.Serializable]
public class Blocks {
    public string name;
    public GameObject prefab;
}

[System.Serializable]
public class Bots {
    public string name;
    public Sprite img;
    public List<int> avaliableCommands;
    public List<int> chosenCommands;
}