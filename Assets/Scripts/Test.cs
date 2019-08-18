using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Test : LambdaTween
{
    // Start is called before the first frame update
    void Start()
    {
        RawImage goRaw = transform.GetComponent<RawImage>();
        LambdaTweenAdd(2.0f, fTime => { transform.position = new Vector3(500.0f, fTime * 100.0f + 300.0f, 0.0f); goRaw.color = new Color(fTime, 1.0f, 1.0f, 1.0f); },
            () => { transform.localScale = Vector3.one * 2.0f; });
    }

    // Update is called once per frame
    void Update()
    {

    }
}