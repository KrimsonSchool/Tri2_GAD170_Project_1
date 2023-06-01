using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public UITextTypeWriter uttw;
    public Player Player;
    public int id;
    [HideInInspector] public int stage;

    public Dialogue[] dialogue;

    string inp;
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        inp = Player.submittedText;

        //handing commands
        if (Player.submittedText == "hlp")
        {
            uttw.story = "\n>ATK - Attack {target} \n>INV - Open inventory\n>";
            SetText();
        }
        else
        {
            //welcome screen
            if (dialogue[id].stage == 0)
            {
                if(inp == "str")
                {
                    id = 1; 
                    SetText();
                    clear();
                }
                if(inp == "qut")
                {
                    SceneManager.LoadScene(0);
                }
            }
            //will you help
            if (dialogue[id].stage == 1)
            {
                if (inp == "yes" || inp == "y")
                {
                    id = 2;
                    SetText();
                }
                if(inp == "no" || inp == "n")
                {
                    id = 3;
                    SetText();
                }
            }
            //will you help please ( if you said no to the above plead for help )
            if (dialogue[id].stage == 2)
            {
                if (inp == "yes" || inp == "y")
                {
                    id = 2;
                    SetText();
                }
                if (inp == "no" || inp == "n")
                {
                    id = 4;
                    SetText();
                }
            }
        }
    }
    public void SetText()
    {
        uttw.story = "";
        uttw.story = dialogue[id].text;
        Player.submittedText = "";
        uttw.StartCoroutine("PlayText");
    }

    public void clear()
    {
        text.text = "";
    }
}
