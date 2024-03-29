using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

//Can be destroyed by GameObjects that invoke break on collision
[RequireComponent(typeof(Collider2D), typeof(Renderer), typeof(AudioSource))]
public class Breakable : Invoker
{
    [Header("Breakable")]
    [SerializeField] private bool m_requiresAshe = true;
    [SerializeField, Tooltip("No Implementation with this one yet")]
    private bool m_requiresTinker = false;
    [SerializeField] private bool restrictPhysicalBreaking = false;
    private bool broken = false;
    [SerializeField] private float velocityImpact = 3f;

    // References
    private ParticleSystem m_particle;
    private BoxCollider2D m_collider;
    private TilemapRenderer m_renderer;
    private AudioSource m_AudioSource;

    private void Start() 
    {
        m_particle = gameObject.GetComponent<ParticleSystem>();
        m_collider = gameObject.GetComponent<BoxCollider2D>();
        m_renderer = gameObject.GetComponentInChildren<TilemapRenderer>();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
    }
    public void ActivateBreak()
    {
        m_AudioSource.Play();
        StartCoroutine(Break());
        broken = true;
    }
    public IEnumerator Break()
    {
        m_particle.Play();
        m_collider.enabled = false;
        Activate();
        //renderer.enabled = false;
        // Replaced with this in order to destroy floating nails
        Destroy(m_renderer.gameObject);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Projectile>())
            {
                child.SetParent(Pooler.Instance.transform, false);
                child.gameObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(m_particle.main.startLifetime.constantMax);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (restrictPhysicalBreaking) return;
        if (m_requiresAshe && m_requiresTinker) { Debug.Log("If and only if tinekr n ash lol"); }
        else if (!broken && (m_requiresAshe && collision.gameObject.tag == "Gauntlet"))
        {
            m_AudioSource.Play();
            var pad = Gamepad.current;
            if (pad != null)
            {
                pad.SetMotorSpeeds(10f, 24f);
            }
            StartCoroutine(Break());
            broken = true;
        }
    }
    // && (collision.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f 
    //        || collision.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f))
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (restrictPhysicalBreaking) return;
        var rb = collider.gameObject.GetComponent<Rigidbody2D>();
        var vel = Vector2.zero;
        if (rb != null) vel = rb.velocity;

        if (!broken && ((m_requiresAshe && collider.gameObject.tag == "Gauntlet") 
            || ((collider.gameObject.tag == "physical") && (Mathf.Abs(vel.x) >= velocityImpact || Mathf.Abs(vel.y) >= velocityImpact))))
        {
            m_AudioSource.Play();
            StartCoroutine(Break());
            broken = true;
        }
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        yield return null;
    }
    // && (collider.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f
    //        || collider.gameObject.GetComponent<Rigidbody2D>().velocity.x >= 2f))
}
