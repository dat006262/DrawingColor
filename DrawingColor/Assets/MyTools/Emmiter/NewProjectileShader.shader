﻿Shader "ProjectileShader_StaticColor" {
  Properties {
    _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    [MaterialToggle] PixelSnap ("Pixel snap", Float) = 1
   // [MaterialToggle] IsFlip ("is Flip", Float) = 0
    [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
    [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	[PowerSlider(1.0)] _ZIndex("ZIndex", Range(0.01, 1)) = 0.01
  }

  SubShader {
    Tags {
      "Queue"="Transparent"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
    }

    Cull Off
    Lighting Off
    ZWrite On
    Blend One OneMinusSrcAlpha

    Pass {
    CGPROGRAM
          #pragma vertex vert
        #pragma vertex SpriteVert
        #pragma fragment SpriteFrag
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ISFLIP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
      //  #pragma instancing_options procedural:setup

        #include "UnityCG.cginc"

        CBUFFER_START(UnityPerDrawSprite)
            float _EnableExternalAlpha;
        CBUFFER_END

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
          StructuredBuffer<float4> positionBuffer;          
        #endif
        	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			StructuredBuffer<float4> colorBuffer;
		#endif
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			StructuredBuffer<float4> flipBuffer;
		#endif

        struct appdata_t
        {
            float4 vertex   : POSITION;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 vertex   : SV_POSITION;
            fixed4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
        };

		fixed _ZIndex;

        void setup() {
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)

          float4 position = positionBuffer[unity_InstanceID];
       
          float cosR = cos(position.w) * position.z;
          float sinR = sin(position.w) * position.z;
		  		  				  
          unity_ObjectToWorld = float4x4(
            cosR, -sinR, 0, position.x,
            sinR,  cosR, 0, position.y,
               0,     0, 0,          -_ZIndex,
               0,     0, 0,          1.00f
          );

          unity_WorldToObject = unity_ObjectToWorld;
          // unity_WorldToObject._14_24_34 *= -1;
          // unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;

        #endif
        }
        void rotate2D(inout float2 v, float r)
            {
                float s, c;
                sincos(r, s, c);
                v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
            }
         v2f vert (appdata_full v, uint instanceID : SV_InstanceID)
        {
             #if SHADER_TARGET >= 45
                float4 position = positionBuffer[instanceID];
            #else
                float4 position = 0;
            #endif
           float rotation = position.w * position.w * _Time.x * 0.5f;
                rotate2D(position.xz, rotation);


              float3 localPosition = v.vertex.xyz * position.w;
                float3 worldPosition = position.xyz + localPosition;
                float3 worldNormal = v.normal;
                 v2f o;
                o.vertex=  mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0f));
               o.texcoord = v.texcoord;
                   return o;
        }

		fixed4 _Color;

        v2f SpriteVert(appdata_t IN)
        {
            v2f OUT;

            UNITY_SETUP_INSTANCE_ID (IN);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
              #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)

               float4 flip = flipBuffer[unity_InstanceID];
               if(flip.y==1)
               {
                   
               IN.vertex.x =-IN.vertex.x; 
               }
          
            #endif
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.texcoord = IN.texcoord;
         
			#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			OUT.color = IN.color * colorBuffer[unity_InstanceID];
		    #else 
			OUT.color = IN.color * _Color;
	    	#endif
					   
           #ifdef PIXELSNAP_ON
            OUT.vertex = UnityPixelSnap (OUT.vertex);
           #endif
            			   
           
            return OUT;
        }
     
        sampler2D _MainTex;
        sampler2D _AlphaTex;

        fixed4 SampleSpriteTexture (float2 uv)
        {
            fixed4 color = tex2D (_MainTex, uv);

        #if ETC1_EXTERNAL_ALPHA
            fixed4 alpha = tex2D (_AlphaTex, uv);
            color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
        #endif
             if (color.a < 0.5f) {
             color.a = 0;
             }
            return color;
        }

        fixed4 SpriteFrag(v2f IN) : SV_Target
        {
            fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
            c.rgb *= c.a;
            return c;
        }
    ENDCG
    }
  }
}