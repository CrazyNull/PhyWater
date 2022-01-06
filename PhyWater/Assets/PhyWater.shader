Shader "Unlit/PhyWater"
{
    Properties
    {
		_Color("Color",Color) = (1,1,1,1)
        _SpecularColor ("Specular Color", Color) = (0,0,0,1)
        _Gloss ("Gloss",Range(0.5,20.0)) = 1.0

        _WaveHeight1("Wave Height 1",Float) = 0.25
        _WaveLenght1("Wave Lenght 1",Float) = 1.0
        _WaveOffset1("Wave Offset 1",Vector) = (0,0,0,0)

        _WaveHeight2("Wave Height 2",Float) = 0.25
        _WaveLenght2("Wave Lenght 2",Float) = 1.0
        _WaveOffset2("Wave Offset 2",Vector) = (0,0,0,0)

        _time("Time",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
                float4 color : Color;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                
				float3 worldPos:TEXCOORD2;
				//float3 worldNormal:TEXCOORD3;
				float3 worldViewDir:TEXCOORD4;
            };

            float4 _Color;
            float4 _SpecularColor;
            float _Gloss;

            float _WaveHeight1;
            float _WaveLenght1;
            float3 _WaveOffset1;

            float _WaveHeight2;
            float _WaveLenght2;
            float3 _WaveOffset2;

            float3 _CenterPos;
            float _time;

            float4 calculationPos(float4 worldPos)
            {                
                float4 result = worldPos;
                float y = _WaveHeight1 * sin(_WaveLenght1 * result.x + _time) + _WaveOffset1.y;
                result.y += y;
                y = _WaveHeight2 * cos(_WaveLenght2 * result.z + _time) + _WaveOffset2.y;
                result.y += y;
                return result;
            }


            v2f vert (appdata v)
            {
                v2f o;

                float4 worldPos = mul(unity_ObjectToWorld,v.vertex);
                if(v.color.r >= 0.01)
                {
                    worldPos = calculationPos(worldPos);
                }
                v.vertex = mul(unity_WorldToObject,worldPos);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);

                o.worldPos = worldPos;
		        //o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				o.worldViewDir = _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(cross(ddy(i.worldPos),ddx(i.worldPos)));
				//float3 worldNormal = normalize(i.worldNormal);
				float3 worldViewDir = normalize(i.worldViewDir);
				float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

                fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                fixed3 halfDir = normalize(viewDir + worldLightDir);
                fixed3 specular = _SpecularColor.rgb * pow(max(0,dot(halfDir,worldNormal)),_Gloss);

				float lambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;
				fixed4 col = lambert * _Color;
                col.rgb += specular.rgb;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
