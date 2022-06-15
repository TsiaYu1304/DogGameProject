using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI元件")]

    public Text textLabel;

    [Header("文本內容")]

    public TextAsset textFile;
    public int index;

    List<string> textList = new List<string>();

    void Awake()
    {
        GetTextFromFile(textFile);


    }
    private void OnEnable() {
        textLabel.text = textList[index];
        index++;
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && index == textList.Count) {
            gameObject.SetActive(false);
            index = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            textLabel.text = textList[index];
            index++;
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;


        var lineData =  file.text.Split('\n');

        foreach (var line in lineData) {
            textList.Add(line);
        }
    }


}
