Shader "Custom/RoundCubeGrid"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5 
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        [KeywordEnum(X, Y, Z)] _Face("Face", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma shader_feature _FACES_X _FACES_Y _FACES_Z
        #pragma surface surƒ standard addshadows fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
           float2 cubeUV; 
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.cubeUV = v.vertex.xy;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
           fixed4 c = tex2D(_MainTex, IN.cubeUV) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        

        ENDCG
        
    }
    FallBack "Diffuse"
}
