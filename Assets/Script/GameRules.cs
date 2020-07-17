using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    public int nombreBoid;
    public List<GameObject> ListWall;
    public bool DebugMode;
    void Start()
    {
        if (!DebugMode)
        {
            for (int i = 0; i < nombreBoid; i++)
            {
                Instantiate(prefab, new Vector3(0, 0, 10), Quaternion.identity);
            }
            foreach (var wall in ListWall)
            {
                wall.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            Controler.activAlgo = !Controler.activAlgo;

        }
    }
}
