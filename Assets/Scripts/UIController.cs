using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject map3D; // obiekt z ca³¹ Twoj¹ map¹ (Plane + œciany)

    public void ToggleMap()
    {
        if (map3D != null)
            map3D.SetActive(!map3D.activeSelf);
    }
}
