Shader "Examples/StencilGeom"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        // Pass that writes the stencil
        Pass
        {
            Blend Zero One
            ZWrite Off
            ZTest LEqual

            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
                Fail Keep
            }
        }

        // Pass that reads stencil and draws only behind the stencil
        Pass
        {
            ZWrite On
            ZTest LEqual

            Stencil
            {
                Ref [_StencilID]
                Comp Equal  // Only draw where stencil matches
                Pass Keep
                Fail Keep
            }
        }
    }
}
