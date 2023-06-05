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
    public Location[] location;

    [HideInInspector] public string inp;

    [HideInInspector]public int cond;
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckCommands()
    {
        if (cond == 0)
        {
            //handing commands
            if (inp == "hlp")
            {
                DirSet("\n>ATK - Attack {target}\n>INV - Open inventory\n>GO - go to {target} location\n>CLS - clear the console\nRES - resume the main quest");
            }
            else if (inp == "go")
            {
                DirSet("\nWhere would you like to go?\n");
                cond = 1;
            }
            else if(inp == "cls")
            {
                clear("use the [hlp] command to list all commands\n");
            }
            else if(inp == "res")
            {
                clear("");
                SetText();
            }
            else
            {
                //welcome screen
                if (dialogue[id].stage == 0)
                {
                    if (inp == "str")
                    {
                        id = 1;
                        SetText();
                        clear("");
                    }
                    if (inp == "qut")
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
                    if (inp == "no" || inp == "n")
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
        else
        {
            CheckLoc();
            cond -= 1;
        }
    }
    public void SetText()
    {
        uttw.story = dialogue[id].text;
        uttw.StartCoroutine("PlayText");
    }

    public void DirSet(string txt)
    {
        uttw.story = txt;
        uttw.StartCoroutine("PlayText");
    }

    public void clear(string i)
    {
        text.text = i;
    }

    public void CheckLoc()
    {
        for (int i = 0; i < location.Length; i++)
        {
            if (location[i].lName == inp && !location[i].locked)
            {

                if (location[i].id == id)
                {
                    id = location[i].qStage;
                    stage = location[i].qStage;
                    clear("\n");
                    DirSet(location[i].descText + "\n\n" + dialogue[id].text);
                }
                else
                {
                    DirSet(location[i].descText);
                }
            }
            else
            {
                DirSet("\nError, Unknown location selected!");
            }
        }
    }
}
