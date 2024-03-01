// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Piloto Studio/Additive Uber Shader"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_MainTex("Main Texture", 2D) = "white" {}
		_Desaturate("Desaturate?", Range( 0 , 1)) = 0
		_MainTextureChannel("Main Texture Channel", Vector) = (1,1,1,0)
		_MainAlphaChannel("Main Alpha Channel", Vector) = (0,0,0,1)
		_MainTexturePanning("Main Texture Panning", Vector) = (0,0,0,0)
		_MainAlphaPanning("Main Alpha Panning", Vector) = (0,0,0,0)
		_AlphaOverride("Alpha Override", 2D) = "white" {}
		_AlphaOverridePanning("Alpha Override Panning", Vector) = (0,0,0,0)
		_AlphaOverrideChannel("Alpha Override Channel", Vector) = (1,0,0,0)
		_FlipbooksColumsRows("Flipbooks Colums & Rows", Vector) = (1,1,0,0)
		_DetailNoise("Detail Noise", 2D) = "white" {}
		_DetailNoisePanning("Detail Noise Panning", Vector) = (0,0,0,0)
		_DetailDistortionChannel("Detail Distortion Channel", Vector) = (1,0,0,0)
		_DistortionIntensity("Distortion Intensity", Float) = 0
		_DetailMultiplyChannel("Detail Multiply Channel", Vector) = (0,0,0,0)
		_DetailAdditiveChannel("Detail Additive Channel", Vector) = (0,0,0,0)
		[Toggle(_USESOFTALPHA_ON)] _UseSoftAlpha("UseSoftAlpha", Float) = 0
		[ASEEnd]_SoftFadeFactor("SoftFadeFactor", Range( 0.1 , 1)) = 0.1

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		//[HideInInspector][ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		//[HideInInspector] _ShadowMatteFilter("Shadow Matte Filter", Float) = 2.006836
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0 // StencilUsage.Clear
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 0 // Nothing
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8 // StencilUsage.TraceReflectionRay
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 2 // StencilUsage.DistortionVectors
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 2 // StencilUsage.DistortionVectors
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector][ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector][ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector] _SurfaceType("Surface Type", Float) = 1
		[HideInInspector] _BlendMode("Blend Mode", Float) = 1
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector][ToggleUI] _ZWrite("ZWrite", Float) = 0
		[HideInInspector][ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 0
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Float) = 0
		[HideInInspector][ToggleUI] _EnableFogOnTransparent("Enable Fog", Float) = 0
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2 // This mode is dedicated to Forward to correctly handle backface then front face rendering thin transparent
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.TransparentCullMode)] _TransparentCullMode("Transparent Cull Mode", Int) = 2// Back culling by default
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4 // Less equal
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Int) = 4// Less equal
		[HideInInspector][ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector][ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector][ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector][ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)
		[HideInInspector] _DistortionEnable("_DistortionEnable",Float) = 0
		[HideInInspector] _DistortionOnly("_DistortionOnly",Float) = 0

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector][ToggleUI] _TransparentWritingMotionVec("Transparent Writing MotionVec", Float) = 0
		[HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)] _OpaqueCullMode("Opaque Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector][ToggleUI] _SupportDecals("Support Decals", Float) = 1
		[HideInInspector][ToggleUI] _ReceivesSSRTransparent("Receives SSR Transparent", Float) = 0
		[HideInInspector] _EmissionColor("Color", Color) = (1, 1, 1)
		[HideInInspector] _UnlitColorMap_MipInfo("_UnlitColorMap_MipInfo", Vector) = (0, 0, 0, 0)

		[HideInInspector][Enum(Auto, 0, On, 1, Off, 2)] _DoubleSidedGIMode("Double sided GI mode", Float) = 0 //DoubleSidedGIMode added in api 12x and higher
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma exclude_renderers glcore gles gles3 ps4 

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward Unlit"
			Tags { "LightMode"="ForwardOnly" }

			Blend [_SrcBlend] [_DstBlend]

			Cull [_CullMode]
			ZTest [_ZTestTransparent]
			ZWrite [_ZWrite]

			ColorMask [_ColorMaskTransparentVel] 1

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120112


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ DEBUG_DISPLAY

			#pragma vertex Vert
			#pragma fragment Frag

	        #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
	        #define _WRITE_TRANSPARENT_MOTION_VECTOR
	        #endif

			#define SHADERPASS SHADERPASS_FORWARD_UNLIT
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
				#define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
				#define HAS_LIGHTLOOP
				#define SHADOW_OPTIMIZE_REGISTER_USAGE 1

				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonLighting.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Shadow/HDShadowContext.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadow.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/PunctualLightCommon.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/HDShadowLoop.hlsl"
			#endif

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USESOFTALPHA_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _DetailNoise_ST;
			float4 _DetailDistortionChannel;
			float4 _MainTextureChannel;
			float4 _DetailAdditiveChannel;
			float4 _DetailMultiplyChannel;
			float4 _AlphaOverride_ST;
			float4 _AlphaOverrideChannel;
			float4 _MainAlphaChannel;
			float2 _MainTexturePanning;
			float2 _DetailNoisePanning;
			float2 _FlipbooksColumsRows;
			float2 _AlphaOverridePanning;
			float2 _MainAlphaPanning;
			float _DistortionIntensity;
			float _Desaturate;
			float _SoftFadeFactor;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _DetailNoise;
			sampler2D _AlphaOverride;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			
			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float4 ShadowTint;
				float Alpha;
				float AlphaClipThreshold;
				float4 VTPackedFeedback;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				surfaceData.color = surfaceDescription.Color;
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription , FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif

				#if defined(_ENABLE_SHADOW_MATTE) && SHADERPASS == SHADERPASS_FORWARD_UNLIT
					HDShadowContext shadowContext = InitShadowContext();
					float shadow;
					float3 shadow3;
					posInput = GetPositionInput(fragInputs.positionSS.xy, _ScreenSize.zw, fragInputs.positionSS.z, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
					float3 normalWS = normalize(fragInputs.tangentToWorld[1]);
					uint renderingLayers = _EnableLightLayers ? asuint(unity_RenderingLayer.x) : DEFAULT_LIGHT_LAYERS;
					ShadowLoopMin(shadowContext, posInput, normalWS, asuint(_ShadowMatteFilter), renderingLayers, shadow3);
					shadow = dot(shadow3, float3(1.0f/3.0f, 1.0f/3.0f, 1.0f/3.0f));

					float4 shadowColor = (1 - shadow)*surfaceDescription.ShadowTint.rgba;
					float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);

					#ifdef _SURFACE_TYPE_TRANSPARENT
						surfaceData.color = lerp(shadowColor.rgb*surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow), surfaceDescription.Alpha);
					#else
						surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow);
					#endif
					localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
					surfaceDescription.Alpha = localAlpha;
				#endif

				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#ifdef UNITY_VIRTUAL_TEXTURING
                builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			float GetDeExposureMultiplier()
			{
			#if defined(DISABLE_UNLIT_DEEXPOSURE)
				return 1.0;
			#else
				return _DeExposureMultiplier;
			#endif
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
				float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
				o.ase_texcoord3 = screenPos;
				
				o.ase_color = inputMesh.ase_color;
				o.ase_texcoord1 = inputMesh.ase_texcoord;
				o.ase_texcoord2 = inputMesh.ase_texcoord1;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				o.positionRWS = positionRWS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#ifdef UNITY_VIRTUAL_TEXTURING
			#define VT_BUFFER_TARGET SV_Target1
			#define EXTRA_BUFFER_TARGET SV_Target2
			#else
			#define EXTRA_BUFFER_TARGET SV_Target1
			#endif

			void Frag( VertexOutput packedInput,
						out float4 outColor : SV_Target0
						#ifdef UNITY_VIRTUAL_TEXTURING
						,out float4 outVTFeedback : VT_BUFFER_TARGET
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.positionRWS;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir( input.positionRWS );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float2 uv_DetailNoise = packedInput.ase_texcoord1.xy * _DetailNoise_ST.xy + _DetailNoise_ST.zw;
				float2 panner209 = ( 1.0 * _Time.y * _DetailNoisePanning + uv_DetailNoise);
				float4 tex2DNode211 = tex2D( _DetailNoise, panner209 );
				float4 break17_g136 = tex2DNode211;
				float4 appendResult18_g136 = (float4(break17_g136.x , break17_g136.y , break17_g136.z , break17_g136.w));
				float4 clampResult19_g136 = clamp( ( appendResult18_g136 * _DetailDistortionChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g136 = clampResult19_g136;
				float clampResult20_g136 = clamp( ( break2_g136.x + break2_g136.y + break2_g136.z + break2_g136.w ) , 0.0 , 1.0 );
				float3 temp_cast_1 = (clampResult20_g136).xxx;
				float3 desaturateInitialColor213 = temp_cast_1;
				float desaturateDot213 = dot( desaturateInitialColor213, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar213 = lerp( desaturateInitialColor213, desaturateDot213.xxx, 1.0 );
				float3 DistortionNoise214 = desaturateVar213;
				float4 texCoord192 = packedInput.ase_texcoord2;
				texCoord192.xy = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult194 = (float2(texCoord192.z , 0.0));
				float2 appendResult195 = (float2(0.0 , 0.0));
				float2 LocalUVOffset197 = ( appendResult194 + appendResult195 );
				float2 texCoord200 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float3 UVFlipbookInput205 = ( ( DistortionNoise214 * _DistortionIntensity ) + float3( ( LocalUVOffset197 + texCoord200 ) ,  0.0 ) );
				float temp_output_4_0_g140 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g140 = _FlipbooksColumsRows.y;
				float2 appendResult7_g140 = (float2(temp_output_4_0_g140 , temp_output_5_0_g140));
				float totalFrames39_g140 = ( temp_output_4_0_g140 * temp_output_5_0_g140 );
				float2 appendResult8_g140 = (float2(totalFrames39_g140 , temp_output_5_0_g140));
				float4 texCoord218 = packedInput.ase_texcoord2;
				texCoord218.xy = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult42_g140 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g140 - 1.0 ) );
				float temp_output_35_0_g140 = frac( ( ( (float)0 + clampResult42_g140 ) / totalFrames39_g140 ) );
				float2 appendResult29_g140 = (float2(temp_output_35_0_g140 , ( 1.0 - temp_output_35_0_g140 )));
				float2 temp_output_15_0_g140 = ( ( UVFlipbookInput205.xy / appendResult7_g140 ) + ( floor( ( appendResult8_g140 * appendResult29_g140 ) ) / appendResult7_g140 ) );
				float2 temp_output_225_0 = temp_output_15_0_g140;
				float2 panner177 = ( 1.0 * _Time.y * _MainTexturePanning + temp_output_225_0);
				float4 break17_g144 = tex2D( _MainTex, panner177 );
				float4 appendResult18_g144 = (float4(break17_g144.x , break17_g144.y , break17_g144.z , break17_g144.w));
				float4 clampResult19_g144 = clamp( ( appendResult18_g144 * _MainTextureChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g144 = clampResult19_g144;
				float clampResult20_g144 = clamp( ( break2_g144.x + break2_g144.y + break2_g144.z + break2_g144.w ) , 0.0 , 1.0 );
				float MainTexInfo180 = clampResult20_g144;
				float3 temp_cast_6 = (MainTexInfo180).xxx;
				float3 desaturateInitialColor175 = temp_cast_6;
				float desaturateDot175 = dot( desaturateInitialColor175, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar175 = lerp( desaturateInitialColor175, desaturateDot175.xxx, _Desaturate );
				float4 break243 = ( _DetailAdditiveChannel * tex2DNode211 );
				float4 appendResult237 = (float4(break243.x , break243.y , break243.z , break243.w));
				float3 desaturateInitialColor241 = appendResult237.xyz;
				float desaturateDot241 = dot( desaturateInitialColor241, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar241 = lerp( desaturateInitialColor241, desaturateDot241.xxx, 1.0 );
				float3 AdditiveNoise245 = desaturateVar241;
				float4 texCoord184 = packedInput.ase_texcoord1;
				texCoord184.xy = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float4 break17_g145 = tex2DNode211;
				float4 appendResult18_g145 = (float4(break17_g145.x , break17_g145.y , break17_g145.z , break17_g145.w));
				float4 clampResult19_g145 = clamp( ( appendResult18_g145 * _DetailMultiplyChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g145 = clampResult19_g145;
				float clampResult20_g145 = clamp( ( break2_g145.x + break2_g145.y + break2_g145.z + break2_g145.w ) , 0.0 , 1.0 );
				float ifLocalVar238 = 0;
				if( ( _DetailMultiplyChannel.x + _DetailMultiplyChannel.y + _DetailMultiplyChannel.z + _DetailMultiplyChannel.w ) <= 0.0 )
				ifLocalVar238 = 1.0;
				else
				ifLocalVar238 = clampResult20_g145;
				float3 temp_cast_11 = (ifLocalVar238).xxx;
				float3 desaturateInitialColor236 = temp_cast_11;
				float desaturateDot236 = dot( desaturateInitialColor236, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar236 = lerp( desaturateInitialColor236, desaturateDot236.xxx, 1.0 );
				float3 MultiplyNoise246 = desaturateVar236;
				float4 temp_output_191_0 = ( packedInput.ase_color * float4( ( desaturateVar175 + AdditiveNoise245 ) , 0.0 ) * ( texCoord184.z + 1.0 ) * float4( MultiplyNoise246 , 0.0 ) );
				
				float2 uv_AlphaOverride = packedInput.ase_texcoord1.xy * _AlphaOverride_ST.xy + _AlphaOverride_ST.zw;
				float temp_output_4_0_g139 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g139 = _FlipbooksColumsRows.y;
				float2 appendResult7_g139 = (float2(temp_output_4_0_g139 , temp_output_5_0_g139));
				float totalFrames39_g139 = ( temp_output_4_0_g139 * temp_output_5_0_g139 );
				float2 appendResult8_g139 = (float2(totalFrames39_g139 , temp_output_5_0_g139));
				float clampResult42_g139 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g139 - 1.0 ) );
				float temp_output_35_0_g139 = frac( ( ( (float)0 + clampResult42_g139 ) / totalFrames39_g139 ) );
				float2 appendResult29_g139 = (float2(temp_output_35_0_g139 , ( 1.0 - temp_output_35_0_g139 )));
				float2 temp_output_15_0_g139 = ( ( ( LocalUVOffset197 + uv_AlphaOverride ) / appendResult7_g139 ) + ( floor( ( appendResult8_g139 * appendResult29_g139 ) ) / appendResult7_g139 ) );
				float2 panner227 = ( 1.0 * _Time.y * _AlphaOverridePanning + temp_output_15_0_g139);
				float4 break2_g141 = ( tex2D( _AlphaOverride, panner227 ) * _AlphaOverrideChannel );
				float AlphaOverride234 = saturate( ( break2_g141.x + break2_g141.y + break2_g141.z + break2_g141.w ) );
				float2 panner226 = ( 1.0 * _Time.y * _MainAlphaPanning + temp_output_225_0);
				float4 break2_g142 = ( tex2D( _MainTex, panner226 ) * _MainAlphaChannel );
				float MainAlpha233 = saturate( ( break2_g142.x + break2_g142.y + break2_g142.z + break2_g142.w ) );
				float temp_output_169_0 = ( AlphaOverride234 * MainAlpha233 );
				float4 texCoord171 = packedInput.ase_texcoord1;
				texCoord171.xy = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_3_0_g143 = ( texCoord171.w - ( 1.0 - temp_output_169_0 ) );
				float temp_output_188_0 = ( packedInput.ase_color.a * temp_output_169_0 * saturate( saturate( ( temp_output_3_0_g143 / fwidth( temp_output_3_0_g143 ) ) ) ) );
				float4 screenPos = packedInput.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth251 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth251 = abs( ( screenDepth251 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _SoftFadeFactor ) );
				#ifdef _USESOFTALPHA_ON
				float staticSwitch249 = ( temp_output_188_0 * saturate( distanceDepth251 ) );
				#else
				float staticSwitch249 = temp_output_188_0;
				#endif
				
				surfaceDescription.Color = temp_output_191_0.rgb;
				surfaceDescription.Emission = temp_output_191_0.rgb;
				surfaceDescription.Alpha = staticSwitch249;
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				surfaceDescription.ShadowTint = float4( 0, 0 ,0 ,1 );
				float2 Distortion = float2 ( 0, 0 );
				float DistortionBlur = 0;

				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );

				#if defined(_ENABLE_SHADOW_MATTE)
				bsdfData.color *= GetScreenSpaceAmbientOcclusion(input.positionSS.xy);
				#endif


			#ifdef DEBUG_DISPLAY
				if (_DebugLightingMode >= DEBUGLIGHTINGMODE_DIFFUSE_LIGHTING && _DebugLightingMode <= DEBUGLIGHTINGMODE_EMISSIVE_LIGHTING)
				{
					if (_DebugLightingMode != DEBUGLIGHTINGMODE_EMISSIVE_LIGHTING)
					{
						builtinData.emissiveColor = 0.0;
					}
					else
					{
						bsdfData.color = 0.0;
					}
				}
			#endif

				float4 outResult = ApplyBlendMode(bsdfData.color * GetDeExposureMultiplier() + builtinData.emissiveColor * GetCurrentExposureMultiplier(), builtinData.opacity);
				outResult = EvaluateAtmosphericScattering(posInput, V, outResult);

				#ifdef DEBUG_DISPLAY
					int bufferSize = int(_DebugViewMaterialArray[0].x);
					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = int(_DebugViewMaterialArray[index].x);
						if (indexMaterialProperty != 0)
						{
							float3 result = float3(1.0, 0.0, 1.0);
							bool needLinearToSRGB = false;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, posInput, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);

							if (!needLinearToSRGB)
								result = SRGBToLinear(max(0, result));

							outResult = float4(result, 1.0);
						}
					}

					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outResult = result;
					}
				#endif

				outColor = outResult;

				#ifdef _DEPTHOFFSET_ON
					outputDepth = posInput.deviceDepth;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
					outVTFeedback = builtinData.vtPackedFeedback;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120112


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma shader_feature EDITOR_VISUALIZATION

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _DetailNoise_ST;
			float4 _DetailDistortionChannel;
			float4 _MainTextureChannel;
			float4 _DetailAdditiveChannel;
			float4 _DetailMultiplyChannel;
			float4 _AlphaOverride_ST;
			float4 _AlphaOverrideChannel;
			float4 _MainAlphaChannel;
			float2 _MainTexturePanning;
			float2 _DetailNoisePanning;
			float2 _FlipbooksColumsRows;
			float2 _AlphaOverridePanning;
			float2 _MainAlphaPanning;
			float _DistortionIntensity;
			float _Desaturate;
			float _SoftFadeFactor;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _DetailNoise;
			sampler2D _AlphaOverride;


            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USESOFTALPHA_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD0;
				float4 LightCoord : TEXCOORD1;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			
			struct SurfaceDescription
			{
				float3 Color;
				float3 Emission;
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData( FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData )
			{
				ZERO_INITIALIZE( SurfaceData, surfaceData );
				surfaceData.color = surfaceDescription.Color;

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData( SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData )
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData( fragInputs, surfaceDescription, V, surfaceData );
				ZERO_INITIALIZE( BuiltinData, builtinData );
				builtinData.opacity = surfaceDescription.Alpha;
				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);
			}

			#define SCENEPICKINGPASS
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/MetaPass.hlsl"

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID( inputMesh );
				UNITY_TRANSFER_INSTANCE_ID( inputMesh, o );

				float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
				float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
				o.ase_texcoord4 = screenPos;
				
				o.ase_color = inputMesh.ase_color;
				o.ase_texcoord2 = inputMesh.uv0;
				o.ase_texcoord3 = inputMesh.uv1;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

			#ifdef EDITOR_VISUALIZATION
				float2 vizUV = 0;
				float4 lightCoord = 0;
				UnityEditorVizData(inputMesh.positionOS.xyz, inputMesh.uv0.xy, inputMesh.uv1.xy, inputMesh.uv2.xy, vizUV, lightCoord);
			#endif

				float2 uv = float2( 0.0, 0.0 );
				if( unity_MetaVertexControl.x )
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if( unity_MetaVertexControl.y )
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				#ifdef EDITOR_VISUALIZATION
					o.VizUV.xy = vizUV;
					o.LightCoord = lightCoord;
				#endif

				o.positionCS = float4( uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0 );
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.uv3 = v.uv3;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.uv3 = patch[0].uv3 * bary.x + patch[1].uv3 * bary.y + patch[2].uv3 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag( VertexOutput packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE( FragInputs, input );
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS );

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float2 uv_DetailNoise = packedInput.ase_texcoord2.xy * _DetailNoise_ST.xy + _DetailNoise_ST.zw;
				float2 panner209 = ( 1.0 * _Time.y * _DetailNoisePanning + uv_DetailNoise);
				float4 tex2DNode211 = tex2D( _DetailNoise, panner209 );
				float4 break17_g136 = tex2DNode211;
				float4 appendResult18_g136 = (float4(break17_g136.x , break17_g136.y , break17_g136.z , break17_g136.w));
				float4 clampResult19_g136 = clamp( ( appendResult18_g136 * _DetailDistortionChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g136 = clampResult19_g136;
				float clampResult20_g136 = clamp( ( break2_g136.x + break2_g136.y + break2_g136.z + break2_g136.w ) , 0.0 , 1.0 );
				float3 temp_cast_1 = (clampResult20_g136).xxx;
				float3 desaturateInitialColor213 = temp_cast_1;
				float desaturateDot213 = dot( desaturateInitialColor213, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar213 = lerp( desaturateInitialColor213, desaturateDot213.xxx, 1.0 );
				float3 DistortionNoise214 = desaturateVar213;
				float4 texCoord192 = packedInput.ase_texcoord3;
				texCoord192.xy = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult194 = (float2(texCoord192.z , 0.0));
				float2 appendResult195 = (float2(0.0 , 0.0));
				float2 LocalUVOffset197 = ( appendResult194 + appendResult195 );
				float2 texCoord200 = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float3 UVFlipbookInput205 = ( ( DistortionNoise214 * _DistortionIntensity ) + float3( ( LocalUVOffset197 + texCoord200 ) ,  0.0 ) );
				float temp_output_4_0_g140 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g140 = _FlipbooksColumsRows.y;
				float2 appendResult7_g140 = (float2(temp_output_4_0_g140 , temp_output_5_0_g140));
				float totalFrames39_g140 = ( temp_output_4_0_g140 * temp_output_5_0_g140 );
				float2 appendResult8_g140 = (float2(totalFrames39_g140 , temp_output_5_0_g140));
				float4 texCoord218 = packedInput.ase_texcoord3;
				texCoord218.xy = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult42_g140 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g140 - 1.0 ) );
				float temp_output_35_0_g140 = frac( ( ( (float)0 + clampResult42_g140 ) / totalFrames39_g140 ) );
				float2 appendResult29_g140 = (float2(temp_output_35_0_g140 , ( 1.0 - temp_output_35_0_g140 )));
				float2 temp_output_15_0_g140 = ( ( UVFlipbookInput205.xy / appendResult7_g140 ) + ( floor( ( appendResult8_g140 * appendResult29_g140 ) ) / appendResult7_g140 ) );
				float2 temp_output_225_0 = temp_output_15_0_g140;
				float2 panner177 = ( 1.0 * _Time.y * _MainTexturePanning + temp_output_225_0);
				float4 break17_g144 = tex2D( _MainTex, panner177 );
				float4 appendResult18_g144 = (float4(break17_g144.x , break17_g144.y , break17_g144.z , break17_g144.w));
				float4 clampResult19_g144 = clamp( ( appendResult18_g144 * _MainTextureChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g144 = clampResult19_g144;
				float clampResult20_g144 = clamp( ( break2_g144.x + break2_g144.y + break2_g144.z + break2_g144.w ) , 0.0 , 1.0 );
				float MainTexInfo180 = clampResult20_g144;
				float3 temp_cast_6 = (MainTexInfo180).xxx;
				float3 desaturateInitialColor175 = temp_cast_6;
				float desaturateDot175 = dot( desaturateInitialColor175, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar175 = lerp( desaturateInitialColor175, desaturateDot175.xxx, _Desaturate );
				float4 break243 = ( _DetailAdditiveChannel * tex2DNode211 );
				float4 appendResult237 = (float4(break243.x , break243.y , break243.z , break243.w));
				float3 desaturateInitialColor241 = appendResult237.xyz;
				float desaturateDot241 = dot( desaturateInitialColor241, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar241 = lerp( desaturateInitialColor241, desaturateDot241.xxx, 1.0 );
				float3 AdditiveNoise245 = desaturateVar241;
				float4 texCoord184 = packedInput.ase_texcoord2;
				texCoord184.xy = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float4 break17_g145 = tex2DNode211;
				float4 appendResult18_g145 = (float4(break17_g145.x , break17_g145.y , break17_g145.z , break17_g145.w));
				float4 clampResult19_g145 = clamp( ( appendResult18_g145 * _DetailMultiplyChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g145 = clampResult19_g145;
				float clampResult20_g145 = clamp( ( break2_g145.x + break2_g145.y + break2_g145.z + break2_g145.w ) , 0.0 , 1.0 );
				float ifLocalVar238 = 0;
				if( ( _DetailMultiplyChannel.x + _DetailMultiplyChannel.y + _DetailMultiplyChannel.z + _DetailMultiplyChannel.w ) <= 0.0 )
				ifLocalVar238 = 1.0;
				else
				ifLocalVar238 = clampResult20_g145;
				float3 temp_cast_11 = (ifLocalVar238).xxx;
				float3 desaturateInitialColor236 = temp_cast_11;
				float desaturateDot236 = dot( desaturateInitialColor236, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar236 = lerp( desaturateInitialColor236, desaturateDot236.xxx, 1.0 );
				float3 MultiplyNoise246 = desaturateVar236;
				float4 temp_output_191_0 = ( packedInput.ase_color * float4( ( desaturateVar175 + AdditiveNoise245 ) , 0.0 ) * ( texCoord184.z + 1.0 ) * float4( MultiplyNoise246 , 0.0 ) );
				
				float2 uv_AlphaOverride = packedInput.ase_texcoord2.xy * _AlphaOverride_ST.xy + _AlphaOverride_ST.zw;
				float temp_output_4_0_g139 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g139 = _FlipbooksColumsRows.y;
				float2 appendResult7_g139 = (float2(temp_output_4_0_g139 , temp_output_5_0_g139));
				float totalFrames39_g139 = ( temp_output_4_0_g139 * temp_output_5_0_g139 );
				float2 appendResult8_g139 = (float2(totalFrames39_g139 , temp_output_5_0_g139));
				float clampResult42_g139 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g139 - 1.0 ) );
				float temp_output_35_0_g139 = frac( ( ( (float)0 + clampResult42_g139 ) / totalFrames39_g139 ) );
				float2 appendResult29_g139 = (float2(temp_output_35_0_g139 , ( 1.0 - temp_output_35_0_g139 )));
				float2 temp_output_15_0_g139 = ( ( ( LocalUVOffset197 + uv_AlphaOverride ) / appendResult7_g139 ) + ( floor( ( appendResult8_g139 * appendResult29_g139 ) ) / appendResult7_g139 ) );
				float2 panner227 = ( 1.0 * _Time.y * _AlphaOverridePanning + temp_output_15_0_g139);
				float4 break2_g141 = ( tex2D( _AlphaOverride, panner227 ) * _AlphaOverrideChannel );
				float AlphaOverride234 = saturate( ( break2_g141.x + break2_g141.y + break2_g141.z + break2_g141.w ) );
				float2 panner226 = ( 1.0 * _Time.y * _MainAlphaPanning + temp_output_225_0);
				float4 break2_g142 = ( tex2D( _MainTex, panner226 ) * _MainAlphaChannel );
				float MainAlpha233 = saturate( ( break2_g142.x + break2_g142.y + break2_g142.z + break2_g142.w ) );
				float temp_output_169_0 = ( AlphaOverride234 * MainAlpha233 );
				float4 texCoord171 = packedInput.ase_texcoord2;
				texCoord171.xy = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_3_0_g143 = ( texCoord171.w - ( 1.0 - temp_output_169_0 ) );
				float temp_output_188_0 = ( packedInput.ase_color.a * temp_output_169_0 * saturate( saturate( ( temp_output_3_0_g143 / fwidth( temp_output_3_0_g143 ) ) ) ) );
				float4 screenPos = packedInput.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth251 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth251 = abs( ( screenDepth251 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _SoftFadeFactor ) );
				#ifdef _USESOFTALPHA_ON
				float staticSwitch249 = ( temp_output_188_0 * saturate( distanceDepth251 ) );
				#else
				float staticSwitch249 = temp_output_188_0;
				#endif
				
				surfaceDescription.Color = temp_output_191_0.rgb;
				surfaceDescription.Emission = temp_output_191_0.rgb;
				surfaceDescription.Alpha = staticSwitch249;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData( surfaceDescription,input, V, posInput, surfaceData, builtinData );

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData( input.positionSS.xy, surfaceData );
				LightTransportData lightTransportData = GetLightTransportData( surfaceData, builtinData, bsdfData );

				float4 res = float4( 0.0, 0.0, 0.0, 1.0 );
				UnityMetaInput metaInput;
				metaInput.Albedo = lightTransportData.diffuseColor.rgb;
				metaInput.Emission = lightTransportData.emissiveColor;
			#ifdef EDITOR_VISUALIZATION
				metaInput.VizUV = packedInput.VizUV;
				metaInput.LightCoord = packedInput.LightCoord;
			#endif
				res = UnityMetaFragment(metaInput);

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull [_CullMode]

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120112


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS 1
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			int _ObjectId;
			int _PassValue;

			CBUFFER_START( UnityPerMaterial )
			float4 _DetailNoise_ST;
			float4 _DetailDistortionChannel;
			float4 _MainTextureChannel;
			float4 _DetailAdditiveChannel;
			float4 _DetailMultiplyChannel;
			float4 _AlphaOverride_ST;
			float4 _AlphaOverrideChannel;
			float4 _MainAlphaChannel;
			float2 _MainTexturePanning;
			float2 _DetailNoisePanning;
			float2 _FlipbooksColumsRows;
			float2 _AlphaOverridePanning;
			float2 _MainAlphaPanning;
			float _DistortionIntensity;
			float _Desaturate;
			float _SoftFadeFactor;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _AlphaOverride;
			sampler2D _MainTex;
			sampler2D _DetailNoise;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USESOFTALPHA_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};


			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
				float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
				o.ase_texcoord2 = screenPos;
				
				o.ase_color = inputMesh.ase_color;
				o.ase_texcoord = inputMesh.ase_texcoord1;
				o.ase_texcoord1 = inputMesh.ase_texcoord;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =   defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_color = v.ase_color;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
					, out float4 outColor : SV_Target0
					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
					#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 texCoord192 = packedInput.ase_texcoord;
				texCoord192.xy = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult194 = (float2(texCoord192.z , 0.0));
				float2 appendResult195 = (float2(0.0 , 0.0));
				float2 LocalUVOffset197 = ( appendResult194 + appendResult195 );
				float2 uv_AlphaOverride = packedInput.ase_texcoord1.xy * _AlphaOverride_ST.xy + _AlphaOverride_ST.zw;
				float temp_output_4_0_g139 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g139 = _FlipbooksColumsRows.y;
				float2 appendResult7_g139 = (float2(temp_output_4_0_g139 , temp_output_5_0_g139));
				float totalFrames39_g139 = ( temp_output_4_0_g139 * temp_output_5_0_g139 );
				float2 appendResult8_g139 = (float2(totalFrames39_g139 , temp_output_5_0_g139));
				float4 texCoord218 = packedInput.ase_texcoord;
				texCoord218.xy = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult42_g139 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g139 - 1.0 ) );
				float temp_output_35_0_g139 = frac( ( ( (float)0 + clampResult42_g139 ) / totalFrames39_g139 ) );
				float2 appendResult29_g139 = (float2(temp_output_35_0_g139 , ( 1.0 - temp_output_35_0_g139 )));
				float2 temp_output_15_0_g139 = ( ( ( LocalUVOffset197 + uv_AlphaOverride ) / appendResult7_g139 ) + ( floor( ( appendResult8_g139 * appendResult29_g139 ) ) / appendResult7_g139 ) );
				float2 panner227 = ( 1.0 * _Time.y * _AlphaOverridePanning + temp_output_15_0_g139);
				float4 break2_g141 = ( tex2D( _AlphaOverride, panner227 ) * _AlphaOverrideChannel );
				float AlphaOverride234 = saturate( ( break2_g141.x + break2_g141.y + break2_g141.z + break2_g141.w ) );
				float2 uv_DetailNoise = packedInput.ase_texcoord1.xy * _DetailNoise_ST.xy + _DetailNoise_ST.zw;
				float2 panner209 = ( 1.0 * _Time.y * _DetailNoisePanning + uv_DetailNoise);
				float4 tex2DNode211 = tex2D( _DetailNoise, panner209 );
				float4 break17_g136 = tex2DNode211;
				float4 appendResult18_g136 = (float4(break17_g136.x , break17_g136.y , break17_g136.z , break17_g136.w));
				float4 clampResult19_g136 = clamp( ( appendResult18_g136 * _DetailDistortionChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g136 = clampResult19_g136;
				float clampResult20_g136 = clamp( ( break2_g136.x + break2_g136.y + break2_g136.z + break2_g136.w ) , 0.0 , 1.0 );
				float3 temp_cast_3 = (clampResult20_g136).xxx;
				float3 desaturateInitialColor213 = temp_cast_3;
				float desaturateDot213 = dot( desaturateInitialColor213, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar213 = lerp( desaturateInitialColor213, desaturateDot213.xxx, 1.0 );
				float3 DistortionNoise214 = desaturateVar213;
				float2 texCoord200 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float3 UVFlipbookInput205 = ( ( DistortionNoise214 * _DistortionIntensity ) + float3( ( LocalUVOffset197 + texCoord200 ) ,  0.0 ) );
				float temp_output_4_0_g140 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g140 = _FlipbooksColumsRows.y;
				float2 appendResult7_g140 = (float2(temp_output_4_0_g140 , temp_output_5_0_g140));
				float totalFrames39_g140 = ( temp_output_4_0_g140 * temp_output_5_0_g140 );
				float2 appendResult8_g140 = (float2(totalFrames39_g140 , temp_output_5_0_g140));
				float clampResult42_g140 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g140 - 1.0 ) );
				float temp_output_35_0_g140 = frac( ( ( (float)0 + clampResult42_g140 ) / totalFrames39_g140 ) );
				float2 appendResult29_g140 = (float2(temp_output_35_0_g140 , ( 1.0 - temp_output_35_0_g140 )));
				float2 temp_output_15_0_g140 = ( ( UVFlipbookInput205.xy / appendResult7_g140 ) + ( floor( ( appendResult8_g140 * appendResult29_g140 ) ) / appendResult7_g140 ) );
				float2 temp_output_225_0 = temp_output_15_0_g140;
				float2 panner226 = ( 1.0 * _Time.y * _MainAlphaPanning + temp_output_225_0);
				float4 break2_g142 = ( tex2D( _MainTex, panner226 ) * _MainAlphaChannel );
				float MainAlpha233 = saturate( ( break2_g142.x + break2_g142.y + break2_g142.z + break2_g142.w ) );
				float temp_output_169_0 = ( AlphaOverride234 * MainAlpha233 );
				float4 texCoord171 = packedInput.ase_texcoord1;
				texCoord171.xy = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_3_0_g143 = ( texCoord171.w - ( 1.0 - temp_output_169_0 ) );
				float temp_output_188_0 = ( packedInput.ase_color.a * temp_output_169_0 * saturate( saturate( ( temp_output_3_0_g143 / fwidth( temp_output_3_0_g143 ) ) ) ) );
				float4 screenPos = packedInput.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth251 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth251 = abs( ( screenDepth251 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _SoftFadeFactor ) );
				#ifdef _USESOFTALPHA_ON
				float staticSwitch249 = ( temp_output_188_0 * saturate( distanceDepth251 ) );
				#else
				float staticSwitch249 = temp_output_188_0;
				#endif
				
				surfaceDescription.Alpha = staticSwitch249;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthForwardOnly"
			Tags { "LightMode"="DepthForwardOnly" }

			Cull [_CullMode]
			ZWrite On
			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			ColorMask 0 0

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120112


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

			CBUFFER_START( UnityPerMaterial )
			float4 _DetailNoise_ST;
			float4 _DetailDistortionChannel;
			float4 _MainTextureChannel;
			float4 _DetailAdditiveChannel;
			float4 _DetailMultiplyChannel;
			float4 _AlphaOverride_ST;
			float4 _AlphaOverrideChannel;
			float4 _MainAlphaChannel;
			float2 _MainTexturePanning;
			float2 _DetailNoisePanning;
			float2 _FlipbooksColumsRows;
			float2 _AlphaOverridePanning;
			float2 _MainAlphaPanning;
			float _DistortionIntensity;
			float _Desaturate;
			float _SoftFadeFactor;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _AlphaOverride;
			sampler2D _MainTex;
			sampler2D _DetailNoise;


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USESOFTALPHA_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_Position;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			void BuildSurfaceData(FragInputs fragInputs, SurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				#ifdef WRITE_NORMAL_BUFFER
				surfaceData.normalWS = fragInputs.tangentToWorld[2];
				#endif
			}

			void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

				#if _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#if _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif

				BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData);
				ZERO_INITIALIZE(BuiltinData, builtinData);
				builtinData.opacity =  surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

				#if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif

                ApplyDebugToBuiltinData(builtinData);
			}

			VertexOutput VertexFunction( VertexInput inputMesh  )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
				float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
				o.ase_texcoord2 = screenPos;
				
				o.ase_color = inputMesh.ase_color;
				o.ase_texcoord = inputMesh.ase_texcoord1;
				o.ase_texcoord1 = inputMesh.ase_texcoord;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =   defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				o.positionCS = TransformWorldToHClip(positionRWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_color = v.ase_color;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( VertexOutput packedInput
						#ifdef WRITE_MSAA_DEPTH
						, out float4 depthColor : SV_Target0
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target1
							#endif
						#else
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target0
							#endif
						#endif
						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
					
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3( 1.0, 1.0, 1.0 );

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 texCoord192 = packedInput.ase_texcoord;
				texCoord192.xy = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult194 = (float2(texCoord192.z , 0.0));
				float2 appendResult195 = (float2(0.0 , 0.0));
				float2 LocalUVOffset197 = ( appendResult194 + appendResult195 );
				float2 uv_AlphaOverride = packedInput.ase_texcoord1.xy * _AlphaOverride_ST.xy + _AlphaOverride_ST.zw;
				float temp_output_4_0_g139 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g139 = _FlipbooksColumsRows.y;
				float2 appendResult7_g139 = (float2(temp_output_4_0_g139 , temp_output_5_0_g139));
				float totalFrames39_g139 = ( temp_output_4_0_g139 * temp_output_5_0_g139 );
				float2 appendResult8_g139 = (float2(totalFrames39_g139 , temp_output_5_0_g139));
				float4 texCoord218 = packedInput.ase_texcoord;
				texCoord218.xy = packedInput.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult42_g139 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g139 - 1.0 ) );
				float temp_output_35_0_g139 = frac( ( ( (float)0 + clampResult42_g139 ) / totalFrames39_g139 ) );
				float2 appendResult29_g139 = (float2(temp_output_35_0_g139 , ( 1.0 - temp_output_35_0_g139 )));
				float2 temp_output_15_0_g139 = ( ( ( LocalUVOffset197 + uv_AlphaOverride ) / appendResult7_g139 ) + ( floor( ( appendResult8_g139 * appendResult29_g139 ) ) / appendResult7_g139 ) );
				float2 panner227 = ( 1.0 * _Time.y * _AlphaOverridePanning + temp_output_15_0_g139);
				float4 break2_g141 = ( tex2D( _AlphaOverride, panner227 ) * _AlphaOverrideChannel );
				float AlphaOverride234 = saturate( ( break2_g141.x + break2_g141.y + break2_g141.z + break2_g141.w ) );
				float2 uv_DetailNoise = packedInput.ase_texcoord1.xy * _DetailNoise_ST.xy + _DetailNoise_ST.zw;
				float2 panner209 = ( 1.0 * _Time.y * _DetailNoisePanning + uv_DetailNoise);
				float4 tex2DNode211 = tex2D( _DetailNoise, panner209 );
				float4 break17_g136 = tex2DNode211;
				float4 appendResult18_g136 = (float4(break17_g136.x , break17_g136.y , break17_g136.z , break17_g136.w));
				float4 clampResult19_g136 = clamp( ( appendResult18_g136 * _DetailDistortionChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g136 = clampResult19_g136;
				float clampResult20_g136 = clamp( ( break2_g136.x + break2_g136.y + break2_g136.z + break2_g136.w ) , 0.0 , 1.0 );
				float3 temp_cast_3 = (clampResult20_g136).xxx;
				float3 desaturateInitialColor213 = temp_cast_3;
				float desaturateDot213 = dot( desaturateInitialColor213, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar213 = lerp( desaturateInitialColor213, desaturateDot213.xxx, 1.0 );
				float3 DistortionNoise214 = desaturateVar213;
				float2 texCoord200 = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float3 UVFlipbookInput205 = ( ( DistortionNoise214 * _DistortionIntensity ) + float3( ( LocalUVOffset197 + texCoord200 ) ,  0.0 ) );
				float temp_output_4_0_g140 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g140 = _FlipbooksColumsRows.y;
				float2 appendResult7_g140 = (float2(temp_output_4_0_g140 , temp_output_5_0_g140));
				float totalFrames39_g140 = ( temp_output_4_0_g140 * temp_output_5_0_g140 );
				float2 appendResult8_g140 = (float2(totalFrames39_g140 , temp_output_5_0_g140));
				float clampResult42_g140 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g140 - 1.0 ) );
				float temp_output_35_0_g140 = frac( ( ( (float)0 + clampResult42_g140 ) / totalFrames39_g140 ) );
				float2 appendResult29_g140 = (float2(temp_output_35_0_g140 , ( 1.0 - temp_output_35_0_g140 )));
				float2 temp_output_15_0_g140 = ( ( UVFlipbookInput205.xy / appendResult7_g140 ) + ( floor( ( appendResult8_g140 * appendResult29_g140 ) ) / appendResult7_g140 ) );
				float2 temp_output_225_0 = temp_output_15_0_g140;
				float2 panner226 = ( 1.0 * _Time.y * _MainAlphaPanning + temp_output_225_0);
				float4 break2_g142 = ( tex2D( _MainTex, panner226 ) * _MainAlphaChannel );
				float MainAlpha233 = saturate( ( break2_g142.x + break2_g142.y + break2_g142.z + break2_g142.w ) );
				float temp_output_169_0 = ( AlphaOverride234 * MainAlpha233 );
				float4 texCoord171 = packedInput.ase_texcoord1;
				texCoord171.xy = packedInput.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_3_0_g143 = ( texCoord171.w - ( 1.0 - temp_output_169_0 ) );
				float temp_output_188_0 = ( packedInput.ase_color.a * temp_output_169_0 * saturate( saturate( ( temp_output_3_0_g143 / fwidth( temp_output_3_0_g143 ) ) ) ) );
				float4 screenPos = packedInput.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth251 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth251 = abs( ( screenDepth251 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _SoftFadeFactor ) );
				#ifdef _USESOFTALPHA_ON
				float staticSwitch249 = ( temp_output_188_0 * saturate( distanceDepth251 ) );
				#else
				float staticSwitch249 = temp_output_188_0;
				#endif
				
				surfaceDescription.Alpha = staticSwitch249;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.positionCS.z;
					#ifdef _ALPHATOMASK_ON
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
					#endif
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif
			}

			ENDHLSL
		}

		
        Pass
		{
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

            Cull [_CullMode]

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120112


			#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC

			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			#if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
			#define _WRITE_TRANSPARENT_MOTION_VECTOR
			#endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_TANGENT_TO_WORLD

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENEPICKINGPASS 1

			#define SHADER_UNLIT

			float4 _SelectionID;

            CBUFFER_START( UnityPerMaterial )
			float4 _DetailNoise_ST;
			float4 _DetailDistortionChannel;
			float4 _MainTextureChannel;
			float4 _DetailAdditiveChannel;
			float4 _DetailMultiplyChannel;
			float4 _AlphaOverride_ST;
			float4 _AlphaOverrideChannel;
			float4 _MainAlphaChannel;
			float2 _MainTexturePanning;
			float2 _DetailNoisePanning;
			float2 _FlipbooksColumsRows;
			float2 _AlphaOverridePanning;
			float2 _MainAlphaPanning;
			float _DistortionIntensity;
			float _Desaturate;
			float _SoftFadeFactor;
			float4 _EmissionColor;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			#ifdef _ENABLE_SHADOW_MATTE
			float _ShadowMatteFilter;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _AlphaCutoff;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			float _EnableBlendModePreserveSpecularLighting;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _AlphaOverride;
			sampler2D _MainTex;
			sampler2D _DetailNoise;


            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USESOFTALPHA_ON


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 tangentWS : TEXCOORD1;
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
            struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};


            void GetSurfaceAndBuiltinData(SurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                #ifdef LOD_FADE_CROSSFADE
			        LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif

                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif

                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif


				ZERO_INITIALIZE(SurfaceData, surfaceData);

				ZERO_BUILTIN_INITIALIZE(builtinData);
				builtinData.opacity = surfaceDescription.Alpha;

				#if defined(DEBUG_DISPLAY)
					builtinData.renderingLayers = GetMeshRenderingLightLayer();
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif

                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif


                ApplyDebugToBuiltinData(builtinData);

            }


			VertexOutput VertexFunction(VertexInput inputMesh  )
			{

				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, o );

				float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
				float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
				o.ase_texcoord4 = screenPos;
				
				o.ase_color = inputMesh.ase_color;
				o.ase_texcoord2 = inputMesh.ase_texcoord1;
				o.ase_texcoord3 = inputMesh.ase_texcoord;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =   defaultVertexValue ;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				o.positionCS = TransformWorldToHClip(positionRWS);
				o.normalWS.xyz =  normalWS;
				o.tangentWS.xyzw =  tangentWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_color = v.ase_color;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput Vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag(	VertexOutput packedInput
						, out float4 outColor : SV_Target0
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				UNITY_SETUP_INSTANCE_ID(packedInput);

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.tangentToWorld = BuildTangentToWorld(packedInput.tangentWS.xyzw, packedInput.normalWS.xyz);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				SurfaceDescription surfaceDescription = (SurfaceDescription)0;
				float4 texCoord192 = packedInput.ase_texcoord2;
				texCoord192.xy = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult194 = (float2(texCoord192.z , 0.0));
				float2 appendResult195 = (float2(0.0 , 0.0));
				float2 LocalUVOffset197 = ( appendResult194 + appendResult195 );
				float2 uv_AlphaOverride = packedInput.ase_texcoord3.xy * _AlphaOverride_ST.xy + _AlphaOverride_ST.zw;
				float temp_output_4_0_g139 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g139 = _FlipbooksColumsRows.y;
				float2 appendResult7_g139 = (float2(temp_output_4_0_g139 , temp_output_5_0_g139));
				float totalFrames39_g139 = ( temp_output_4_0_g139 * temp_output_5_0_g139 );
				float2 appendResult8_g139 = (float2(totalFrames39_g139 , temp_output_5_0_g139));
				float4 texCoord218 = packedInput.ase_texcoord2;
				texCoord218.xy = packedInput.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float clampResult42_g139 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g139 - 1.0 ) );
				float temp_output_35_0_g139 = frac( ( ( (float)0 + clampResult42_g139 ) / totalFrames39_g139 ) );
				float2 appendResult29_g139 = (float2(temp_output_35_0_g139 , ( 1.0 - temp_output_35_0_g139 )));
				float2 temp_output_15_0_g139 = ( ( ( LocalUVOffset197 + uv_AlphaOverride ) / appendResult7_g139 ) + ( floor( ( appendResult8_g139 * appendResult29_g139 ) ) / appendResult7_g139 ) );
				float2 panner227 = ( 1.0 * _Time.y * _AlphaOverridePanning + temp_output_15_0_g139);
				float4 break2_g141 = ( tex2D( _AlphaOverride, panner227 ) * _AlphaOverrideChannel );
				float AlphaOverride234 = saturate( ( break2_g141.x + break2_g141.y + break2_g141.z + break2_g141.w ) );
				float2 uv_DetailNoise = packedInput.ase_texcoord3.xy * _DetailNoise_ST.xy + _DetailNoise_ST.zw;
				float2 panner209 = ( 1.0 * _Time.y * _DetailNoisePanning + uv_DetailNoise);
				float4 tex2DNode211 = tex2D( _DetailNoise, panner209 );
				float4 break17_g136 = tex2DNode211;
				float4 appendResult18_g136 = (float4(break17_g136.x , break17_g136.y , break17_g136.z , break17_g136.w));
				float4 clampResult19_g136 = clamp( ( appendResult18_g136 * _DetailDistortionChannel ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
				float4 break2_g136 = clampResult19_g136;
				float clampResult20_g136 = clamp( ( break2_g136.x + break2_g136.y + break2_g136.z + break2_g136.w ) , 0.0 , 1.0 );
				float3 temp_cast_3 = (clampResult20_g136).xxx;
				float3 desaturateInitialColor213 = temp_cast_3;
				float desaturateDot213 = dot( desaturateInitialColor213, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar213 = lerp( desaturateInitialColor213, desaturateDot213.xxx, 1.0 );
				float3 DistortionNoise214 = desaturateVar213;
				float2 texCoord200 = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float3 UVFlipbookInput205 = ( ( DistortionNoise214 * _DistortionIntensity ) + float3( ( LocalUVOffset197 + texCoord200 ) ,  0.0 ) );
				float temp_output_4_0_g140 = _FlipbooksColumsRows.x;
				float temp_output_5_0_g140 = _FlipbooksColumsRows.y;
				float2 appendResult7_g140 = (float2(temp_output_4_0_g140 , temp_output_5_0_g140));
				float totalFrames39_g140 = ( temp_output_4_0_g140 * temp_output_5_0_g140 );
				float2 appendResult8_g140 = (float2(totalFrames39_g140 , temp_output_5_0_g140));
				float clampResult42_g140 = clamp( texCoord218.x , 0.0001 , ( totalFrames39_g140 - 1.0 ) );
				float temp_output_35_0_g140 = frac( ( ( (float)0 + clampResult42_g140 ) / totalFrames39_g140 ) );
				float2 appendResult29_g140 = (float2(temp_output_35_0_g140 , ( 1.0 - temp_output_35_0_g140 )));
				float2 temp_output_15_0_g140 = ( ( UVFlipbookInput205.xy / appendResult7_g140 ) + ( floor( ( appendResult8_g140 * appendResult29_g140 ) ) / appendResult7_g140 ) );
				float2 temp_output_225_0 = temp_output_15_0_g140;
				float2 panner226 = ( 1.0 * _Time.y * _MainAlphaPanning + temp_output_225_0);
				float4 break2_g142 = ( tex2D( _MainTex, panner226 ) * _MainAlphaChannel );
				float MainAlpha233 = saturate( ( break2_g142.x + break2_g142.y + break2_g142.z + break2_g142.w ) );
				float temp_output_169_0 = ( AlphaOverride234 * MainAlpha233 );
				float4 texCoord171 = packedInput.ase_texcoord3;
				texCoord171.xy = packedInput.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_3_0_g143 = ( texCoord171.w - ( 1.0 - temp_output_169_0 ) );
				float temp_output_188_0 = ( packedInput.ase_color.a * temp_output_169_0 * saturate( saturate( ( temp_output_3_0_g143 / fwidth( temp_output_3_0_g143 ) ) ) ) );
				float4 screenPos = packedInput.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth251 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth251 = abs( ( screenDepth251 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _SoftFadeFactor ) );
				#ifdef _USESOFTALPHA_ON
				float staticSwitch249 = ( temp_output_188_0 * saturate( distanceDepth251 ) );
				#else
				float staticSwitch249 = temp_output_188_0;
				#endif
				
				surfaceDescription.Alpha = staticSwitch249;
				surfaceDescription.AlphaClipThreshold =  _AlphaCutoff;


				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);
				outColor = _SelectionID;
			}

            ENDHLSL
        }

		Pass
		{

            Name "FullScreenDebug"
            Tags 
			{ 
				"LightMode" = "FullScreenDebug" 
			}

            Cull [_CullMode]
			ZTest LEqual
			ZWrite Off

			HLSLPROGRAM

			/*ase_pragma_before*/

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer

			#pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC

			#pragma vertex Vert
			#pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl"

            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT

            #define SHADERPASS SHADERPASS_FULL_SCREEN_DEBUG
			#define SHADER_UNLIT

            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
            #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            struct AttributesMesh
			{
				 float3 positionOS : POSITION;
				 float3 normalOS : NORMAL;
				 float4 tangentOS : TANGENT;
				#if UNITY_ANY_INSTANCING_ENABLED
				 uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			struct VaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
				 uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			struct VertexDescriptionInputs
			{
				 float3 ObjectSpaceNormal;
				 float3 ObjectSpaceTangent;
				 float3 ObjectSpacePosition;
			};

			struct SurfaceDescriptionInputs
			{
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED
				 uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
			{
				PackedVaryingsMeshToPS output;
				ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

			VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
			{
				VaryingsMeshToPS output;
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

            struct VertexDescription
			{
				float3 Position;
				float3 Normal;
				float3 Tangent;
			};

			VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
			{
				VertexDescription description = (VertexDescription)0;
				description.Position = IN.ObjectSpacePosition;
				description.Normal = IN.ObjectSpaceNormal;
				description.Tangent = IN.ObjectSpaceTangent;
				return description;
			}


            struct SurfaceDescription
			{
				float3 BaseColor;
				float3 Emission;
				float Alpha;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
				surface.Emission = float3(0, 0, 0);
				surface.Alpha = 1;
				return surface;
			}



			VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
			{
				VertexDescriptionInputs output;
				ZERO_INITIALIZE(VertexDescriptionInputs, output);
				output.ObjectSpaceNormal =                          input.normalOS;
				output.ObjectSpaceTangent =                         input.tangentOS.xyz;
				output.ObjectSpacePosition =                        input.positionOS;
				return output;
			}

			AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters )
			{
				VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
				VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
				input.positionOS = vertexDescription.Position;
				input.normalOS = vertexDescription.Normal;
				input.tangentOS.xyz = vertexDescription.Tangent;
				return input;
			}

			FragInputs BuildFragInputs(VaryingsMeshToPS input)
			{
				FragInputs output;
				ZERO_INITIALIZE(FragInputs, output);

				output.tangentToWorld = k_identity3x3;
				output.positionSS = input.positionCS;

				return output;
			}


			FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
			{
				UNITY_SETUP_INSTANCE_ID(input);
				VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
				return BuildFragInputs(unpacked);
			}
				SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
			{
				SurfaceDescriptionInputs output;
				ZERO_INITIALIZE(SurfaceDescriptionInputs, output);


        		return output;
			}

			#define DEBUG_DISPLAY
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/FullScreenDebug.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"

			PackedVaryingsType Vert(AttributesMesh inputMesh)
			{
				VaryingsType varyingsType;
				varyingsType.vmesh = VertMesh(inputMesh);
				return PackVaryingsType(varyingsType);
			}

			#if !defined(_DEPTHOFFSET_ON)
			[earlydepthstencil]
			#endif
			void Frag(PackedVaryingsToPS packedInput)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				FragInputs input = UnpackVaryingsToFragInputs(packedInput);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz);

			#ifdef PLATFORM_SUPPORTS_PRIMITIVE_ID_IN_PIXEL_SHADER
				if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_QUAD_OVERDRAW)
				{
					IncrementQuadOverdrawCounter(posInput.positionSS.xy, input.primitiveID);
				}
			#endif
			}

            ENDHLSL
        }
		
	}
	
	CustomEditor "Rendering.HighDefinition.LitShaderGraphGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.CommentaryNode;158;-999.8004,-2121.479;Inherit;False;1884.647;1001.187;Extra Noise Setup;22;247;246;245;244;243;242;241;240;239;238;237;236;235;214;213;212;211;210;209;208;207;206;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;206;-960.6143,-1878.96;Inherit;True;Property;_DetailNoise;Detail Noise;10;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;207;-706.5922,-1589.672;Inherit;False;Property;_DetailNoisePanning;Detail Noise Panning;11;0;Create;False;0;0;0;False;0;False;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;208;-711.1732,-1705.759;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;159;941.6688,-1589.481;Inherit;False;869.2021;446.9999;UV Offset Controlled by custom vertex stream;6;197;196;195;194;193;192;;0.3699214,0.2971698,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;209;-473.1751,-1630.759;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;192;973.6688,-1525.481;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;210;-262.3925,-1667.27;Inherit;False;Property;_DetailDistortionChannel;Detail Distortion Channel;12;0;Create;False;0;0;0;False;0;False;1,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;193;989.6688,-1285.482;Inherit;False;Constant;_InitialOffset1;Initial Offset;16;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;211;-286.3775,-1875.46;Inherit;True;Property;_TextureSample4;Texture Sample 3;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;194;1181.669,-1461.482;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;212;16.89372,-1865.296;Inherit;False;Channel Picker;-1;;136;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;195;1261.669,-1285.482;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;1453.669,-1317.482;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DesaturateOpNode;213;44.27821,-1778.763;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;214;26.39369,-1691.096;Inherit;False;DistortionNoise;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;197;1613.669,-1317.482;Inherit;False;LocalUVOffset;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;160;949.5999,-2249.169;Inherit;False;853.4072;636.7309;Set UV Modifiers For Main Tex;8;205;204;203;202;201;200;199;198;;1,0.8279877,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;1039.233,-1904.855;Inherit;False;197;LocalUVOffset;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;200;1006.238,-1776.438;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;201;1003.6,-2199.169;Inherit;False;214;DistortionNoise;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;199;999.5999,-2037.169;Inherit;False;Property;_DistortionIntensity;Distortion Intensity;13;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;202;1265.09,-1927.62;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;1225.602,-2124.17;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;161;-3057.841,-2413.333;Inherit;False;1894.068;530.1917;Alpha Override;12;234;232;230;228;227;224;223;222;221;217;216;215;;0,0.5461459,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;204;1402.602,-1991.169;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;215;-3027.099,-2354.74;Inherit;True;Property;_AlphaOverride;Alpha Override;6;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;216;-2765.5,-2199.046;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;162;-2694.571,-1378.17;Inherit;False;1576.333;998.0396;Main Texture Set Vars;16;233;231;229;226;225;220;182;181;180;179;178;177;166;165;164;163;;0,0.5461459,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;217;-2739.098,-2274.74;Inherit;False;197;LocalUVOffset;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;1569.008,-1935.55;Inherit;False;UVFlipbookInput;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;-2670.452,-1310.065;Inherit;False;205;UVFlipbookInput;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;222;-2547.099,-2274.74;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;219;-2684.954,-1681.634;Inherit;False;Property;_FlipbooksColumsRows;Flipbooks Colums & Rows;9;0;Create;False;0;0;0;False;0;False;1,1;1,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.IntNode;221;-2422.593,-2113.9;Inherit;False;Constant;_Int;Int ;9;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;218;-2658.401,-1844.135;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;163;-2433.973,-1142.627;Inherit;False;Constant;_Int0;Int 0;9;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;224;-2421.211,-2274.714;Inherit;False;Flipbook;-1;;139;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.Vector2Node;164;-2112.326,-1163.986;Inherit;False;Property;_MainAlphaPanning;Main Alpha Panning;5;0;Create;False;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;223;-2435.979,-2039.085;Inherit;False;Property;_AlphaOverridePanning;Alpha Override Panning;7;0;Create;False;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;225;-2435.66,-1302.777;Inherit;False;Flipbook;-1;;140;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.TexturePropertyNode;165;-2144.326,-971.9857;Inherit;True;Property;_MainTex;Main Texture;0;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;227;-2147.098,-2242.74;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;226;-2112.326,-1275.986;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;230;-1897.099,-2152.74;Inherit;False;Property;_AlphaOverrideChannel;Alpha Override Channel;8;0;Create;False;0;0;0;False;0;False;1,0,0,0;0,0,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;228;-1955.099,-2338.74;Inherit;True;Property;_TextureSample3;Texture Sample 2;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;166;-1863.4,-1068.224;Inherit;False;Property;_MainAlphaChannel;Main Alpha Channel;3;0;Create;False;0;0;0;False;0;False;0,0,0,1;0,0,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;229;-1889.326,-1291.986;Inherit;True;Property;_TextureSample2;Texture Sample 1;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;232;-1651.099,-2338.74;Inherit;False;Channel Picker Alpha;-1;;141;e49841402b321534583d1dc019041b68;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;231;-1568.326,-1291.986;Inherit;False;Channel Picker Alpha;-1;;142;e49841402b321534583d1dc019041b68;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;234;-1444.099,-2338.74;Inherit;True;AlphaOverride;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;233;-1344.326,-1291.986;Inherit;True;MainAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;168;-332.8427,-311.5;Inherit;False;233;MainAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;167;-355.2169,-387.1024;Inherit;False;234;AlphaOverride;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;169;-140.8424,-407.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;170;41.97596,-291.4886;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;171;-18.77578,-216.675;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;172;218.8035,-224.0667;Inherit;False;Step Antialiasing;-1;;143;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;252;18.66154,186.41;Inherit;False;Property;_SoftFadeFactor;SoftFadeFactor;17;0;Create;False;0;0;0;False;0;False;0.1;0;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;173;399.2592,-224.9552;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;251;301.6616,115.41;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;174;564.4019,-428.0732;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;584.1821,-268.765;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;250;557.6616,110.41;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;780.0134,-89.69116;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;236;426.1782,-1408.064;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;244;-339.9926,-1436.573;Inherit;False;Property;_DetailMultiplyChannel;Detail Multiply Channel;14;0;Create;False;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;180;-1344.326,-635.9854;Inherit;True;MainTexInfo;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;239;-90.65478,-1406.945;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;185;426.1795,-1043.084;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;184;374.8037,-748.0399;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;187;-77.69921,-943.9547;Inherit;False;180;MainTexInfo;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;190;466.4144,-871.5717;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;183;622.5521,-760.8052;Inherit;False;246;MultiplyNoise;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;246;583.5939,-1409.197;Inherit;False;MultiplyNoise;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ConditionalIfNode;238;268.4739,-1408.582;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;175;-72.77139,-873.7276;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;182;-1895.393,-607.8138;Inherit;False;Property;_MainTextureChannel;Main Texture Channel;2;0;Create;False;0;0;0;False;0;False;1,1,1,0;1,1,1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;181;-1904.326,-827.9854;Inherit;True;Property;_TextureSample1;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;177;-2112.326,-699.9854;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;178;-2128.326,-587.9854;Inherit;False;Property;_MainTexturePanning;Main Texture Panning;4;0;Create;False;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DesaturateOpNode;241;658.2786,-1900.865;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;648.5884,-901.9236;Inherit;False;4;4;0;COLOR;1,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;245;655.1942,-1812.597;Inherit;False;AdditiveNoise;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;242;5.107399,-2047.171;Inherit;False;Property;_DetailAdditiveChannel;Detail Additive Channel;15;0;Create;False;0;0;0;False;0;False;0,0,0,0;0,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;186;419.0553,-585.8185;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;280.4949,-1897.861;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;237;530.0615,-1905.428;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;179;-1552.326,-635.9854;Inherit;False;Channel Picker;-1;;144;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;247;37.09401,-1359.597;Inherit;False;Channel Picker;-1;;145;dc5f4cb24a8bdf448b40a1ec5866280e;0;2;5;FLOAT4;1,0,0,0;False;7;FLOAT4;0,0,0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;249;844.0134,-281.6912;Inherit;False;Property;_UseSoftAlpha;UseSoftAlpha;16;0;Create;False;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;243;403.2616,-1902.327;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;189;123.0929,-846.6474;Inherit;False;245;AdditiveNoise;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;235;106.974,-1223.582;Inherit;False;Constant;_Float1;Float 0;15;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-131.8308,-787.2311;Inherit;False;Property;_Desaturate;Desaturate?;1;0;Create;False;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;116;647.0098,-625.7703;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;8;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps5;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;117;647.0098,-625.7703;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;META;0;2;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;8;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps5;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;121;647.0098,-625.7703;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DistortionVectors;0;6;DistortionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;8;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps5;switch;0;False;True;4;1;False;;1;False;;4;1;False;;1;False;;True;1;False;;1;False;;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDistortionVec;255;False;;255;True;_StencilWriteMaskDistortionVec;7;False;;3;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;2;False;;True;3;False;;False;True;1;LightMode=DistortionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;119;647.0098,-625.7703;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;DepthForwardOnly;0;4;DepthForwardOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;8;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps5;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;False;False;True;1;LightMode=DepthForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;118;647.0098,-625.7703;Float;False;False;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;13;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;8;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps5;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;115;1020.969,-746.8214;Float;False;True;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;13;Piloto Studio/Additive Uber Shader;7f5cb9c3ea6481f469fdd856555439ef;True;Forward Unlit;0;0;Forward Unlit;9;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;5;True;8;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps5;switch;0;True;True;4;1;True;_SrcBlend;0;True;_DstBlend;0;0;True;_AlphaSrcBlend;0;True;_AlphaDstBlend;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVel;False;False;False;False;False;True;True;0;True;_StencilRef;255;False;;255;True;_StencilWriteMask;7;False;;3;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;0;True;_ZWrite;True;0;True;_ZTestTransparent;False;True;1;LightMode=ForwardOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;30;Surface Type;1;0;  Rendering Pass ;0;0;  Rendering Pass;1;0;  Blending Mode;2;0;  Receive Fog;0;0;  Distortion;0;0;    Distortion Mode;0;0;    Distortion Only;1;0;  Depth Write;0;0;  Cull Mode;0;0;  Depth Test;4;0;Double-Sided;0;0;Alpha Clipping;0;0;Motion Vectors;0;0;  Add Precomputed Velocity;0;0;Shadow Matte;0;0;Cast Shadows;0;0;DOTS Instancing;0;0;GPU Instancing;1;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;LOD CrossFade;0;0;0;8;True;False;True;True;True;False;False;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;253;1020.969,-696.8214;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;1;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;MotionVectors;0;5;MotionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefMV;255;False;;255;True;_StencilWriteMaskMV;7;False;;3;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=MotionVectors;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;254;1020.969,-686.8214;Float;False;False;-1;2;Rendering.HighDefinition.HDUnlitGUI;0;1;New Amplify Shader;7f5cb9c3ea6481f469fdd856555439ef;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;208;2;206;0
WireConnection;209;0;208;0
WireConnection;209;2;207;0
WireConnection;211;0;206;0
WireConnection;211;1;209;0
WireConnection;194;0;192;3
WireConnection;212;5;211;0
WireConnection;212;7;210;0
WireConnection;195;0;193;0
WireConnection;196;0;194;0
WireConnection;196;1;195;0
WireConnection;213;0;212;0
WireConnection;214;0;213;0
WireConnection;197;0;196;0
WireConnection;202;0;198;0
WireConnection;202;1;200;0
WireConnection;203;0;201;0
WireConnection;203;1;199;0
WireConnection;204;0;203;0
WireConnection;204;1;202;0
WireConnection;216;2;215;0
WireConnection;205;0;204;0
WireConnection;222;0;217;0
WireConnection;222;1;216;0
WireConnection;224;13;222;0
WireConnection;224;4;219;1
WireConnection;224;5;219;2
WireConnection;224;24;218;1
WireConnection;224;2;221;0
WireConnection;225;13;220;0
WireConnection;225;4;219;1
WireConnection;225;5;219;2
WireConnection;225;24;218;1
WireConnection;225;2;163;0
WireConnection;227;0;224;0
WireConnection;227;2;223;0
WireConnection;226;0;225;0
WireConnection;226;2;164;0
WireConnection;228;0;215;0
WireConnection;228;1;227;0
WireConnection;229;0;165;0
WireConnection;229;1;226;0
WireConnection;232;5;228;0
WireConnection;232;7;230;0
WireConnection;231;5;229;0
WireConnection;231;7;166;0
WireConnection;234;0;232;0
WireConnection;233;0;231;0
WireConnection;169;0;167;0
WireConnection;169;1;168;0
WireConnection;170;0;169;0
WireConnection;172;1;170;0
WireConnection;172;2;171;4
WireConnection;173;0;172;0
WireConnection;251;0;252;0
WireConnection;188;0;174;4
WireConnection;188;1;169;0
WireConnection;188;2;173;0
WireConnection;250;0;251;0
WireConnection;248;0;188;0
WireConnection;248;1;250;0
WireConnection;236;0;238;0
WireConnection;180;0;179;0
WireConnection;239;0;244;1
WireConnection;239;1;244;2
WireConnection;239;2;244;3
WireConnection;239;3;244;4
WireConnection;190;0;175;0
WireConnection;190;1;189;0
WireConnection;246;0;236;0
WireConnection;238;0;239;0
WireConnection;238;2;247;0
WireConnection;238;3;235;0
WireConnection;238;4;235;0
WireConnection;175;0;187;0
WireConnection;175;1;176;0
WireConnection;181;0;165;0
WireConnection;181;1;177;0
WireConnection;177;0;225;0
WireConnection;177;2;178;0
WireConnection;241;0;237;0
WireConnection;191;0;185;0
WireConnection;191;1;190;0
WireConnection;191;2;186;0
WireConnection;191;3;183;0
WireConnection;245;0;241;0
WireConnection;186;0;184;3
WireConnection;240;0;242;0
WireConnection;240;1;211;0
WireConnection;237;0;243;0
WireConnection;237;1;243;1
WireConnection;237;2;243;2
WireConnection;237;3;243;3
WireConnection;179;5;181;0
WireConnection;179;7;182;0
WireConnection;247;5;211;0
WireConnection;247;7;244;0
WireConnection;249;1;188;0
WireConnection;249;0;248;0
WireConnection;243;0;240;0
WireConnection;115;0;191;0
WireConnection;115;1;191;0
WireConnection;115;2;249;0
ASEEND*/
//CHKSM=D8E04BE0A046B0C8394D00D20CE449B266318884