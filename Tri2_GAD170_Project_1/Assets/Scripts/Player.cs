using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player Stats")]
    public int hp;
    public int attack;
    public int level;
    public float xp;
    public float xpMax;

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
    }
}
