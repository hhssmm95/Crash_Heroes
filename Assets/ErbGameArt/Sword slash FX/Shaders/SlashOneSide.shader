Shader "ErbGameArt/Particles/SlashOneSide" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0,0.5019608,1,1)
        _EmissionRGB ("Emission/R/G/B", Vector) = (1,0.4,0.4,1)
        _Startopacity ("Start opacity", Float ) = 40
        [MaterialToggle] _Sideopacity ("Side opacity", Float ) = 1
        _Sideopacitypower ("Side opacity power", Float ) = 40
        _Finalopacity ("Final opacity", Range(0, 1)) = 1
        _LenghtSet1ifyouuseinPS ("Lenght(Set 1 if you use in PS)", Range(0, 1)) = 1
        _PathSet0ifyouuseinPS ("Path(Set 0 if you use in PS)", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fog
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _Startopacity;
            uniform float4 _EmissionRGB;
            uniform float _Sideopacitypower;
            uniform fixed _Sideopacity;
            uniform float _Finalopacity;
            uniform float _LenghtSet1ifyouuseinPS;
            uniform float _PathSet0ifyouuseinPS;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float Path = ((_PathSet0ifyouuseinPS+i.uv0.b)*-1.5+2.5);
                float Lenght = ((_LenghtSet1ifyouuseinPS*i.uv0.a)*-1.0+1.0);
                float2 node_5293 = saturate(float2((saturate((((Path*Path*Path*Path*Path)*i.uv0.r)-Lenght))*(1.0 / (Lenght*-0.999+1.0))),i.uv0.g));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5293, _MainTex));
                float3 emissive = (((_MainTex_var.r*_EmissionRGB.g)+(_MainTex_var.g*_EmissionRGB.b)+(_MainTex_var.b*_EmissionRGB.a))*i.vertexColor.rgb*_TintColor.rgb*_EmissionRGB.r);
                float re = node_5293.r;
                fixed4 finalRGBA = fixed4(emissive,(_MainTex_var.a*i.vertexColor.a*_TintColor.a*(saturate(((re*-1.0+1.0)*_Startopacity))*saturate((re*_Startopacity))*lerp( 1.0, saturate(((i.uv0.g*-1.0+1.0)*_Sideopacitypower)), _Sideopacity ))*_Finalopacity));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
}
