Shader "Custom/Effect/Decals/Base" 
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
			#include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
			#include "Packages/com.kink3d.decals/ShaderLibrary/Core.hlsl"
            
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

                half4 texColor = SAMPLE_DECAL2D(_BaseMap, input.positionPS);
                half3 color = texColor.rgb * _BaseColor.rgb;
                half alpha = texColor.a * _BaseColor.a;
                AlphaDiscard(alpha, _Cutoff);

                #ifdef _ALPHAPREMULTIPLY_ON
                    color *= alpha;
                #endif

                half4 finalColor = half4(color, alpha);
                finalColor.rgb = MixFog(finalColor.rgb, input.fogCoord);
                CLAMP_PROJECTION(finalColor, input.positionPS, input.normalWS);
                return finalColor;
            }

			ENDHLSL
		}
	}
	CustomEditor "Framework.Editor.CustomDecalBaseShaderGUI"
	FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
