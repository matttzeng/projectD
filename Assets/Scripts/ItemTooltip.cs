using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectDInternal 
{
    public class ItemTooltip : MonoBehaviour
    {
        //�гyTextMeshPro�ܼ�
        public TMP_Text Name;
        public TMP_Text DescriptionText;

        RectTransform m_RectTransform;
        CanvasScaler m_CanvasScaler;
        Canvas m_Canvas;
    
        void Awake()
        {
            //���θ�۷ƹ��]�F  �S�ƹ��F
            // m_RectTransform = GetComponent<RectTransform>();
            // m_CanvasScaler = GetComponentInParent<CanvasScaler>();
            //m_Canvas = GetComponentInParent<Canvas>();
        }

        void OnEnable()
        {
            //���θ�۷ƹ��]�F  �S�ƹ��F
            //  UpdatePosition();
        }

        void Update()
        {
           //���θ�۷ƹ��]�F  �S�ƹ��F
          //  UpdatePosition();
        }

        public void UpdatePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
        
            Vector3[] corners = new Vector3[4];    
            m_RectTransform.GetWorldCorners(corners);

            float width = corners[3].x - corners[0].x;
            float height = corners[1].y - corners[0].y;

            if (width + 16 < Screen.width - mousePosition.x)
            {
                m_RectTransform.transform.position = mousePosition + Vector3.right * 16;
            }
            else
            {
                m_RectTransform.transform.position = mousePosition + Vector3.left * (width + 16);
            }
        }
    }
}