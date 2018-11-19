using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Bolt.EntityEventListener<IProjectileState>
{
    float maxFrames = 50;

    float smoothFrames;

    // int frame;
    bool init = false;

    ProjectileFakeController projectileFakeController;

    public override void Attached()
    {
        state.SetTransforms(state.transform, transform);

    }

    // Use this for initialization
    void Start()
    {
        smoothFrames = maxFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (projectileFakeController != null)
        {
            if (smoothFrames > 0)
                smoothFrames--;
            projectileFakeController.graphic.transform.position = Vector3.Lerp(transform.position, projectileFakeController.transform.position, (smoothFrames / maxFrames));
            projectileFakeController.graphic.transform.rotation = Quaternion.Lerp(transform.rotation, projectileFakeController.transform.rotation, (smoothFrames / maxFrames));
        }
    }

    private void FixedUpdate()
    {
        if (BoltNetwork.isServer)
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * 10f);
        else
        {
            if (init == false)
            {
                if (state.frame != 0)
                {
                    var ArrayPFC = GameObject.FindObjectsOfType<ProjectileFakeController>();

                    foreach (ProjectileFakeController PFC in ArrayPFC)
                    {
                        if (PFC.frame == state.frame)
                        {
                            init = true;

                            projectileFakeController = PFC;
                            projectileFakeController.found = true;
                            break;
                        }
                    }

                }

            }
        }
    }
}
