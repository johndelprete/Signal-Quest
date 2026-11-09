using UnityEngine;

public static class SignalMath
{
    public static float UnitStep(int n)
    {
        return n >= 0 ? 1f : 0f;
    }

    public static float Ramp(int n)
    {
        return n >= 0 ? n : 0f;
    }

    // x[n] = -u[n+2] + r[n+2] + u[n+1] + u[n]
    //        -u[n-1] - r[n-1]
    public static float OriginalSignal(int n)
    {
        return -UnitStep(n + 2)
             + Ramp(n + 2)
             + UnitStep(n + 1)
             + UnitStep(n)
             - UnitStep(n - 1)
             - Ramp(n - 1);
    }

    public static float XOf2MinusN(int n)
    {
        return OriginalSignal(2 - n);
    }

    public static float XOf2NPlus1(int n)
    {
        return OriginalSignal(2 * n + 1);
    }
}