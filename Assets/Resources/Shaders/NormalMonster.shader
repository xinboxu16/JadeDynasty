// Unlit shader. Simplest possible textured shader.
// - DFM Simplest Shader

Shader "DFM/NormalMonster" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Tags {"RenderType"="Opaque" "Queue"="Transparent-10"}//表示Transparent(3000)渲染队列值减10
	LOD 100

	// Non-lightmapped
	Pass {
		Name "NORMALMONSTER"
		Tags {"LightMode" = "Vertex"}
		Lighting Off //定义材质块中的设定是否有效
		SetTexture [_MainTex] { combine texture }
	}
}
}
