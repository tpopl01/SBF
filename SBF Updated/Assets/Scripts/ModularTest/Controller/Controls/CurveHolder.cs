using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
 (
     fileName = "Animation Curve",
     menuName = "Curves/Animation Curve"
 )]
public class CurveHolder : ScriptableObject
{
    public AnimationCurve curve;
}
