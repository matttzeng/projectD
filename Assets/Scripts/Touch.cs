using UnityEngine;
using System.Collections;

//�W�[Ĳ�I�ާ@�\��A������v���B��
public class Touch : MonoBehaviour
{

	//�������Ĳ�I��m
	Vector2 m_screenPos = new Vector2();

	void Start()
	{
		//���\�h�IĲ�I
		Input.multiTouchEnabled = true;
	}


	void Update()
	{
		//�P�_���x
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)

		MobileInput ();

#else

		DesktopInput();

#endif
	}

	//�ڭ̦b Update ���P�_���x�O�b"�q��"�٬O"���"�A���x�����i�H�Ѧҳo�� �x��Documentation
	//���O�[�JMobileInput �M DeskopInput ��Ӥ�k�B�z�e������


	// �m�bDeskopInput �W�[�{���X
	void DesktopInput()
	{
		//�����ƹ����䪺���ʶZ��
		float mx = Input.GetAxis("Mouse X");
		float my = Input.GetAxis("Mouse Y");

		float speed = 6.0f;

		if (mx != 0 || my != 0)
		{

			//�ƹ�����
			if (Input.GetMouseButton(0))
			{

				//������v����m
				Camera.main.transform.Translate(new Vector3(-mx * Time.deltaTime * speed, -my * Time.deltaTime * speed, 0));
			}
		}
	}
	//��oX�b�PY�b����m�A�ñ�����v����m


	//�bMobileInput �W�[�{���X

	void MobileInput()
	{
		if (Input.touchCount <= 0)
			return;

		//1�Ӥ��Ĳ�I�ù�
		if (Input.touchCount == 1)
		{

			//�}�lĲ�I
			if (Input.touches[0].phase == TouchPhase.Began)
			{

				//����Ĳ�I��m
				m_screenPos = Input.touches[0].position;

				//�������
			}
			else if (Input.touches[0].phase == TouchPhase.Moved)
			{

				//������v��
				Camera.main.transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x * Time.deltaTime, -Input.touches[0].deltaPosition.y * Time.deltaTime, 0));
			}


			//������}�ù�
			if (Input.touches[0].phase == TouchPhase.Ended && Input.touches[0].phase == TouchPhase.Canceled)
			{

				Vector2 pos = Input.touches[0].position;

				//�����������
				if (Mathf.Abs(m_screenPos.x - pos.x) > Mathf.Abs(m_screenPos.y - pos.y))
				{
					if (m_screenPos.x > pos.x)
					{
						//����V���ư�
					}
					else
					{
						//����V�k�ư�
					}
				}
				else
				{
					if (m_screenPos.y > pos.y)
					{
						//����V�U�ư�
					}
					else
					{
						//����V�W�ư�
					}
				}
			}

			//��v���Y��A�p�G1�Ӥ���H�WĲ�I�ù�
		}
		else if (Input.touchCount > 1)
		{

			//�O����Ӥ����m
			Vector2 finger1 = new Vector2();
			Vector2 finger2 = new Vector2();

			//�O����Ӥ�����ʶZ��
			Vector2 move1 = new Vector2();
			Vector2 move2 = new Vector2();

			//�O�_�O�p��2�IĲ�I
			for (int i = 0; i < 2; i++)
			{

				UnityEngine.Touch touch = UnityEngine.Input.touches[i];

				if (touch.phase == TouchPhase.Ended)
					break;

				if (touch.phase == TouchPhase.Moved)
				{
					//�C�������m
					float move = 0;

					//Ĳ�I�@�I
					if (i == 0)
					{
						finger1 = touch.position;
						move1 = touch.deltaPosition;
						//�t�@�I
					}
					else
					{
						finger2 = touch.position;
						move2 = touch.deltaPosition;

						//���̤jX
						if (finger1.x > finger2.x)
						{
							move = move1.x;
						}
						else
						{
							move = move2.x;
						}

						//���̤jY�A�ûP���X��X�֥[
						if (finger1.y > finger2.y)
						{
							move += move1.y;
						}
						else
						{
							move += move2.y;
						}

						//�����Z���V���AZ��m�[���V�h�A�ۤϤ�
						Camera.main.transform.Translate(0, 0, move * Time.deltaTime);
					}
				}
			}//end for
		}//end else if 
	}//end void
}