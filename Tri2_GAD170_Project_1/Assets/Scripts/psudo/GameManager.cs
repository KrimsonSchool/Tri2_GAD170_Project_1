using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public UITextTypeWriter uttw;
    public Player player;
    public int id;
    [HideInInspector] public int stage;

    public Dialogue[] dialogue;
    public Location[] location;

    [HideInInspector] public string inp;

    [HideInInspector]public int cond;

    public Item[] itemsToGive;

    Location currentLocation;

    int turn;
    float turnTimer;
    bool combat;

    int killed;

    Enemy currentEnemy;

    //ONLY EVER USE += WITH BACK BUFFER!!!!!!!!!!!!!!!!!!!!!
    string backBuffer;
    //ONLY EVER USE += WITH BACK BUFFER!!!!!!!!!!!!!!!!!!!!!

    // Start is called before the first frame update
    void Start()
    {
        //set the text to write and the buffer to be blank
        uttw.story = "";
        uttw.buffer = "";

        //calls function SetText
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        //if the id is equal to 5 then
        if (id == 5)
        {
            //loops 2 times
            for (int i = 0; i < 2; i++)
            {
                //set the items in the players inventory to the items to give to them
                player.items[i] = itemsToGive[i];
            }
        }

        //if the player is in combat then
        if (combat)
        {
            //if turn is greater or equal to 1 then
            if (turn >= 1)
            {
                //the turn timer counts up in seconds
                turnTimer += Time.deltaTime;
            }

            //if the turn timer is greater or equal to 1 and turn is greater or equal to 1 then
            if (turnTimer >= 1 && turn >= 1)
            {
                //set the string atk to be blank
                string atk = "";
                //for every enemy in the current area
                for (int i = 0; i < currentLocation.enemy.Length; i++)
                {
                    //if the enemy slot is not null then
                    if(currentLocation.enemy[i] != null)
                    {
                        //if the turn is equal to the speed of the enemy then
                        if (turn == currentLocation.enemy[i].spd)
                        {
                            //the players health is minused by the enemies attack
                            player.hpBase -= currentLocation.enemy[i].attk;
                            //print that the player took damage and how much and who from
                            atk += "\nyou take " + currentLocation.enemy[i].attk + " damage from " + currentLocation.enemy[i].name;

                            //decriment the turn
                            turn -= 1;
                        }
                    }
                    
                }
                //print the atk text
                DirSet(atk);

                //set the turn timer to 0
                turnTimer = 0;
            }
        }



        //ONLY EVER USE += WITH BACK BUFFER!!!!!!!!!!!!!!!!!!!!!
        if(uttw.buffer == "" && backBuffer!="")
        {
            //^ if the buffer is empty  and the back buffer is not empty then
            //set the buffer to be the back buffer
            uttw.buffer = backBuffer;
            //set the back buffer to be blank
            backBuffer = "";
        }
        //ONLY EVER USE += WITH BACK BUFFER!!!!!!!!!!!!!!!!!!!!!
    }

    //function to check player input commands
    public void CheckCommands()
    {
        //if the condition is 0 then
        if (cond == 0)
        {
            //handing commands

            //if the input is hlp then
            if (inp == "hlp")
            {
                //list all of the commands available to the player
                DirSet("\n>ATK - Attack {target}\n>INV - Open inventory\n>GO - go to {target} location\n>CLS - clear the console\n>RES - resume the main quest\n>EQP - Equip {Target} item\n>LOC - prints current location\n>PLR - print player stats");
            }
            //if the input is go and the player is not in combat then
            else if (inp == "go" && !combat)
            {
                //ask the player where they want to go
                DirSet("\nWhere would you like to go?\n");
                //set the condition to 1
                cond = 1;
            }
            //if the input is cls then
            else if (inp == "cls")
            {
                //call the clear command and print how to call the hlp command
                clear("use the [hlp] command to list all commands\n");
            }
            //if the input is res and the player is not in combat then
            else if (inp == "res" && !combat)
            {
                //call the clear function
                clear("");
                //call the set text function
                SetText();
            }
            //if the input is inv then
            else if (inp == "inv")
            {
                //set the string itemsa to be blank
                string itemsa = "";
                //for every item the playe has
                for (int i = 0; i < player.items.Length; i++)
                {
                    //if the item slot is not empty then
                    if (player.items[i] != null)
                    {
                        //if the item is equipped then
                        if (player.items[i].equipped)
                        {
                            //add that item to the string itemsa with (E) before it to signify its equipped
                            itemsa += "\n>(E)" + player.items[i].name;
                        }
                        //if not then
                        else
                        {
                            //add the item to the string itemsa
                            itemsa += "\n>" + player.items[i].name;
                        }
                    }

                }
                //call the clear function then print the word inventory
                clear("INVENTORY:\n");
                //print the string itemsa
                DirSet(itemsa);
            }
            //if input is eqp then
            else if (inp == "eqp")
            {
                //ask the player what item they would like to equip
                DirSet("\nWhat item would you like to equip?\n");
                //set the condition to 2
                cond = 2;
            }
            //if input is atk and the player is in combat and the turn is greater or equal to 0 then
            else if (inp == "atk" && combat && turn >= 0)
            {
                //ask who the player wants to attak
                DirSet("\nWho would you like to attack?\n");
                //set the condition to 3
                cond = 3;
            }
            //if input is loc then
            else if(inp == "loc")
            {
                //call the clear function
                clear("");
                //call the prntloc function
                PrntLoc();
            }
            //if input is plr then
            else if(inp == "plr")
            {
                //print the players current stats
                DirSet("\nCURRENT STATS:\n\n" + "Level: " + player.level + "\n" + player.xp + "/" + player.xpMax + " Xp\nAttack: " + player.attack + "\nHealth: " + player.hp);
            }
            //if not then
            else
            {
                //welcome screen

                //if the dialogues stage is 0 then
                if (dialogue[id].stage == 0)
                {
                    //if input is str then
                    if (inp == "str")
                    {
                        //set the id to 1
                        id = 1;
                        //call the function settext
                        SetText();
                        //call the function clear
                        clear("");
                    }
                    //if input is qut
                    if (inp == "qut")
                    {
                        //load the prior scene
                        SceneManager.LoadScene(0);
                    }
                }
                //will you help

                //if the dialogues stage is 1 then
                else if (dialogue[id].stage == 1)
                {
                    //if input is yes or y then
                    if (inp == "yes" || inp == "y")
                    {
                        //set the id to 2
                        id = 2;
                        //call the function settext
                        SetText();
                    }
                    //if input is no or n then
                    if (inp == "no" || inp == "n")
                    {
                        //set the id to 3
                        id = 3;
                        //call the function settext
                        SetText();
                    }
                }
                //will you help please ( if you said no to the above plead for help )

                //if the dialogues stage is 2 then
                else if (dialogue[id].stage == 2)
                {
                    //if input is yes or y then
                    if (inp == "yes" || inp == "y")
                    {
                        //set the id to 2
                        id = 2;
                        //call the function settext
                        SetText();
                    }
                    //if input is no or n then
                    if (inp == "no" || inp == "n")
                    {
                        //set the id to 4
                        id = 4;
                        //call the function settext
                        SetText();
                    }
                }
                //if not then
                else
                {
                    //print that the command was not found and to use the hlp command
                    DirSet("Error, command not found, use the [HLP] command to check all commands");
                }
            }
        }
        //if condition is 1 then
        else if(cond == 1)
        {
            //call the function checkloc
            CheckLoc();
            //set condition to 0
            cond = 0;
        }
        //if condition is 2 then
        else if(cond == 2)
        {
            //call the function checkitem
            CheckItem();
            //set condition to 0
            cond = 0;
        }
        //if condition is 3 then
        else if(cond == 3)
        {
            //call the function attack process
            AttackProcess();
            //set condition to 0
            cond = 0;
        }
    }

    //the function settext
    public void SetText()
    {
        //if the text to print is blank then
        if(uttw.story == "")
        {
            //set the text to print to the dialogue of the current id's text
            uttw.story = dialogue[id].text;
            //start to coroutine to type the text
            uttw.StartCoroutine("PlayText");
        }
        //if not then
        else
        {
            //call the function buffset with the dialogue of the current id'd text
            BuffSet(dialogue[id].text);
        }
    }

    //the function dirset
    public void DirSet(string txt)
    {
        //if the text to set is blank then
        if (uttw.story == "")
        {
            //set the text to set to the txt string
            uttw.story = txt;
            //start the coroutine to type the text
            uttw.StartCoroutine("PlayText");
        }
        //if not then
        else
        {
            //call the function buffset with the txt string
            BuffSet("\n\n"+txt);
        }
    }

    //the buffset function
    public void BuffSet(string txt)
    {
        //if the buffer is blank then
        if(uttw.buffer == "")
        {
            //set the buffer to the txt variable
            uttw.buffer = txt;
        }
        //if not then
        else
        {
            //add the txt variable to the backbuffer

            //BuffSet(txt);
            backBuffer += "\n\n"+txt;
            //print("Buffer's full");
        }
    }

    //the clear function
    public void clear(string i)
    {
        //set the on screen text to be blank
        text.text = ""+i;
    }

    //check the entered location name against the array of location names

    //the checkloc function
    public void CheckLoc()
    {
        //set the boolean noset to true
        bool noset = true;
        //for every location
        for (int i = 0; i < location.Length; i++)
        {
            //if the locations name is equal to the input and the location isn't locked and the player isn't in combat then
            if (location[i].lName == inp && !location[i].locked && !combat)
            {
                //set the players current location to be the locaton
                currentLocation = location[i];
                //if the locations idme variable is true then
                if (currentLocation.idMe)
                {
                    //if the locations id is equal to id then
                    if (location[i].id == id)
                    {
                        //id is set to the locations qstage variable
                        id = location[i].qStage;
                        //stage is set to the locations qstage variable
                        stage = location[i].qStage;
                        //the clear function is called
                        clear("\n");
                        //print the locations description text aswell as any quest dialogue
                        DirSet(location[i].descText + "\n\n" + dialogue[id].text);
                        //set the boolean noset to be false
                        noset = false;
                    }
                    //if not then
                    else
                    {
                        //set noset to be false
                        noset = false;
                        //call the function prntloc
                        PrntLoc();
                    }
                }
                //if not then
                else
                {
                    //set noset to false
                    noset = false;
                    //call the function prntloc
                    PrntLoc();
                }

                //if the location has enemies then
                if (location[i].enemies)
                {
                    //do nothing????????? idk
                }
            }
        }

        //if noset is true then
        if (noset)
        {
            //print that the entered location name was invalid
            DirSet("\nError, Unknown location selected!");
        }
    }

    //the function check item
    public void CheckItem()
    {
        //set the boolean noset to be true
        bool noset=true;
        //log to the console what the player has equipped in slot 0
        print("EQP: "+player.equippedItems[0]);
        //for every player item
        for (int i = 0; i < player.items.Length; i++)
        {
            //if the items name is equal to the input and the items not equipped then
            if (player.items[i].name == inp && !player.items[i].equipped)
            {
                //set the intiger lowest empty to be 999 (a large number)
                int lowestEmpty=999;

                //for every equipped player item
                for (int j = 0; j < player.equippedItems.Length; j++)
                {
                    //if the equipped slot is empty then
                    if (player.equippedItems[j] == null)
                    {
                        //if the slot no. is smallet than lowest empty then
                        if (j < lowestEmpty)
                        {
                            //lowest empty is set to the slot no.
                            lowestEmpty = j;
                        }
                    }
                }

                //set the equipped item to be in the lowest empty slot
                player.equippedItems[lowestEmpty] = player.items[i];
                //set the item as being equipped
                player.items[i].equipped = true;
                //print that the player equipped the item
                DirSet("You equip " + player.items[i].name);
                //set noset to be false
                noset = false;
                
            }
            //if the item name is equal to the input and the item is equipped then
            else if(player.items[i].name == inp && player.items[i].equipped)
            {
                //print that the item is already equipped
                DirSet(player.items[i].name + " is already equipped!");
                //set noset to be false
                noset = false;
                
            }
            
        }

        //if noset is true then
        if (noset)
        {
            //print that there is no item with the inputted name
            DirSet("No item with name " + inp + " found, use [INV] to check the items in your inventory.");
        }
    }

    //processing player attack command

    //the attack process command
    public void AttackProcess()
    {
        //set enemy check to be 0
        int enmyCheck = 0;

        //for every enemy in the current area
        for (int i = 0; i < currentLocation.enemy.Length; i++)
        {
            //if there is an enemy in selected slot
            if (currentLocation.enemy[i] != null)
            {
                //and if the player input name matches the selected enemies name then
                if (currentLocation.enemy[i].name == inp)
                {
                    //set that enemy as the enemy thats being currently attacked
                    currentEnemy = currentLocation.enemy[i];

                    //the enemy takes damage accoring to the players attack
                    currentEnemy.hp -= player.attack;
                    print("dealt" + player.attack + " damage!");

                    //if the enemies health is 0 then
                    if (currentEnemy.hp <= 0)
                    {
                        //the amount of enemies killed is +1
                        killed += 1;
                        
                        //if the player has killed the amount of enemies in the area then
                        if (killed == currentLocation.enemy.Length)
                        {
                            //display that you killed the enemy
                            DirSet("You hit " + currentEnemy + ", killing them and are rewarded with " + currentEnemy.xpReward + "xp. \n\n You've cleared the area!");
                            //add xp to player
                            player.xp += currentEnemy.xpReward;
                            print("You gained xp!");
                            //destroy the enemy object
                            Destroy(currentEnemy);
                            //set the enemy slot to null
                            currentLocation.enemy[i] = null;

                            //set so that the player is out of combat
                            combat = false;
                            killed = 0;
                            currentLocation.enemies = false;

                            //reset the player hp
                            player.hp = player.hpBase;

                            //clear the screen
                            clear("");
                            //show the location text
                            PrntLoc();
                        }
                        //if not then
                        else
                        {
                            //display that the player hit an enemy
                            DirSet("You hit " + currentEnemy + ", killing them and are rewarded with " + currentEnemy.xpReward + "xp.");
                            //add xp to player
                            player.xp += currentEnemy.xpReward;
                            print("You gained xp!");
                            //destroy the enemy object
                            Destroy(currentEnemy);
                            //set the enemy slot to null
                            currentLocation.enemy[i] = null;

                            //clear the screen
                            clear("");
                            //show the location text
                            PrntLoc();
                        }
                    }
                    //if not then
                    else
                    {
                        //display that the player hit the enemy
                        DirSet("You hit the " + currentEnemy.name + " for " + player.attack);
                    }

                    //increase the turn counter
                    turn += 1;
                    turnTimer = 0;
                }
                //if not then
                else
                {
                    //increase the amount of checked enemies
                    enmyCheck++;
                }
            }
            
        }

        //if the amount of checked enemies is equal to the amount of enemies then
        if(enmyCheck==currentLocation.enemy.Length)
        {
            //display that there is no enemy with the input name
            DirSet("No enemy with that name");
        }
    }

    //a fucntion to print the location text

    //the functio prtnloc
    void PrntLoc()
    {
        //if the location has children locations then
        if (currentLocation.childLocs)
        {
            //set the string locs to be blank
            string locs = "";
            //if the location has enemies then (it will actually do something this time???)
            if (currentLocation.enemies)
            {
                //set the string enm to be blank
                string enm = "";
                //for every enemy in the location
                for (int k = 0; k < currentLocation.enemy.Length; k++)
                {
                    //if the enemy slot is not blank then
                    if (currentLocation.enemy[k] != null)
                    {
                        //log to the console that an enemy has appeared
                        print("enemy appeared!");
                        //add the enemy name and location to the enm string
                        enm += "\nAn enemy " + currentLocation.enemy[k].name + " appeared! They have " + currentLocation.enemy[k].hp+"Hp.";
                    }
                }
                //for each child location of the location
                foreach (Location l in currentLocation.GetComponentsInChildren<Location>())
                {
                    //add their names to the locs string
                    locs += "\n>" + l.name;
                }
                //print the locations description text, the locs string and the enm string
                DirSet("\n\n" + currentLocation.descText + "\n\nFollowing Locations: " + locs + "\n\nEnemies: " + enm);
                //set that the player is in combat
                combat = true;
            }
            //if not
            else
            {
                //for each child location of the location
                foreach (Location l in currentLocation.GetComponentsInChildren<Location>())
                {
                    //add their names to the locs string
                    locs += "\n>" + l.name;
                }
                //print the locations description and the locs string
                DirSet("\n\n" + currentLocation.descText + "\n\nFollowing Locations: " + locs);
            }
        }
        //if not
        else
        {
            //if the location has enemies then
            if (currentLocation.enemies)
            {
                //set the string enm to be blank
                string enm = "";
                //for every enemy in the location
                for (int k = 0; k < currentLocation.enemy.Length; k++)
                {
                    //if the enemy slot is not empty then
                    if (currentLocation.enemy[k] != null)
                    {
                        //log to the console that an enemy appeared
                        print("enemy appeared!");
                        //add the enemies name and location text to the enm string
                        enm += "\nAn enemy " + currentLocation.enemy[k].name + " appeared! They have " + currentLocation.enemy[k].hp+"Hp.";
                    }
                }
                //print the locations description text aswell as the enm string
                DirSet("\n\n" + currentLocation.descText + "\n\nEnemies: " + enm);
                //set that the player is in combat
                combat = true;
            }
            //if not then
            else
            {
                //print the locations description text
                DirSet("\n\n" + currentLocation.descText);
            }
        }
    }

    //the function quit
    public void Quit()
    {
        //exit the application (only works when the game is built)
        Application.Quit();
        //set the time scale to be 0
        Time.timeScale = 0;
        //call the clear command
        clear("");
        //loog to the console that the player has died
        print("you died!");
    }
}
