#ifndef CELLIGHT_INCLUDED
#define CELLIGHT_INCLUDED

float _LightMultiplier;

half4 LightingCel (SurfaceOutput s, half3 lightDir, half atten) {
    half NdotL = dot (s.Normal, lightDir);
    NdotL = min(1, max(0, NdotL)*20)*_LightMultiplier;
    half4 c;
    c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
    c.a = s.Alpha;
    return c;
}

#endif