Shader "Unlit/GerstnerWater"
{
    Properties
    {
		_Color("Color",Color) = (1,1,1,1)
        _WaveHeight("Wave Height",Float) = 0.25
        _WaveLenght("Wave Lenght",Float) = 1.0
        _time("Time",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "WaveSimulation.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
                float4 color : Color;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 worldNormal : TEXCOORD2;
            };

            float4 _Color;

            float _WaveHeight;
            float _WaveLenght;
            float _time;

            v2f vert (appdata v)
            {
                v2f o;

                float4 worldPos = mul(unity_ObjectToWorld,v.vertex);
                GerstnerResult result = SimulationGerstnerWave(worldPos,_time,_WaveHeight,_WaveLenght);
                if(v.color.r >= 0.01)
                {
                    worldPos = result.pos;
                }
                v.vertex = mul(unity_WorldToObject,worldPos);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);

                o.worldNormal = result.normal;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = i.worldNormal; // normalize(cross(ddy(i.worldPos),ddx(i.worldPos)));
				float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				float lambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				fixed4 col = lambert * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
