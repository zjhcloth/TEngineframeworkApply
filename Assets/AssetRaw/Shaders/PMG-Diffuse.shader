Shader "PMG/Diffuse"
{
    Properties
    {
        _MainTex ("基础贴图:UV1", 2D) = "white" {}
        _Color("颜色", Color) = (1,1,1,1)
        [Space(25)]
        _CubeMap ("环境映射贴图" ,Cube) = "_Skybox" {}
        _FresnelPow ("环境光圈",Range(1,15)) = 5
        _RefINT ("环境映射强度", Range(0.1,1)) = 1
        _CubeMip ("环境球MIP等级", Range(0, 6)) = 2
        [Space(25)]
        _SpecPow ("高光范围", Range(10,100)) = 30
        _SpecINT ("高光强度", Range(0,2)) = 1
        [Space(25)]
        [Toggle] _FOG ("===受雾效影响===", Float) = 1
        [Space(25)]
		[Toggle] _Use3Cut("==========启用三段图==========", Float) = 0  
        _3CutTex ("三段图", 2D) = "white" {}
        [Space(25)]
        [Toggle] _OutLight("==========边缘光==========", Float) = 0
        _RimPow ("边缘光宽度",Range(1,15)) = 5
        _RimColor("边缘光颜色", Color) = (1,1,1,1)
        _RimINT ("边缘光强度", Range(0,5)) = 1
        [Space(25)]
        [Toggle] _UseEm("==========启用自发光==========", Float) = 0 
        _EmTex ("自发光贴图:UV3", 2D) = "white" {} 
        _EmColorR("自发光颜色R", Color) = (1,1,1,1)
        _EmColorG("自发光颜色G", Color) = (1,1,1,1)
        _EmColorB("自发光颜色B", Color) = (1,1,1,1)
        _EmColorA("自发光颜色A", Color) = (1,1,1,1)
        _EmIntR("自发光强度R",Range(0, 5)) = 0
        _EmIntG("自发光强度G",Range(0, 5)) = 0
        _EmIntB("自发光强度B",Range(0, 5)) = 0
        _EmIntA("自发光强度A",Range(0, 5)) = 0
        [Header(NEON EFFECT)]
        _HuiDu("灰度值",Range(0, 1)) = 0
        _PinLv("频率",Range(0, 20)) = 0	
        [Header(SHINNY)]
        _Shinny("闪烁速度",Range(0, 20)) = 0	
    }

    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
            
        Pass
        {
            CGPROGRAM
            sampler2D _MainTex;     fixed4 _MainTex_ST;
            samplerCUBE _CubeMap;   
            half _CubeMip;
            half _FresnelPow;
            fixed _RefINT;  
            half _SpecPow;
            fixed _SpecINT;            
            #ifndef _USE3CUT_OFF
            sampler2D _3CutTex; fixed4 _3CutTex_ST;
            #endif
            #ifndef _OUTLIGHT_OFF
            half4 _RimColor;
            fixed _RimINT;
            half _RimPow;
            #endif
            #ifndef _USEEM_OFF
            sampler2D _EmTex;     fixed4 _EmTex_ST;
            half4 _EmColorR;
            half4 _EmColorG;
            half4 _EmColorB;
            half4 _EmColorA;
            half _EmIntR;
            half _EmIntG;
            half _EmIntB;
            half _EmIntA;
            half _HuiDu;
            half _PinLv;
            half _Shinny;
            #endif

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma shader_feature _FOG_ON
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile _USE3CUT_OFF _USE3CUT_ON
            #pragma multi_compile _OUTLIGHT_OFF _OUTLIGHT_ON
            #pragma multi_compile _USEEM_OFF _USEEM_ON

            #pragma multi_compile_fog                    
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_instancing
            
            

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID 
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                half3 normal : NORMAL; 
            };

            struct v2f
            {                             
                float2 texUV : TEXCOORD0;
                #ifndef _USEEM_OFF
                float2 emUV : TEXCOORD7;
                #endif 

                #if _FOG_ON
                UNITY_FOG_COORDS(1)
                #endif
                
                float4 pos : SV_POSITION;
                half3 nDir : TEXCOORD2;      //nDir=法线方向
                half3 vDir : TEXCOORD3;     //视角方向
                half3 refDir : TEXCOORD4;   //反射方向
                float3 wPos : TEXCOORD5; 
                
                #ifndef LIGHTMAP_OFF
				half2 uvLM : TEXCOORD6;
                #endif 
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };
            UNITY_INSTANCING_BUFFER_START(Props)     
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)   //定义一个具有特定类型和名字的每个Instance独有的Shader属性。这个宏实际会定义一个Uniform数组。
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v,   o);
                o.pos = UnityObjectToClipPos(v.vertex);               
                o.nDir = UnityObjectToWorldNormal(v.normal);        //nDir=worldNormal,
                o.wPos = mul(unity_ObjectToWorld,v.vertex);
                o.vDir =UnityWorldSpaceViewDir(o.wPos);
                o.refDir = reflect(-o.vDir , o.nDir);   
                o.texUV = TRANSFORM_TEX(v.uv0, _MainTex);
                #ifndef _USEEM_OFF
                o.emUV = TRANSFORM_TEX(v.uv2, _EmTex);
                #endif 

                #if _FOG_ON
                UNITY_TRANSFER_FOG(o,o.pos);
                #endif

                #ifndef LIGHTMAP_OFF
				o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half3 nDir = normalize(i.nDir);
                half3 vDir = normalize(i.vDir);
                half3 refDir = normalize(i.refDir);
                half3 lDir = normalize(_WorldSpaceLightPos0.xyz);
                float ref = 1 -dot(nDir,vDir);
                float fresnel = pow(ref,_FresnelPow);
                fixed4 cubCol = texCUBElod(_CubeMap, float4(refDir,_CubeMip));
                
                fixed4 tex = tex2D(_MainTex, i.texUV) ;
                fixed4 color =  UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                #ifndef _USE3CUT_OFF
                float ndotl = dot(i.nDir,lDir);
                float2 remap = float2((ndotl*0.5+0.5),0.2);
                fixed4 cutTex = tex2D(_3CutTex,remap);
                fixed4 texCol = tex * color * cutTex;
                #else
                fixed4 texCol = tex * color;
                #endif
               
                #ifndef _OUTLIGHT_OFF
				float RimRange = pow(ref, _RimPow);
				half4 dirRim = RimRange*_RimColor*_RimINT;  
                fixed4 refCol =  cubCol * fresnel * _RefINT * tex.a + dirRim;
                #else
                fixed4 refCol =  cubCol * fresnel * _RefINT * tex.a;
                #endif
                
                float phone = dot(reflect((lDir*(-1.0)),nDir),vDir);
                half specCol = pow(max(0,phone),_SpecPow)*_SpecINT * tex.a;
               
                #ifndef _USEEM_OFF
                float4 time = _Time;               
                float4 em = tex2D(_EmTex, i.emUV);
                if(_HuiDu!=0){
                    float4 emStep = step(_HuiDu,(sin(time.g*_PinLv)*em));
                    em = emStep ; 
                }          
                if(_Shinny!=0){
                    float4 emSin = max(0,(sin(time.g*_Shinny)*em));
                    em = emSin ; 
                }   
                fixed4 emColR = em.r*_EmColorR*_EmIntR;
                fixed4 emColG = em.g*_EmColorG*_EmIntG;
                fixed4 emColB = em.b*_EmColorB*_EmIntB;
                fixed4 emColA = em.a*_EmColorA*_EmIntA;
                fixed4 mixEmCol = emColR + emColG + emColB + emColA;               
                fixed4 col = texCol + refCol + specCol + mixEmCol;
                #else
                fixed4 col = texCol + refCol + specCol;
                #endif
                
                UNITY_OPAQUE_ALPHA(col.a);
                #ifndef LIGHTMAP_OFF
				fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));
                col.rgb*=lm;
				#endif

                // apply fog
                #if _FOG_ON
                UNITY_APPLY_FOG(i.fogCoord, col);
                #endif    
                return col;
            }
            ENDCG
        }
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual Cull back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct v2f {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            v2f vert( appdata_base v )
            {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag( v2f i ) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
