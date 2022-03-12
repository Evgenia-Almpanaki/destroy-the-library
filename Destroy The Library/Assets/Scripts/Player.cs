using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private const float LIGHT_AREA_INCREASE_AMOUNT = 0.4F;
    private new Light2D light;
    private Vector3 velocity; 
    private float lightArea = 1f;
    private float lightDecreasingSpeed = 0.1f;
    private float speed = 3f;

    private void Awake()
    {
        light = this.GetComponent<Light2D>();
    }

    private void Update()
    {
        if (lightArea > 1)
        {
            lightArea -= Time.deltaTime * lightDecreasingSpeed;
            light.pointLightOuterRadius = lightArea; 
        }
        Move();
    }
    
    /// <summary>
    /// Moves based on velocity vector.
    /// </summary>
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                 transform.position + velocity, 
                                                 Time.deltaTime * speed);
    }

    /// <summary>
    /// Increases the light area if the player uses a match effectively.
    /// </summary>
    /// <param name="context"></param>
    public void OnLightMatch(InputAction.CallbackContext context) {
        
        context.action.started += (context) =>
        {
            if (GamePlay.UseMatch())
                IncreaseLightArea();
        };
    }

    /// <summary>
    /// Changes the velocity of the object.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        velocity = new Vector3(context.ReadValue<Vector2>().x,
                               context.ReadValue<Vector2>().y,
                               0);
    }

    /// <summary>
    /// Increases the light area per fixed amount.
    /// </summary>
    private void IncreaseLightArea()
    {
        this.lightArea += LIGHT_AREA_INCREASE_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Element>() && collision.GetComponent<Element>().IsElementType(Element.Type.Door))
        {
            LevelManager.LoadNextScene();
        }
    }
}
