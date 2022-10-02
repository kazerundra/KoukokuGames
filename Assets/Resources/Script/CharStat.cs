using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharStat : MonoBehaviour
{
    #region public field
    public TextMeshProUGUI powerText;
    public int power;
    #endregion

    #region public function
    public void ChangePowerText(int newPower)
    {
        power = newPower;
        powerText.text = power.ToString();
    }
    #endregion


}
