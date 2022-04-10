//////////////////////////////////////////////////////////////////////
/// 
///PLEASE DON'T TOUCH THIS CLASS IF YOU DON'T HAVE TO.
///AND PLEASE CONTACT PHAP IF ANY ERROR(S) LEAD TO THIS CLASS.
///
///YOU FIND NO EXPLANATIONS OR COMMENTS IN THIS CLASS FOR A REASON.
///
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.Events;

public class EntityTrigger : MonoBehaviour
{
    public UnityEvent mouseOverEvent;
    public UnityEvent mouseExitEvent;
    public UnityEvent onClick;

    Ray ray;
    RaycastHit hit;

    void Update()
    {
        if (!Camera.main) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.GetComponent<EntityTrigger>())
                    onClick?.Invoke();
            }
        }
    }

    void OnMouseOver()
    {
        mouseOverEvent?.Invoke();
    }

    void OnMouseExit()
    {
        mouseExitEvent?.Invoke();
    }

    public void Debug(string message)
    {
        print(message);
    }
}
