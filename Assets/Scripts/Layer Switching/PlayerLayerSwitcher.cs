﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.Experimental.Input;

public class PlayerLayerSwitcher : MonoBehaviour, ILayerSwitchingActions
{
    public GameObject player;
    public GameObject foreground;
    public GameObject midground;
    public GameObject background;
    private LayerMask playerLayer;
    private LayerMask foregroundLayer;
    private LayerMask midgroundLayer;
    private LayerMask foregroundLaserLayer;
    private LayerMask midgroundLaserLayer;
    private int foregroundSortingLayer;
    private int midgroundSortingLayer;
    private BoxCollider2D playerCollider;
    private int currentPlayerLayer;
    private bool collisionPlayerFeedbackHappening = false;
    private float collisionPlayerFeedbackZPos;
    private float collisionPlayerFeedbackStartZPos;
    private LayerMask overlapLayerMask;
    private Collider2D[] detectedOverlaps;

    public PlayerInputMapping playerInput;

    public void Awake() {
        playerInput.LayerSwitching.SetCallbacks(this);
    }

    //call this function to switch the player to a layer (foreground, background, etc...)
    public void SwitchPlayerLayer(int destinationLayerNum)
    {
        if (destinationLayerNum != currentPlayerLayer) {
            overlapLayerMask = LayerMask.GetMask(LayerMask.LayerToName(destinationLayerNum));
            // detects when the player will overlap/ collide with the layer they are attempting to switch to and returns (exits function without doing anything);
            Collider2D targetLayerCollisions = Physics2D.OverlapCircle(player.transform.position, (float)0.1, overlapLayerMask);
            if (targetLayerCollisions != null && targetLayerCollisions.isTrigger == false)
            {
                collisionPlayerFeedbackHappening = true;
                if (destinationLayerNum == foregroundLayer.value) {
                    collisionPlayerFeedbackStartZPos = midground.transform.position.z;
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, midground.transform.position.z - (float)0.5);
                }
                if (destinationLayerNum == midgroundLayer.value) {
                    collisionPlayerFeedbackStartZPos = foreground.transform.position.z;
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, foreground.transform.position.z + (float)0.5);
                }
            }
            else
            {
                if (destinationLayerNum == foregroundLayer.value)
                {
                    // set the player's collision mask & depth/z position to be that of the target layer (fore)
                    Physics2D.IgnoreLayerCollision(playerLayer, foregroundLayer, false);
                    Physics2D.IgnoreLayerCollision(playerLayer, foregroundLaserLayer, false);
                    Physics2D.IgnoreLayerCollision(playerLayer, midgroundLayer, true);
                    Physics2D.IgnoreLayerCollision(playerLayer, midgroundLaserLayer, true);
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, foreground.transform.position.z);
                    ChangeLayerOpacity(foreground, 1);
                    Util.SetSortingLayerRecursively(player, foregroundSortingLayer);
                }
                else if (destinationLayerNum == midgroundLayer.value)
                {
                    // set the player's collision mask & depth/z position to be that of the target layer (midground)
                    Physics2D.IgnoreLayerCollision(playerLayer, midgroundLayer, false);
                    Physics2D.IgnoreLayerCollision(playerLayer, midgroundLaserLayer, false);
                    Physics2D.IgnoreLayerCollision(playerLayer, foregroundLayer, true);
                    Physics2D.IgnoreLayerCollision(playerLayer, foregroundLaserLayer, true);
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, midground.transform.position.z);
                    // fade the foreground so the the midground is more visible
                    ChangeLayerOpacity(foreground, (float)0.8);
                    Util.SetSortingLayerRecursively(player, midgroundSortingLayer);
                }
                /*
                 * Set the player's own layer to the desired layer integer so that the player will
                 * collide with that layer (self-collision is setup in the layer properites). Calls
                 * SetLayerRecursively so that all children of player will also take the same layer.
                 * This is necessary for changing the interaction trigger, joints, etc.
                 */

                // set everything straight after switching layers.
                currentPlayerLayer = destinationLayerNum;

            }
        }
    }


    // Use this for initialization
    void Start()  {
        if (foreground == null || midground == null || background == null) {
            gameObject.GetComponent<PlayerLayerSwitcher>().enabled = false;
            if (foreground == null) foreground = GameObject.Find("foreground");
            if (midground == null) midground = GameObject.Find("midground");
            if (background == null) background = GameObject.Find("background");
        }
        playerLayer = LayerMask.NameToLayer("Player");
        foregroundLayer = LayerMask.NameToLayer("Foreground");
        foregroundLaserLayer = LayerMask.NameToLayer("Foreground Laser Transparent");
        midgroundLayer = LayerMask.NameToLayer("Midground");
        midgroundLaserLayer = LayerMask.NameToLayer("Midground Laser Transparent");
        currentPlayerLayer = foregroundLayer.value;
        foregroundSortingLayer = foreground.GetComponentInChildren<Renderer>().sortingLayerID;
        midgroundSortingLayer = midground.GetComponentInChildren<Renderer>().sortingLayerID;
        // Debug.Log(foregroundSortingLayer);



    }

    void Update()
    {
        if (collisionPlayerFeedbackHappening) {
            // Moves the player back to the z depth of the current layer to give the "jerk" effect
            print(player.transform.position.z - collisionPlayerFeedbackStartZPos);

            player.transform.Translate(new Vector3 (0,0, collisionPlayerFeedbackStartZPos - player.transform.position.z) * Time.deltaTime * 5);
            if (Mathf.Abs(collisionPlayerFeedbackStartZPos - player.transform.position.z)  < 0.02 || (Mathf.Abs(collisionPlayerFeedbackStartZPos - player.transform.position.z) > 1)) collisionPlayerFeedbackHappening = false;
        }
    }


    void ChangeLayerOpacity(GameObject layerRoot, float opaqueness) {
        // Get all tilemap renderer components in all the children of the layerRoot gameObject
        TilemapRenderer[] tilemapRenderers = layerRoot.GetComponentsInChildren<TilemapRenderer>();
        // for every tilemapRender component, set its material's "tint color" to be transparent (specified by the opaqueness arguement)
        for (int i = 0, tilemapRenderersLength = tilemapRenderers.Length; i < tilemapRenderersLength; i++) {
            TilemapRenderer mapRenderer = tilemapRenderers[i];
            mapRenderer.material.color = new Color(mapRenderer.material.color.r, mapRenderer.material.color.g, mapRenderer.material.color.b, opaqueness);
        }
        // Do the same for all sprite renderer components:
        SpriteRenderer[] spriteRenderers = layerRoot.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0, spriteRenderersLength = spriteRenderers.Length; i < spriteRenderersLength; i++) {
            SpriteRenderer mapRenderer = spriteRenderers[i];
            mapRenderer.material.color = new Color(mapRenderer.material.color.r, mapRenderer.material.color.g, mapRenderer.material.color.b, opaqueness);
        }
    }

    public void OnToggleLayerSwitching(InputAction.CallbackContext context) {
        if (context.performed && !PauseMenu.isPaused)
        {
            if (currentPlayerLayer == foregroundLayer.value) {
                SwitchPlayerLayer(midgroundLayer.value);
           } else {
                SwitchPlayerLayer(foregroundLayer.value);
            }
        }
    }
}
