using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Muratich {
    public class MenuDrop : MonoBehaviour
    {   
        [SerializeField] private GameObject panel;
        [SerializeField] private Button selectionButton;
        
        public void OpenPanel(InputAction.CallbackContext value) {
            if (Controller.control) {
                Controller.control = false;
                panel.SetActive(true);
                selectionButton.Select();
            }
        }

        public void ClosePanel() {
            Controller.control = true;
            panel.SetActive(false);
        }

        public void Restart() =>  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        public void Menu() => SceneManager.LoadScene(0);
    }
}
