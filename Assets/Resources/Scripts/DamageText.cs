using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
	float moveSpeed = 0.05f;
	Color alpha;
	float alphaSpeed = 1f;
	[SerializeField]
	TextMeshProUGUI text;
	public int damage = 0;
	// Start is called before the first frame update
	void Start()
	{
		// text = GetComponent<TextMeshPro>();
		alpha = text.color;
		text.text = damage.ToString();
		Invoke("DestroyObject", 2f);
	}

	// Update is called once per frame
	void Update()
	{
		text.rectTransform.position += Vector3.up * moveSpeed * Time.fixedDeltaTime;
		alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
		text.color = alpha;
	}
	private void DestroyObject()
	{
		Destroy(gameObject);
	}
	public void setPosition(Vector3 position)
	{
		text.rectTransform.position = position;
	}
	public void setCri()
	{
		float fSize = 1 + ((GameManager.instance.getCriticalDMG() - 1) / 2);
		if (fSize > 1.8)
		{
			text.fontSize *= 1.8f;
		}
		else
		{
			text.fontSize *= fSize;
		}
		text.color = Color.red;
	}
	public void setHeal()
	{
		text.color = Color.green;
	}
}

