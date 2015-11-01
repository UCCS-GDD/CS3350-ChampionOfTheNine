using UnityEngine;
using System.Collections;

public class TutorialRanger : Tutorial {

	protected override void Start()
	{
		base.Start ();
		StageOne ();
	}

	public override void ButtonTextChange ()
	{
		base.ButtonTextChange ();
	}

	public void NextStage()
	{
		isCompleted [(int)stage] = true;
		stage++;
		switch ((int)stage) 
		{
		case 0:
			StageOne ();
			break;
		case 1:
			StageTwo ();
			break;
		case 2:
			StageThree ();
			break;
		case 3:
			StageFour ();
			break;
		case 4:
			StageFive ();
			break;
		}
	}

	void StageOne(){
		ChangeText (new string[] {"<b>Ranger tutorial</b> " +
			"\nHere you will learn how to control the ranger well enough to survive the trials put before you.",
			"<b>Objectives:</b> " +
			"\nMovement" +
			"\nRanger Abilities" +
			"\nUI elements", 
			"<b>Movement</b> " +
			"\n[A] Move left" +
			"\n[D] Move right" +
			"\n[SPACE] Jump" +
			"\nTry these actions."});
	}

	void StageTwo()
	{
		ChangeText (new string[] {"Nice work. You now know the basics of movement so lets amp it up a bit.",
			"<b>Basic Ability</b>" +
			"\nThe left mouse button will make your avatar fire a basic arrow, use it to attack the dummy."});
	}

	void StageThree()
	{
		ChangeText (new string[] {"You shot the dummy!" +
			"\nGood work, now lets try something more fun.",
			"<b>Secondary Ability</b>" +
			"\nThe right mouse button will make your avatar fire an explosive arrow. Blow up the dummy."});
	}

	void StageFour()
	{
		ChangeText (new string[] {"Woah! That was an epic explosion." +
			"\nLets get more advanced here shall we?",
			"<b>Special Ability</b>" +
			"\nHolding down [E] will fire a barrage of penetrating arrows. Test it on these dummies"});
	}

	void StageFive()
	{
		ChangeText (new string[] {"Devastating! If you keep this up, you will become champion in no time." +
			" Every champion needs their boost ability though.",
			"<b>Boost Ability</b>" +
			"\nIf you press [R] your champion gets increased mobility, power, and efficiency for a short period. Give it a shot."});
	}
}
