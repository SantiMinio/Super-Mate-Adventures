// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Slowarea"
{
	Properties
	{
		_Color0("Color 0", Color) = (1,0.8720647,0,0)
		_Float1("Float 1", Float) = 0
		_Exp("Exp", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform float4 _Color0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Float1;
		uniform float _Exp;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.11 + 23.26 * pow( 1.0 - fresnelNdotV1, 14.96 ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth18 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth18 = abs( ( screenDepth18 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( pow( _Float1 , _Exp ) ) );
			float temp_output_21_0 = ( 1.0 - distanceDepth18 );
			float lerpResult23 = lerp( fresnelNode1 , temp_output_21_0 , temp_output_21_0);
			float temp_output_25_0 = saturate( lerpResult23 );
			float4 temp_output_28_0 = ( _Color0 * temp_output_25_0 );
			o.Albedo = temp_output_28_0.rgb;
			o.Emission = temp_output_28_0.rgb;
			o.Alpha = temp_output_25_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1984;-70;1015;579;1487.766;-196.4566;1.304828;True;False
Node;AmplifyShaderEditor.RangedFloatNode;19;-743.8068,567.8572;Inherit;True;Property;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;0;1.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-942.7125,520.0542;Inherit;False;Property;_Exp;Exp;2;0;Create;True;0;0;0;False;0;False;0;3.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;33;-545.5971,518.7491;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;18;-366.4925,440.8744;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;-162.8888,347.5971;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-792.8499,272.1792;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0.11;False;2;FLOAT;23.26;False;3;FLOAT;14.96;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;65.99146,225.776;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-235.4096,-30.96004;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;1,0.8720647,0,0;1,0.8720647,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;25;102.3505,75.79977;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;276.5836,-51.61435;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;495.9989,-170.0024;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Slowarea;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;19;0
WireConnection;33;1;34;0
WireConnection;18;0;33;0
WireConnection;21;0;18;0
WireConnection;23;0;1;0
WireConnection;23;1;21;0
WireConnection;23;2;21;0
WireConnection;25;0;23;0
WireConnection;28;0;2;0
WireConnection;28;1;25;0
WireConnection;0;0;28;0
WireConnection;0;2;28;0
WireConnection;0;9;25;0
ASEEND*/
//CHKSM=0AAC1BDDCBFAE9C99F57B80FBE94AB087EF2FE23