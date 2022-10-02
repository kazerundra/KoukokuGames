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

    /// <summary>
    /// キャラのパワーテクスト変更
    /// </summary>
    /// <param name="newPower">パワー数字</param>
    public void ChangePowerText(int newPower)
    {
        power = newPower;
        powerText.text = power.ToString();
    }
    #endregion


}
