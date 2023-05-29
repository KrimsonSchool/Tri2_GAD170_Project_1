using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFSettingsAttacheSycope : MonoBehaviour
{
    TMPro.TMP_InputField ipf;
    // Start is called before the first frame update
    void Start()
    {
        ipf = GetComponent<TMPro.TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        ipf.Select();
        ipf.ActivateInputField();
    }
}
