using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DetailMenu : MonoBehaviour
{
    public GameObject Panel;
    public Text header;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
            foreach (RaycastHit hit in hitsInOrder)
            {
                Debug.Log(hit.transform.GetComponent<ZoneBuilding>() == null);
            }
        }
    }
    //TODO Fix display
    public void DisplayPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }
}
