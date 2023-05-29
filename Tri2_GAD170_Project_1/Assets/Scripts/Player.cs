using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public string username;
    public int hp;
    public int attack;
    public int level;
    public float xp;
    public float xpMax;

    [Header("Text Stuff")]
    public TMPro.TMP_InputField player_Input;
    public TMPro.TextMeshProUGUI TEXT;
    string submittedText;

    public Text_Manager Text_Manager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //handing xp, when xp is above or at max xp:
        if (xp >= xpMax)
        {
            //xp is minused by xp max
            xp -= xpMax;
            //xp max is set to 125% of value
            xpMax *= 1.25f;
            //level has 1 added to it
            level += 1;
        }

        //handing player submitting text
        if (Input.GetKeyDown(KeyCode.Return) && player_Input.text!= " ")
        {
            TEXT.text += "\n" + player_Input.text + "\n";
            submittedText = player_Input.text;
            player_Input.text = " ";


            if (Text_Manager.id != 0)
            {
                SUBMIT_TEXT();
            }
            else
            {
                username = submittedText;

                Text_Manager.id += 1;
                Text_Manager.SetText();
            }
        }

    }

    public void SUBMIT_TEXT()
    {
        //if the user submits input before text type, interuppt
        FindAnyObjectByType<UITextTypeWriter>().story = "";

        //handing commands
        if (submittedText == "hlp")
        {
            set(2);
        }
        else if(submittedText == "lst")
        {
            set(3);
        }
        else if(submittedText == "cls")
        {
            TEXT.text = "";
            set(1);
        }
        else
        {
            set(4);
        }
    }

    public void set(int id)
    {
        Text_Manager.id = id;
        Text_Manager.SetText();
    }
}
