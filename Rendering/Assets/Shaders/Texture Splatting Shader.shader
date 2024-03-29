// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture Splatting"{

    Properties{
        _MainTex ("Slpat Map", 2D) = "white" {}
        [NoScaleOffset]_Texture1 ("Texture 1", 2D) = "white" { }
        [NoScaleOffset]_Texture2 ("Texture 2", 2D) = "white" { }
        [NoScaleOffset]_Texture3 ("Texture 3", 2D) = "white" { }
        [NoScaleOffset]_Texture4 ("Texture 4", 2D) = "white" { }
        [NoScaleOffset]_DetailTexture1("Detail Texture 1", 2D) = "gray" {}
        [NoScaleOffset]_DetailTexture2("Detail Texture 2", 2D) = "gray" {}
        [NoScaleOffset]_DetailTexture3("Detail Texture 3", 2D) = "gray" {}
        [NoScaleOffset]_DetailTexture4("Detail Texture 4", 2D) = "gray" {}
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
                float2 uvSplat : TEXCOORD1;
            };

            struct VertexData
            {
               float4 position : POSITION; 
               float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex, _Texture1, _Texture2, _Texture3, _Texture4;
            sampler2D  _DetailTexture1, _DetailTexture2, _DetailTexture3, _DetailTexture4;
            float4 _MainTex_ST;

            Interpolators MyVertexProgram (VertexData v)
            {
                Interpolators i;
                //i.localPosition = v.position.xyz;
                //i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                i.uvSplat = v.uv;
                i.position = UnityObjectToClipPos(v.position);
                 return i;
             }
                  

            float4 MyFragmentProgram (Interpolators i) : SV_TARGET
            {
                float4 splat = tex2D(_MainTex, i.uvSplat);
                return tex2D(_Texture1, i.uv) * splat.r * tex2D(_DetailTexture1, i.uv) * unity_ColorSpaceDouble
                + tex2D(_Texture2, i.uv) * splat.g * tex2D(_DetailTexture2, i.uv)* unity_ColorSpaceDouble
                + tex2D(_Texture3, i.uv) * splat.b * tex2D(_DetailTexture3, i.uv)* unity_ColorSpaceDouble
                + tex2D(_Texture4, i.uv) * (1-splat.r - splat.g - splat.b)* tex2D(_DetailTexture4, i.uv)* unity_ColorSpaceDouble;
            }

            ENDCG
        }
    }
}