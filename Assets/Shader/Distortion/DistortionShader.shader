// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "WDR/Distortion" {
Properties {
	_Farbe ("Farbe", Color) = (0.500000,0.500000,0.500000,0.500000)
	_TintColor ("Tint Color", Color) = (0.500000,0.500000,0.500000,0.500000)
	_BumpAmt  ("Distortion", range (0,128)) = 10
	_MainTex ("Tint Color (RGB)", 2D) = "white" {}
	_AlphaTex ("Alpha Map (R)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}

Category {

	// We must be transparent, so other objects are drawn before this one.
	Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGB
	ZWrite Off
	//Blend OneMinusDstColor One

	SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
		}
		
		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#include "UnityCG.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
	fixed4 color : COLOR;
};

struct v2f {
	float4 vertex : SV_POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uvmain : TEXCOORD2;
	float2 alpha : TEXCOORD3;
	float4 color : COLOR;
	UNITY_FOG_COORDS(3)
};

float _BumpAmt;
float4 _BumpMap_ST;
float4 _MainTex_ST;
float4 _AlphaTex_ST;
fixed4 _TintColor;

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uvgrab = ComputeGrabScreenPos(o.vertex);
	o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
	o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
	o.alpha = TRANSFORM_TEX( v.texcoord, _AlphaTex);
	o.color = v.color;
	UNITY_TRANSFER_FOG(o,o.vertex);
	return o;
}

sampler2D _GrabTexture;
float4 _GrabTexture_TexelSize;
sampler2D _BumpMap;
sampler2D _MainTex;
sampler2D _AlphaTex;
float4 _Farbe;

half4 frag (v2f i) : SV_Target
{
	#if UNITY_SINGLE_PASS_STEREO
	i.uvgrab.xy = TransformStereoScreenSpaceTex(i.uvgrab.xy, i.uvgrab.w);
	#endif

	// calculate perturbed coordinates
	half alpha = tex2D(_AlphaTex, i.alpha).r;
	half2 bump = UnpackNormal(tex2D( _BumpMap, float2(_Time.x+i.uvbump.x, i.uvbump.y))).rg; // we could optimize this by just reading the x & y without reconstructing the Z
	float2 offset = alpha * (bump + 1) * _BumpAmt * _GrabTexture_TexelSize.xy;

	#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
		i.uvgrab.xy = offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(i.uvgrab.z) + i.uvgrab.xy;
	#else
		i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
	#endif

	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab)) * fixed4(_Farbe.xyz,1) *  i.color;
	half4 tint = tex2D(_MainTex, i.uvmain).r;

	//col *= (tint/2+1) * i.color;
	col.a = alpha;

	//col = half4(i.color.r,i.color.g,i.color.b,i.color.a);

	//UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
}
ENDCG
		}
	}

	// ------------------------------------------------------------------
	// Fallback for older cards and Unity non-Pro

	SubShader {
		Blend DstColor Zero
		Pass {
			Name "BASE"
			SetTexture [_MainTex] {	combine texture }
		}
	}
}

}
