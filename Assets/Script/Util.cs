using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MathUtil
{
    public class Util : MonoBehaviour
    {
        public static bool CanSeeObj(GameObject destination, GameObject origin, float range)
        {
            Vector3 dir = Vector3.Normalize(destination.transform.position - origin.transform.position);

            float angleDist = Vector3.Dot(origin.transform.forward, dir);

            Debug.DrawRay(destination.transform.position, origin.transform.forward * 10, Color.red);
            Debug.DrawRay(origin.transform.position, dir * 10, Color.green);

            if(angleDist > range)
            {
                return true;
            }
            else
            {
                return false;
            }




            
        }
    }
}

