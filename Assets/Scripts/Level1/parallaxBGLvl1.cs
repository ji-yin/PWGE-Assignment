/*
    Created by Eshan Kang
	For parallax background effect in lvl1
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxBGLvl1 : MonoBehaviour {

    [SerializeField] private Vector2 multiplier;

    private Transform camTransform;
    private Vector3 lastCamPos;
    private float textureUnitSizeX;
    private float textureUnitSizeY;

    private void Start() {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void Update() {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * multiplier.x, deltaMovement.y * multiplier.y);
        lastCamPos = camTransform.position;
    }
}
