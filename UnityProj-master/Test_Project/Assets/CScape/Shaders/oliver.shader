// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "oliver"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureArray0("Texture Array 0", 2DArray ) = "" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.5
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform UNITY_DECLARE_TEX2DARRAY( _TextureArray0 );
		uniform float4 _TextureArray0_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureArray0 = i.uv_texcoord * _TextureArray0_ST.xy + _TextureArray0_ST.zw;
			o.Albedo = UNITY_SAMPLE_TEX2DARRAY_LOD(_TextureArray0, float3(uv_TextureArray0, 0.0) , 2.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
89;228;1715;780;1393.5;254.8001;1.3;True;True
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;5;Float;ASEMaterialInspector;Standard;oliver;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;2;-774.7001,280.7999;Float;False;Constant;_Float0;Float 0;1;0;2;0;0
Node;AmplifyShaderEditor.TextureArrayNode;1;-482.9005,41.59987;Float;False;Property;_TextureArray0;Texture Array 0;0;0;None;0;Object;-1;MipLevel;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False
WireConnection;0;0;1;0
WireConnection;1;2;2;0
ASEEND*/
//CHKSM=BAFD19E26E3C9B61B9F9885F1D8999F6873019B5