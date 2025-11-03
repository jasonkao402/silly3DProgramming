using UnityEngine;

public class chainchainGen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int chainNumber = 10;
    public Vector3 offsetPerLink = new Vector3(0, -0.1f, 0);
    public GameObject chainLinkPrefab;
    void Start()
    {
        GenerateChain(chainNumber);
    }
    void GenerateChain(int chainNumber)
    {
        for (int i = 0; i < chainNumber; i++)
        {
            GameObject newChainRoot = Instantiate(chainLinkPrefab, transform.position + offsetPerLink * i, transform.rotation, transform.parent);
            chainGen cg = newChainRoot.GetComponent<chainGen>();
            if (cg != null)
            {
                cg.chainNumber = 10+i;
            }
            // newChainRoot.transform.parent = ;

        }
    }
}
