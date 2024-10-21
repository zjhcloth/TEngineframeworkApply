Shader "PMG/Additive" {
Properties {
    _TintColor ("颜色", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("贴图", 2D) = "white" {}
    _InvFade ("淡入淡出", Range(0.01,3.0)) = 1.0
    [Toggle] _UseFog("======接收雾效影响======", Float) = 0
}

Category {
    Tags { "Queue"="Transparent" 
    "IgnoreProjector"="True" 
    "RenderType"="Transparent" 
    "PreviewType"="Plane" }
    Blend SrcAlpha One
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off

    SubShader {
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _USEFOG_OFF _USEFOG_ON

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            // fixed4 _TintColor;
   

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv0 : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv0 : TEXCOORD0;
                #ifndef _USEFOG_OFF
                UNITY_FOG_COORDS(1)
                #endif

                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4,_TintColor) 
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                o.uv0 = TRANSFORM_TEX(v.uv0,_MainTex);

                #ifndef _USEFOG_OFF
                UNITY_TRANSFER_FOG(o,o.vertex);
                #endif

                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate (_InvFade * (sceneZ-partZ));
                i.color.a *= fade;
                #endif

                fixed4 col = 2.0f * i.color 
                * UNITY_ACCESS_INSTANCED_PROP(Props, _TintColor) * tex2D(_MainTex, i.uv0);
                col.a = saturate(col.a); 

                #ifndef _USEFOG_OFF
                UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
                #endif 

                return col;
            }
            ENDCG
        }
    }
}
}
