using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Manager : MonoBehaviour
{
    public UITextTypeWriter uttw;
    public RESPONSE_TEXT[] RESPONSE_TEXT;
    public User User;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText()
    {
        if (User.username != null)
        {
            RESPONSE_TEXT[id].TEXT = RESPONSE_TEXT[id].TEXT.Replace("%", User.username);
        }
        uttw.story = RESPONSE_TEXT[id].TEXT;
        uttw.StartCoroutine("PlayText");
    }
}
