using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderObject : MonoBehaviour {

    [HideInInspector]
    public SpriteRenderer rend;
    public GameObject targetObj;
    public bool skipZOrderUpdate;

    // Use this for initialization
    public virtual void Start()
    {
        if(targetObj!=null)
            rend = targetObj.GetComponent<SpriteRenderer>();
        else
            rend = GetComponent<SpriteRenderer>();
		
	}

    // Update is called once per frame
    public virtual void Update()
    {
        if (rend.isVisible && !skipZOrderUpdate)
        {
            rend.sortingOrder = -(int)(transform.position.y*100);
        }
        
    }
}
