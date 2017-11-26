// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CScape/CSRooftops"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_diffuse("diffuse", 2D) = "white" {}
		_normal("normal", 2D) = "bump" {}
		_IlluminationStrenght("IlluminationStrenght", Float) = 0
		_Float1("Float 1", Float) = 0
		_Float0("Float 0", Float) = 0
		_Float6("Float 6", Float) = 0.1
		_Float10("Float 10", Range( 0 , 1)) = 0.32
		_Float4("Float 4", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.5
		#pragma only_renderers d3d11 glcore gles3 metal 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
		};

		uniform sampler2D _normal;
		uniform float4 _normal_ST;
		uniform sampler2D _diffuse;
		uniform float4 _diffuse_ST;
		uniform float _CSReLight;
		uniform float _Float10;
		uniform float _IlluminationStrenght;
		uniform float _Float4;
		uniform float _Float6;
		uniform float4 _reLightColor;
		uniform float _CSReLightDistance;
		uniform float _Float0;
		uniform float _Float1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_normal = i.uv_texcoord * _normal_ST.xy + _normal_ST.zw;
			float3 tex2DNode2 = UnpackNormal( tex2D( _normal, uv_normal ) );
			o.Normal = tex2DNode2;
			float2 uv_diffuse = i.uv_texcoord * _diffuse_ST.xy + _diffuse_ST.zw;
			float4 tex2DNode1 = tex2D( _diffuse, uv_diffuse );
			float4 temp_output_24_0 = ( tex2DNode1 * 0.5 );
			o.Albedo = temp_output_24_0.rgb;
			float4 temp_output_4_0 = ( ( 1.0 - i.vertexColor.r ) * ( float4(1,0,0,0) * _IlluminationStrenght ) );
			float3 ase_worldPos = i.worldPos;
			float clampResult32 = clamp( ( ( ase_worldPos.y * 0.6 ) * _Float4 ) , 0.0 , 1.0 );
			float3 appendResult36 = (float3(frac( ( ase_worldPos.x * _Float4 ) ) , frac( ( ase_worldPos.z * _Float4 ) ) , clampResult32));
			float componentMask57 = ( temp_output_24_0 * ( tex2DNode2.y + tex2DNode2.z ) * ( 1.0 - distance( ( appendResult36 * _Float6 ) , float3( ( float2( 0.5,0.5 ) * _Float6 ) ,  0.0 ) ) ) ).r;
			float clampResult58 = clamp( componentMask57 , 0.0 , 1.0 );
			float clampResult48 = clamp( ( distance( ase_worldPos , _WorldSpaceCameraPos ) * _CSReLightDistance ) , 0.0 , 1.0 );
			float4 ifLocalVar53 = 0;
			UNITY_BRANCH if( _CSReLight > _Float10 )
				ifLocalVar53 = temp_output_4_0;
			else UNITY_BRANCH if( _CSReLight < _Float10 )
				ifLocalVar53 = ( temp_output_4_0 + ( ( pow( clampResult58 , 1.5 ) * ( _reLightColor.a * 10.0 ) ) * _reLightColor * clampResult48 ) );
			o.Emission = ifLocalVar53.rgb;
			o.Metallic = _Float0;
			o.Smoothness = _Float1;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13105
353;464;1423;432;2020.6;-484.989;2.41251;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;26;-1000.478,1624.198;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-619.9309,1912.975;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.6;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;28;-921.3585,1870.08;Float;False;Property;_Float4;Float 4;7;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-382.9889,1651.427;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-343.4489,1929.359;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-347.6935,1797.479;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;32;-111.2458,1949.581;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.FractNode;33;-176.3209,1526.147;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.FractNode;34;-157.9625,1639.828;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;37;79.93488,1769.481;Float;False;Property;_Float6;Float 6;5;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;36;132.7357,1903.855;Float;False;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.Vector2Node;35;57.346,1607.293;Float;False;Constant;_Vector1;Vector 1;42;0;0.5,0.5;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;338.0814,1604.988;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;363.1517,1816.428;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;2;-640.9995,-36.49993;Float;True;Property;_normal;normal;1;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;25;-188.814,-106.17;Float;False;Constant;_Float2;Float 2;5;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;40;589.6712,1802.026;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0.5,0.5,0;False;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;54;-941.7598,1095.587;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-634.7,-252.8;Float;True;Property;_diffuse;diffuse;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;41;840.3099,1720.979;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;98.48602,-212.7701;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-663.467,1090.332;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-506.7203,1076.013;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WorldSpaceCameraPos;42;-96.11493,1429.503;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.ComponentMaskNode;57;-351.6847,1133.542;Float;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;45;235.1302,1362.864;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;43;280.1873,1547.777;Float;False;Global;_CSReLightDistance;_CSReLightDistance;48;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;58;-116.8346,1110.13;Float;False;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;44;-638.7869,1296.207;Float;False;Global;_reLightColor;_reLightColor;5;0;0.8676471,0.7320442,0.4402033,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;9;-969.9001,440.1001;Float;False;Constant;_Color0;Color 0;3;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-283.2028,1370.288;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;10.0;False;1;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;3;-718.8996,254.7002;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-538.8001,681.0996;Float;False;Property;_IlluminationStrenght;IlluminationStrenght;2;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;458.3949,1373.386;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.01;False;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;59;66.79913,1121.414;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;1.5;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;48;738.7051,1419.598;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;259.0521,1139.871;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-477.1003,393.6999;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;6;-479.6997,238.2;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-248.3002,254.7001;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;934.274,1326.386;Float;False;3;3;0;FLOAT;0.0;False;1;COLOR;0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;52;1167.115,1204.133;Float;False;Global;_CSReLight;_CSReLight;45;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;50;1133.332,1294.275;Float;False;Property;_Float10;Float 10;6;0;0.32;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;1184.891,1393.621;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.PosFromTransformMatrix;22;-1166.801,-436.7995;Float;False;1;0;FLOAT4x4;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.PosFromTransformMatrix;14;-902.9998,-412.0997;Float;False;1;0;FLOAT4x4;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ConditionalIfNode;53;1493.643,1249.85;Float;False;True;5;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0.0;False;4;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-233.4004,-306.7998;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.1234;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-245.9001,13.00006;Float;False;Property;_Float0;Float 0;4;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;11;-720.4001,547.3002;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-368.0002,-392.5998;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SinTimeNode;7;-937.3999,676.4986;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TransformVariables;20;-1182.401,-269.0995;Float;False;_Object2World;0;1;FLOAT4x4
Node;AmplifyShaderEditor.WorldToObjectMatrix;21;-1055.001,-135.1996;Float;False;0;1;FLOAT4x4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;104.4997,-322.3998;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.MMatrixNode;23;-1139.401,-326.2995;Float;False;0;1;FLOAT4x4
Node;AmplifyShaderEditor.FractNode;18;-104.7004,-378.2997;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-243.5001,101.2001;Float;False;Property;_Float1;Float 1;3;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;16;-644.9002,-405.5996;Float;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;699.5999,-172.7;Float;False;True;5;Float;ASEMaterialInspector;0;0;Standard;CScape/CSRooftops;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;False;True;True;False;True;True;False;False;False;False;False;False;False;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;26;2
WireConnection;29;0;26;1
WireConnection;29;1;28;0
WireConnection;30;0;27;0
WireConnection;30;1;28;0
WireConnection;31;0;26;3
WireConnection;31;1;28;0
WireConnection;32;0;30;0
WireConnection;33;0;29;0
WireConnection;34;0;31;0
WireConnection;36;0;33;0
WireConnection;36;1;34;0
WireConnection;36;2;32;0
WireConnection;39;0;35;0
WireConnection;39;1;37;0
WireConnection;38;0;36;0
WireConnection;38;1;37;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;54;0;2;0
WireConnection;41;0;40;0
WireConnection;24;0;1;0
WireConnection;24;1;25;0
WireConnection;55;0;54;1
WireConnection;55;1;54;2
WireConnection;56;0;24;0
WireConnection;56;1;55;0
WireConnection;56;2;41;0
WireConnection;57;0;56;0
WireConnection;45;0;26;0
WireConnection;45;1;42;0
WireConnection;58;0;57;0
WireConnection;47;0;44;4
WireConnection;46;0;45;0
WireConnection;46;1;43;0
WireConnection;59;0;58;0
WireConnection;48;0;46;0
WireConnection;60;0;59;0
WireConnection;60;1;47;0
WireConnection;8;0;9;0
WireConnection;8;1;10;0
WireConnection;6;0;3;1
WireConnection;4;0;6;0
WireConnection;4;1;8;0
WireConnection;49;0;60;0
WireConnection;49;1;44;0
WireConnection;49;2;48;0
WireConnection;51;0;4;0
WireConnection;51;1;49;0
WireConnection;14;0;23;0
WireConnection;53;0;52;0
WireConnection;53;1;50;0
WireConnection;53;2;4;0
WireConnection;53;4;51;0
WireConnection;19;0;17;0
WireConnection;11;0;7;3
WireConnection;17;0;16;0
WireConnection;17;1;16;2
WireConnection;15;0;18;0
WireConnection;15;1;1;0
WireConnection;18;0;19;0
WireConnection;16;0;14;0
WireConnection;0;0;24;0
WireConnection;0;1;2;0
WireConnection;0;2;53;0
WireConnection;0;3;12;0
WireConnection;0;4;13;0
ASEEND*/
//CHKSM=B267CF94FB3073DF9A43AFF6ED3027FB7B8BA913