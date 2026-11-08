Shader "CG/Mid_WaterSurface"
{
    Properties
    {
        _DeepColor ("Deep Water", Color) = (0.09, 0.12, 0.36, 1)
        _RayColor ("Surface Light", Color) = (0.12, 0.90, 0.82, 1)
        _Strength ("Light Strength", Range(0, 2)) = 0.9
        _Width ("Light Width", Range(0.08, 0.45)) = 0.23
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            Cull Off
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _DeepColor;
                float4 _RayColor;
                float _Strength;
                float _Width;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target  
            {
                // 수조의 실제 가로·세로 비율에 맞춰 물결 크기를 잡는다.
                float2 p = input.uv * float2(6.1, 8.0);
                float t = _Time.y;

                // 서로 다른 방향에서 오는 물결이 만나도록 한다.
                float waveA = sin(p.x * 5.0 + p.y * 2.1 + t * 0.8
                                + 0.8 * sin(p.y * 2.3 - t * 0.4));
                float waveB = sin(p.y * 6.2 - p.x * 1.7 - t * 0.65
                                + 0.7 * sin(p.x * 2.5 + t * 0.3));

                // 물결이 겹치는 좁은 부분에만 밝은 반짝임을 준다.
                float crest = 0.5 + 0.25 * (waveA + waveB);
                float glint = smoothstep(0.72, 0.96, crest);

                float3 water = _DeepColor.rgb * (0.85 + 0.3 * crest);
                return half4(water + _RayColor.rgb * glint * _Strength, 1.0);
            }
            ENDHLSL
        }
    }
}