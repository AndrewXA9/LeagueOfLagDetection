using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Run : MonoBehaviour {
	
	private Dictionary<int,string> IPs = new Dictionary<int, string>();
	
	private int server = 6;
	
	private Ping ping;
	
	public Slider slider;
	
	private List<GameObject> bars;
	public List<int> pings;
	public Sprite bar;
	
	public GameObject barContainer;
	
	private float timer = 0f;
	public float timeout = 1f;
	
	public Text maxText;
	public Text curText;
	
	void Start () {
		IPs.Add(1,"0.0.0.0");
		IPs.Add(2,"31.186.224.42");
		IPs.Add(3,"185.40.65.1");
		IPs.Add(4,"0.0.0.0");
		IPs.Add(5,"0.0.0.0");
		IPs.Add(6,"104.160.131.1");
		IPs.Add(7,"103.240.227.5");
		IPs.Add(8,"0.0.0.0");
		IPs.Add(9,"0.0.0.0");
		IPs.Add(10,"0.0.0.0");
		IPs.Add(11,"0.0.0.0");
		IPs.Add(12,"0.0.0.0");
		
		
		bars = new List<GameObject>();
		pings = new List<int>();
		for(int i=0;i<32;i++){
			GameObject b = new GameObject();
			Image NewImage = b.AddComponent<Image>();
			NewImage.sprite = bar;
			NewImage.color = Color.green;
			RectTransform r = b.GetComponent<RectTransform>();
			r.SetParent(barContainer.transform);
			r.anchorMin = new Vector2(1,0);
			r.anchorMax = new Vector2(1,0);
			r.pivot = new Vector2(1,0);
			r.position = new Vector3(512-(i*16),6,0);
			r.sizeDelta = new Vector2(16,0);
			b.SetActive(true);
			
			bars.Add(b);
			pings.Add(0);
		}
		
		
		
		ping = new Ping(IPs[server]);
		
	}
	
	public void UpdateServer(){
		server = (int)slider.value;
		ping = new Ping(IPs[server]);
	}
	
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			
			int newping = 1000;
			if(Time.time < 1f){
				newping = 0;
			}
			
			if(ping.isDone){
				newping = ping.time;
			}
			
			timer = timeout;
			//Debug.Log(ping.time);
			int maxping = newping;
			for(int i=30;i>-1;i--){
				pings[i+1] = pings[i];
				if(pings[i] > maxping){
					maxping = pings[i];
					maxText.text = "Max: "+maxping.ToString();
				}
			}
			pings[0] = newping;
			curText.text = "Current: "+newping.ToString();
			for(int i=0;i<32;i++){
				bars[i].GetComponent<RectTransform>().sizeDelta = new Vector2(16,(pings[i]/(float)maxping)*64);
				bars[i].GetComponent<Image>().color = Color.Lerp(Color.green,Color.red,Mathf.Clamp(pings[i]/300f,0f,1f));
			}
			
			ping = new Ping(IPs[server]);
			

		}
	}
	
	
}
