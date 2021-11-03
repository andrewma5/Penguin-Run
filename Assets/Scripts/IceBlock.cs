using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceBlock : MonoBehaviour
{
    string[] AllColors = { "Red", "Blue", "Green", "Yellow", "White", "Black" };
    string color = "Red";
    bool up = true;
    public Material RedM;
    public Material BlueM;
    public Material GreenM;
    public Material YellowM;
    public Material WhiteM;
    public Material BlackM;
    public Material RedMS;
    public Material BlueMS;
    public Material GreenMS;
    public Material YellowMS;
    public Material WhiteMS;
    public Material BlackMS;
    MeshRenderer mr;
    MeshCollider mc;

    private int IceModels = 3;
    GameObject ActiveIce;

    // Start is called before the first frame update
    void Start()
    {
        Remodel();
        Recolor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleIce(string[] c)
    {
        if (c.Contains(color))
        {
            if (up)
            {
                GoDown();
            }
            else
            {
                GoUp();
            }
        }
    }

    private void GoDown()
    {
        mr.enabled = false;
        mc.enabled = false;
        up = false;
    }

    private void GoUp()
    {
        mr.enabled = true;
        mc.enabled = true;
        switch (color)
        {
            case "Red":
                mr.material = RedMS;
                break;
            case "Blue":
                mr.material = BlueMS;
                break;
            case "Green":
                mr.material = GreenMS;
                break;
            case "Yellow":
                mr.material = YellowMS;
                break;
            case "White":
                mr.material = WhiteMS;
                break;
            case "Black":
                mr.material = BlackMS;
                break;
        }
        up = true;
    }

    public void Fade(string s)
    {
        if (color == s)
        {
            switch (color)
            {
                case "Red":
                    mr.material = RedM;
                    break;
                case "Blue":
                    mr.material = BlueM;
                    break;
                case "Green":
                    mr.material = GreenM;
                    break;
                case "Yellow":
                    mr.material = YellowM;
                    break;
                case "White":
                    mr.material = WhiteM;
                    break;
                case "Black":
                    mr.material = BlackM;
                    break;
            }
        }
    }

    public void Reuse(Vector3 NewPos, List<string> ColorsOff)
    {
        Remodel();
        Recolor();
        transform.position = NewPos;
        if (ColorsOff.Contains(color))
        {
            GoDown();
        }
        else
        {
            GoUp();
        }
    }

    private void Recolor()
    {
        int col = Random.Range(0, AllColors.Length);
        color = AllColors[col];
        switch (color)
        {
            case "Red":
                mr.material = RedMS;
                break;
            case "Blue":
                mr.material = BlueMS;
                break;
            case "Green":
                mr.material = GreenMS;
                break;
            case "Yellow":
                mr.material = YellowMS;
                break;
            case "White":
                mr.material = WhiteMS;
                break;
            case "Black":
                mr.material = BlackMS;
                break;
        }
    }

    private void Remodel()
    {
        if (mr)
        {
            GoUp();
        }
        int model = Random.Range(0, IceModels);
        ActiveIce = transform.GetChild(model).gameObject;
        for (int i = 0; i < IceModels; i++)
        {
            if (i != model)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        mr = ActiveIce.GetComponent<MeshRenderer>();
        mc = ActiveIce.GetComponent<MeshCollider>();
    }
}
