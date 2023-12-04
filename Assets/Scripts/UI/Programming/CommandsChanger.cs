using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsChanger : MonoBehaviour {
    Transform commandList;
    private ApplyCommands applyCommands;
        
    void Start()
    {
        applyCommands = gameObject.GetComponent<ApplyCommands>();
    }

    public void UpdateUI() {
        commandList = GameObject.FindGameObjectWithTag("CommandList").transform;
        applyCommands.cmds = new List<int>();
        for (int itemNum = 0; itemNum < commandList.childCount; itemNum++) {
            Transform obj = commandList.GetChild(itemNum).transform;

            if (obj.childCount != 0) {
                Transform child = obj.transform.GetChild(0);
                switch (child.gameObject.name)
                {
                    case "Move":
                        { applyCommands.cmds.Add((int)BotCommands.Move); break; }
                    case "Rotate":
                        { applyCommands.cmds.Add((int)BotCommands.Rotate); break; }
                    case "Jump":
                        { applyCommands.cmds.Add((int)BotCommands.Jump); break; }
                    case "Up":
                        { applyCommands.cmds.Add((int)BotCommands.Up); break; }
                    case "Down":
                        { applyCommands.cmds.Add((int)BotCommands.Down); break; }
                    case "Pick":
                        { applyCommands.cmds.Add((int)BotCommands.Pick); break; }
                    case "Put":
                        { applyCommands.cmds.Add((int)BotCommands.Put); break; }
                    case "Attack":
                        { applyCommands.cmds.Add((int)BotCommands.Attack); break; }
                }
            }
        }
    }
}