using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
//Edited by: Josh Bromberg: Student Number 301063558
public class PlayerController : MonoBehaviour
{
    public Speed speed;
    public Boundary boundary;

    public GameController gameController;

    // private instance variables
    private AudioSource _thunderSound;
    private AudioSource _yaySound;

    private string[] directionString = { "Horizontal", "Vertical" }; //String for changing direction
    private bool getFinalController; //bool to see if the final gameController has been chosen

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _thunderSound = gameController.audioSources[(int)SoundClip.THUNDER];
            _yaySound = gameController.audioSources[(int)SoundClip.YAY];
        }
        catch (System.NullReferenceException) { }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBounds();
    }

    public void Move()
    {
        Vector2 newPosition = transform.position;

        #region Set Game Controller
        if (getFinalController == false)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            getFinalController = true;
            _thunderSound = gameController.audioSources[(int)SoundClip.THUNDER];
            _yaySound = gameController.audioSources[(int)SoundClip.YAY];
        }
        #endregion
        int axisSetter = gameController.Level == 0 ? 0 : 1; //Grabs the right axis for the player to use
        int speedModulator = gameController.Level % 2 == 1 ? -1 : 1; //Inverse speed when on even levels (odd in code)

        if (Input.GetAxis(directionString[axisSetter]) > 0.0f)
        {
            newPosition += new Vector2(speed.max*speedModulator, 0.0f);
        }

        if (Input.GetAxis(directionString[axisSetter]) < 0.0f)
        {
            newPosition += new Vector2(speed.min*speedModulator, 0.0f);
        }

        transform.position = newPosition;
    }

    public void CheckBounds()
    {
        // check right boundary
        if(transform.position.x > boundary.Right)
        {
            transform.position = new Vector2(boundary.Right, transform.position.y);
        }

        // check left boundary
        if (transform.position.x < boundary.Left)
        {
            transform.position = new Vector2(boundary.Left, transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "Cloud":
                _thunderSound.Play();
                gameController.Lives -= 1;
                break;
            case "Island":
                _yaySound.Play();
                gameController.Score += 100;
                break;
        }
    }

}
