using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFakeController : MonoBehaviour
{
    public bool found;

    public int frame;

    public GameObject graphicPrefab;

    public GameObject graphic;
    // Use this for initialization
    void Start()
    {
        graphic = GameObject.Instantiate(graphicPrefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (found == false)
        {
            graphic.transform.position = transform.position;
            graphic.transform.rotation = transform.rotation;
        }
    }
    private void FixedUpdate()
    {

        transform.Translate(Vector3.forward * Time.fixedDeltaTime * 10f);
    }
}
