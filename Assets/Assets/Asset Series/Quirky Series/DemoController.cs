/* Scripted by Bamabu - omabuarts@gmail.com */
using UnityEngine;
using UnityEngine.UI;

public class DemoController : MonoBehaviour
{

    [Space(10)]
    public Animator[] anim;
    public Dropdown dropdown;

    void Update()
    {

        if (Input.GetKeyDown("right"))
        {
            NextAnim();
        }
        else if (Input.GetKeyDown("left"))
        {
            PrevAnim();
        }
    }

    public void NextAnim()
    {

        if (dropdown.value >= dropdown.options.Count - 1)
            dropdown.value = 0;
        else
            dropdown.value++;

        PlayAnim();
    }

    public void PrevAnim()
    {

        if (dropdown.value <= 0)
            dropdown.value = dropdown.options.Count - 1;
        else
            dropdown.value--;

        PlayAnim();
    }

    public void PlayAnim()
    {

        // anim.SetTrigger (dropdown.options [dropdown.value].text);
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play(dropdown.options[dropdown.value].text);
        }
    }

    public void GoToWebsite(string URL)
    {

        Application.OpenURL(URL);
    }
}