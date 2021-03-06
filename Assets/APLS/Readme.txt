**********************************************
			Advanced Planet Shader
				
		Copyright © 2014 Dark Anvil
		  http://www.darkanvil.com

		contact@darkanvil.com
**********************************************

A package of 5 shaders to create planet  surfaces. Allows creation of planet like earth, sun, volcanic planet, etc ...<br>

Create the media takes a lot of time if you are happy with your purchase, or not   
do not hesitate to post a review on the unity store, which will encourage us to continue.


**********************************************
Shaders
**********************************************

Atmosphere
===========
This shader creates an external atmosphere effect , it should be applied to a second object other than your planet.

Planet BA / BAE / BSACE
=======================
B = bumped
A = Atmosphere
E = Emission
S = Specular
C = Cloud

Allows creation of the simplest to the most complex texture

Planet Ring
===========
Allows creation of simple ring surface.


**********************************************
Shader parameters
**********************************************

Diffuse parameters
==================
Enable Ambient 	: Taking into account ambient light
Diffuse Map 	: Color texture
Normal Map		: Normal map for bumped effect
Diffuse Power	: Hardness between lighting and shading
Full right		: Color texture will not be affected by light source

Specular parameters
===================
Specular Color	: color of the specular
Specular Map	: Specular texture (white = specular, black = no specular)
Gloss			: shine

Atmosphere parameters
=====================
Enable Atm	: Enable or not atmosphere effect
Atm Color	: Atmosphere color
Atm Power	: Color Intensity
Atm Size	: Atmosphere size
Atm Full Bright : Atmosphere effect  will not be affected by light source

Emission parameters
===================
The texture is applied with the blend "Color Burn" for Planet BAE & a simple "add" for Planet BSACE

Emission Color	: Emisson Color
Emission Map	: Emission texture. The black color doesn't emits
Only Shadow		: The texture is visible only on the shaded portion of the object
Emission FallOf	: Progressive disappearance, if "Only Shadow"=  true
Intensity		: Intensity of the effect. (No recognized if "Pulsation" = true)
Pulsation		: Enables pulsation effect 
Min Intensity	: Minimum intensity
Max IPntensity	: Maximum intensity
Pulsation Speed	: Pulsation speed

Cloud parameters
================
Cloud Color		: Color of clouds
Cloud Map		: Cloud texture "black color = transparent"
Cloud height	: Height of the cloud layer
Cloud Opacity	: Cloud transparency 
Cloud Speed		: Cloud speed (can be negative)