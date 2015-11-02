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

//	public void NextStage()
//	{
//		isCompleted [(int)stage] = true;
//		stage++;
//		switch ((int)stage) 
//		{
//		case 0:
//			StageOne ();
//			break;
//		case 1:
//			StageTwo ();
//			break;
//		case 2:
//			StageThree ();
//			break;
//		case 3:
//			StageFour ();
//			break;
//		case 4:
//			StageFive ();
//			break;
//		}
//	}

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
		TutorialDummy.PlayerMoved += StageTwo;
	}

	void StageTwo()
	{
		stage++;
		TutorialDummy.PlayerMoved -= StageTwo;
		ChangeText (new string[] {"Nice work. You now know the basics of movement so lets amp it up a bit.",
			"<b>Basic Ability</b>" +
			"\nThe left mouse button will make your avatar fire a basic arrow, use it to attack the dummy."});
		TutorialDummy.BasicAttack += StageThree;
	}

	void StageThree()
	{
		stage++;
		TutorialDummy.BasicAttack -= StageThree;
		ChangeText (new string[] {"You shot the dummy!" +
			"\nGood work, now lets try something more fun.",
			"<b>Secondary Ability</b>" +
			"\nThe right mouse button will make your avatar fire an explosive arrow. Blow up the dummy."});
		TutorialDummy.SecondaryAttack += StageFour;
	}

	void StageFour()
	{
		stage++;
		TutorialDummy.SecondaryAttack -= StageFour;
		ChangeText (new string[] {"Woah! That was an epic explosion." +
			"\nLets get more advanced here shall we?",
			"<b>Special Ability</b>" +
			"\nHolding down [E] will fire a barrage of penetrating arrows. Test it on these dummies"});
		TutorialDummy.SpecialAttack += StageFive;
	}

	void StageFive()
	{
		stage++;
		TutorialDummy.SpecialAttack -= StageFive;
		ChangeText (new string[] {"Devastating! If you keep this up, you will become champion in no time. " +
			"Every champion needs their boost ability though.",
			"<b>Boost Ability</b>" +
			"\nIf you press [R] your champion gets increased mobility, power, and efficiency for a short period. Give it a shot."});
		TutorialDummy.Booster += StageSix;
	}

	void StageSix()
	{
		stage++;
		TutorialDummy.Booster -= StageSix;
		ChangeText (new string[] {
			"Look at how fast and strong you are right now! Use this at the right time to throw the advantage your way.",
			"Don't think you are going to spend all of your time attacking dummies. Your enemies will be actively trying to end your life.",
			"Look at the bottom left corner of your screen. Here you can see the UI elements that you will need to be successful in your quests.",
			"The red bar is your health. If this reaches zero you lose! Avoid enemy attacks to stay healthy and be triumphant.",
			"The yellow bar is your energy bar. Each of your abilities use up energy, so you need to learn how to balance them and be as effective as possible.",
			"The ability bars display the cooldown of each of your abilities. If you use your stronger abilities at un-opportune moments you will find yourself in a pickle.",
			"If you press [ESC] it will open the pause menu. Now that you have graduated from the tutorial, use the pause menu to exit the tutorial."
		});
	}
}
