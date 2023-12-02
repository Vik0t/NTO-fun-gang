using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muratich {
    public class ApplyCommands : MonoBehaviour
    {
        private GroundBot groundBot;
        private FlightBot flightBot;
        public List<int> cmds;
        private int currentBotInd;

        private void Start() {
            groundBot = GameObject.FindGameObjectWithTag("GroundBot").GetComponent<GroundBot>();
            flightBot = GameObject.FindGameObjectWithTag("FlightBot").GetComponent<FlightBot>();
        }

        public void Apply() {
            if (currentBotInd == 0) groundBot.StartDoCommands(cmds);
            else if (currentBotInd == 1) flightBot.StartDoCommands(cmds);
        }

        public void ChangeBot() {
            currentBotInd += 1;
            if (currentBotInd >= 2) currentBotInd = 0;
            UpdateUI();
        }

        private void UpdateUI() {

        }

        public void Delete() {
            
        }
    }
}
