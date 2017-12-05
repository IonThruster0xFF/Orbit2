// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


//Mask遮罩, 且Mask边缘加深

 Shader "Custom/PlanetMask" 
{
    Properties 
    {
    	_PlanetBrightness("Planet Brightness",Range(0.9,1.5)) = 1
    	_PlanetRadius("Planet Radius",Range(40,120)) = 40
        _MainTex ("Altitude Texture", 2D) = "white" {}
        _SandMaskTex("Sand Mask Texture", 2D) = "white"{}
        _MaskTex ("Mask Texture", 2D) = "white" {}

        _SeaAltitude ("Sea Altitude", Range(0,1)) = 0 //海拔
        _SandAmount ("Sand Amount",Range(0.5,1.2)) = 0 //沙漠数量

        _LightPosX("Light Position X", Range(0,1)) = 0.5
        _LightPosY("Light Position Y", Range(0,1)) = 0.5


        _LandColor("Land Color", Color) = (1,1,1,1)
        _WaterColor("Water Color", Color) = (1,1,1,1)
        _SandColor("Sand Color",Color) = (1,1,1,1)

        _LandBright("Land Brightness", Range(0,1)) = 0
        _WaterBright("Water Brightness", Range(0,1)) = 0
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
            sampler2D _SandMaskTex;
            sampler2D _MaskTex;

            float _PlanetBrightness;
            float _PlanetRadius;

            float _SeaAltitude;
            float _SandAmount;

            float _LightPosX;
            float _LightPosY;

            float4 _LandColor;
            float4 _WaterColor;
            float4 _SandColor;

            float _LandBright;
            float _WaterBright;

          
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
            	float sandLevel = 1 - _SandAmount; // 超过这个水平, 就生成沙漠
            	float4 oColor = tex2D(_MainTex, i.uv[2]);
            	float landAltitude = oColor.r;//获得高度图当前位置高度
            	float sandAltitude = tex2D(_SandMaskTex, i.uv[2]).r + 0.0001f;//获得沙漠高度图当前位置高度

            	//防止rgb为0
            	oColor.rgb += float3(0.0001f,0.0001f,0.0001f);

            	//沙漠地带, 沙漠颜色和陆地颜色各占一部分, 沙漠深处的陆地颜色比例更少
            	float sandRatio = clamp(0.0f,1.0f,(sandAltitude-sandLevel)*10.0f);

				// 陆地/海水/沙漠 颜色, 乘以高度变化
            	oColor.rgb = 
            		//沙漠, 只在陆地生成
            		step(_SeaAltitude,landAltitude)*step(sandLevel, sandAltitude)*
            			(_SandColor*sandRatio + _LandColor*(_LandBright + landAltitude-_SeaAltitude)*(1 - sandRatio)  ) + 

            		//陆地
            		(1-step(sandLevel, sandAltitude))*step(_SeaAltitude,landAltitude)*
            			_LandColor*(_LandBright + landAltitude-_SeaAltitude) + 	

					//海洋
					(1 - step(_SeaAltitude,landAltitude))*
						_WaterColor*(_WaterBright + _SeaAltitude - landAltitude);	



            	//遮罩效果
            	oColor.a = tex2D(_MaskTex, i.uv[2]).a;

            	//阴影效果
            	float shaderRadio = _PlanetRadius/128;
            	//oColor.rgb *= (0.5f-length(float2(i.uv[2].x-0.5f,i.uv[2].y-0.5f)))/0.5f; 
            	oColor.rgb *= (shaderRadio-length(float2(i.uv[2].x-_LightPosX,i.uv[2].y-_LightPosY)))/shaderRadio; 


            	//边缘加深效果
            	float e = 0.0f;
            	e += tex2D(_MaskTex, i.uv[0]).a;
            	e += tex2D(_MaskTex, i.uv[1]).a;
            	e += tex2D(_MaskTex, i.uv[2]).a;
            	e += tex2D(_MaskTex, i.uv[3]).a;
            	e += tex2D(_MaskTex, i.uv[4]).a;
            	e *= 0.2f;//按照这个倍率变暗rgb
            	//oColor.rgb *= e; 

            	//边缘变亮
            	oColor.rgb *= pow((2 - e),1.0f); 

                return oColor*_PlanetBrightness;
            }
            ENDCG
        }
    }
    FallBack Off
}