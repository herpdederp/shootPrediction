using UnityEngine;
using System.Collections;
using Bolt;

public class PlayerController : Bolt.EntityEventListener<IPlayerState>
{
    public GameObject projectile;
    public LayerMask validLayers = new LayerMask();
    public CharacterController _cc;

    public override void Attached()
    {
        _cc = GetComponent<CharacterController>();
        state.SetTransforms(state.transform, transform);
    }



    public override void SimulateController()
    {
        IPlayerCommandInput input = PlayerCommand.Create();
        input.forward = Input.GetKey(KeyCode.W);
        input.back = Input.GetKey(KeyCode.S);
        input.left = Input.GetKey(KeyCode.A);
        input.right = Input.GetKey(KeyCode.D);


        Vector3 position = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000, validLayers);


        if (Input.GetMouseButtonDown(0))
            foreach (RaycastHit hit in hits)
            {
                //Debug.Log(hit);
                if (!hit.collider.isTrigger)
                {
                    //input.shootDirection = (transform.position - hit.point).normalized;
                    Vector3 t = (hit.point - transform.position);
                    t.y = 0;
                    input.shootDirection = t;

                    break;
                }
            }


        entity.QueueInput(input);

    }

    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand)command;

        if (resetState)
        {
            //owner has sent a correction to the controller
            transform.position = cmd.Result.position;

        }
        else
        {
            if (cmd.IsFirstExecution)
            {
                if (cmd.Input.shootDirection != Vector3.zero)
                {
                    if (BoltNetwork.isServer)
                    {
                        BoltEntity BE = BoltNetwork.Instantiate(BoltPrefabs.Projectile, transform.position, Quaternion.identity);
                        BE.transform.LookAt(transform.position + cmd.Input.shootDirection);
                        BE.GetComponent<ProjectileController>().state.frame = cmd.ServerFrame;
                    }
                    else
                    {
                        GameObject GO = GameObject.Instantiate(projectile, transform.position, Quaternion.identity);
                        GO.transform.LookAt(transform.position + cmd.Input.shootDirection);
                        GO.GetComponent<ProjectileFakeController>().frame = cmd.ServerFrame;
                    }
                }
            }

            if (cmd.Input.forward)
                _cc.Move(Vector3.forward * Time.fixedDeltaTime * 10f);
            else if (cmd.Input.back)
                _cc.Move(Vector3.back * Time.fixedDeltaTime * 10f);
            if (cmd.Input.left)
                _cc.Move(Vector3.left * Time.fixedDeltaTime * 10f);
            else if (cmd.Input.right)
                _cc.Move(Vector3.right * Time.fixedDeltaTime * 10f);


            cmd.Result.position = transform.position;

        }
    }

}
