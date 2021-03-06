Shader "Custom/Effect/Decals/FillColor" 
{
	Properties 
	{
		// Surface Options
		[HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _AlphaClip("__clip", Float) = 0.0
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		// Surface Inputs
		_BaseMap("Albedo", 2D) = "white" {}
		_BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _FillColor("Fill Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Fill ("Fill", Range (0,1)) = 1
        [HideInInspector]_Expand ("Expand", Range (0,1)) = 0.25
	}
	Subshader 
	{
		Tags { "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite Off
		Offset -1, -1

		Pass 
		{
			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
			#pragma target 2.0

			#pragma vertex vert
            #pragma fragment frag

			// -------------------------------------
            // Material Keywords
			#pragma shader_feature _BLEND_ALPHA
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON

			// -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

			// -------------------------------------
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.kink3d.decals/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _BaseColor;
            half _Cutoff;
            half _Glossiness;
            half _Metallic;

            float _Expand;
            float _Fill;
            half4 _FillColor;
            CBUFFER_END

            float gt_than(float x, float y)
            {
                return max(sign(x - y), 0);
            }

            float ls_than(float x, float y)
            {
                return max(sign(y - x), 0);
            }
              
            float dist(float2 from, float2 to)
            {
                return sqrt((to.x - from.x) * (to.x - from.x) + (to.y - from.y) * (to.y - from.y));
            }

            float Remap(float input, float inputMin, float inputMax, float outputMin, float outPutMax)
            {
                return outputMin + (input - inputMin) * (outPutMax - outputMin) / (inputMax - inputMin);
            }

            // -------------------------------------
            // Structs
            struct Attributes
            {
                float4 positionOS       : POSITION;
                float3 normalOS         : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionPS       : TEXCOORD0;
                float fogCoord  		: TEXCOORD1;
                float3 normalWS         : TEXCOORD2;
                float4 vertex 			: SV_POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // -------------------------------------
            // Vertex
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.positionPS = TransformObjectToProjection(input.positionOS);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);

                return output;
            }

            // -------------------------------------
            // Fragment
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half4 base = SAMPLE_DECAL2D(_BaseMap, input.positionPS);
                half4 main = base * _BaseColor;
                half4 fill = base * _FillColor;

                _Fill = Remap(_Fill, 0, 1, 0, 1.3) + 0.055;
                float mainBlit = gt_than(dist(float2(0, 0), input.positionPS.yy), _Fill);
                float fillBlit = ls_than(dist(float2(0, 0), input.positionPS.yy), _Fill);
                
                float4 color = float4(0, 0, 0, 0);
                color += main * float4(mainBlit, mainBlit, mainBlit, mainBlit);
                color += fill * float4(fillBlit, fillBlit, fillBlit, fillBlit);

                if (fillBlit > 0)
                {
                    color.a = step(0.001, color.a) * Remap(color.a, 0, 1, 0.15, 1);
                }

                AlphaDiscard(color.a, _Cutoff);

                #ifdef _ALPHAPREMULTIPLY_ON
                    color.rgb *= alpha;
                #endif

                color.rgb = MixFog(color.rgb, input.fogCoord);
                CLAMP_PROJECTION(color, input.positionPS, input.normalWS);
                return color;
            }

			ENDHLSL
		}
	}
	CustomEditor "Framework.Editor.CustomDecalFillShaderGUI"
	FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
