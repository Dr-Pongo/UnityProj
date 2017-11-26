// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CScape/CSAdvertising"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MainTex("MainTex", 2D) = "white" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Specular("Specular", Range( 0 , 1)) = 0
		_Emission("Emission", Range( 0 , 6)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 4.5
		#pragma only_renderers d3d11 glcore gles3 metal 
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Emission;
		uniform float _Specular;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = tex2DNode2.xyz;
			o.Emission = ( tex2DNode2 * _Emission ).xyz;
			float3 temp_cast_2 = (_Specular).xxx;
			o.Specular = temp_cast_2;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13105
220;281;1297;787;956.2599;310.7586;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;1;-438,-180.5;Float;True;Property;_MainTex;MainTex;0;0;None;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.SamplerNode;2;-169,-179.5;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;49.94005,-3.958624;Float;False;Property;_Emission;Emission;3;0;0;0;6;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;4;-150.2599,167.6414;Float;False;Property;_Smoothness;Smoothness;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;146.14,-102.7586;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;3;-145.06,57.14138;Float;False;Property;_Specular;Specular;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;321.3999,-200.9;Float;False;True;5;Float;ASEMaterialInspector;0;0;StandardSpecular;CScape/CSAdvertising;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;False;True;True;False;True;True;False;False;False;False;False;False;False;True;True;True;True;False;1;255;255;2;3;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;6;0;2;0
WireConnection;6;1;5;0
WireConnection;0;0;2;0
WireConnection;0;2;6;0
WireConnection;0;3;3;0
WireConnection;0;4;4;0
ASEEND*/
//CHKSM=B2A7B48BFEC8041EA871CE3D580DDB71EEBF6D4A