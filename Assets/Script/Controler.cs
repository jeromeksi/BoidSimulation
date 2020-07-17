using Assets.Script;
using UnityEngine;

public class Controler : MonoBehaviour
{
    // Start is called before the first frame update
    private static System.Random rand = new System.Random();
    public bool showGizmo = false;
    public float radiusDetectionWall = 1;

    public float radiusClose = 0.1f;
    public float radiusToFar = 0.1f;
    public float radiusToOptimize = 0.1f;

    public static bool activAlgo = true;

    public bool test = false;
    public float SpeedIndice = 1f;
    private float SpeedCalc;

    public LayerMask wall;
    public LayerMask boid;
    public bool showDebug = false;

    public bool RandRotateStart = true;


    private void OnDrawGizmos()
    {
        if (showGizmo)
        {

            Gizmos.color = Color.red;
            Vector2 currentPos = transform.localPosition;
            Gizmos.DrawWireSphere(currentPos, radiusClose);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(currentPos, radiusToFar);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentPos, radiusToOptimize);
            Gizmos.color = Color.red;
        }
    }
   
    void Start()
    {
        if (RandRotateStart)
            this.transform.Rotate(new Vector3(0, 0, 1) * rand.Next(0, 360));
        SpeedCalc = SpeedIndice;

    }
    void FixedUpdate()
    {
        if (activAlgo)
        {
            Algo();
        }
        else
        {
            RotateRand();
        }
        Move();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "WallL":
                transform.position = new Vector2(8.91f, transform.position.y);
                break;
            case "WallR":
                transform.position = new Vector2(-8.91f, transform.position.y);
                break;
            case "WallU":
                transform.position = new Vector2(transform.position.x, -4.6f);
                break;
            case "WallD":
                transform.position = new Vector2(transform.position.x, 4.7f);
                break;
        }
    }
    private void Algo()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radiusToFar, boid);

        if (hitColliders.Length > 1)
        {
            Collider2D collider2DCloser = null;
            foreach (var v in hitColliders)
            {
                if (v.gameObject != gameObject)
                {
                    if (Helper.WorldSpaceToLocalSpace(v.transform.position, transform).y > 0)
                    {
                        if (collider2DCloser == null)
                        {
                            collider2DCloser = v;
                        }
                        else if (Vector2.Distance(transform.position, v.transform.position) < Vector2.Distance(collider2DCloser.transform.position, transform.position))
                        {
                            collider2DCloser = v;
                        }

                    }

                }
            }
            if (collider2DCloser != null)
            {
                SpeedCalc = SpeedIndice * 1;

                var _distance = Vector3.Distance(collider2DCloser.transform.position, transform.position);
                float delt = 1f;
                if (_distance < radiusClose)
                {
                    SpeedCalc = SpeedIndice * 0.5f;
                    delt = 50;

                    if (showDebug) Debug.Log("s'éloigner");
                }
                else if (_distance <= radiusToOptimize && _distance >= radiusClose)
                {
                    SpeedCalc = SpeedIndice;
                    delt = 0;

                    if (showDebug) Debug.Log("imiter");
                }
                else if (_distance > radiusToOptimize)
                {
                    SpeedCalc = SpeedIndice * 1.4f;
                    delt = -5;

                    if (showDebug) Debug.Log("se rapprocher");

                }
                float res = collider2DCloser.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z;
                res = res + Mathf.Sign(res) * delt;
                if(_distance < radiusClose)
                {
                    if (res > 180)
                    {
                        res = (collider2DCloser.transform.rotation.eulerAngles.z - 360) - transform.rotation.eulerAngles.z + delt;

                    }
                    else if (res < -180)
                    {
                        res = (collider2DCloser.transform.rotation.eulerAngles.z + 360) - transform.rotation.eulerAngles.z - delt;


                    }
                }
               
                res *= Mathf.Deg2Rad;
                transform.Rotate(Vector3.forward * (res) * Time.deltaTime * 70);

            }
            else
            {
                RotateRand();
            }
        }
        else
        {
            SpeedCalc = SpeedIndice * 1.8f;
            RotateRand();
        }
    }
    private void RotateRand()
    {

        var randd = rand.Next(0, 2) == 1 ? -1 : 1;
        this.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 100 * randd);
    }
    private void Move()
    {
        this.transform.Translate(Vector3.up * Time.deltaTime * SpeedCalc);
    }
}
