using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    private float startPos,lenght;
    public GameObject cam;
    [SerializeField]private float parallax;
    // Start is called before the first frame update
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
        
       
        transform.position = new Vector3(startPos + distance,transform.position.y,transform.position.z);
        if(temp >startPos + lenght)
        {
            startPos += lenght;
        }
        else if(temp < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
