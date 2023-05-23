/*
    Created by Takashi Yoshinaga
    Copyright (C) 2023 Takashi Yoshinaga. All Rights Reserved.
*/

Shader "AR_Fukuoka/SemanticStencils_Old"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Unlabeled ("Unlabeled", Color) = (1,1,1,1)
        _Sky ("Sky", Color) = (1,1,1,1)
        _Building ("Building", Color) = (1,1,1,1)
        _Tree ("Tree", Color) = (1,1,1,1)
        _Road ("Road", Color) = (1,1,1,1)
        _Sidewalk ("Sidewalk", Color) = (1,1,1,1)
        _Terrain ("Terrain", Color) = (1,1,1,1)
        _Structure ("Structure", Color) = (1,1,1,1)
        _Object ("Object", Color) = (1,1,1,1)
        _Vehicle ("Vehicle", Color) = (1,1,1,1)
        _Person ("Person", Color) = (1,1,1,1)
        _Water ("Water", Color) = (1,1,1,1) 
        _DefaultColor ("Default Color", Color) = (1,1,1,1)    
        _MaskAreaVisibility("MaskAreaVisibility", Range(0,1)) = 1           
    }
    SubShader
    {
        Tags { "Queue"="Geometry-10" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Stencil
            {
                Ref 2
                Comp Always
                Pass Replace
            }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Unlabeled;
            fixed4 _Sky;
            fixed4 _Building;
            fixed4 _Tree;
            fixed4 _Road;
            fixed4 _Sidewalk;
            fixed4 _Terrain;
            fixed4 _Structure;
            fixed4 _Object;
            fixed4 _Vehicle;
            fixed4 _Person;
            fixed4 _Water;
            fixed4 _DefaultColor;
            float _MaskAreaVisibility;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = float2(1.0-i.uv.y, 1.0-i.uv.x);
                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                int semanticLabel= round(col.r*255);
                switch(semanticLabel)
                {
                    case 0: col = _Unlabeled; break;
                    case 1: col = _Sky; break;
                    case 2: col = _Building; break;
                    case 3: col = _Tree; break;
                    case 4: col = _Road; break;
                    case 5: col = _Sidewalk; break;
                    case 6: col = _Terrain; break;
                    case 7: col = _Structure; break;
                    case 8: col = _Object; break;
                    case 9: col = _Vehicle; break;
                    case 10: col = _Person; break;
                    case 11: col = _Water; break;
                    default: col = _DefaultColor; break;
                }
                if(col.a == 0.0) discard;
                col.a=col.a*_MaskAreaVisibility;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
