using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeScreenshot : MonoBehaviour
{
    private bool IsScreenSHotTake = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsScreenSHotTake)
        {
            Debug.Log("Screenshot");
            ScreenCapture.CaptureScreenshot("TestScreenSHot", 2);

            IsScreenSHotTake = true;
        }
    }
}
