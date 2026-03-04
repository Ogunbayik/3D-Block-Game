using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestWithSpace 
{
    public static void TestCode(Action testVoid)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            testVoid.Invoke();
    }
}
