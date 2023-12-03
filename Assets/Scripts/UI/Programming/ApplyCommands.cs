using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GroundBot groundBot;
    private FlightBot flightBot;
    public List<int> cmds;
    private int currentBotInd;
    private CommandsChanger commandsChanger;
    [SerializeField] private List<Blocks> blocks = new List<Blocks>();
    private Transform commandList;

    private void Start() {
        //groundBot = GameObject.FindGameObjectWithTag("GroundBot").GetComponent<GroundBot>();
        flightBot = GameObject.FindGameObjectWithTag("FlightBot").GetComponent<FlightBot>();
        cmds = new List<int>();
        commandsChanger = gameObject.GetComponent<CommandsChanger>();
    }

    public void Apply() {
        Debug.Log("Apply");
        foreach (int i in cmds) {
            Debug.Log("Ind_"+i.ToString());
        }
        //groundBot.StartDoCommands(cmds);
        flightBot.StartDoCommands(cmds);
    }

    public void ChangeBot() {
        currentBotInd += 1;
        if (currentBotInd >= 2) currentBotInd = 0;
        Delete();
        UpdateAvaliableList();
    }

    public void UpdateAvaliableList() {

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
}

[System.Serializable]
public class Blocks {
    public string name;
    public GameObject prefab;
    public int botIndex;
}