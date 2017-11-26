// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clouds"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_Texture0("Texture 0", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_PortalNormal("PortalNormal", 2D) = "bump" {}
		_DepthFade("DepthFade", Float) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Fadedistance("Fadedistance", Float) = 0
		_cloudsSpeed("cloudsSpeed", Float) = 0
		_Float1("Float 1", Float) = 0
		_Float4("Float 4", Float) = 0
		_Float3("Float 3", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 texcoord_0;
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldPos;
			float4 screenPos;
		};

		struct SurfaceOutputStandardSpecularCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			fixed3 Specular;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			fixed3 Translucency;
		};

		uniform sampler2D _PortalNormal;
		uniform float _cloudsSpeed;
		uniform sampler2D _Texture0;
		uniform float _Float0;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _Float1;
		uniform float4 _Texture0_ST;
		uniform float _Float4;
		uniform float _Float3;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform sampler2D _CameraDepthTexture;
		uniform float _DepthFade;
		uniform float _Fadedistance;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( maxSamples, minSamples, dot( normalWorld, viewWorld ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
				currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
				if ( currHeight > currRayZ )
				{
					stepIndex = numSteps + 1;
				}
				else
				{
					stepIndex++;
					prevTexOffset = currTexOffset;
					prevRayZ = currRayZ;
					prevHeight = currHeight;
					currTexOffset += deltaTex;
					currRayZ -= layerHeight;
				}
			}
			int sectionSteps = 5;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
				intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
				finalTexOffset = prevTexOffset + intersection * deltaTex;
				newZ = prevRayZ - intersection * layerHeight;
				newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
				if ( newHeight > newZ )
				{
					currTexOffset = finalTexOffset;
					currHeight = newHeight;
					currRayZ = newZ;
					deltaTex = intersection * deltaTex;
					layerHeight = intersection * layerHeight;
				}
				else
				{
					prevTexOffset = finalTexOffset;
					prevHeight = newHeight;
					prevRayZ = newZ;
					deltaTex = ( 1 - intersection ) * deltaTex;
					layerHeight = ( 1 - intersection ) * layerHeight;
				}
				sectionIndex++;
			}
			return uvs + finalTexOffset;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		inline half4 LightingStandardSpecularCustom(SurfaceOutputStandardSpecularCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandardSpecular r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Specular = s.Specular;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandardSpecular (r, viewDir, gi) + c;
		}

		inline void LightingStandardSpecularCustom_GI(SurfaceOutputStandardSpecularCustom s, UnityGIInput data, inout UnityGI gi )
		{
			UNITY_GI(gi, s, data);
		}

		void surf( Input i , inout SurfaceOutputStandardSpecularCustom o )
		{
			float mulTime53 = _Time.y * ( _cloudsSpeed * 0.01 );
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 temp_output_28_0 = ( tex2D( _TextureSample2, uv_TextureSample2 ) * _Float1 );
			float4 temp_output_29_0 = ( _Float0 + temp_output_28_0 );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float2 OffsetPOM1 = POM( _Texture0, ( i.texcoord_0 + mulTime53 ), ddx(( i.texcoord_0 + mulTime53 )), ddx(( i.texcoord_0 + mulTime53 )), ase_worldNormal, worldViewDir, i.viewDir, 80, 80, temp_output_29_0.x, 0, _Texture0_ST.xy, float2(0,0), 0.0 );
			float3 tex2DNode8 = UnpackNormal( tex2D( _PortalNormal, OffsetPOM1 ) );
			o.Normal = tex2DNode8;
			float4 tex2DNode3 = tex2D( _Texture0, OffsetPOM1, ddx( i.texcoord_0 ), ddy( i.texcoord_0 ) );
			float3 temp_cast_2 = (tex2DNode3.r).xxx;
			o.Albedo = temp_cast_2;
			o.Emission = ( unity_FogColor * tex2DNode3.r ).rgb;
			float3 temp_cast_4 = (_Float4).xxx;
			o.Specular = temp_cast_4;
			o.Smoothness = _Float3;
			float3 temp_cast_5 = (tex2DNode3.r).xxx;
			o.Translucency = temp_cast_5;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float screenDepth30 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth30 = abs( ( screenDepth30 - LinearEyeDepth( ase_screenPos.z/ ase_screenPos.w ) ) / ( _DepthFade ) );
			float clampResult36 = clamp( distanceDepth30 , 0.0 , 1.0 );
			float3 ase_worldPos = i.worldPos;
			float clampResult40 = clamp( ( distance( ase_worldPos , _WorldSpaceCameraPos ) * _Fadedistance ) , 0.0 , 1.0 );
			o.Alpha = ( tex2DNode3.r * clampResult36 * ( 1.0 - clampResult40 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecularCustom alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecularCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecularCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13105
231;284;1423;689;2936.1;374.6816;2.130193;True;True
Node;AmplifyShaderEditor.RangedFloatNode;55;-1537.005,-260.3518;Float;False;Property;_cloudsSpeed;cloudsSpeed;17;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;50;337.1747,793.1516;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceCameraPos;37;462.1352,1203.959;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1417.872,-143.2194;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.01;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-1322.803,480.8435;Float;False;Property;_Float1;Float 1;18;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;27;-1674.176,594.9432;Float;True;Property;_TextureSample2;Texture Sample 2;14;0;Assets/AmplifyShaderEditor/Examples/Assets/Textures/Misc/SimpleFoam.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;39;706.1577,811.2537;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;985.7744,1056.428;Float;False;Property;_Fadedistance;Fadedistance;15;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-1234.8,167.6046;Float;False;Property;_Float0;Float 0;8;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1513.012,245.3539;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleTimeNode;53;-1237.193,-121.2569;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1870.984,-455.5067;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;4;-985.2495,646.5851;Float;False;Tangent;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1107.381,820.444;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1151.187,-645.533;Float;True;Property;_Texture0;Texture 0;7;0;None;False;white;LockedToTexture2D;0;1;SAMPLER2D
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-1035.362,-95.6344;Float;False;2;2;0;FLOAT2;0.0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;35;377.6801,681.4106;Float;False;Property;_DepthFade;DepthFade;13;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-1000.468,188.0822;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;1;-710.6988,-44.56414;Float;False;0;80;80;5;0.02;0;False;1,1;False;0,0;False;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT2;0,0;False;6;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DepthFade;30;567.7577,658.6979;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DdyOpNode;10;-659.9597,-142.3795;Float;False;1;0;FLOAT2;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ClampOpNode;40;1080.851,651.3776;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DdxOpNode;9;-631.9178,-242.5288;Float;False;1;0;FLOAT2;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ClampOpNode;36;755.5048,628.2393;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-48.74212,-58.23717;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Derivative;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;25;627.3137,-137.5408;Float;False;unity_FogColor;0;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;52;1289.816,571.1776;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;23;-1997.814,2.053297;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;8;-256.0307,230.8145;Float;True;Property;_PortalNormal;PortalNormal;9;0;Assets/AmplifyShaderEditor/Examples/Assets/Textures/Misc/PortalNormal.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;474.5232,92.11123;Float;False;Property;_Float4;Float 4;20;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;38;980.8974,2499.002;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;558.4214,988.1583;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;17;405.3503,370.7871;Float;False;Property;_Float2;Float 2;19;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;58;-2156.668,504.2483;Float;True;Property;_TextureSample3;Texture Sample 3;12;0;Assets/AmplifyShaderEditor/Examples/Assets/Textures/Misc/SimpleFoam.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ParallaxMappingNode;14;-1428.911,-26.97075;Float;False;Planar;4;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.01;False;3;FLOAT3;0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;15;-2132.81,196.1511;Float;True;Property;_SimpleFoam;SimpleFoam;11;0;Assets/AmplifyShaderEditor/Examples/Assets/Textures/Misc/SimpleFoam.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;43;326.6408,982.9611;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;24;121.7092,208.6317;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RotatorNode;20;-1775.216,-226.4594;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;100.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-861.4933,312.4988;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.OneMinusNode;7;448.4956,-137.9341;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;759.0203,405.8185;Float;False;3;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;18;391.3232,12.11132;Float;False;Property;_Float3;Float 3;21;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;13;259.4001,-328.6568;Float;False;Property;_Color0;Color 0;10;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-973.1364,45.2903;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;12;-80.9558,477.2873;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;11;116.4501,522.5756;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;663.2946,244.4911;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;62;-1714.403,121.6774;Float;False;0;80;80;5;0.02;0;False;1,1;False;0,0;False;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT2;0,0;False;6;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;61;-1152.326,80.06393;Float;False;Property;_Float5;Float 5;16;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1035.954,93.45665;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;Clouds;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;9.5;24.86;True;0.34;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;0;-1;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;0;55;0
WireConnection;39;0;50;0
WireConnection;39;1;37;0
WireConnection;28;0;27;0
WireConnection;28;1;16;0
WireConnection;53;0;57;0
WireConnection;41;0;39;0
WireConnection;41;1;42;0
WireConnection;56;0;6;0
WireConnection;56;1;53;0
WireConnection;29;0;5;0
WireConnection;29;1;28;0
WireConnection;1;0;56;0
WireConnection;1;1;2;0
WireConnection;1;2;29;0
WireConnection;1;3;4;0
WireConnection;30;0;35;0
WireConnection;10;0;6;0
WireConnection;40;0;41;0
WireConnection;9;0;6;0
WireConnection;36;0;30;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;3;3;9;0
WireConnection;3;4;10;0
WireConnection;52;0;40;0
WireConnection;8;1;1;0
WireConnection;14;0;6;0
WireConnection;14;1;15;1
WireConnection;14;2;28;0
WireConnection;14;3;4;0
WireConnection;24;0;3;1
WireConnection;20;0;6;0
WireConnection;20;2;23;1
WireConnection;59;0;58;1
WireConnection;59;1;29;0
WireConnection;7;0;3;1
WireConnection;34;0;3;1
WireConnection;34;1;36;0
WireConnection;34;2;52;0
WireConnection;60;0;58;1
WireConnection;60;1;61;0
WireConnection;12;0;8;0
WireConnection;11;0;12;0
WireConnection;26;0;25;0
WireConnection;26;1;3;1
WireConnection;62;3;4;0
WireConnection;0;0;3;1
WireConnection;0;1;8;0
WireConnection;0;2;26;0
WireConnection;0;3;19;0
WireConnection;0;4;18;0
WireConnection;0;7;3;1
WireConnection;0;9;34;0
ASEEND*/
//CHKSM=095B8A491F6E15C0CC1BE57E2B75DE3C3608C1BC