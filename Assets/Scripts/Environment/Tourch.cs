using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourch : MonoBehaviour
{
    [Header("Tourch Specs")]
    private Animator m_animator;
    private float duration_swap;
    private float duration_wind;
    private void Start()
    {
       m_animator = GetComponent<Animator>();
       duration_swap = Random.Range(1f, 3f) + Time.time;
       duration_wind = Random.Range(8f, 20f) + Time.time;
    }

    private void Update()
    {
        if (Time.time >= duration_swap)
        {
            m_animator.SetBool("left", !m_animator.GetBool("left"));
            duration_swap = Random.Range(3f, 10f) + Time.time;
        }
        if (Time.time >= duration_wind)
        {
            m_animator.ResetTrigger("Wind");
            m_animator.SetTrigger("Wind");
            duration_wind = Random.Range(8f, 20f) + Time.time;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gauntlet" || collision.gameObject.layer == LayerMask.GetMask("Players"))
        {
            // TODO: Fix this logic for player swoosh
            bool punchLeft = collision.gameObject.transform.parent.GetComponent<Animator>().GetFloat("MoveX") > 0;
            m_animator.Play(punchLeft ? "Touch_wind_LR" : "Touch_wind_RL");
            m_animator.SetBool("left", punchLeft);
            duration_wind = Random.Range(8f, 20f) + Time.time;
        }
    }
}