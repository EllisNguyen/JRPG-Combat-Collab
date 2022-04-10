Shader "Unlit/SolidSpriteColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_SolidColor("Solid Color", Color) = (1,1,1,1)

	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragment SolidSpriteFrag;

				#include "UnityCG.cginc"

				fixed4 _SolidColor;
				
				fixed4 SolidSpriteFrag(v2f IN) : SV_Target
				{
					fixed4 c = _SolidColor;// SampleSpriteTexture(IN.texcoord) * IN.color;


					c.rgb *= c.a;
					return c;
				}


				ENDCG
        }
    }
}
