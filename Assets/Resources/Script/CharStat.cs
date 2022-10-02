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
    /// �L�����̃p���[�e�N�X�g�ύX
    /// </summary>
    /// <param name="newPower">�p���[����</param>
    public void ChangePowerText(int newPower)
    {
        power = newPower;
        powerText.text = power.ToString();
    }
    #endregion


}
