Shader "Hidden/UnderwaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", float) = 1
        _NoiseFrequency ("Noise Frequency", float) = 1
        _NoiseSpeed ("Noise Speed", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            //#include "noiseSimplex.cginc"

            uniform float _NoiseFrequency, _NoiseScale, _NoiseSpeed;

            //// SIMPLEX NOISE FUNCTION
            float hash( float n )
			{
			    return frac(sin(n)*43758.5453);
			}

			float noise( float3 x )
			{
			    // The noise function returns a value in the range -1.0f -> 1.0f

			    float3 p = floor(x);
			    float3 f = frac(x);
                f = f*f*(3.0-2.0*f);
			    float n = p.x + p.y*57.0 + 113.0*p.z;

			    return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
			                   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
			               lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
			                   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
			}

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD1;
            };

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.screen_pos = ComputeScreenPos(OUT.vertex);
                OUT.uv = IN.uv;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f IN) : COLOR
            {
                float3 screen_pos = float3(IN.screen_pos.x, IN.screen_pos.y, 0) * _NoiseFrequency;
                screen_pos.z += _Time.x * _NoiseSpeed;
                float n = _NoiseScale * (noise(screen_pos) + 1)/2;
                fixed4 col = fixed4(1, 0, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
