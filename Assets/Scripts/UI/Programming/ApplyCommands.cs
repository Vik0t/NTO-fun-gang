using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Muratich {
    public class ApplyCommands : MonoBehaviour
    {
        private GroundBot groundBot;
        private FlightBot flightBot;
        public List<int> cmds;

        private void Start() {
            groundBot = GameObject.FindGameObjectWithTag("GroundBot").GetComponent<GroundBot>();
            flightBot = GameObject.FindGameObjectWithTag("FlightBot").GetComponent<FlightBot>();
        }

        public void ApplyForGround() {
            groundBot.StartDoCommands(cmds);
        }

        public void ApplyForFlight() {
            flightBot.StartDoCommands(cmds);
        }
    }
}
