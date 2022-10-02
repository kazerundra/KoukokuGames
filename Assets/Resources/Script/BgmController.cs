using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    #region private Field
    private AudioSource audioSource;
    #endregion

    #region UnityCallBack
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    #endregion

}
