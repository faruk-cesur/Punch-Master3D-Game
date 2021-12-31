﻿//base include for outline
#ifndef MK_TOON_OUTLINE_ONLY_BASE
	#define MK_TOON_OUTLINE_ONLY_BASE
	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputOutlineOnly outlinevert(VertexInputOutlineOnly v)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		VertexOutputOutlineOnly o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputOutlineOnly, o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		v.vertex.xyz += normalize(v.normal) * _OutlineSize;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = _OutlineColor;
		//texcoords
		#if _MKTOON_DISSOLVE
			o.uv = TRANSFORM_TEX(v.texcoord0, _DissolveMap);
		#endif
		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	fixed4 outlinefrag(VertexOutputOutlineOnly o) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(o);
		#if _MKTOON_DISSOLVE
			fixed3 sg =  tex2D (_DissolveMap, o.uv).r - _DissolveAmount;
			Clip0(sg);
		#endif
		#if _MK_MODE_CUTOUT
			//skip fragment in cutoff mode
			if(o.color.a < _Cutoff) discard;
		#endif

		#if _MK_DISSOLVE_RAMP
			//apply color for dissolving
			o.color.rgb = DissolveRamp(sg, _DissolveRamp, _DissolveColor, _DissolveRampSize, _DissolveAmount, o.uv, o.color);
		#endif

		UNITY_APPLY_FOG(o.fogCoord, o.color);
		return o.color;
	}
#endif