/*
    Created by Takashi Yoshinaga
    Copyright (C) 2023 Takashi Yoshinaga. All Rights Reserved.
*/

Shader "AR_Fukuoka/SemanticStencils"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            //Array for 12 semantic labels defined by ARCore + 1 for default color
            float4 _LabelColorArray[13];
            float _MaskFlagArray[13];
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
                // switch(semanticLabel)
                // {
                //     case 0: col = _Unlabeled; break;
                //     case 1: col = _Sky; break;
                //     case 2: col = _Building; break;
                //     case 3: col = _Tree; break;
                //     case 4: col = _Road; break;
                //     case 5: col = _Sidewalk; break;
                //     case 6: col = _Terrain; break;
                //     case 7: col = _Structure; break;
                //     case 8: col = _Object; break;
                //     case 9: col = _Vehicle; break;
                //     case 10: col = _Person; break;
                //     case 11: col = _Water; break;
                //     default: col = _DefaultColor; break;
                // }
                //Limit semanticLabel to 0-12 (13 labels)
                semanticLabel = clamp(semanticLabel, 0, 12);
                //If mask flag is 0 stencil is not applied
                if(_MaskFlagArray[semanticLabel]<0.5){
                    discard;
                }
                //Select color from array
                col = _LabelColorArray[semanticLabel];
                //Apply mask area opacity
                col.a=col.a*_MaskAreaVisibility;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
