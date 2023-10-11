using System;

public class PlayerInfo
{
	public enum ECrewPosition
	{
		Driver,
		Gunner
	}

	public string Name;
	public int Id;
	public ECrewPosition crewPosition;

	public PlayerInfo(string _name, int _id)
	{
		Name = _name;
		Id = _id;

		//TODO
		if (Id == 1)
			crewPosition = ECrewPosition.Driver;
		else
			crewPosition = ECrewPosition.Gunner;
	}
}
