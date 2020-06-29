using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour {

    private float initPosY;

    void Awake()
    {
        initPosY = transform.position.y;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y - initPosY > 1)
        {
            Destroy(gameObject);
        }
	}

    private void FixedUpdate()
    {
        transform.Translate(new Vector2(0, 1) * Time.deltaTime);
    }
}
