// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CScape/CSStreetShader"
{
	Properties
	{
		_Normal1("Normal 1", 2D) = "bump" {}
		_Specular("Specular", 2D) = "white" {}
		_Smoothness("Smoothness", 2D) = "white" {}
		_Puddles("Puddles", Float) = 0
		_TextureSample6("Texture Sample 6", 2D) = "white" {}
		_Float3("Float 3", Float) = 1
		_Float2("Float 2", Float) = 0
		_Float6("Float 6", Float) = 0
		_grate("grate", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_GrateFrequency("GrateFrequency", Float) = 0
		_faloff("faloff", Float) = 0
		_grate1("grate 1", 2D) = "bump" {}
		_GrateSpecular("GrateSpecular", Float) = 0
		_NormalStrenght("NormalStrenght", Float) = 0
		_Mettalic("Mettalic", Float) = 0
		_Float7("Float 7", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_StreetDecalls("StreetDecalls", 2DArray ) = "" {}
		_Float9("Float 9", Float) = 0
		_AlbedoCol("AlbedoCol", Color) = (0,0,0,0)
		_StreetsArray("StreetsArray", 2DArray ) = "" {}
		_TireShineless("TireShineless", Float) = 0
		_ScaleNoise2("ScaleNoise2", Float) = 0
		_ScaleNoise1("ScaleNoise1", Float) = 0
		_lightsContour("lightsContour", Float) = 0.1
		_LightsDistance("LightsDistance", Float) = 0.1
		_ReLightTreshold("ReLightTreshold", Range( 0 , 1)) = 0.32
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord4( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.5
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
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv4_texcoord4;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _NormalStrenght;
		uniform sampler2D _Normal1;
		uniform float _Puddles;
		uniform sampler2D _TextureSample6;
		uniform float _ScaleNoise2;
		uniform float _Float2;
		uniform sampler2D _Smoothness;
		uniform float _ScaleNoise1;
		uniform sampler2D _grate1;
		uniform float _faloff;
		uniform sampler2D _Noise;
		uniform float _GrateFrequency;
		uniform sampler2D _grate;
		uniform UNITY_DECLARE_TEX2DARRAY( _StreetsArray );
		uniform UNITY_DECLARE_TEX2DARRAY( _StreetDecalls );
		uniform float4 _StreetDecalls_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _AlbedoCol;
		uniform float _CSReLight;
		uniform float _ReLightTreshold;
		uniform float4 _reLightColor;
		uniform float _LightsDistance;
		uniform float _lightsContour;
		uniform float _CSReLightDistance;
		uniform float _Float3;
		uniform float _Float7;
		uniform sampler2D _Specular;
		uniform float _Float6;
		uniform float _GrateSpecular;
		uniform float _Mettalic;
		uniform float _Float9;
		uniform float _TireShineless;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 appendResult427 = (float2(ase_worldPos.y , ase_worldPos.z));
			float2 appendResult423 = (float2(ase_worldPos.y , ase_worldPos.x));
			float2 appendResult376 = (float2(ase_worldPos.x , ase_worldPos.z));
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float temp_output_415_0 = abs( ase_vertexNormal.z );
			float2 lerpResult422 = lerp( appendResult423 , appendResult376 , step( temp_output_415_0 , 0.5 ));
			float2 lerpResult426 = lerp( appendResult427 , lerpResult422 , step( abs( ase_vertexNormal.x ) , 0.5 ));
			float2 worldUV414 = lerpResult426;
			float3 tex2DNode318 = UnpackScaleNormal( tex2D( _Normal1, worldUV414 ) ,_NormalStrenght );
			float4 tex2DNode337 = tex2D( _TextureSample6, ( worldUV414 * _ScaleNoise2 ) );
			float smoothstepResult327 = smoothstep( _Puddles , ( _Puddles + 0.7 ) , ( ( tex2DNode337.r * _Float2 ) + tex2D( _Smoothness, ( worldUV414 * _ScaleNoise1 ) ).r ));
			float3 lerpResult331 = lerp( float3(0,0,1) , tex2DNode318 , smoothstepResult327);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 appendResult370 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 temp_output_373_0 = ( appendResult370 * float2( 1,1 ) );
			float smoothstepResult358 = smoothstep( _faloff , ( _faloff + 0.01 ) , ( tex2D( _Noise, ( temp_output_373_0 / float2( 64,64 ) ) ).r * _GrateFrequency ));
			float4 tex2DNode353 = tex2D( _grate, temp_output_373_0 );
			float temp_output_367_0 = ( smoothstepResult358 * tex2DNode353.a );
			float3 lerpResult362 = lerp( lerpResult331 , UnpackNormal( tex2D( _grate1, temp_output_373_0 ) ) , temp_output_367_0);
			o.Normal = lerpResult362;
			float4 texArray400 = UNITY_SAMPLE_TEX2DARRAY(_StreetsArray, float3(worldUV414, i.uv4_texcoord4.x)  );
			float2 uv_StreetDecalls = i.uv_texcoord * _StreetDecalls_ST.xy + _StreetDecalls_ST.zw;
			float4 texArray384 = UNITY_SAMPLE_TEX2DARRAY(_StreetDecalls, float3(uv_StreetDecalls, ( i.vertexColor.r * 10.0 ))  );
			float4 temp_cast_0 = (texArray384.x).xxxx;
			float4 lerpResult352 = lerp( ( texArray400 * smoothstepResult327 ) , temp_cast_0 , texArray384.x);
			float4 lerpResult357 = lerp( lerpResult352 , tex2DNode353 , temp_output_367_0);
			float4 lerpResult382 = lerp( lerpResult357 , tex2D( _TextureSample0, worldUV414 ) , i.vertexColor.g);
			float4 temp_output_398_0 = ( lerpResult382 * _AlbedoCol );
			o.Albedo = temp_output_398_0.rgb;
			float2 appendResult450 = (float2(frac( ( ase_worldPos.x * _LightsDistance ) ) , frac( ( ase_worldPos.z * _LightsDistance ) )));
			float clampResult470 = clamp( ( distance( ase_worldPos , _WorldSpaceCameraPos ) * _CSReLightDistance ) , 0.0 , 1.0 );
			float4 ifLocalVar464 = 0;
			UNITY_BRANCH 
			if( _CSReLight < _ReLightTreshold )
				ifLocalVar464 = ( ( temp_output_398_0 * _reLightColor * ase_worldNormal.y * ( 1.0 - distance( ( appendResult450 * _lightsContour ) , ( float2( 0.5,0.5 ) * _lightsContour ) ) ) * ( _reLightColor.a * 10.0 ) ) * clampResult470 );
			o.Emission = ifLocalVar464.rgb;
			float clampResult381 = clamp( smoothstepResult327 , 0.1 , 1.0 );
			float lerpResult326 = lerp( _Float3 , smoothstepResult327 , clampResult381);
			float4 temp_cast_3 = (( lerpResult326 + _Float7 )).xxxx;
			float4 lerpResult335 = lerp( temp_cast_3 , ( tex2D( _Specular, worldUV414 ) * _Float6 ) , clampResult381);
			float4 temp_cast_4 = (( tex2DNode353.r * _GrateSpecular )).xxxx;
			float4 lerpResult363 = lerp( lerpResult335 , temp_cast_4 , temp_output_367_0);
			float4 clampResult395 = clamp( lerpResult363 , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Metallic = ( clampResult395 * _Mettalic ).r;
			o.Smoothness = ( ( lerpResult363 * _Float9 ) + ( texArray384.y * _TireShineless * temp_output_398_0 ) ).r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma only_renderers d3d11 glcore gles3 metal 
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.5
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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				float4 texcoords23 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.texcoords23 = float4( v.texcoord2.xy, v.texcoord3.xy );
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
				surfIN.uv4_texcoord4.xy = IN.texcoords23.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13401
374;291;1143;690;-4815.665;-2198.934;1;True;False
Node;AmplifyShaderEditor.NormalVertexDataNode;417;1838.051,2092.208;Float;False;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;375;1419.291,1495.922;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;415;1735.746,1777.943;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;424;1715.017,1894.96;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StepOpNode;421;1950.157,1899.866;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;423;1779.877,1586.398;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;376;1678.686,1486.139;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.LerpOp;422;1997.956,1520.266;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0,0,0;False;2;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;427;1912.469,1715.265;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.StepOpNode;425;1929.428,2016.883;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;426;2188.847,1653.221;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0,0,0;False;2;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RegisterLocalVarNode;414;2196.628,1477.531;Float;False;worldUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.PosVertexDataNode;369;2713.759,3700.51;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;411;2845.989,1479.731;Float;False;Property;_ScaleNoise2;ScaleNoise2;37;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;410;2908.001,1338.53;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.DynamicAppendNode;370;3030.558,3654.014;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;412;2864.933,1255.691;Float;False;Property;_ScaleNoise1;ScaleNoise1;38;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;2825.161,1088.132;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;337;3104.063,1144.786;Float;True;Property;_TextureSample6;Texture Sample 6;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;342;3628.602,1073.813;Float;False;Property;_Float2;Float 2;10;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;373;3214.359,3801.212;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;325;3005.422,933.4631;Float;True;Property;_Smoothness;Smoothness;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;341;3845.802,1021.012;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;328;3464.001,1527.112;Float;False;Property;_Puddles;Puddles;7;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;372;3298.258,3513.614;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;64,64;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;329;3723.805,1580.812;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.7;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;340;3690.803,1279.713;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;347;3675.506,2566.312;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;360;3479.756,3490.91;Float;False;Property;_faloff;faloff;15;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;356;3408.656,3308.21;Float;False;Property;_GrateFrequency;GrateFrequency;14;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;354;3351.658,3011.811;Float;True;Property;_Noise;Noise;13;0;Assets/CScape/Textures/Noise.psd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexCoordVertexDataNode;403;2009.105,1072.194;Float;False;3;2;0;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;447;5383.348,2961.312;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;462;5416.572,3240.254;Float;False;Property;_LightsDistance;LightsDistance;43;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SmoothstepOpNode;327;3913.603,1408.711;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureArrayNode;400;2414.07,997.0045;Float;True;Property;_StreetsArray;StreetsArray;34;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;396;3847.854,2931.812;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;10.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;355;3670.857,3051.71;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;359;3806.356,3232.21;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.01;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;460;5669.444,3009.551;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;461;5651.866,3139.184;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureArrayNode;384;4024.452,2775.808;Float;True;Property;_StreetDecalls;StreetDecalls;23;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;336;4070.7,2340.313;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;353;3478.058,3685.009;Float;True;Property;_grate;grate;12;0;Assets/CScape/Textures/grate.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SmoothstepOpNode;358;3901.356,3051.71;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.FractNode;449;5844.901,2823.859;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.FractNode;448;5854.411,2717.779;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;381;4027.354,1810.71;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.1;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;330;3553.202,1736.711;Float;False;Property;_Float3;Float 3;9;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;352;4299.955,2566.011;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;456;6021.876,2933.626;Float;False;Property;_lightsContour;lightsContour;42;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;450;6020.785,2697.479;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;367;4282.56,3182.007;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector2Node;458;6287.499,2900.321;Float;False;Constant;_Vector2;Vector 2;42;0;0.5,0.5;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;321;3309.104,2343.011;Float;True;Property;_Specular;Specular;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;326;3898.4,2055.414;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;350;3675.955,2312.408;Float;False;Property;_Float6;Float 6;11;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;380;4001.259,2118.812;Float;False;Property;_Float7;Float 7;20;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;459;6512.328,2848.971;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.LerpOp;357;4796.358,2809.507;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;383;4713.253,1810.509;Float;True;Property;_TextureSample0;Texture Sample 0;22;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;455;6224.597,2688.146;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.WorldSpaceCameraPos;473;6103.038,2484.39;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.DistanceOpNode;472;6372.493,2365.226;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;379;4188.359,2074.612;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;444;5521.817,2346.998;Float;False;Global;_reLightColor;_reLightColor;41;0;0.8676471,0.7320442,0.4402033,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;474;6450.713,2515.111;Float;False;Global;_CSReLightDistance;_CSReLightDistance;45;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;366;4500.659,3054.807;Float;False;Property;_GrateSpecular;GrateSpecular;17;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;382;5226.756,2010.708;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;397;5218.714,1787.749;Float;False;Property;_AlbedoCol;AlbedoCol;33;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;349;3760.8,2202.214;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.DistanceOpNode;451;6703.454,2730.335;Float;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;335;4344.698,2295.615;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;398;5501.296,1966.372;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;374;2494.358,1681.812;Float;False;Property;_NormalStrenght;NormalStrenght;18;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;365;4648.458,2547.607;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;445;5430.16,2197.207;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;454;6442.242,2617.465;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;463;5740.201,2479.74;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;10.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;471;6591.831,2373.939;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.01;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;394;5220.456,2646.79;Float;False;Property;_Float9;Float 9;32;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;443;5921.039,2165.941;Float;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;3;FLOAT;0,0,0,0;False;4;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;407;5183.982,2829.319;Float;False;Property;_TireShineless;TireShineless;36;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;318;2813.003,1643.811;Float;True;Property;_Normal1;Normal 1;0;0;None;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;470;6774.042,2346.321;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;363;4997.356,2467.609;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.Vector3Node;333;3424.403,1855.112;Float;False;Constant;_Vector0;Vector 0;17;0;0,0,1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;393;5354.853,2382.08;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;361;4283.355,3543.71;Float;True;Property;_grate1;grate 1;16;0;Assets/CScape/Textures/grate 1.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;467;5968.244,2064.123;Float;False;Property;_ReLightTreshold;ReLightTreshold;44;0;0.32;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;395;5056.756,2304.212;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;406;5408.436,2689.744;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;466;6322.142,2016.222;Float;False;Global;_CSReLight;_CSReLight;44;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;331;3512.902,2251.912;Float;False;3;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;469;6378.26,2177.981;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;378;4863.66,2212.411;Float;False;Property;_Mettalic;Mettalic;19;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;413;1936.499,1265.714;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;301;2977.879,2359.496;Float;False;Property;_Float12;Float 12;28;0;3.7;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;275;1845.183,3105.596;Float;False;Property;_StreetLightDistances;StreetLightDistances;24;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;295;2370.078,3242.7;Float;False;Property;_StreetIlluminationShape;StreetIlluminationShape;27;0;0.4;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;405;5557.326,2542.627;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;404;2177.966,825.8027;Float;False;Property;_Float10;Float 10;35;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;385;3526.151,2882.209;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;392;3697.854,2964.608;Float;False;Property;_Float8;Float 8;21;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;273;1770.583,2862.994;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;307;1431.302,3374.099;Float;False;Property;_ColorVariationfactor1;Color Variation factor 1;31;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;377;5195.261,2203.711;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.Vector2Node;277;2144.783,3129.796;Float;False;Constant;_Vector1;Vector 1;37;0;0.5,0.5;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;322;3197.9,1639.411;Float;True;Property;_TextureSample3;Texture Sample 3;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FractNode;304;1899.501,3332.099;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;435;5482.845,1763.643;Float;False;Property;_Raindrops;Raindrops;40;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;282;3168.484,2738.495;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;436;5494.545,1884.543;Float;False;Constant;_Float13;Float 13;41;0;0.01;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCIf;440;6117.942,1489.342;Float;False;6;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;0.0;False;5;FLOAT;0.05;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;344;3725.406,1394.812;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.ConditionalIfNode;464;6596.646,2024.523;Float;False;True;5;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT;0.0;False;4;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;431;5636.145,2115.843;Float;False;Constant;_Float11;Float 11;40;0;0.3;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;432;5474.946,1550.443;Float;True;Property;_rainDrops;rainDrops;39;0;Assets/CScape/Textures/rainDrops.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;293;2806.079,2517.598;Float;False;Constant;_Float5;Float 5;38;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;416;1561.034,2085.881;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldToObjectTransfNode;280;2980.783,2782.091;Float;False;1;0;FLOAT4;0,0,0,0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;312;2425.105,2139.502;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SmoothstepOpNode;434;5856.544,1755.843;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;429;6048.924,1868.824;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;305;1708.501,3233.3;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.BlendNormalsNode;323;3164.302,1965.011;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ConditionalIfNode;433;6464.447,1549.142;Float;False;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;303;1833.079,2461.896;Float;False;Property;_Color0;Color 0;29;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;437;5705.146,1819.543;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;2081.885,2957.494;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.OneMinusNode;315;2819.801,2845.012;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;319;3248.601,1675.711;Float;False;Property;_Float0;Float 0;3;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;364;4294.758,3338.708;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;439;6121.846,1688.243;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;390;4022.454,2615.409;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;2.0;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;311;2621.002,3343.703;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;279;3031.885,3067.493;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;343;3276.836,1459.573;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;300;3251.962,2466.14;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;320;4223.002,1421.512;Float;False;Property;_Float1;Float 1;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;309;2080.802,3334.9;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;2.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;314;2504.5,1861.113;Float;False;Constant;_Color2;Color 2;7;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;285;3456.682,2575.496;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;294;2513.878,2963.997;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;310;2373,3347.601;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;399;1680.946,2696.691;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.PowerNode;288;3679.881,2473.301;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;276;2355.283,2931.398;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;430;5772.647,1983.244;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;324;2771.602,1953.711;Float;True;Property;_Normal2;Normal 2;1;0;None;True;0;True;bump;LockedToTexture2D;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;345;3529.4,1353.012;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.4;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;291;3115.782,2083.501;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ClampOpNode;281;3228.981,2584.498;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;386;3646.549,2842.509;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;313;1833.701,2139.504;Float;False;Property;_Color1;Color 1;30;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;452;5781.417,3329.09;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;420;5401.044,2094.303;Float;False;-1;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;408;2673.963,1285.192;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;289;3459.078,2480.395;Float;False;Property;_Float4;Float 4;26;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SmoothstepOpNode;418;2229.779,1895.48;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;334;4608.704,2184.712;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.FractNode;272;2238.083,2898.695;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.ConditionalIfNode;438;5850.747,1458.142;Float;False;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;1.0;False;4;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;283;2885.483,3396.796;Float;False;Property;_Lightsheight;Lightsheight;25;0;0.05;0;0.05;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;362;4930.456,3029.31;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0,0;False;2;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;271;1453.483,2911.897;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;441;6294.842,1728.643;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6962.279,1960.81;Float;False;True;5;Float;ASEMaterialInspector;0;0;Standard;CScape/CSStreetShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;False;True;True;False;True;True;False;False;False;False;False;False;False;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;8;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;415;0;417;3
WireConnection;424;0;417;1
WireConnection;421;0;415;0
WireConnection;423;0;375;2
WireConnection;423;1;375;1
WireConnection;376;0;375;1
WireConnection;376;1;375;3
WireConnection;422;0;423;0
WireConnection;422;1;376;0
WireConnection;422;2;421;0
WireConnection;427;0;375;2
WireConnection;427;1;375;3
WireConnection;425;0;424;0
WireConnection;426;0;427;0
WireConnection;426;1;422;0
WireConnection;426;2;425;0
WireConnection;414;0;426;0
WireConnection;410;0;414;0
WireConnection;410;1;411;0
WireConnection;370;0;369;1
WireConnection;370;1;369;3
WireConnection;409;0;414;0
WireConnection;409;1;412;0
WireConnection;337;1;410;0
WireConnection;373;0;370;0
WireConnection;325;1;409;0
WireConnection;341;0;337;1
WireConnection;341;1;342;0
WireConnection;372;0;373;0
WireConnection;329;0;328;0
WireConnection;340;0;341;0
WireConnection;340;1;325;1
WireConnection;354;1;372;0
WireConnection;327;0;340;0
WireConnection;327;1;328;0
WireConnection;327;2;329;0
WireConnection;400;0;414;0
WireConnection;400;1;403;1
WireConnection;396;0;347;1
WireConnection;355;0;354;1
WireConnection;355;1;356;0
WireConnection;359;0;360;0
WireConnection;460;0;447;1
WireConnection;460;1;462;0
WireConnection;461;0;447;3
WireConnection;461;1;462;0
WireConnection;384;1;396;0
WireConnection;336;0;400;0
WireConnection;336;1;327;0
WireConnection;353;1;373;0
WireConnection;358;0;355;0
WireConnection;358;1;360;0
WireConnection;358;2;359;0
WireConnection;449;0;461;0
WireConnection;448;0;460;0
WireConnection;381;0;327;0
WireConnection;352;0;336;0
WireConnection;352;1;384;1
WireConnection;352;2;384;1
WireConnection;450;0;448;0
WireConnection;450;1;449;0
WireConnection;367;0;358;0
WireConnection;367;1;353;4
WireConnection;321;1;414;0
WireConnection;326;0;330;0
WireConnection;326;1;327;0
WireConnection;326;2;381;0
WireConnection;459;0;458;0
WireConnection;459;1;456;0
WireConnection;357;0;352;0
WireConnection;357;1;353;0
WireConnection;357;2;367;0
WireConnection;383;1;414;0
WireConnection;455;0;450;0
WireConnection;455;1;456;0
WireConnection;472;0;447;0
WireConnection;472;1;473;0
WireConnection;379;0;326;0
WireConnection;379;1;380;0
WireConnection;382;0;357;0
WireConnection;382;1;383;0
WireConnection;382;2;347;2
WireConnection;349;0;321;0
WireConnection;349;1;350;0
WireConnection;451;0;455;0
WireConnection;451;1;459;0
WireConnection;335;0;379;0
WireConnection;335;1;349;0
WireConnection;335;2;381;0
WireConnection;398;0;382;0
WireConnection;398;1;397;0
WireConnection;365;0;353;1
WireConnection;365;1;366;0
WireConnection;454;0;451;0
WireConnection;463;0;444;4
WireConnection;471;0;472;0
WireConnection;471;1;474;0
WireConnection;443;0;398;0
WireConnection;443;1;444;0
WireConnection;443;2;445;2
WireConnection;443;3;454;0
WireConnection;443;4;463;0
WireConnection;318;1;414;0
WireConnection;318;5;374;0
WireConnection;470;0;471;0
WireConnection;363;0;335;0
WireConnection;363;1;365;0
WireConnection;363;2;367;0
WireConnection;393;0;363;0
WireConnection;393;1;394;0
WireConnection;361;1;373;0
WireConnection;395;0;363;0
WireConnection;406;0;384;2
WireConnection;406;1;407;0
WireConnection;406;2;398;0
WireConnection;331;0;333;0
WireConnection;331;1;318;0
WireConnection;331;2;327;0
WireConnection;469;0;443;0
WireConnection;469;1;470;0
WireConnection;405;0;393;0
WireConnection;405;1;406;0
WireConnection;385;0;347;4
WireConnection;385;1;347;1
WireConnection;273;0;271;1
WireConnection;273;1;271;3
WireConnection;377;0;395;0
WireConnection;377;1;378;0
WireConnection;304;0;305;0
WireConnection;282;0;280;2
WireConnection;282;1;283;0
WireConnection;440;0;432;1
WireConnection;440;1;435;0
WireConnection;344;0;345;0
WireConnection;344;1;400;0
WireConnection;464;0;466;0
WireConnection;464;1;467;0
WireConnection;464;4;469;0
WireConnection;280;0;279;0
WireConnection;312;0;303;0
WireConnection;312;1;313;0
WireConnection;312;2;311;0
WireConnection;434;0;432;1
WireConnection;434;1;435;0
WireConnection;434;2;437;0
WireConnection;429;0;398;0
WireConnection;429;1;430;0
WireConnection;429;2;441;0
WireConnection;305;0;271;1
WireConnection;305;1;307;0
WireConnection;323;0;318;0
WireConnection;323;1;324;0
WireConnection;437;0;435;0
WireConnection;437;1;436;0
WireConnection;274;0;273;0
WireConnection;274;1;275;0
WireConnection;315;0;294;0
WireConnection;364;0;353;1
WireConnection;439;0;434;0
WireConnection;439;1;440;0
WireConnection;390;0;347;1
WireConnection;311;0;310;0
WireConnection;343;0;337;1
WireConnection;300;0;291;0
WireConnection;300;1;301;0
WireConnection;309;0;304;0
WireConnection;285;0;281;0
WireConnection;294;0;276;0
WireConnection;294;1;295;0
WireConnection;310;0;309;0
WireConnection;399;0;271;1
WireConnection;399;1;271;3
WireConnection;288;0;285;0
WireConnection;288;1;289;0
WireConnection;276;0;272;0
WireConnection;276;1;277;0
WireConnection;430;0;398;0
WireConnection;430;1;431;0
WireConnection;345;0;343;0
WireConnection;291;0;312;0
WireConnection;291;1;315;0
WireConnection;281;0;282;0
WireConnection;386;0;385;0
WireConnection;418;0;415;0
WireConnection;334;0;344;0
WireConnection;334;1;327;0
WireConnection;272;0;274;0
WireConnection;438;0;432;1
WireConnection;438;1;435;0
WireConnection;362;0;331;0
WireConnection;362;1;361;0
WireConnection;362;2;367;0
WireConnection;441;0;434;0
WireConnection;441;1;440;0
WireConnection;0;0;398;0
WireConnection;0;1;362;0
WireConnection;0;2;464;0
WireConnection;0;3;377;0
WireConnection;0;4;405;0
ASEEND*/
//CHKSM=9F3ABC475EAE5D763BCB8170594B81A7F71F03A9