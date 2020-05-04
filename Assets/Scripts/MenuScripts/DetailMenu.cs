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
        RaycastHit[] hitsInOrder = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition)).OrderBy(h => h.distance).ToArray();
        foreach (RaycastHit hit in hitsInOrder)
        {
            if (Input.GetMouseButtonDown(0) && hit.transform.TryGetComponent(out ZoneBuilding zoneBuilding))
            {
                Debug.Log(zoneBuilding.buildingType);
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
