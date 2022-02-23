// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale' with 'float4(1,1,1,1)'
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "M/Swing" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		//_Cutoff ("Alpha cutoff", Range (0,1)) = 0.5
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OffsetScale("Scale", Vector) = (0.1, 0.1, 0.1, 1)
		_OffsetSpeed("Offset Speed", float) = 1
	}
	SubShader {
		Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Grass"}
		LOD 200
		
		Pass {
			//Cull Off
			//AlphaTest Greater [_Cutoff]
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"


			float4 _Color;
			sampler2D _MainTex;
			// sampler2D	 unity_Lightmap; //Far lightmap.
			// sampler2D	 unity_LightmapInd; //Near lightmap (indirect lighting only).
			// float4	 unity_LightmapST; //Lightmap atlasing data.
			float4 unity_LightmapIndST;
			float3 _OffsetScale;
			float _OffsetSpeed;
			

 
			struct appdata {
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};
			
			struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float2 lightmapUV : TEXCOORD1;
				//float4 color : TEXCOORD2;
			};

			float4 _MainTex_ST;

 
			v2f vert (appdata v)
			{ 
				v2f o;
				float3 objWorldPos = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float spd = (objWorldPos.x + objWorldPos.z + worldPos.y + _Time.y) * _OffsetSpeed;
				spd = sin(spd) + cos(3 * spd);
				float3 offset = _OffsetScale * spd * v.color.aaa;
				worldPos.xyz += offset;
				float4 objectPos = mul(unity_WorldToObject, worldPos) * float4(float4(1,1,1,1).www, 1);
				
				o.pos = UnityObjectToClipPos (objectPos);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				o.lightmapUV = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				//o.color = v.color;
				
				return o;
			}

 
			fixed4 frag (v2f i) : COLOR
			{
			
				fixed4 texcol = tex2D(_MainTex, i.uv);
				fixed4 light = UNITY_SAMPLE_TEX2D (unity_Lightmap, i.lightmapUV);
				light.xyz = DecodeLightmap(light);
				light.w = 1;
	
				return (texcol * light * _Color);

			}

			ENDCG
		}
	} 
}
