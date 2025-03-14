Shader "Unlit/Radial_360_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 0
        _Color ("Color", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float _FillAmount;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv - 0.5;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float angle = atan2(uv.y, uv.x);
                angle = angle < 0 ? angle + 6.28318530718 : angle;
                
                float fillAngle = _FillAmount * 6.28318530718;
                
                fixed4 col = tex2D(_MainTex, i.uv + 0.5) * _Color;
                col.a *= angle <= fillAngle ? 1 : 0;
                
                return col;
            }
            ENDCG
        }
    }
}
