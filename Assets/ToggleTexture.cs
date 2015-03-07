using System.Net.Mime;
using Assets.Scripts.GameComponents;
using UnityEngine;
using System.Collections;

public class ToggleTexture : MonoBehaviour
{
    public string TextureName;
    public GameObject Planet;
    private bool _enabled = true;
    private Texture2D _oldText;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        gameObject.GetComponent<GUIText>().color = new Color(10 / 255f, 120 / 255f, 174 / 255f);
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<GUIText>().color = Color.white;
    }

    void OnMouseUp()
    {
        if (_enabled)
        {
            _oldText = Planet.transform.GetComponent<Renderer>().material.GetTexture(TextureName) as Texture2D;
            Planet.transform.GetComponent<Renderer>().material.SetTexture(TextureName, null);
            _enabled = false;
            gameObject.GetComponent<GUIText>().text = "Enable " + TextureName;
        }
        else
        {
            Planet.transform.GetComponent<Renderer>().material.SetTexture(TextureName, _oldText);
            _oldText = null;
            _enabled = true;
            gameObject.GetComponent<GUIText>().text = "Disable " + TextureName;
        }
    }
}