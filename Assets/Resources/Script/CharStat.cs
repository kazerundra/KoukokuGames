using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharStat : MonoBehaviour
{
    public TextMeshProUGUI powerText;
    public int power;
    public void ChangePowerText(int newPower)
    {
        power = newPower;
        powerText.text = power.ToString();
    }
}
