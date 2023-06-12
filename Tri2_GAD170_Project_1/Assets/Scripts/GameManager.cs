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
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        if (id == 5)
        {
            for (int i = 0; i < 2; i++)
            {
                player.items[i] = itemsToGive[i];
            }
        }
        if (combat)
        {
            if (turn >= 1)
            {
                turnTimer += Time.deltaTime;
            }

            if (turnTimer >= 1 && turn >= 1)
            {
                string atk = "";
                for (int i = 0; i < currentLocation.enemy.Length; i++)
                {
                    if(currentLocation.enemy[i] != null)
                    {
                        if (turn == currentLocation.enemy[i].spd)
                        {
                            player.hpBase -= currentLocation.enemy[i].attk;
                            atk += "\nyou take " + currentLocation.enemy[i].attk + " damage from " + currentLocation.enemy[i].name;

                            turn -= 1;
                        }
                    }
                    
                }
                DirSet(atk);

                turnTimer = 0;
            }

            
        }
    }
    public void CheckCommands()
    {
        if (cond == 0)
        {
            //handing commands
            if (inp == "hlp")
            {
                DirSet("\n>ATK - Attack {target}\n>INV - Open inventory\n>GO - go to {target} location\n>CLS - clear the console\nRES - resume the main quest\nEQP - Equip {Target} item");
            }
            else if (inp == "go" && !combat)
            {
                DirSet("\nWhere would you like to go?\n");
                cond = 1;
            }
            else if (inp == "cls")
            {
                clear("use the [hlp] command to list all commands\n");
            }
            else if (inp == "res" && !combat)
            {
                clear("");
                SetText();
            }
            else if (inp == "inv")
            {
                string itemsa = "";
                for (int i = 0; i < player.items.Length; i++)
                {
                    if (player.items[i] != null)
                    {
                        if (player.items[i].equipped)
                        {
                            itemsa += "\n>(E)" + player.items[i].name;
                        }
                        else
                        {
                            itemsa += "\n>" + player.items[i].name;
                        }
                    }

                }
                clear("INVENTORY:\n");
                DirSet(itemsa);
            }
            else if (inp == "eqp")
            {
                DirSet("\nWhat item would you like to equip?\n");
                cond = 2;
            }
            else if (inp == "atk" && combat && turn >= 0)
            {
                DirSet("\nWho would you like to attack?\n");
                cond = 3;
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
                else if (dialogue[id].stage == 1)
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
                else if (dialogue[id].stage == 2)
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
                else
                {
                    DirSet("Error, command not found, use the [HLP] command to check all commands");
                }
            }
        }
        else if(cond == 1)
        {
            CheckLoc();
            cond = 0;
        }
        else if(cond == 2)
        {
            CheckItem();
            cond = 0;
        }
        else if(cond == 3)
        {
            AttackProcess();
            cond = 0;
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

    //check the entered location name against the array of location names
    public void CheckLoc()
    {
        bool noset = true;
        for (int i = 0; i < location.Length; i++)
        {
            if (location[i].lName == inp && !location[i].locked && !combat)
            {
                currentLocation = location[i];
                if (location[i].id == id)
                {
                    id = location[i].qStage;
                    stage = location[i].qStage;
                    clear("\n");
                    DirSet(location[i].descText + "\n\n" + dialogue[id].text);
                    noset = false;
                }
                else
                {
                    noset = false;

                    if (location[i].childLocs)
                    {
                        string locs = "";
                        if (location[i].enemies)
                        {
                            string enm = "";
                            for (int k = 0; k < location[i].enemy.Length; k++)
                            {
                                if (location[i].enemy[k] != null)
                                {
                                    enm += "\nAn enemy " + location[i].enemy[k].name + " appeared!";
                                }
                            }
                            foreach (Location l in location[i].GetComponentsInChildren<Location>())
                            {
                                locs += "\n>" + l.name;
                            }
                            DirSet(location[i].descText + "\n\nFollowing Locations: " + locs + "\n\nEnemies: " + enm);
                            combat = true;
                        }
                        else
                        {
                            foreach (Location l in location[i].GetComponentsInChildren<Location>())
                            {
                                locs += "\n>" + l.name;
                            }
                            DirSet(location[i].descText + "\n\nFollowing Locations: " + locs);
                        }
                    }
                    else
                    {
                        DirSet(location[i].descText);
                    }
                }

                

                if (location[i].enemies)
                {

                }
            }
        }

        if (noset)
        {
            DirSet("\nError, Unknown location selected!");
        }
    }

    public void CheckItem()
    {
        bool noset=true;
        print("EQP: "+player.equippedItems[0]);
        for (int i = 0; i < player.items.Length; i++)
        {
            if (player.items[i].name == inp && !player.items[i].equipped)
            {
                int lowestEmpty=999;

                for (int j = 0; j < player.equippedItems.Length; j++)
                {
                    if (player.equippedItems[j] == null)
                    {
                        if (j < lowestEmpty)
                        {
                            lowestEmpty = j;
                        }
                    }
                }

                player.equippedItems[lowestEmpty] = player.items[i];
                player.items[i].equipped = true;
                DirSet("You equip " + player.items[i].name);
                noset = false;
                
            }
            else if(player.items[i].name == inp && player.items[i].equipped)
            {
                DirSet(player.items[i].name + " is already equipped!");
                noset = false;
                
            }
            
        }

        if (noset)
        {
            DirSet("No item with name " + inp + " found, use [INV] to check the items in your inventory.");
        }
    }

    //processing player attack command
    public void AttackProcess()
    {
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

                    //if the enemies health is 0 then
                    if (currentEnemy.hp <= 0)
                    {
                        //the amount of enemies killed is +1
                        killed += 1;
                        
                        //if the player has killed the amount of enemies in the area then
                        if (killed == currentLocation.enemy.Length)
                        {
                            DirSet("You hit " + currentEnemy + ", killing them and are rewarded with " + currentEnemy.xpReward + "xp. \n\n You've cleared the area!");
                            player.xp += currentEnemy.xpReward;
                            Destroy(currentEnemy);
                            currentLocation.enemy[i] = null;

                            combat = false;
                            killed = 0;
                            currentLocation.enemies = false;
                        }
                        //if not then
                        else
                        {
                            DirSet("You hit " + currentEnemy + ", killing them and are rewarded with " + currentEnemy.xpReward + "xp.");
                            player.xp += currentEnemy.xpReward;
                            Destroy(currentEnemy);
                            currentLocation.enemy[i] = null;
                        }
                    }
                    else
                    {
                        DirSet("You hit the " + currentEnemy.name + " for " + player.attack);
                    }
                    turn += 1;
                    turnTimer = 0;
                }
                else
                {
                    enmyCheck++;
                }
            }
            
        }

        if(enmyCheck==currentLocation.enemy.Length)
        {
            DirSet("No enemy with that name");
        }
    }
}
