using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Run : MonoBehaviour {
	
	//IP's
	private Dictionary<int,string> IPs = new Dictionary<int, string>();
	
	//default server (NA)
	private int server = 6;
	
	//private Ping ping;
	private int webPing;
	
	//slider for picking ip
	public Slider slider;
	
	//colored bars for visual display
	private List<GameObject> bars;
	//returned ping values
	public List<int> pings;
	//bar sprite
	public Sprite bar;
	
	//for placement of the bars
	public GameObject barContainer;
	
	//ping timer info
	private float timer = 0f;
	public float timeout = 1f;
	
	//text for data number display
	public Text maxText;
	public Text curText;
	
	
	public Text debugText;
	
	void Start () {
		//populate IP list. need moar ip's
		IPs.Add(1,"0.0.0.0");		//BR
		IPs.Add(2,"0.0.0.0");		//EUNE
		IPs.Add(3,"185.40.64.65");	//EUW
		IPs.Add(4,"0.0.0.0");		//LAN
		IPs.Add(5,"0.0.0.0");		//LAS
		IPs.Add(6,"104.160.131.1");	//NA
		IPs.Add(7,"0.0.0.0");		//OCE
		IPs.Add(8,"0.0.0.0");		//RU
		IPs.Add(9,"0.0.0.0");		//TR
		IPs.Add(10,"0.0.0.0");		//SEA
		IPs.Add(11,"0.0.0.0");		//KR
		IPs.Add(12,"0.0.0.0");		//PBE
		
		//construct the ping lists
		bars = new List<GameObject>();
		pings = new List<int>();
		//create and position all the bar sprites
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
		
		
		//web player initial ping
		//ping = new Ping(IPs[server]);
		
		//GL javascript initial ping, -1 means no response
		webPing = -1;
		Application.ExternalCall("RequestPing");
	}
	
	public void UpdateServer(){
		//get current server selection
		server = (int)slider.value;
		
		//web player ping
		//ping = new Ping(IPs[server]);
		
		//GL ping
		webPing = -1;
		Application.ExternalCall("RequestPing");
	}
	
	void Update () {
		
		//decrement ping timer
		timer -= Time.deltaTime;
		
		//update data and start new ping after 1000 ms
		if(timer <= 0){
			
			//initialize new ping
			int newping = 1000;
			if(Time.time < 1f){
				newping = 0;
			}
			
			/*
			//web player ping
			if(ping.isDone){
				newping = ping.time;
			}*/
			
			//GL ping
			if (webPing != -1){
				newping = webPing;
				webPing = -1;
				Application.ExternalCall("RequestPing");
			}
			
			timer = timeout;
			//Debug.Log(ping.time);
			//shift all ping values down list
			int maxping = newping;
			for(int i=30;i>-1;i--){
				pings[i+1] = pings[i];
				
				//override max ping text value if new highest number found
				if(pings[i] > maxping){
					maxping = pings[i];
					maxText.text = "Max: "+maxping.ToString();
				}
			}
			
			//apply newest ping to text and bars
			pings[0] = newping;
			curText.text = "Current: "+newping.ToString();
			
			//adjust sizes based on max ping and calculate color
			for(int i=0;i<32;i++){
				bars[i].GetComponent<RectTransform>().sizeDelta = new Vector2(16,(pings[i]/(float)maxping)*64);
				//0=green, 300=red
				bars[i].GetComponent<Image>().color = Color.Lerp(Color.green,Color.red,Mathf.Clamp(pings[i]/300f,0f,1f));
			}
			
			//web player ping
			//ping = new Ping(IPs[server]);
			
			//GL ping
			webPing = -1;
			Application.ExternalCall("RequestPing");

		}
	}
	
	//for getting javascript ping value back from browser
	void GetPing(string newWebPing){
		webPing = int.Parse(newWebPing);
	}
	
	
}
