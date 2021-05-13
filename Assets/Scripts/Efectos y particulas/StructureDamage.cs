using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureDamage : MonoBehaviour
{
    private LifeHandler mylife;
    public GameObject firepart;
    public GameObject smokePart;
    public GameObject diepart;
   
    // Start is called before the first frame update
    void Start()
    {
        mylife = GetComponent<LifeHandler>();
        firepart.SetActive(false);
        smokePart.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {     
       if(mylife.maxLife / mylife._currentLife > 2)
        {
            firepart.SetActive(true);
        }
        if (mylife.maxLife / mylife._currentLife > 3)
        {
            smokePart.SetActive(true);
        }
    }
}
