using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muratich
{
    public class CommandsChanger : MonoBehaviour
    {
        GameObject commandList;
        ApplyCommands applyCommands = new ApplyCommands();
        public enum GroundBotCommands
        {
            Move,
            Rotate,
            Jump,
            Pick,
            Put,
            Attack
        }
        
        // Start is called before the first frame update
        void Start()
        {
            commandList = GameObject.Find("CommandList");
        }

        // Update is called once per frame
        void Update()
        {
            for (int itemNum = 0; itemNum < applyCommands.cmds.Count; itemNum++) 
            {
                GameObject child = commandList.transform.Find($"InventorySlot ({itemNum})").gameObject;
                switch (child.name)
                {
                    case "move":
                        { applyCommands.cmds[itemNum] = (int)GroundBotCommands.Move; break; }
                    case "Rotate":
                        { applyCommands.cmds[itemNum] = (int)GroundBotCommands.Rotate; break; }
                    case "Jump":
                        { applyCommands.cmds[itemNum] = (int)GroundBotCommands.Jump; break; }
                    case "Pick":
                        { applyCommands.cmds[itemNum] = (int)GroundBotCommands.Pick; break; }
                    case "Put":
                        { applyCommands.cmds[itemNum] = (int)GroundBotCommands.Put; break; }
                    case "Attack":
                        { applyCommands.cmds[itemNum] = (int)GroundBotCommands.Attack; break; }
                }

            }
        }
    }
}
