IMPORT AND INFO FOR HD RP 5.7 - very fluid and unstable.
	- trees use unity lit shaders, they could have broken bend. Unity is in process of changing shaders. 
	- In lower hd rp versions wind work fine in higher until 5.x seams they are broken. 
	We wait for final engine solution for wind until we provide own solution. Seams it's very fluid now.
	- dynamic snow cover on terrain and trees is not working yet
	- hd rp 5.7 doesnt support any grass at terrain yet. We found info about grass in 6.x hd rp. 
	This means proper version of this pack must wait.

	BEFORE YOU START:
		- you need Unity 2019.1 
		- you need HD SRP pipline 5.7, if you use higher etc custom shaders will not work. 
		- wind setup is gone in 5.7 but materials work. It will back just be patient.
		Be patient this tech is so fluid... we coudn't fallow ever beta version


	Step 1 - Setup Shadows and other render setups.
	Find File "HDRenderPipelineAsset" and shadow atlas width and height to 4096 or 2048.
	Find Material section at "HDRenderPipelineAsset" and drag and drop our SSS settings diffusion profiles for foliage into Diffusion profile list:
		NM_SSSSettings_Skin_Foliage
		NM_SSSSettings_Skin_NM Foliage
		NM_SSSSettings_Skin_NM Foliage Trees
	Without this foliage materials will not become affected by scattering.
	Optionaly turn on "increase resolution of volumetrics (a bit expensive but I didn't notice big drop so..) 

IMPORT AND INFO FOR HD RP 4.9 - very fluid and unstable.
	Nothing additional have to be done but
	- translucency at ice shaders is mostly broken (we hope to fix that in upcomming rp versions)
	- trees use unity lit shaders, they could have broken bend. Unity is in process of changing shaders. 
	- In lower hd rp versions wind work fine in higher until 5.x seams they are broken. 
	We wait for final engine solution for wind until we provide own solution. Seams it's very fluid now.
	- dynamic snow cover on terrain and trees is not working yet
	- hd rp 4.9 doesnt support any grass at terrain yet. We found info about grass in 6.x hd rp. 
	This means proper version of this pack must wait. Anyway 
	- few ice world demo crash on hd rp 4.9 We wait until unity will fix that.
