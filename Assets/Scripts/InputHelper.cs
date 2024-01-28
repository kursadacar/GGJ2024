using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class InputHelper
{
    public static bool IsKeyPressed(params KeyCode[] codes)
    {
        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKeyDown(codes[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsKeyDown(params KeyCode[] codes)
    {
        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKey(codes[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsKeyReleased(params KeyCode[] codes)
    {
        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKeyUp(codes[i]))
            {
                return true;
            }
        }

        return false;
    }
}
