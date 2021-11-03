using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceManager : MonoBehaviour
{
    List<IceBlock> AllIce = new List<IceBlock>();
    string[] AllColors = { "Red", "Blue", "Green", "Yellow", "White", "Black"};
    List<string> ColorsOff = new List<string>();
    int curColor = -1;
    [SerializeField] GameObject IcePrefab, PowerupPrefab;
    Player player;
    GameObject playerObject;
    int oldestZ = 6;
    int newestZ = 100;
    int lastPower = -20;

    private float cooldown = 2f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        playerObject = GameObject.Find("Player");
        SpawnIce();
        InvokeRepeating("ChangeIce", 1f, cooldown);
        InvokeRepeating("FadeIce", 1.5f, cooldown);
    }

    // Update is called once per frame
    void Update()
    {   
        if (player.GetMaxZ() > oldestZ + 12)
        {
            List<IceBlock> tempIce = new List<IceBlock>();
            for (int i = 0; i < 5; i++)
            {
                tempIce.Add(AllIce[0]);
                AllIce[0].Reuse(new Vector3(2 * i - 4, 0, newestZ), ColorsOff);
                AllIce.RemoveAt(0);
                if (newestZ > lastPower + 30)
                {
                    int pow = Random.Range(1, 21);
                    if (pow == 2)
                    {
                        Instantiate(PowerupPrefab, new Vector3(2 * i - 4, 1.5f, newestZ), Quaternion.identity);
                        lastPower = newestZ;
                    }
                }
            }
            AllIce.AddRange(tempIce);
            oldestZ += 2;
            newestZ += 2;
        }
    }

    void ChangeIce()
    {
        if (!player.ded)
        {
            foreach (IceBlock g in AllIce)
            {
                if (curColor == -1)
                {
                    g.ToggleIce(new string[] { AllColors[0], AllColors[AllColors.Length - 1] });
                    ToggleColorsOff(new string[] { AllColors[0], AllColors[AllColors.Length - 1] });
                }
                else if (curColor == 0)
                {
                    g.ToggleIce(new string[] { AllColors[1], AllColors[AllColors.Length - 1] });
                    ToggleColorsOff(new string[] { AllColors[1], AllColors[AllColors.Length - 1] });
                }
                else if (curColor < AllColors.Length - 1)
                {
                    g.ToggleIce(new string[] { AllColors[curColor - 1], AllColors[curColor + 1] });
                    ToggleColorsOff(new string[] { AllColors[curColor - 1], AllColors[curColor + 1] });
                }
                else
                {
                    g.ToggleIce(new string[] { AllColors[curColor - 1], AllColors[0] });
                    ToggleColorsOff(new string[] { AllColors[curColor - 1], AllColors[0] });
                }

            }
            curColor++;
            if (curColor >= AllColors.Length)
            {
                curColor = 0;
            }
        }
    }

    void FadeIce()
    {
        foreach (IceBlock g in AllIce)
        {
            if (curColor <= 0)
            {
                g.Fade(AllColors[1]);
            }
            else if (curColor < AllColors.Length - 1)
            {
                g.Fade(AllColors[curColor + 1]);
            }
            else
            {
                g.Fade(AllColors[0]);
            }
        }
    }

    void SpawnIce()
    {
        for (int i=6; i<100; i+=2)
        {
            for (int j = -4; j < 6; j += 2)
            {
                GameObject icee = Instantiate(IcePrefab, new Vector3(j, 0f, i), Quaternion.identity);
                AllIce.Add(icee.GetComponent<IceBlock>());
                if (i > lastPower + 30)
                {
                    int pow = Random.Range(1, 21);
                    if (pow == 2)
                    {
                        Instantiate(PowerupPrefab, new Vector3(j, 1.5f, i), Quaternion.identity);
                        lastPower = i;
                    }
                }
            }
        }
    }

    void ToggleColorsOff(string[] c)
    {
        foreach (string s in c)
        {
            if (ColorsOff.Contains(s))
            {
                ColorsOff.Remove(s);
            }
            else
            {
                ColorsOff.Add(s);
            }
        }
    }
}
