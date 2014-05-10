using UnityEngine;
using System.Collections;

public abstract class Weapon {

	private int 	m_damage;
	private float 	m_speed;
	private float	m_range;
	private int 	m_ammunition;
	
	
	public int Damage {
		set {
			this.m_damage = value;
		}
		get {
			return this.m_damage;
		}
	}
	
	public float Range {
		set {
			this.m_range = value;
		}
		get {
			return this.m_range;
		}
	}
	
	public int Ammunition {
		set {
			this.m_ammunition = value;
		}
		get {
			return this.m_ammunition;
		}
	}
	
	public abstract void fire();

}
