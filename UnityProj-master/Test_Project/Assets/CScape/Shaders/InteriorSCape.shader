// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "InteriorSCape"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Float1("Float 1", Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _TextureSample1;
		uniform float _Float0;
		uniform float _Float1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 x = frac( ( i.texcoord_0 * float2( -1,-1 ) ) );
			float3 a = (float3(x * 2 - 1, -1));
			float3 b = ( float3( 1,1,1 ) / i.viewDir );
			float3 c = (abs(b) - a * b);
			float3 temp_output_27_0 = ( (float3(x * 2 - 1, -1)) + ( (min(min(c.x, c.y), c.z)) * i.viewDir ) );
			float3 pos = temp_output_27_0;
			float depthScale = 1.0;
			float interp1 = (pos.z * 0.5 + 0.5);
			float realZ = (saturate(interp1) / depthScale + 1);
			float interp2 = ((1.0 - (1.0 / realZ)) * (depthScale +1.0));
			float farFrac = _Float1;
			float2 interiorUV = (pos.xy * lerp(1.0, farFrac, interp2));
			o.Emission = tex2Dbias( _TextureSample1,float4( (interiorUV * -0.5 - 0.5), 0, _Float0)).xyz;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 )
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
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
Version=5105
247;425;1176;569;6722.491;3299.556;12.07921;True;True
Node;AmplifyShaderEditor.CommentaryNode;147;-3002.094,-231.4368;Float;False;6082.19;1413.166;Comment;19;50;122;145;142;144;141;140;132;138;137;136;18;135;33;134;2;17;32;1;
Node;AmplifyShaderEditor.CommentaryNode;134;-1810.03,208.8077;Float;False;350.9301;215.5199;float3 pos = float3(roomUV * 2 - 1, -1) ;1;24;Float3 pos
Node;AmplifyShaderEditor.CommentaryNode;135;-1827.829,651.9077;Float;False;262.45;262.4499;float3 k = abs(id) - pos * id ;1;19;Float3 K
Node;AmplifyShaderEditor.CommentaryNode;136;-1509.13,718.9074;Float;False;270.6901;140.56;Comment;1;22;float kMin = min(min(k.x, k.y), k.z) 
Node;AmplifyShaderEditor.CommentaryNode;137;-1251.733,373.107;Float;False;336.7304;187.8801;Comment;2;28;27;pos += kMin * i.tangentViewDir 
Node;AmplifyShaderEditor.CommentaryNode;138;-720.5356,429.907;Float;False;268.84;127.79;Comment;1;139;float interp = pos.z * 0.5 + 0.5 
Node;AmplifyShaderEditor.CommentaryNode;140;-308.3364,496.6064;Float;False;381.25;162.5;Comment;1;130;float realZ = saturate(interp) / depthScale + 1 
Node;AmplifyShaderEditor.CommentaryNode;141;228.0635,604.0057;Float;False;429.2799;148.4;Comment;1;131;interp = 1.0 - (1.0 / realZ) 
Node;AmplifyShaderEditor.CommentaryNode;142;813.0641,690.4064;Float;False;497.1002;211.9099;Comment;1;143;float2 interiorUV = pos.xy * lerp(1.0, farFrac, interp) 
Node;AmplifyShaderEditor.CommentaryNode;145;1344.562,884.0056;Float;False;406.85;146.9299;Comment;1;146;interiorUV = interiorUV * 0.5 + 0.5 
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3233.648,1813.791;Float;False;True;2;Float;ASEMaterialInspector;Standard;InteriorSCape;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-2449.441,381.2695;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-2166.549,396.8932;Float;False;0;FLOAT2;0.0;False;1;FLOAT2;-1,-1;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;17;-2542.14,659.3924;Float;False;Tangent
Node;AmplifyShaderEditor.FractNode;2;-1998.658,400.9932;Float;False;0;FLOAT2;0.0;False
Node;AmplifyShaderEditor.RelayNode;33;-2192.348,678.3932;Float;False;0;FLOAT3;0,0,0,0;False
Node;AmplifyShaderEditor.CustomExpressionNode;24;-1784.043,322.5929;Float;False;float3(x * 2 - 1, -1);3;1;True;x;FLOAT2;0,0;0;FLOAT2;0,0;False
Node;AmplifyShaderEditor.SimpleDivideOpNode;18;-2003.547,723.2924;Float;False;0;FLOAT3;1,1,1;False;1;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.CustomExpressionNode;19;-1777.846,763.5924;Float;False;abs(b) - a * b;3;2;True;a;FLOAT3;0,0,0;True;b;FLOAT3;0,0,0;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.CustomExpressionNode;22;-1465.544,749.6918;Float;False;min(min(c.x, c.y), c.z);1;1;True;c;FLOAT3;0,0,0;0;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1225.948,460.093;Float;False;0;FLOAT;0.0;False;1;FLOAT3;0.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1053.646,437.693;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;132;-604.6301,812.807;Float;False;Constant;_depthScale;depthScale;2;0;1;0;0
Node;AmplifyShaderEditor.CustomExpressionNode;139;-637.0351,477.4068;Float;False;pos.z * 0.5 + 0.5;1;1;True;pos;FLOAT3;0,0,0;0;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.CustomExpressionNode;130;-256.3322,536.307;Float;False;saturate(interp1) / depthScale + 1;1;2;True;depthScale;FLOAT;0.0;True;interp1;FLOAT;0.0;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;144;492.862,959.1061;Float;False;Property;_Float1;Float 1;3;0;1;0;0
Node;AmplifyShaderEditor.CustomExpressionNode;131;341.7696,648.4087;Float;False;(1.0 - (1.0 / realZ)) * (depthScale +1.0);1;2;True;depthScale;FLOAT;0.0;True;realZ;FLOAT;0.0;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.CustomExpressionNode;143;944.1642,758.8066;Float;False;pos.xy * lerp(1.0, farFrac, interp2);2;3;True;pos;FLOAT3;0,0,0;True;interp2;FLOAT;0.0;True;farFrac;FLOAT;0.0;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.CustomExpressionNode;146;1418.762,943.105;Float;False;interiorUV * -0.5 - 0.5;2;1;True;interiorUV;FLOAT2;0,0;0;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;122;2012.764,892.9095;Float;False;Property;_Float0;Float 0;2;0;0;0;0
Node;AmplifyShaderEditor.SamplerNode;50;2211.026,576.3103;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;None;True;0;False;white;LockedToTexture2D;False;Object;-1;MipBias;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
WireConnection;0;2;50;0
WireConnection;32;0;1;0
WireConnection;2;0;32;0
WireConnection;33;0;17;0
WireConnection;24;0;2;0
WireConnection;18;1;33;0
WireConnection;19;0;24;0
WireConnection;19;1;18;0
WireConnection;22;0;19;0
WireConnection;28;0;22;0
WireConnection;28;1;33;0
WireConnection;27;0;24;0
WireConnection;27;1;28;0
WireConnection;139;0;27;0
WireConnection;130;0;132;0
WireConnection;130;1;139;0
WireConnection;131;0;132;0
WireConnection;131;1;130;0
WireConnection;143;0;27;0
WireConnection;143;1;131;0
WireConnection;143;2;144;0
WireConnection;146;0;143;0
WireConnection;50;1;146;0
WireConnection;50;2;122;0
ASEEND*/
//CHKSM=E81A09E5B288EF1154AC6734300493BAC3497586