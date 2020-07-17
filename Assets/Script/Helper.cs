using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    static class Helper
    {
        public static Vector2 WorldSpaceToLocalSpace(Vector2 target, Transform reference)
        {

            var zAx = reference.rotation.eulerAngles.z * Mathf.Deg2Rad;

            var targX = target.x - reference.position.x;
            var targY = target.y - reference.position.y;


            return new Vector2(targX * Mathf.Cos(zAx) + targY * Mathf.Sin(zAx),
                -targX * Mathf.Sin(zAx) + targY * Mathf.Cos(zAx));
        }
        public static Vector2 LocalSpaceToWolrdSpace(Vector2 target, Transform reference)
        {
            var zAx = (reference.rotation.eulerAngles.z) * Mathf.Deg2Rad;


            var x = target.x * Mathf.Cos(zAx) - target.y * Mathf.Sin(zAx);
            var y = target.x * Mathf.Sin(zAx) + target.y * Mathf.Cos(zAx);


            return new Vector2(x, y);
        }
    }
}
