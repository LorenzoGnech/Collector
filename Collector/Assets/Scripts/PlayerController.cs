using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	Rigidbody2D rigidbody;
	public Text collectedText;
	public static int collectedAmount = 0;
	public GameObject bulletPrefab;

	public float bulletSpeed;

	private float lastFire;

	public float fireDelay;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		fireDelay = GameController.FireRate;
		speed = GameController.MoveSpeed;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		float shootHorizontal = Input.GetAxis("ShootHorizontal");
		float shootVertical = Input.GetAxis("ShootVertical");

		if((shootHorizontal != 0 || shootVertical != 0) && (Time.time > lastFire + fireDelay)){
			Shoot(shootHorizontal, shootVertical);
			lastFire = Time.time;
		};

		rigidbody.velocity = new Vector3(horizontal*speed, vertical * speed, 0);
		collectedText.text = "Items collected: " + collectedAmount;
	}

	void Shoot(float x, float y){
		GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
		bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
			(x<0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
			(y<0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
			0
		);
	}
}
