using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doodColor : MonoBehaviour {

    [SerializeField] Color _color;
    [SerializeField] float _happiness;
    MaterialPropertyBlock propertyBlock;
    Renderer renderer;
    public Color color {
        get {
            return _color;
        }
        set {
            _color = value;
            propertyBlock.SetColor("_Color", _color);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }

    public float happiness {
        get {
            return _happiness;
        }
        set {
            _happiness = value;
            propertyBlock.SetFloat("_Happiness", happiness);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }

	void Start () {
        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", color);
        propertyBlock.SetFloat("_Happiness", happiness);
        renderer = GetComponent<MeshRenderer>();
        renderer.SetPropertyBlock(propertyBlock);
    }

}
