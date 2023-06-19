using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int attk;
    public float spd;
    public int xpReward;
    public int level;

    private void Start()
    {
        level = level + Random.Range(1, 26);
        hp = Mathf.RoundToInt(level * 1.25f);
    }
    private void Update()
    {
    }
}
