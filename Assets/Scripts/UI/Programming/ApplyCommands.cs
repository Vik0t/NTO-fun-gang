using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private CommandsChanger commandsChanger;
    [SerializeField] private List<Blocks> blocks = new List<Blocks>();
    [SerializeField] private List<Bots> bots = new List<Bots>();
    public GameObject[] buttons;
    private Transform commandList;
    public Image botChangeButton;
    private GameObject groundBot;
    private GameObject flyingBot;

    private void Start() {
        commandsChanger = gameObject.GetComponent<CommandsChanger>();
        UpdateAvaliableList();
        botChangeButton.sprite = bots[currentBotInd].img;  

        groundBot = GameObject.FindGameObjectWithTag(bots[0].name);
        flyingBot = GameObject.FindGameObjectWithTag(bots[1].name);
    }

    public void Apply() {
        groundBot.GetComponent<GroundBot>().StartDoCommands(cmds);
        flyingBot.GetComponent<FlightBot>().StartDoCommands(cmds);
    }

    public void Restart() =>  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ChangeBot() {
        Delete();
        currentBotInd += 1;
        if (currentBotInd >= bots.Count) currentBotInd = 0;
        UpdateAvaliableList();
        botChangeButton.sprite = bots[currentBotInd].img;
    }

    public void Delete() {
        bots[currentBotInd].chosenCommands = cmds;
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
    public List<int> chosenCommands;
}