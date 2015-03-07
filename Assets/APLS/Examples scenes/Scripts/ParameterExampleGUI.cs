using UnityEngine;
using System.Collections;

public class ParameterExampleGUI : MonoBehaviour {

	public GUISkin skin;

	public Material earth;

	public GameObject ExternalAtmosphere;
	public Material atmosphere;
	
	// diffuse
	private Color diffuseColor;
	private float diffusePower;
	private bool diffuseFullBright;

	// Atmosphere
	private bool enableAtm;
	private Color atmColor;
	private float atmSize;
	private float atmPower;
	private bool atmFullBright;

	//Specular
	private bool enableSpecular;
	private Color SpecularColor;
	private float gloss;

	// Cloud
	private bool enableCloud;
	private float cloudHeight;
	private Color cloudColor;
	private float cloudOpacity;
	private float cloudSpeed;
	private bool cloudFullBright;

	// Emission
	private bool enableEmission;
	private Color emissionColor;
	private float intensity;
	private float emissionFallOff;
	private bool onlyOnShadow;

	// external Atmosphere
	private bool eAtmEnable;
	private float eAtmSize;
	private Color eAtmColor;
	private float eAtmFallOff;
	private bool eAtmFullBright;

	void Start(){
		// Diffuse
		diffuseColor = earth.GetColor( "_DiffuseColor");
		diffusePower = earth.GetFloat( "_DiffusePower");
		diffuseFullBright = false;

		// Atmosphere
		enableAtm = true;
		atmColor = earth.GetColor( "_AtmColor");
		atmSize = earth.GetFloat("_AtmSize");
		atmPower = earth.GetFloat("_AtmPower");
		atmFullBright = false;

		// Specular
		enableSpecular = true;
		SpecularColor = earth.GetColor("_SpecularColor");
		gloss = earth.GetFloat("_Gloss");

		// Cloud
		enableCloud = true;
		cloudHeight = earth.GetFloat("_CloudHeight");
		cloudColor = earth.GetColor("_CloudColor");
		cloudOpacity = earth.GetFloat("_CloudOpacity");
		cloudSpeed = earth.GetFloat("_CloudSpeed");
		cloudFullBright = false;

		// Emission
		enabled = true;
		enableEmission = true;
		intensity = earth.GetFloat("_Intensity");
		emissionFallOff = earth.GetFloat("_EmissionFallOff");
		onlyOnShadow = true;
		emissionColor = earth.GetColor("_EmissionColor");

		// External Atmosphere
		eAtmEnable = true;
		eAtmSize = atmosphere.GetFloat( "_Size");
		eAtmFallOff = atmosphere.GetFloat("_FallOff");
		eAtmFullBright = false;
		eAtmColor = atmosphere.GetColor("_Color");
	}

	void OnGUI(){

		GUI.skin = skin;

		#region Diffuse

		GUI.Box(new Rect(5,5,135,130),"Diffuse");
	
		GUI.color = diffuseColor;
		GUI.backgroundColor = Color.red;
		GUI.Label( new Rect(10,28,10,20),"R");
		GUI.color = Color.white;
		diffuseColor.r = GUI.HorizontalSlider( new Rect(24,32,110,16),diffuseColor.r,0f,1f);

		GUI.color = diffuseColor;
		GUI.backgroundColor = Color.green;
		GUI.Label( new Rect(10,28+16,10,20),"G");
		GUI.color = Color.white;
		diffuseColor.g = GUI.HorizontalSlider( new Rect(24,32+16,110,16),diffuseColor.g,0f,1f);

		GUI.color = diffuseColor;
		GUI.backgroundColor = Color.blue;
		GUI.Label( new Rect(10,28+32,10,20),"B");
		GUI.color = Color.white;
		diffuseColor.b = GUI.HorizontalSlider( new Rect(24,32+32,110,16),diffuseColor.b,0f,1f);

		GUI.backgroundColor = Color.white;

		GUI.Label( new Rect(10,28+56,50,20),"Power");
		diffusePower = GUI.HorizontalSlider( new Rect(50,34+56,82,16),diffusePower,0f,1f);

		diffuseFullBright = GUI.Toggle( new Rect(10,34+75,82,20),diffuseFullBright, " Full bright");


		#endregion
	
		#region Atmosphere
		GUI.Box(new Rect(5,150,135,165),"Atmosphere");
		enableAtm = GUI.Toggle( new Rect(10,150+24,82,20),enableAtm," Enable");

		GUI.color = atmColor;
		GUI.backgroundColor = Color.red;
		GUI.Label( new Rect(10,170+28,10,20),"R");
		GUI.color = Color.white;
		atmColor.r = GUI.HorizontalSlider( new Rect(24,170+32,110,16),atmColor.r,0f,1f);
		
		GUI.color = atmColor;
		GUI.backgroundColor = Color.green;
		GUI.Label( new Rect(10,170+28+16,10,20),"G");
		GUI.color = Color.white;
		atmColor.g = GUI.HorizontalSlider( new Rect(24,170+32+16,110,16),atmColor.g,0f,1f);
		
		GUI.color = atmColor;
		GUI.backgroundColor = Color.blue;
		GUI.Label( new Rect(10,170+28+32,10,20),"B");
		GUI.color = Color.white;
		atmColor.b = GUI.HorizontalSlider( new Rect(24,170+32+32,110,16),atmColor.b,0f,1f);
		
		GUI.backgroundColor = Color.white;

		GUI.Label( new Rect(10,225+28,50,20),"Size");
		GUI.color = Color.white;
		atmSize = GUI.HorizontalSlider( new Rect(50,225+32,82,16),atmSize,0f,1f);

		GUI.Label( new Rect(10,225+28+16,50,20),"Power");
		GUI.color = Color.white;
		atmPower = GUI.HorizontalSlider( new Rect(50,225+32+16,82,16),atmPower,0f,10f);

		GUI.color = Color.white;
		GUI.backgroundColor = Color.white;

		atmFullBright = GUI.Toggle( new Rect(10,225+32+36,82,16),atmFullBright," Full bright");
		#endregion

		#region Specular
		GUI.Box(new Rect(5,330,135,120),"Specular");

		enableSpecular = GUI.Toggle( new Rect(10,330+24,82,20),enableSpecular," Enable");
		
		GUI.color = SpecularColor;
		GUI.backgroundColor = Color.red;
		GUI.Label( new Rect(10,350+28,10,20),"R");
		GUI.color = Color.white;
		SpecularColor.r = GUI.HorizontalSlider( new Rect(24,350+32,110,16),SpecularColor.r,0f,1f);
		
		GUI.color = SpecularColor;
		GUI.backgroundColor = Color.green;
		GUI.Label( new Rect(10,350+28+16,10,20),"G");
		GUI.color = Color.white;
		SpecularColor.g = GUI.HorizontalSlider( new Rect(24,350+32+16,110,16),SpecularColor.g,0f,1f);
		
		GUI.color = SpecularColor;
		GUI.backgroundColor = Color.blue;
		GUI.Label( new Rect(10,350+28+32,10,20),"B");
		GUI.color = Color.white;
		SpecularColor.b = GUI.HorizontalSlider( new Rect(24,350+32+32,110,16),SpecularColor.b,0f,1f);

		GUI.color = Color.white;
		GUI.backgroundColor = Color.white;

		GUI.Label( new Rect(10,350+28+52,50,20),"Gloss");
		gloss = GUI.HorizontalSlider( new Rect(50,350+32+52,82,16),gloss,0f,1f);
		#endregion

		#region Cloud
		GUI.Box(new Rect(820,5,135,180),"Cloud");

		enableCloud = GUI.Toggle( new Rect(825,5+24,82,20),enableCloud," Enable");

		GUI.color = cloudColor;
		GUI.backgroundColor = Color.red;
		GUI.Label( new Rect(825,22+28,10,20),"R");
		GUI.color = Color.white;
		cloudColor.r = GUI.HorizontalSlider( new Rect(839,22+32,110,16),cloudColor.r,0f,1f);
		
		GUI.color = cloudColor;
		GUI.backgroundColor = Color.green;
		GUI.Label( new Rect(825,22+28+16,10,20),"G");
		GUI.color = Color.white;
		cloudColor.g = GUI.HorizontalSlider( new Rect(839,22+32+16,110,16),cloudColor.g,0f,1f);
		
		GUI.color = cloudColor;
		GUI.backgroundColor = Color.blue;
		GUI.Label( new Rect(825,22+28+32,10,20),"B");
		GUI.color = Color.white;
		cloudColor.b = GUI.HorizontalSlider( new Rect(839,22+32+32,110,16),cloudColor.b,0f,1f);
		GUI.color = Color.white;
		GUI.backgroundColor = Color.white;

		GUI.Label( new Rect(825,86+20,50,20),"Opacity");
		cloudOpacity = GUI.HorizontalSlider( new Rect(875,86+24,75,16),cloudOpacity,0f,1f);

		GUI.Label( new Rect(825,86+20+16,50,20),"Speed");
		cloudSpeed = GUI.HorizontalSlider( new Rect(875,86+24+16,75,16),cloudSpeed,-0.1f,0.1f);

		GUI.Label( new Rect(825,86+20+32,50,20),"Height");
		cloudHeight = GUI.HorizontalSlider( new Rect(875,86+24+32,75,16),cloudHeight,0f,1f);


		cloudFullBright = GUI.Toggle( new Rect(825,86+24+48,75,16),cloudFullBright," Full bright");
		#endregion

		#region Emission
		GUI.Box(new Rect(820,200,135,160),"Emission");

		enableEmission = GUI.Toggle( new Rect(825,200+24,82,20),enableEmission," Enable");

		GUI.color = emissionColor;
		GUI.backgroundColor = Color.red;
		GUI.Label( new Rect(825,221+28,10,20),"R");
		GUI.color = Color.white;
		emissionColor.r = GUI.HorizontalSlider( new Rect(839,221+32,110,16),emissionColor.r,0f,1f);
		
		GUI.color = emissionColor;
		GUI.backgroundColor = Color.green;
		GUI.Label( new Rect(825,221+28+16,10,20),"G");
		GUI.color = Color.white;
		emissionColor.g = GUI.HorizontalSlider( new Rect(839,221+32+16,110,16),emissionColor.g,0f,1f);
		
		GUI.color = emissionColor;
		GUI.backgroundColor = Color.blue;
		GUI.Label( new Rect(825,221+28+32,10,20),"B");
		GUI.color = Color.white;
		emissionColor.b = GUI.HorizontalSlider( new Rect(839,221+32+32,110,16),emissionColor.b,0f,1f);
		GUI.color = Color.white;
		GUI.backgroundColor = Color.white;

		GUI.Label( new Rect(825,285+16,50,20),"Intensity");
		intensity = GUI.HorizontalSlider( new Rect(875,285+20,75,16),intensity,0f,1f);

		GUI.Label( new Rect(825,285+16+16,50,20),"Fall Off");
		emissionFallOff = GUI.HorizontalSlider( new Rect(875,285+20+16,75,16),emissionFallOff,0f,1f);

		onlyOnShadow = GUI.Toggle(new Rect(825,285+16+16+20,150,20),onlyOnShadow," Only on shadow");
		#endregion

		#region External atmosphere
		GUI.Box(new Rect(820,375,135,180),"External Atmosphere");

		eAtmEnable = GUI.Toggle( new Rect(825,375+24,82,20),eAtmEnable," Enable");

		GUI.color = eAtmColor;
		GUI.backgroundColor = Color.red;
		GUI.Label( new Rect(825,375+24+28,10,20),"R");
		GUI.color = Color.white;
		eAtmColor.r = GUI.HorizontalSlider( new Rect(839,375+24+32,110,16),eAtmColor.r,0f,1f);
		
		GUI.color = eAtmColor;
		GUI.backgroundColor = Color.green;
		GUI.Label( new Rect(825,375+24+28+16,10,20),"G");
		GUI.color = Color.white;
		eAtmColor.g = GUI.HorizontalSlider( new Rect(839,375+24+32+16,110,16),eAtmColor.g,0f,1f);
		
		GUI.color = eAtmColor;
		GUI.backgroundColor = Color.blue;
		GUI.Label( new Rect(825,375+24+28+32,10,20),"B");
		GUI.color = Color.white;
		eAtmColor.b = GUI.HorizontalSlider( new Rect(839,375+24+32+32,110,16),eAtmColor.b,0f,1f);
		GUI.color = Color.white;
		GUI.backgroundColor = Color.white;

		GUI.Label( new Rect(825,459+20,50,20),"Size");
		eAtmSize = GUI.HorizontalSlider( new Rect(875,459+24,75,16),eAtmSize,0.5f,10f);

		GUI.Label( new Rect(825,459+20+16,50,20),"Fall Off");
		eAtmFallOff = GUI.HorizontalSlider( new Rect(875,459+24+16,75,16),eAtmFallOff,1f,15f);

		eAtmFullBright = GUI.Toggle(new Rect(825,459+24+16+16,150,20),eAtmFullBright," Full Bright");
		#endregion


		if (GUI.changed){
			#region Diffuse
			earth.SetColor( "_DiffuseColor", diffuseColor);
			earth.SetFloat( "_DiffusePower",diffusePower);
			if (diffuseFullBright){
				earth.SetInt( "_DiffuseFullBright",1);
			}
			else{
				earth.SetInt( "_DiffuseFullBright",0);
			}
			#endregion

			#region Atmosphere
			if (enableAtm){
				earth.SetInt( "_EnableAtm",1);
			}
			else{
				earth.SetInt( "_EnableAtm",0);
			}
			earth.SetColor( "_AtmColor",atmColor);
			earth.SetFloat( "_AtmSize",atmSize);
			earth.SetFloat( "_AtmPower",atmPower);

			if (atmFullBright){
				earth.SetInt( "_AtmFullBright",1);
			}
			else{
				earth.SetInt( "_AtmFullBright",0);
			}
			#endregion

			#region Specular
			if (enableSpecular){
				earth.SetInt( "_EnableSpecular",1);
			}
			else{
				earth.SetInt( "_EnableSpecular",0);
			}
			earth.SetColor("_SpecularColor",SpecularColor);
			earth.SetFloat("_Gloss",gloss);
			#endregion

			#region Cloud
			if (enableCloud){
				earth.SetInt( "_EnableCloud",1);
			}
			else{
				earth.SetInt( "_EnableCloud",0);
			}

			earth.SetColor("_CloudColor",cloudColor);
			earth.SetFloat( "_CloudHeight",cloudHeight);
			earth.SetFloat( "_CloudOpacity",cloudOpacity);
			earth.SetFloat( "_CloudSpeed",cloudSpeed);

			if (cloudFullBright){
				earth.SetInt( "_CloudFullBright",1);
			}
			else{
				earth.SetInt( "_CloudFullBright",0);
			}
			#endregion

			#region Emission
			if (enableEmission){
				earth.SetInt( "_EnableEmission",1);
			}
			else{
				earth.SetInt( "_EnableEmission",0);
			}

			earth.SetColor("_EmissionColor",emissionColor);
			earth.SetFloat("_Intensity",intensity);
			earth.SetFloat("_EmissionFallOff",emissionFallOff);

			if (onlyOnShadow){
				earth.SetFloat("_OnlyOnShadow",1);
			}
			else{
				earth.SetFloat("_OnlyOnShadow",0);
			}
			#endregion

			ExternalAtmosphere.SetActive( eAtmEnable );
			atmosphere.SetColor("_Color",eAtmColor);
			atmosphere.SetFloat("_Size",eAtmSize);
			atmosphere.SetFloat("_FallOff",eAtmFallOff);

			if (eAtmFullBright){
				atmosphere.SetFloat("_FullBright",1);
			}
			else{
				atmosphere.SetFloat("_FullBright",0);
			}
		}
	}

}
