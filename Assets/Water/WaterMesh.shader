Shader "Custom/Water"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
        _EdgeIntensity("Edge Intensity", float) = 1
        _DepthFactor("Depth Factor", float) = 1.0
        _DepthRampTex("Depth Ramp", 2D) = "white" {}
        _NoiseTex("Noise", 2D) = "white" {}
        _EdgeTex("Edge Tex", 2D) = "white" {}
        _DistorctionFactor("Distorction Factor", float) = 1.0
        _Scale("Scale", float) = 1.0
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
			"Queue"="Transparent" 
			"ForceNoShadowCasting" = "True" 
			"IgnoreProjector"="True" 
        }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert 
		#pragma target 4.0

        struct Input 
		{
			float4 grabUV;
			float4 screenPos;

			float3 wNormal;
			float3 worldPos;

			float2 uv_MainTex;
			float2 flowMap;
			float2 uv_Normal;
		};

        sampler2D _MainTex;
        sampler2D _CameraDepthTexture;
        sampler2D _DepthRampTex;
        sampler2D _NoiseTex;
        sampler2D _EdgeTex;
        float4 _EdgeColor;
        float _EdgeIntensity;
        float _DepthFactor;
        float _DistorctionFactor;
        float _Scale;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float4 pos = UnityObjectToClipPos(v.vertex);

			o.wNormal = v.normal;
			o.worldPos = worldPos;

			o.screenPos = ComputeScreenPos(pos);
			COMPUTE_EYEDEPTH(o.screenPos.z);

            o.flowMap = worldPos.xyz;
		}

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float clamp(float val, float mi, float ma)
		{
			return val < mi ? mi : val > ma ? ma : val;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            //nosie texture
            fixed distortx = tex2D(_NoiseTex, (IN.uv_MainTex.xy * _Scale) + (_Time.x)).r;

            //albedo collor noised
            half4 mainTex = tex2D(_MainTex, (IN.uv_MainTex * _Scale) - (distortx  * _DistorctionFactor));
            
            // Get Depth for Foam
			float foamSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, IN.screenPos);
            float LinearDepth = LinearEyeDepth(foamSample).r;
            float foamLine = 1 - saturate((_DepthFactor - (distortx * _DepthFactor)) * (LinearDepth - IN.screenPos.w));
            
            //uncomment to use degrade texture (foamRamp)
            //foamLine = float4(tex2D(_DepthRampTex, float2(foamLine, 0.5)).rgb, 1.0);
            _EdgeColor = (foamLine) * (_EdgeColor * _EdgeIntensity);
        
            o.Albedo = _EdgeColor + mainTex * _Color;
            
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
