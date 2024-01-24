using System;
using UnityEngine;


public abstract class Response
{
    public abstract float ComputeValue(float x);
}

public class ResponseCurve : Response
{
    public enum CurveType
    {
        Linear,
        Polynomial,
        Logistic,
        Logit,
        Normal,
        Sine
    }

    CurveType _type;
    float _slope;
    float _exponent;
    float _xShift;
    float _yShift;

    float _min;
    float _max;
    bool _shouldNormalize;

    public ResponseCurve(CurveType type, float slope=1.0f, float exponent=1.0f, float xshift=0.0f, float yshift=0.0f)
    {
        _type = type;
        _slope = slope;
        _exponent = exponent;
        _xShift = xshift;
        _yShift = yshift;
    }

    public void SetInputRange(float min, float max)
    {
        _min = min;
        _max = max;
        _shouldNormalize = true;
    }

    public float Normalize(float val)
    {
        if (val < _min)
            val = _min;
        else if (val > _max)
            val = _max;

        return (val - _min) / (_max - _min);
    }


    public override float ComputeValue(float x)
    {
        if (_shouldNormalize)
            x = Normalize(x);

        if (x < 0 || x > 1)
            throw new System.Exception("x is not in range!");

        return _type switch
        {
            CurveType.Linear =>
                    Sanitize((_slope * (x - _xShift)) + _yShift),
            CurveType.Polynomial =>
                Sanitize((_slope * MathF.Pow(x - _xShift, _exponent)) + _yShift),
            CurveType.Logistic =>
                Sanitize((_slope / (1 + MathF.Exp(-10.0f * _exponent * (x - 0.5f - _xShift)))) + _yShift),
            CurveType.Logit =>
                Sanitize(_slope * MathF.Log((x - _xShift) / (1.0f - (x - _xShift))) / 5.0f + 0.5f + _yShift),
            CurveType.Normal =>
                Sanitize(_slope * MathF.Exp(-30.0f * _exponent * (x - _xShift - 0.5f) * (x - _xShift - 0.5f)) + _yShift),
            CurveType.Sine =>
                Sanitize(0.5f * _slope * MathF.Sin(2.0f * MathF.PI * (x - _xShift)) + 0.5f + _yShift),
            _ => 0.0f,
        };
    }

    float Sanitize(float y)
    {
        if (float.IsInfinity(y))
            return 0.0f;

        if (float.IsNaN(y))
            return 0.0f;

        if (y < 0.0)
            return 0.0f;

        if (y > 1.0)
            return 1.0f;

        return y;
    }
}

public class BooleanResponse : Response
{
    public enum BooleanResponseType
    {
        Equals,
        NotEquals,
        Lesser,
        Greater
    }

    BooleanResponseType _type;
    float _value;

    public BooleanResponse(BooleanResponseType type, float val)
    {
        _type = type;
        _value = val;
    }

    public override float ComputeValue(float x)
    {
        bool isGood = _type switch
        {
            BooleanResponseType.Equals => Mathf.Approximately(x, _value),
            BooleanResponseType.NotEquals => !Mathf.Approximately(x, _value),
            BooleanResponseType.Lesser => x < _value,
            BooleanResponseType.Greater => x > _value,
            _ => false
        };

        return isGood ? 1 : 0;
    }
}