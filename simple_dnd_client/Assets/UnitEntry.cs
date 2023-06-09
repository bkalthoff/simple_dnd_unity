using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UnitEntry : MonoBehaviour
{
    public string instanceID;
    public string unitName;
    public int health;
    public int initiative;

    public int armorClass;

    public TMPro.TMP_Text nameText;

    
    public TMPro.TMP_InputField initiativeInputField;
    public TMPro.TMP_InputField healthInputField;

    public TMPro.TMP_InputField armorClassInputField;

    // Start is called before the first frame update
    void Start()
    {
        health = 0;
        initiative = 0;
        armorClass = 0;
        healthInputField.text = health.ToString();
        initiativeInputField.text = initiative.ToString();
        armorClassInputField.text = armorClass.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = unitName;
        //tryparse
        int _initiative;
        int _health;
        int _armorClass;
        if (int.TryParse(initiativeInputField.text, out _initiative))
        {
            initiative = _initiative;
        }
        if (int.TryParse(healthInputField.text, out _health))
        {
            health = _health;
        }
        if (int.TryParse(armorClassInputField.text, out _armorClass))
        {
            armorClass = _armorClass;
        }
    }

    public void IncrementHealth()
    {
        health++;
        healthInputField.text = health.ToString();
    }

    public void DecrementHealth()
    {
        health--;
        healthInputField.text = health.ToString();
    }

    public void MegaIncrementHealth()
    {
        health += 5;
        healthInputField.text = health.ToString();
    }

    public void MegaDecrementHealth()
    {
        health -= 5;
        healthInputField.text = health.ToString();
    }


}
