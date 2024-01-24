using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Consideration
{
    protected Response _responseCurve;

    public void SetResponseCurve(Response curve)
    {
        _responseCurve = curve;
    }

    public abstract float Score(Combat combat, AIContext context);
}


public class ConsiderationRandom : Consideration
{
    public override float Score(Combat combat, AIContext context)
    {
        return Random.Range(0f, 1f);
    }
}
