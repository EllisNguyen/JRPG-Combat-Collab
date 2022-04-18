using UnityEngine;

public class PlayerEntity : MonoBehaviour
{

    public GameObject inventory;
    public GameObject party;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && party.active == false) inventory.SetActive(true);
        if (Input.GetKeyDown(KeyCode.U) && inventory.active == false) party.SetActive(true);

    }
}
