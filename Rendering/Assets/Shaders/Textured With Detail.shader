// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Textured With Detail"{

    Properties{
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {} 
        _DetailTex ("Texture", 2D) = "gray" {}
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
                float2 uvDetail : TEXCOORD1;
            };

            struct VertexData
            {
               float4 position : POSITION; 
               float2 uv : TEXCOORD0;
            };
            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DetailTex; 
            float4 _DetailTex_ST;

            Interpolators MyVertexProgram (VertexData v)
            {
                Interpolators i;
                //i.localPosition = v.position.xyz;
                //i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.uvDetail = TRANSFORM_TEX(v.uv, _DetailTex);
                i.position = UnityObjectToClipPos(v.position);
                 return i;
            }

            float4 MyFragmentProgram (Interpolators i) : SV_TARGET
            {
                float4 color = tex2D(_MainTex, i.uv) * _Tint; 
                color *= tex2D(_DetailTex, i.uvDetail) * unity_ColorSpaceDouble;
                return color; 
            }

            ENDCG
        }
    }
}