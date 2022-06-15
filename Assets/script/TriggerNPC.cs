using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNPC : MonoBehaviour
{
    public GameObject Button;
    public GameObject TextUI;


    void OnTriggerEnter2D (Collider2D other) {
        Button.SetActive(true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Button.SetActive(false);
    }

    private void Update() {

        if (Button.activeSelf && Input.GetKeyDown(KeyCode.A)) {
            TextUI.SetActive(true);

        }



    }



}
