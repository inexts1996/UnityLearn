// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My First Shader"{

    Properties{
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader{
        Pass{

            CGPROGRAM
            
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            
            #include "UnityCG.cginc"

            struct Interpolators
            {
                float4 position : SV_POSITION;
                //float3 localPosition : TEXCOORD0;
                float2 uv : TEXCOORD0;
            };

            struct VertexData
            {
               float4 position : POSITION; 
               float2 uv : TEXCOORD0;
            };
            float4 _Tint;

            Interpolators MyVertexProgram (VertexData v)
            {
                Interpolators i;
                //i.localPosition = v.position.xyz;
                i.uv = v.uv;
                i.position = UnityObjectToClipPos(v.position);
                 return i;
            }

            float4 MyFragmentProgram (Interpolators i) : SV_TARGET
            {
                //return float4(i.localPosition + 0.5, 1) * _Tint; 
                return float4(i.uv, 1, 1);
            }

            ENDCG
        }
    }
}