using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    #region private field
    private float startPos, lenght;
    [SerializeField] private float parallax;
    #endregion

    #region public field
    public GameObject cam;
    #endregion

    #region UnityCallBack
    void Start()
    {
        startPos = transform.position.x;
        cam = GameObject.Find("Main Camera");
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallax));
        float distance = (cam.transform.position.x * parallax);


        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if (temp > startPos + lenght)
        {
            startPos += lenght;
        }
        else if (temp < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
    #endregion

}
