using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BotCommands {
    Move,
    Rotate,
    Jump,
    Up,
    Down,
    Pick,
    Put,
    Attack
}

public class ApplyCommands : MonoBehaviour
{
    public List<int> cmds;
    private int currentBotInd;
    private CommandsChanger commandsChanger;
    [SerializeField] private List<Blocks> blocks = new List<Blocks>();
    [SerializeField] private List<Bots> bots = new List<Bots>();
    public GameObject[] buttons;
    private Transform commandList;
    public Image botChangeButton;

    private void Start() {
        commandsChanger = gameObject.GetComponent<CommandsChanger>();
        UpdateAvaliableList();
        botChangeButton.sprite = bots[currentBotInd].img;  
    }

    public void Apply() {
        switch (bots[currentBotInd].name) {
            case "GroundBot":
                GroundBot bot = GameObject.FindGameObjectWithTag(bots[currentBotInd].name).GetComponent<GroundBot>();
                bot.StartDoCommands(cmds);
                break;
            case "FlightBot":
                FlightBot bot1 = GameObject.FindGameObjectWithTag(bots[currentBotInd].name).GetComponent<FlightBot>();
                break;
        }
    }

    public void BackBot() {

    }

    public void ChangeBot() {
        currentBotInd += 1;
        if (currentBotInd >= bots.Count) currentBotInd = 0;
        Delete();
        UpdateAvaliableList();
        botChangeButton.sprite = bots[currentBotInd].img;  
    }

    public void Delete() {
        commandList = GameObject.FindGameObjectWithTag("CommandList").transform;
        for (int itemNum = 0; itemNum < commandList.childCount; itemNum++) {
            Transform obj = commandList.GetChild(itemNum).transform;
            
            if (obj.childCount == 0) break;
            else {
                Transform child = obj.GetChild(0).transform;
                Destroy(child.gameObject);
            }
        }
        cmds = new List<int>();
    }

    private void AppedNewCommand(int i) {
        commandList = GameObject.FindGameObjectWithTag("CommandList").transform;       
        for (int itemNum = 0; itemNum < commandList.childCount; itemNum++) {
            Transform obj = commandList.GetChild(itemNum).transform;
            
            if (obj.childCount == 0) {
                GameObject newObj = Instantiate(blocks[i].prefab);
                newObj.transform.position = obj.position;
                newObj.transform.parent = obj;
                newObj.name = blocks[i].name;
                newObj.transform.localScale = new Vector3(1,1,1);
                break;
            }
        }
        commandsChanger.UpdateUI();
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
}