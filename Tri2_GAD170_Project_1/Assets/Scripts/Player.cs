using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player Stats")]
    public int hp;
    public int hpBase;
    public int attack;
    public int attackBase;
    public int level;
    public float xp;
    public float xpMax;
    public Item[] items;
    public Item[] equippedItems;

    [Header("Text Stuff")]
    public TMPro.TMP_InputField player_Input;
    public TMPro.TextMeshProUGUI TEXT;
    [HideInInspector] public string submittedText;
    public UITextTypeWriter typer;

    public GameManager GameManager;

    public GameObject levelUpButton;
    public GameObject win;
    void Start()
    {
        attack = attackBase;
        hp = hpBase;
    }

    // Update is called once per frame
    void Update()
    {
        //handing xp, when xp is above or at max xp:
        if (xp >= xpMax)
        {
            levelUpButton.SetActive(true);
        }

        //handing player submitting text
        if (Input.GetKeyDown(KeyCode.Return) && player_Input.text != " ")
        {
            TEXT.text += "\n" + player_Input.text + "\n";
            submittedText = player_Input.text;
            GameManager.inp = submittedText;
            player_Input.text = " ";

            SUBMIT_TEXT();
        }

        if(hp<= 0)
        {
            GameManager.Quit();
        }


        //if the players level is 5 or greater then
        if (level >= 5)
        {
            //they win!!!
            win.SetActive(true);
            print("You Win!!!");
        }
    }

    public void SUBMIT_TEXT()
    {
        //print(GameManager.inp);
        //if the user submits input before text type, interuppt
        typer.story = "";
        GameManager.CheckCommands();
    }

    public void CalcStats()
    {
        //for (int i = 0; i < equippedItems.Length; i++)
        //{
        //   if (equippedItems[i] != null)
        //    {
        //        attack += equippedItems[i].atk;
        //        hp += equippedItems[i].def;
        //    }
        //}

        hpBase = Mathf.RoundToInt(hp * 1.25f);
        attackBase = Mathf.RoundToInt(attack * 1.2525f);

        attack = attackBase;
        hp = hpBase;
    }

    public void LevelUp()
    {
        CalcStats();
        //xp is minused by xp max
        xp -= xpMax;
        //xp max is set to 125% of value
        xpMax *= 1.25f;
        //level has 1 added to it
        level += 1;

        levelUpButton.SetActive(false);
        print("You leveled up!");

        print("attack value is " + attack);
    }
}
