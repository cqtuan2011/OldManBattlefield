using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BA
{
    public static class Extensions
    {
        public static GameObject DestroyChildCamera(this GameObject gameObject)
        {
            foreach (Transform obj in gameObject.transform)
            {
                if (obj.gameObject.name.Equals("PlayerCamera"))
                {
                    return obj.gameObject;
                }
            }
            return null;
        }
    }
}

