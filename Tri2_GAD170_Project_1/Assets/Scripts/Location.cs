using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public string lName;
    public bool locked;

    [TextAreaAttribute(15, 20)]
    public string descText;

    public int qStage;
    public int id;

    public bool enemies;
    public Enemy[] enemy;

    public bool childLocs;
}
