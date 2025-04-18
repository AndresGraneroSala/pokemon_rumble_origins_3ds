Shader "Custom/AlwaysOnTopWithColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Overlay" }
        ZTest Always
        ZWrite Off
        Cull Off
        Lighting Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            // Usamos el color del SpriteRenderer
            SetTexture [_MainTex] 
            {
                combine primary * texture
            }
        }
    }
}
