using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour 
{
	private Rigidbody2D rb2d;

    Vector3 vecOriginPos;

    private void Awake()
    {
        CManagerReplay.instance.p_Event_OnChange_ReplayState.Subscribe += OnReplay;
    }

    private void OnEnable()
    {
        vecOriginPos = transform.position;
    }

    private void OnDestroy()
    {
        CManagerReplay.instance?.p_Event_OnChange_ReplayState.DoRemove_Listener(OnReplay);
    }

    private void OnReplay(CManagerReplay.EReplayState eState)
    {
        if(eState == CManagerReplay.EReplayState.Replaying)
        {
            transform.position = vecOriginPos;
            rb2d.velocity = new Vector2(GameControl.instance.scrollSpeed, 0);
        }
    }

    // Use this for initialization
    void Start () 
	{
		//Get and store a reference to the Rigidbody2D attached to this GameObject.
		rb2d = GetComponent<Rigidbody2D>();

		//Start the object moving.
		rb2d.velocity = new Vector2 (GameControl.instance.scrollSpeed, 0);
	}

	void Update()
	{
		// If the game is over, stop scrolling.
		if(GameControl.instance.gameOver == true)
		{
			rb2d.velocity = Vector2.zero;
		}
	}
}
