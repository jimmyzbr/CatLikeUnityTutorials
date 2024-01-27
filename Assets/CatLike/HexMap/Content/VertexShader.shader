Shader "MyURPShader/VertexShader"
{
    Properties
    {
        [MainTexture] _MainTex("MainTex", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
       Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }


        Pass
        {
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader.
            #pragma vertex vert
            // This line defines the name of the fragment shader.
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // To make the Unity shader SRP Batcher compatible, declare all
            // properties related to a Material in a a single CBUFFER block with
            // the name UnityPerMaterial.
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _BaseColor variable, so that you
                // can use it in the fragment shader.
                half4 _BaseColor;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
                float4 color               		: COLOR;
                float2 uv : TEXCOORD0;
            };

            // The vertex shader definition with properties defined in the Varyings
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous clip space.
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                // Returning the output.
                return OUT;
            }

            // The fragment shader definition.
            half4 frag(Varyings IN) : SV_Target
            {
                // Defining the color variable and returning it.
                float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
                return baseTex * _BaseColor;
            }
            ENDHLSL
        }
    }
}
