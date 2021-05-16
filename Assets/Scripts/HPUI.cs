using UnityEngine.UI;
using UnityEngine;

public class HPUI : MonoBehaviour
{
    public Text HPText;

    void Update()
    {
        HPText.text = PlayerStats11.PlayHP.ToString();
      
    }

}
