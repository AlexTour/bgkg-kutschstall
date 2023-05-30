Shader "Custom/Wavy" {
 Properties {
 
 _Maintex ("texture", 2D) = "white" {} // texture
 _Color("Color", Color) = (1,1,1,1)
 _Arange("Amplitute", float) = 1
 _Frequency ("frequency", float) = 2 // fluctuation frequency
 _Speed ("speed", float) = 0.5 // controls the speed of texture movement

 
 }
 
 SubShader
 {
 ZWrite Off
 Blend SrcAlpha OneMinusSrcAlpha

 Pass
 { 
 CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #include "UnityCG.cginc"
 
 struct appdata
 {
 float4 vertex:POSITION;
 float2 uv:TEXCOORD0;
 };
 
 struct v2f
 {
 float2 uv:TEXCOORD0;
 float4 vertex:SV_POSITION;
 };

 float4 _Color;
 float _Frequency;
 float _Arange;
 float _Speed;
 
 v2f vert(appdata v)
 {
 v2f o;
 
 float timer = _Time.y *_Speed;
 //Make a fluctuation before the change y = asin (ω x + φ)
 float waver = _Arange*sin(timer + v.vertex.x *_Frequency);
 v.vertex.y = v.vertex.y + waver;
 o.vertex = UnityObjectToClipPos(v.vertex);
 o.uv = v.uv;;
 return o;
 }
 
 sampler2D _MainTex;
 
 fixed4 frag(v2f i) :SV_Target
 {
 fixed4 col = tex2D(_MainTex, i.uv) * _Color;
 return col;
 } 
 
 ENDCG
 }
 }
 
 FallBack "Diffuse"
}