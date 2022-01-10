using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionBlock : MonoBehaviour
{
    public PuzzleController pc;

    public int id=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == gameObject.tag)
        {
            if(id==1)
            {
                pc.p1=true;
            }
            if(id==2)
            {
                pc.p2=true;
            }
            if(id==3)
            {
                pc.p3=true;
            }
            if(id==3)
            {
                pc.p4=true;
            }
            //Play Animation
            Destroy(other.gameObject);
        }
        
    }
}
