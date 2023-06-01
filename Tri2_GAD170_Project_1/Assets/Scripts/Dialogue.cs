using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [TextAreaAttribute(15, 20)]
    public string text;

    public int stage;
}
