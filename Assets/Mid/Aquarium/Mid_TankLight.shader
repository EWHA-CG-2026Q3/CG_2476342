Shader "CG/Mid_TankLight"
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
                float2 uv = input.uv;
                float time = _Time.y;

                // 수면에서 내려온 빛이 아래로 갈수록 약해진다.
                float fromSurface = pow(saturate(uv.y), 1.25);

                // 중앙에 넓은 빛 한 줄기를 만들고 가장자리를 부드럽게 흐린다.
                float center = 0.50 + 0.012 * sin(time * 0.45 + uv.y * 3.0);
                float offset = (uv.x - center) / _Width;
                float beam = 0.32 + 0.68 * exp(-1.4 * offset * offset);

                // 폭이 다른 세로 결을 겹쳐 수면이 천천히 일렁이는 느낌을 낸다.
                float ripple = uv.x + 0.007 * sin(uv.y * 5.0 + time * 0.6);
                float strands = 0.76
                    + 0.15 * sin(ripple * 96.0 + time * 0.75)
                    + 0.09 * sin(ripple * 163.0 - time * 0.50);

                float light = fromSurface * beam * strands * _Strength;
                float3 water = _DeepColor.rgb * lerp(0.72, 1.15, uv.y);
                return half4(water + _RayColor.rgb * light, 1);
            }
            ENDHLSL
        }
    }
}