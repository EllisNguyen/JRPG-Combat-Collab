///Author: Phap Nguyen.
///Description: Attribute bar will spawn by the number of core attributes of the selected item in the panel.
///Day created: 28/03/2022
///Last edited: 28/03/2022 - Phap Nguyen.

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoreAttributeBar : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] TextMeshProUGUI attributeName;
    [SerializeField] TextMeshProUGUI bonusNumber;
    [SerializeField] Image attributeBar;

    public void SetAttributeInfo(CoreAttribute attribute)
    {
        attributeName.text = attribute.AttributeName;
        bonusNumber.text = "+" + attribute.AttributeGain.ToString();
        SmoothFill(attributeBar, (float)attribute.AttributeGain / 100);
    }

    async void SmoothFill(Image progressBar, float newVal)
    {
        progressBar.fillAmount = 0;

        while (progressBar.fillAmount < newVal)
        {
            progressBar.fillAmount += 1.5f * Time.deltaTime;
            await Task.Yield();
        }
        progressBar.fillAmount = newVal;
    }
}
