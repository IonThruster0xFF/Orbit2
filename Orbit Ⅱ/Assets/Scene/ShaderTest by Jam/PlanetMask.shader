// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


//Mask遮罩, 且Mask边缘加深

 Shader "Custom/PlanetMask" 
{
    Properties {
        _MainTex ("Altitude Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _Altitude ("Altitude", Range(0,1)) = 0 //海拔

        _Para1("Para1", Range(0,1)) = 0
        _Para2("Para2", Range(0,1)) = 0
    }
 
    SubShader{
        Tags{"Queue"="Transparent" "RenderType"="Opaque" "IgnoreProject"="True"}
        Pass{
            Tags{"LightMode"="ForwardBase"}
 
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            half4 _MainTex_TexelSize;
            sampler2D _MainTex;
            sampler2D _MaskTex;
            float _Altitude;

            float _Para1;
            float _Para2;

          
            struct a2v {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
             
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv[5] : TEXCOORD0;
            };
 
 
            v2f vert(a2v v)
            {
                v2f o;

                float4 worldPos = mul(unity_ObjectToWorld,v.vertex);
            
                o.pos = UnityObjectToClipPos(v.vertex);

                float2 uv = v.texcoord.xy;


                o.uv[0] = uv + _MainTex_TexelSize.xy * half2(0,-1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(1,0);
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(0,0);
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(0,1);
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(-1,0);


                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
            	float4 oColor = tex2D(_MainTex, i.uv[2]);
            	// 根据海平面高度, 以及高度图高度, 生成海洋和陆地
            	//1. 陆地
            	oColor.g += step(_Altitude,oColor.r)*(_Para1 + oColor.r-_Altitude);//第二个参数大于第一个参数返回1, 避免分支语句
            	//2. 海洋
            	oColor.b += step(oColor.r,_Altitude)*(_Para2 + _Altitude - oColor.r);
            	//oColor.rgb += float3(0.2f,0.2f,0.2f);


            	//遮罩效果
            	oColor.a = tex2D(_MaskTex, i.uv[2]).a;

            	//阴影效果
            	oColor.rgb *= (0.5f-length(float2(i.uv[2].x-0.5f,i.uv[2].y-0.5f)))/0.5f; 

            	//边缘加深效果
            	float e = 0.0f;
            	e += tex2D(_MaskTex, i.uv[0]).a;
            	e += tex2D(_MaskTex, i.uv[1]).a;
            	e += tex2D(_MaskTex, i.uv[2]).a;
            	e += tex2D(_MaskTex, i.uv[3]).a;
            	e += tex2D(_MaskTex, i.uv[4]).a;
            	e *= 0.2f;//按照这个倍率变暗rgb

            	oColor.rgb *= e; 


                return oColor;
            }
            ENDCG
        }
    }
    FallBack Off
}