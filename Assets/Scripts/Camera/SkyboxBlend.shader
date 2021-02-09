Shader "Hidden/SkyboxBlend"
{
    Properties {
		_MainTex ("CubeMap", 2D) = "white" {}
        _Blend ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

        GrabPass
        {
        
        }


		Pass{  
            CGPROGRAM  
            #include "UnityCG.cginc"  
            #pragma vertex vert_img  
            #pragma fragment frag  
            uniform sampler2D _MainTex;
            uniform sampler2D _Blend;
            //去色
            fixed4 DelColor(fixed4 _color){
                fixed c = _color.r + _color.g + _color.b;
                c /= 3;
                return fixed4(c,c,c,1.0f);
            }
            //曝光
            fixed4 Exposure(fixed4 _color,fixed force){
                fixed r = min(1,max(0,_color.r * pow(2,force)));
                fixed g = min(1,max(0,_color.g * pow(2,force)));
                fixed b = min(1,max(0,_color.b * pow(2,force)));
                return fixed4(r,g,b,1.0f);
            }
            //颜色加深
            fixed4 ColorPlus(fixed4 _color){
                fixed r = 1-(1-_color.r)/_color.r;
                fixed g = 1-(1-_color.g)/_color.g;
                fixed b = 1-(1-_color.b)/_color.b;
                return fixed4(r,g,b,1.0f);
            }
            //颜色减淡
            fixed4 ColorMinus(fixed4 _color){
                fixed r = _color.r + pow(_color.r,2)/(1-_color.r);
                fixed g = _color.g + pow(_color.g,2)/(1-_color.g);
                fixed b = _color.b + pow(_color.b,2)/(1-_color.b);
                return fixed4(r,g,b,1.0f);
            }
            //滤色
            fixed4 Screen(fixed4 _color){
                fixed r = 1-(pow((1-_color.r),2));
                fixed g = 1-(pow((1-_color.g),2));
                fixed b = 1-(pow((1-_color.b),2));
                return fixed4(r,g,b,1.0f);
            }
            //正片叠底
            fixed4 Muitiply(fixed4 _color){
                fixed r = pow(_color.r,2);
                fixed g = pow(_color.g,2);
                fixed b = pow(_color.b,2);
                return fixed4(r,g,b,1.0f);
            }
            //强光
            fixed4 ForceLight(fixed4 _color){
                fixed r = 1-pow((1-_color.r),2) / 0.5f;
                fixed g = 1-pow((1-_color.g),2) / 0.5f;
                fixed b = 1-pow((1-_color.b),2) / 0.5f;
                if(_color.r < 0.5f) r = pow(_color.r,2)/0.5f;
                if(_color.g < 0.5f) g = pow(_color.g,2)/0.5f;
                if(_color.b < 0.5f) b = pow(_color.b,2)/0.5f;
                return fixed4(r,g,b,1.0f);
            }
            float4 frag( v2f_img o ) : COLOR  
            {  
                fixed4 _color = tex2D(_MainTex, o.uv); 
                //_color = DelColor(_color);
                _color = tex2D(_Blend, o.uv);
                return _color;
            }  
            ENDCG  
        }  
	}
	FallBack "Diffuse"
}
