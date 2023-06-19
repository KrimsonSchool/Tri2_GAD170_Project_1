using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteme : MonoBehaviour
{
    string[] firstNames;
    string[] lastNames;
    // Start is called before the first frame update
    void Start()
    {
        printMyName(5, "March", "7th"); 


        printMyNameArray();
    }

    public void printMyName(int j, string fn, string ln)
    {
        for (int i = 0; i < j; i++)
        {
            print(fn + " " + ln);
        }
    }



    public void printMyNameArray()
    {
        for (int i = 0; i < firstNames.Length; i++)
        {
            print(firstNames[i] + " " + lastNames[i]);
        }
    }
}
