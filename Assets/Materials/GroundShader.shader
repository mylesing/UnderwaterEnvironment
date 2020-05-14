// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GroundShader" {
    Properties {
        //_MainTexture("Texture", 2D) = "white"{}
        // FBM properties
        _offsetX ("OffsetX",Float) = 0.0
        _offsetY ("OffsetY",Float) = 0.0      
        _octaves ("Octaves",Int) = 7
        _lacunarity("Lacunarity", Range( 1.0 , 5.0)) = 2
        _gain("Gain", Range( 0.0 , 1.0)) = 0.5
        _value("Value", Range( -2.0 , 2.0)) = 0.0
        _amplitude("Amplitude", Range( 0.0 , 5.0)) = 1.5
        _frequency("Frequency", Range( 0.0 , 6.0)) = 2.0
        _power("Power", Range( 0.1 , 5.0)) = 1.0
        _scale("Scale", Float) = 1.0

        // object color
        _Color("Main Color", Color) = (1, 0, 0, 1)
        [Toggle] _monochromatic("Monochromatic", Float) = 0
        _range("Monochromatic Range", Range( 0.0 , 1.0)) = 0.5

        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _fogColor ("Fog Color", Color) = (0.5,0.5,0.5,0.5)
    }

    SubShader {
        Tags { "RenderType" = "Opaque"}
        Pass {
            // indicate that our pass is the "base" pass in forward
            // rendering pipeline. It gets ambient and main directional
            // light data set up; light direction in _WorldSpaceLightPos0
            // and color in _LightColor0
            //Tags {"LightMode"="ForwardBase"}

            CGPROGRAM

            // function instantiation
            #pragma vertex vertex_func
            #pragma fragment fragment_func
            #pragma multi_compile_particles
            #pragma multi_compile_fog
            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor
            #include "UnityStandardUtils.cginc" // for ShadeSHPerPixel

            // vertex to fragment 
            struct v2f {
                fixed4 color : COLOR0; // diffuse color
                float4 position : SV_POSITION; // output position
                float3 normal : TEXCOORD3;
                UNITY_FOG_COORDS(1)
            };

            float _octaves,_lacunarity,_gain,_value,_amplitude,_frequency, _offsetX, _offsetY, _power, _scale, _monochromatic, _range;
            float4 _color;

            float fbm( float2 p )
            {
                p = p * _scale + float2(_offsetX,_offsetY);
                for( int i = 0; i < _octaves; i++ )
                {
                    float2 i = floor( p * _frequency );
                    float2 f = frac( p * _frequency );      
                    float2 t = f * f * f * ( f * ( f * 6.0 - 15.0 ) + 10.0 );
                    float2 a = i + float2( 0.0, 0.0 );
                    float2 b = i + float2( 1.0, 0.0 );
                    float2 c = i + float2( 0.0, 1.0 );
                    float2 d = i + float2( 1.0, 1.0 );
                    a = -1.0 + 2.0 * frac( sin( float2( dot( a, float2( 127.1, 311.7 ) ),dot( a, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    b = -1.0 + 2.0 * frac( sin( float2( dot( b, float2( 127.1, 311.7 ) ),dot( b, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    c = -1.0 + 2.0 * frac( sin( float2( dot( c, float2( 127.1, 311.7 ) ),dot( c, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    d = -1.0 + 2.0 * frac( sin( float2( dot( d, float2( 127.1, 311.7 ) ),dot( d, float2( 269.5,183.3 ) ) ) ) * 43758.5453123 );
                    float A = dot( a, f - float2( 0.0, 0.0 ) );
                    float B = dot( b, f - float2( 1.0, 0.0 ) );
                    float C = dot( c, f - float2( 0.0, 1.0 ) );
                    float D = dot( d, f - float2( 1.0, 1.0 ) );
                    float noise = ( lerp( lerp( A, B, t.x ), lerp( C, D, t.x ), t.y ) );              
                    _value += _amplitude * noise;
                    _frequency *= _lacunarity;
                    _amplitude *= _gain;
                }
                _value = clamp( _value, -1.0, 1.0 );
                return pow(_value * 0.5 + 0.5,_power);
            }

             float3 getDeform(float3 p)
        {
            return (float3(p.xy, 0.3) * (cos(p.z) + sin(p.x * 10) + cos(p.y * 10))) / 10 +
                float3(0,cos(p.z + cos(p.z)) / 6, 0);
        }

            // vertex deformation / info to pass to fragment shader
            v2f vertex_func(appdata_full IN) {
                v2f OUT;
                ///// VERTEX DISPLACEMENT /////
                // get xz position and change height according to fmb value
                float2 xz = IN.vertex.xz / 10;
                float y = fbm(xz);
                float4 modified_pos = IN.vertex;
                modified_pos.y += y;
                OUT.position = UnityObjectToClipPos(modified_pos);

                ///// UPDATE NORMALS /////
                float3 tan_shift = modified_pos + IN.tangent * 0.01;
                tan_shift.y += fbm(tan_shift.xz);

                float3 bitangent = cross(modified_pos, IN.tangent);
                float3 bitan_shift = IN.vertex + bitangent * 0.01;
                bitan_shift.y += fbm(bitan_shift.xz);

                float3 new_tan = -(tan_shift - IN.vertex) ;
                float3 new_bitan = -(bitan_shift - IN.vertex) ;

                float3 modifiedNormal = cross(new_tan, new_bitan);
                IN.normal = normalize(modifiedNormal);
                OUT.normal = IN.normal;

                ///// DIFFUSE LIGHTING /////
                // float2 nor_xz = IN.normal.xy;
                // float nor_y = fbm(nor_xz);
                // IN.normal.y += nor_y;
                // float4 p = OUT.position;
                // p.xyz = getDeform(p);
                // IN.normal = p;

                // get vert normal in world space
                half3 world_nor = UnityObjectToWorldNormal(IN.normal);

                // dot product between normal and light dir
                half nl = max(0, dot(world_nor, _WorldSpaceLightPos0.xyz));

                // factor in the light color
                OUT.color = (1.2 - nl) * _LightColor0;

                UNITY_TRANSFER_FOG(OUT,OUT.position);

                return OUT;
            }

            fixed4 _Color;
            fixed4 _fogColor;

            // fragment shader
            fixed4 fragment_func(v2f IN) : SV_Target {
                //fixed4 col = _Color;
                // // multiply by lighting
                //col *= IN.color;
                //return col;
                half3 currentAmbient = half3(0, 0, 0);
                half3 ambient = ShadeSHPerPixel(IN.normal, currentAmbient, IN.position);
                fixed4 col = _Color;
                col.xyz += ambient;
                col *= IN.color;
                UNITY_APPLY_FOG_COLOR(IN.fogCoord, col, _fogColor);
                // float dist = sqrt(IN.position.x * IN.position.x + IN.position.y * IN.position.y);
                // if (dist > 300) {
                //     col += (dist / 500) * _fogColor;
                // }
                return col; //float4(c,c,c,1);
            }
 
            ENDCG
        }
    }
}
