using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strongman : MonoBehaviour
{
    public float hp;
    public float atk;
    public float spatk;
    // Start is called before the first frame update
    void Start()
    {
        //save for database, stub first
        hp = 100;
        atk = 50;
        spatk = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
