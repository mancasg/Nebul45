using UnityEngine;
using System.Collections;

public class MissileLauncher : Weapon {
	
	private string  	m_missileID;
	private Transform	m_transform;
	
	public MissileLauncher(Transform transform, string missileID) {
		this.m_missileID = missileID;
		this.m_transform = transform;
	}
	
	public override void fire() {	
		GameObject missile = (GameObject)Network.Instantiate(Resources.Load(m_missileID),
		                                                     m_transform.position,
		                                                     m_transform.rotation,
		                                                     0
		                                                     );
		missile.AddComponent<Bullet>();
	}
}
