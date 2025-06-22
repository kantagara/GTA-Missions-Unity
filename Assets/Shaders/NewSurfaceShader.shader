Shader "Custom/CylinderHole"
{
    Properties
    {
        _Color ("Base Color", Color) = (1,1,1,1)
        _AlphaBottom ("Alpha Bottom", Range(0,1)) = 1
        _AlphaTop ("Alpha Top", Range(0,1)) = 0
        _StartHeight ("Start Height (0-1)", Range(0,1)) = 0.7
    }
    SubShader
    {
        Tags{"RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float heightLocal : TEXCOORD0;
            };
            
            float4 _Color;
            float _AlphaBottom;
            float _AlphaTop;
            float _StartHeight;
            
            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.heightLocal = IN.positionOS.y;
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                // Iznutra (dno) je alphaBottom do startHeight
                float t = saturate((IN.heightLocal - _StartHeight) / (1 - _StartHeight));
                float alpha = lerp(_AlphaBottom, _AlphaTop, t);
                return float4(_Color.rgb, alpha * _Color.a);
            }
            ENDHLSL
        }
    }
}
