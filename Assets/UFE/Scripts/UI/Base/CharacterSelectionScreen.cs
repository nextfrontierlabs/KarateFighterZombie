using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;
public class CharacterSelectionScreen : UFEScreen {
	#region public instance properties
	public AudioClip selectSound;
	public AudioClip cancelSound;
	public GameObject StartButton;
	public GameObject selectButton;
	public GameObject buyButton;
	#endregion
	public GameObject[] flags;
	public GameObject flagParent;
	public GameObject player2Txt; // its just player-2 text on black label
	public GameObject player2NameTxt;
	public GameObject backContainer;
	public GameObject hoverForPlayer2Animation;
	public Text countryName;
	#region protected instance fields
	protected int p1HoverIndex = 0;
	protected int p2HoverIndex = 0;
	protected bool closing = false;
	#endregion

	#region public instance methods
	public virtual int GetHoverIndex(int player){
		if (player == 1){
			return this.p1HoverIndex;
		}else if (player == 2){
			return this.p2HoverIndex;
		}

		throw new ArgumentOutOfRangeException("player");
	}

	public virtual CharacterInfo[] GetSelectableCharacters(){
		if (UFE.gameMode == GameMode.StoryMode){
			return UFE.GetStoryModeSelectableCharacters();
		}else if (UFE.gameMode == GameMode.TrainingRoom){
			return UFE.GetTrainingRoomSelectableCharacters();
		}else{
			return UFE.GetVersusModeSelectableCharacters();
		}
	}

	public virtual void GoToPreviousScreen(){
		this.closing = true;
		
		if (UFE.gameMode == GameMode.VersusMode && UFE.GetVersusModeScreen() != null){
			//UFE.DelaySynchronizedAction(this.GoToVersusModeScreen, 0.1f);
			UFE.DelaySynchronizedAction(this.GoToMainMenuScreen, 0.1f);
		}else if (UFE.gameMode == GameMode.NetworkGame){
			UFE.DelaySynchronizedAction(this.GoToNetworkGameScreen, 0.1f);
		}else{
			UFE.DelaySynchronizedAction(this.GoToMainMenuScreen, 0.1f);
		}
	}
	CharacterInfo selectedCharacter;
	int selectedPlayerNumber = 0;
	int characterIndex = 1;
	public void SelectPlayer()
	{

		UFE.SetPlayer(selectedPlayerNumber, selectedCharacter);
		selectButton.SetActive(false);
		if (
			UFE.config.player1Character != null &&
			UFE.config.player2Character == null && DefaultCharacterSelectionScreen.isMultiplayerSelected)
		{
			flagParent.SetActive(true);
			backContainer.SetActive(true);
			player2Txt.SetActive(true);

			countryName.gameObject.SetActive(true);
			Invoke("ChooseCountryRandomly", 8);
			return;
		}
		this.SetHoverIndex(selectedPlayerNumber, characterIndex);


		// And check if we should start loading the next screen...
		if (
			UFE.config.player1Character != null &&
			(UFE.config.player2Character != null && (UFE.gameMode == GameMode.VersusMode || UFE.gameMode == GameMode.TrainingRoom))
		)
		{

			//			StartButton.SetActive(true);
			GoToNextScreen();
		}
		if (PlayerPrefs.GetInt("FirstTimeSelection") == 0)
		{
			DefaultCharacterSelectionScreen.SetSelectionForFirstTime();

		}
	}
	public void SelectPlayerAuto()
	{

		UFE.SetPlayer(selectedPlayerNumber, selectedCharacter);

		this.SetHoverIndex(selectedPlayerNumber, characterIndex);
		selectButton.SetActive(false);

		// And check if we should start loading the next screen...
		if (
			UFE.config.player1Character != null &&
			(UFE.config.player2Character != null && (UFE.gameMode == GameMode.VersusMode || UFE.gameMode == GameMode.TrainingRoom))
		)
		{

			//			StartButton.SetActive(true);
			GoToNextScreen();
		}
		if (PlayerPrefs.GetInt("FirstTimeSelection") == 0)
		{
			DefaultCharacterSelectionScreen.SetSelectionForFirstTime();

		}
	}
	void ChooseCountryRandomly()
	{
		flagParent.GetComponent<Animator>().enabled = false;
		for (int i = 0; i < flags.Length; i++)
		{
			flags[i].SetActive(false);
		}
		int random = UnityEngine.Random.Range(0, flags.Length);
		flags[random].SetActive(true);
		countryName.gameObject.SetActive(true);
		countryName.text = flags[random].gameObject.name;
		hoverForPlayer2Animation.SetActive(true);
	
		Invoke("GotoNext", 5);

	}
	void GotoNext()
	{
		hoverForPlayer2Animation.SetActive(false);
		selectButton.SetActive(false);
		int randomPlayer = UnityEngine.Random.Range(0, this.GetSelectableCharacters().Length);
		this.TrySelectCharacter(randomPlayer, 2);
		charcterint.inst.showChar1FtnAuto(randomPlayer);
		selectButton.SetActive(false);
		player2NameTxt.SetActive(true);
		Invoke("SelectPlayerAuto", 3);

	}
	public virtual void OnCharacterSelectionAllowed(int characterIndex, int player){
		// If we haven't started loading a different screen....
		if (!this.closing){
//			print ("player: " + player);
			// Check if we are trying to select or deselect a character...
			CharacterInfo[] selectableCharacters = this.GetSelectableCharacters();
			if (characterIndex >= 0 && characterIndex < selectableCharacters.Length){
				// If we are selecting a character, check if the player has already selected a character...
				/* previously working
//				if(
//					player == 1 && UFE.config.player1Character == null ||
//					player == 2 && UFE.config.player2Character == null
//				){*/
				if(UFE.gameMode == GameMode.StoryMode){


					CharacterInfo character = selectableCharacters[characterIndex];
					if (this.selectSound != null) UFE.PlaySound(this.selectSound);
					if (character != null && character.selectionSound != null) UFE.PlaySound(character.selectionSound);
					UFE.SetPlayer(player, character);
//					print ("Set player");
					StartButton.SetActive(true);
					// And check if we should start loading the next screen...
					/*previously working
//					if(
//						UFE.config.player1Character != null && 
//						(UFE.config.player2Character != null || UFE.gameMode == GameMode.StoryMode)
//					){
////						this.GoToNextScreen();
//					}*/
				}
				else if(UFE.gameMode == GameMode.TrainingRoom){ // extra
					// If the player hasn't selected any character yet, process the request...
					this.SetHoverIndex(player, characterIndex);
					CharacterInfo character = selectableCharacters[characterIndex];
					if (this.selectSound != null) UFE.PlaySound(this.selectSound);
					if (character != null && character.selectionSound != null) UFE.PlaySound(character.selectionSound);
					selectedCharacter = new CharacterInfo ();
					selectedPlayerNumber = player;
					selectedCharacter = character;
					this.characterIndex = characterIndex;
					if ((PlayerPrefs.GetInt ("Characters Bought" + (characterIndex + 1), 0) == 1)) {
						selectButton.SetActive (true);
						buyButton.SetActive (false);

					} else {
						buyButton.SetActive (true);
						selectButton.SetActive (false);
					}
					if (player == 2) {
						selectButton.SetActive (true);
						buyButton.SetActive (false);
					}

                    Debug.Log (player+"Set player"+characterIndex);

				}
				else if(UFE.gameMode == GameMode.VersusMode){ // extra
					// If the player hasn't selected any character yet, process the request...
					this.SetHoverIndex(player, characterIndex);
					CharacterInfo character = selectableCharacters[characterIndex];
					if (this.selectSound != null) UFE.PlaySound(this.selectSound);
					if (character != null && character.selectionSound != null) UFE.PlaySound(character.selectionSound);
					selectedCharacter = new CharacterInfo ();
					selectedPlayerNumber = player;
					selectedCharacter = character;
					this.characterIndex = characterIndex;
					if ((PlayerPrefs.GetInt ("Characters Bought" + (characterIndex + 1), 0) == 1)) {
						selectButton.SetActive (true);
						buyButton.SetActive (false);

					} else {
						buyButton.SetActive (true);
						selectButton.SetActive (false);
					}
					if (player == 2) {
						selectButton.SetActive (true);
						buyButton.SetActive (false);
					}
					// And check if we should start loading the next screen...
					if(
						(UFE.config.player1Character != null && UFE.config.player2Character != null)
						 && UFE.gameMode == GameMode.VersusMode){
						Debug.Log (UFE.config.player1Character.characterName);
						Debug.Log (UFE.config.player2Character.characterName);
						//						this.GoToNextScreen();
						StartButton.SetActive(true);
					}

				}
			}
			else if (characterIndex < 0){
//				print ("else if (characterIndex < 0){");
				// If we are trying to deselect a character, check if at least one player has selected a character
				if (UFE.config.player1Character != null || UFE.config.player2Character != null){
					// In that case, check if the player that wants to deselect his current character has already
					// selected a character and try to deselect that character.
					if(
						player == 1 && UFE.config.player1Character != null ||
						player == 2 && UFE.config.player2Character != null
					){
						if (this.cancelSound != null) UFE.PlaySound(this.cancelSound);
						UFE.SetPlayer(player, null);
					}
				}
				/* working previously
				else{
					// If none of the players has selected a character and one of the player wanted to deselect
					// his current character, that means that the player wants to return to the previous menu instead.
					this.GoToPreviousScreen();
				}*/
			}
		}
	}

	public virtual void SetHoverIndex(int player, int characterIndex){
		if (!this.closing){
			if (characterIndex >= 0 && characterIndex < this.GetSelectableCharacters().Length){
				if (player == 1){
					p1HoverIndex = characterIndex;

				}else if (player == 2){
					p2HoverIndex = characterIndex;
				}
			}
		}
	}

	// newly Added
	public void GotoNextScreen(){
		if (UFE.gameMode == GameMode.TrainingRoom) {
			if (UFE.config.player2Character != null)
				this.GoToNextScreen ();
			
		} else if (UFE.gameMode == GameMode.StoryMode) {
			if (UFE.config.player1Character != null)
				this.GoToNextScreen ();

		}
		else if (UFE.gameMode == GameMode.VersusMode) {
			if (UFE.config.player1Character != null)
				this.GoToNextScreen ();

		}
	}

//	public void GoToPreviousScreen(){
//		this.GoToPreviousScreen();
//	}

	public void TryDeselectCharacter(){
		if (UFE.gameMode == GameMode.StoryMode) { // extra
			if (Network.peerType == NetworkPeerType.Disconnected) {
				// If it's a local game, update the corresponding character immediately...
				if (UFE.config.player2Character != null && UFE.gameMode != GameMode.StoryMode && !UFE.GetCPU (2)) {
					this.TryDeselectCharacter (2);
				} else {
					this.TryDeselectCharacter (1);
				}
			} else {
				// If it's an online game, find out if the local player is Player1 or Player2
				// and update the selection only for the local player...
				this.TryDeselectCharacter (UFE.GetLocalPlayer ());
			}
		}
	}

	public void TryDeselectCharacter(int player){
		this.TrySelectCharacter(-1, player);
	}

	public void TrySelectCharacter(){
		// If it's a local game, update the corresponding character immediately...
		if (Network.peerType == NetworkPeerType.Disconnected){
			if (UFE.config.player1Character == null){
				this.TrySelectCharacter(this.p1HoverIndex, 1);
			}else if (UFE.config.player2Character == null){
				this.TrySelectCharacter(this.p2HoverIndex, 2);
			}
		}else{
			// If it's an online game, find out if the local player is Player1 or Player2
			// and update the selection only for the local player...
			int localPlayer = UFE.GetLocalPlayer();

			if (localPlayer == 1){
				this.TrySelectCharacter(this.p1HoverIndex, localPlayer);
			}else if (localPlayer == 2){
				this.TrySelectCharacter(this.p2HoverIndex, localPlayer);
			}
		}
	}

	public void TrySelectCharacter(int characterIndex){
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			if (
		UFE.config.player1Character != null &&
		UFE.config.player2Character == null && DefaultCharacterSelectionScreen.isMultiplayerSelected)
			{
				return;
			}
			// If it's a local game, update the corresponding character immediately...
			if (UFE.config.player1Character == null)
			{

				this.TrySelectCharacter(characterIndex, 1);
				characterSHows.inst.showChar(characterIndex);

			}
			else if (UFE.config.player2Character == null && UFE.gameMode != GameMode.StoryMode)
			{
				this.TrySelectCharacter(characterIndex, 2);
				characterSHows.inst.showChar2(characterIndex);

			}
		}
		else
		{

			// If it's an online game, find out if the local player is Player1 or Player2
			// and update the selection only for the local player...
			this.TrySelectCharacter(characterIndex, UFE.GetLocalPlayer());
		}
	}
	
	public virtual void TrySelectCharacter(int characterIndex, int player){
		// Check if he was playing online or not...
		if (Network.peerType == NetworkPeerType.Disconnected){
			// If it's a local game, update the corresponding character immediately...
			this.OnCharacterSelectionAllowed(characterIndex, player);
		}else{
			// If it's an online game, find out if the requesting player is the local player
			// because we will only accept requests for the local player...
			int localPlayer = UFE.GetLocalPlayer();
			if (player == localPlayer){
				UFEController controller = UFE.GetController(localPlayer);
				
				// We don't invoke the OnCharacterSelected() method immediately because we are using the frame-delay 
				// algorithm to keep players synchronized, so we can't invoke the OnCharacterSelected() method
				// until the other player has received the message with our choice.
				controller.GetType().GetMethod(
					"RequestOptionSelection",
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy,
					null,
					new Type[]{typeof(int)},
					null
				).Invoke(controller, new object[]{characterIndex});
			}
		}
	}
	#endregion

	#region public override methods
	public override void OnShow (){

		if (UFE.gameMode != GameMode.StoryMode) {
			base.OnShow ();

			UFE.SetPlayer1 (null);
			UFE.SetPlayer2 (null);
			this.SetHoverIndex (1, 2);
			characterSHows.inst.showChar(2);
			this.SetHoverIndex (2, this.GetSelectableCharacters ().Length - 1);
			characterSHows.inst.showChar2(3);



		} else {
			base.OnShow ();

			UFE.SetPlayer1 (null);
			UFE.SetPlayer2 (null);
			print ("onshow " + PlayerPrefs.GetInt ("selectedTekkenIndex",2));

			if (!PlayerPrefs.HasKey ("selectedTekkenIndex")) {

				PlayerPrefs.SetInt ("selectedTekkenIndex", 2);
				PlayerPrefs.Save ();
			}
			this.SetHoverIndex (1, PlayerPrefs.GetInt ("selectedTekkenIndex",2));
			TryDeselectCharacter ();
			TrySelectCharacter (PlayerPrefs.GetInt ("selectedTekkenIndex",2));
			charcterint.inst.showChar1Ftn (PlayerPrefs.GetInt ("selectedTekkenIndex",2));

		
			this.SetHoverIndex (2, this.GetSelectableCharacters ().Length - 1);
		}
	}

	public override void SelectOption (int option, int player){
		this.OnCharacterSelectionAllowed(option, player);
	}
	#endregion

	#region protected instance methods
	protected void GoToMainMenuScreen(){
		this.closing = true;
		UFE.StartMainMenuScreen();
	}

	protected void GoToNetworkGameScreen(){
		this.closing = true;
		UFE.StartNetworkGameScreen();
	}

	protected virtual void GoToNextScreen(){
		this.closing = true;

		if (UFE.gameMode == GameMode.StoryMode){
			PlayerPrefs.SetInt ("FirstCharacterSelected",1);
			UFE.DelaySynchronizedAction(this.StartStoryMode, 0.1f);
			characterSHows.inst.hideModels ();
		}else{
			UFE.DelaySynchronizedAction(this.GoToStageSelectionScreen, 0.1f);
		}
	}

	protected void GoToStageSelectionScreen(){
		this.closing = true;
		UFE.StartStageSelectionScreen();
	}
	
	protected void GoToVersusModeScreen(){
		this.closing = true;
		UFE.StartVersusModeScreen();
	}

	protected void StartStoryMode(){
		this.closing = true;
		UFE.StartStoryModeOpeningScreen();
	}
	#endregion
}
