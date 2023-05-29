using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            
        }
    }
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.keyCode != KeyCode.None && Input.anyKeyDown && e.keyCode != KeyCode.Space && e.keyCode!=KeyCode.Backspace&&e.keyCode!=KeyCode.Return)
        {
            Text.text += e.keyCode;
        }
        else if(e.isKey && e.keyCode == KeyCode.Space && Input.anyKeyDown)
        {
            Text.text += " ";
        }
        else if (e.isKey && e.keyCode == KeyCode.Backspace && Input.anyKeyDown)
        {
            Text.text.Substring(Text.text.Length - 1);
        }
    }
}
