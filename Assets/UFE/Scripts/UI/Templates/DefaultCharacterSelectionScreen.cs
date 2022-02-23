using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DefaultCharacterSelectionScreen : CharacterSelectionScreen {
	#region public instance properties
	public int numberOfRows{
		get{
			int totalCharacters = this.GetSelectableCharacters().Length;
			int rows = 0;

			if (this.charactersPerRow > 0){
				rows = totalCharacters / this.charactersPerRow;

				if (totalCharacters % this.charactersPerRow != 0){
					++rows;
				}
			}

			return rows;
		}
	}
	#endregion

	#region public instance fields
	public GameObject warningTxt;
	public GameObject warningPowerTxt;
	public Sprite [] powerShotEnable;
	public Sprite [] specialMoveEnable;
	public Color unselected;
	public GameObject powerPanel;
	public static ArrayList specialMovesContainer =  new ArrayList();
	public Button [] specialMovesBtns;
	public static ArrayList powerMovesContainer =  new ArrayList();
	public Button [] powerMovesBtns;

	public AudioClip moveCursorSound;
	public AudioClip onLoadSound;
	public AudioClip music;
	public bool stopPreviousSoundEffectsOnLoad = false;
	public float delayBeforePlayingMusic = 0.1f;
	public int charactersPerRow = 4;
	public Text namePlayer1;
	public Text namePlayer2;
	public Image portraitPlayer1;
	public Image portraitPlayer2;
	public Image[] characters;
	public Image[] storyModeCharacters;
	public Sprite[] charactersForTrainingAndVersus;
	public Sprite[] CharactersSprites;
	public GameObject storyModeCharacterSelection;
	public GameObject TrainingAndVersusModeCharacterSelection;
	public GameObject storyModeProgressionScreen;
	public Animator hudPlayer1;
	public Animator hudPlayer2;
	public Animator hudBothPlayers;
	public Sprite noCharacterSprite;
	public GameObject popDia;
	public charcterint charScript;

	public static bool cameFromCharacterSelectionPanelToShop;
	public static bool isMultiplayerSelected;
	#endregion


	#region public instance methods
	public static void SetSelectionForFirstTime()
	{
		
			PlayerPrefs.SetInt ("FirstTimeSelection", 1);
			for (int i = 0; i < groupSwitchOffForTraining.Length; i++) {
				groupSwitchOffForTraining[i].alpha = 1f;
				groupSwitchOffForTraining[i].interactable = true;
			}
		
	}

	public void OnEnable()
	{

		//Only Hunk is unlocked

		if (MainMenuScreen.isStoryMode) {
			if (PlayerPrefs.GetInt ("FirstCharacterSelected") == 1 && !MainMenuScreen.isStoryModeCharacterSelection) {
				
				openStoryModeProgressScreen ();
			} else {
				openStoryModeCharacterSelectionScreen ();
			}

		} 
		else 
		{
			storyModeCharacterSelection.SetActive (false);
			TrainingAndVersusModeCharacterSelection.SetActive (true);
		}

		for (int i = 0; i < specialMovesBtns.Length; i++) 
		{
			if (PlayerPrefs.GetInt ("Special Move Bought" + (i + 1), 0) == 1) {
				specialMovesBtns [i].GetComponent<Image> ().sprite = specialMoveEnable [i];
			} else {
				specialMovesBtns [i].interactable = true;
			}
	
			// Getting data from preferences and add it in static arrayList
			if (PlayerPrefs.HasKey ("Special Move" + (i + 1))) {
				specialMovesBtns [PlayerPrefs.GetInt ("Special Move" + (i + 1)) - 1].transform.GetChild (0).gameObject.SetActive (true);

				if (!specialMovesContainer.Contains (PlayerPrefs.GetInt ("Special Move" + (i + 1))))
				{
					specialMovesContainer.Add(PlayerPrefs.GetInt ("Special Move" + (i + 1)));
				}
			}
		}

		for (int i = 0; i < powerMovesBtns.Length; i++) 
		{
			// Getting data from preferences and add it in static arrayList
			if (PlayerPrefs.HasKey ("Power Move" + (i + 1))) {
				powerMovesBtns [i].GetComponent<Image>().sprite = powerShotEnable[i];

				if (!powerMovesContainer.Contains (PlayerPrefs.GetInt ("Power Move" + (i + 1))))
				{
					powerMovesContainer.Add(PlayerPrefs.GetInt ("Power Move" + (i + 1)));
				}
			}
		}

		for (int i = 0; i < storyModeCharacters.Length; i++) 
		{
		 
			if (PlayerPrefs.GetInt ("Characters Bought" + (i + 1), 0) == 1) {

				storyModeCharacters [i].sprite = CharactersSprites [i];
			}
		}
		for (int i = 0; i < storyModeCharacters.Length; i++) 
		{

				characters [i].sprite = charactersForTrainingAndVersus [i];

		}
		for (int i = 0; i < storyModeCharacters.Length; i++)
		{

			characters[i].sprite = charactersForTrainingAndVersus[i];
			if ((PlayerPrefs.GetInt("Characters Bought" + (i + 1), 0) == 0))
			{
				characters[i].color = new Color(1, 1, 1, 0.3f);
			}
			else
			{
				characters[i].color = new Color(1, 1, 1, 1f);
			}

		}
		if (PlayerPrefs.GetInt ("FirstTimeSelection") == 0) {
			groupSwitchOffForTraining =   groupSwitchOffForTrainingNonStatic;
			for (int i = 0; i < groupSwitchOffForTraining.Length; i++) {
				groupSwitchOffForTraining[i].alpha = 0.48f;
				groupSwitchOffForTraining[i].interactable = false;
			}
		} else {
			for (int i = 0; i < groupSwitchOffForTrainingNonStatic.Length; i++) {
				groupSwitchOffForTrainingNonStatic[i].alpha = 1f;
				groupSwitchOffForTrainingNonStatic[i].interactable = true;
			}

		}
	}

	public void GotoShopAtPlayersPanel()
	{
		cameFromCharacterSelectionPanelToShop = true;
		characterSHows.inst.GotoShopAtPlayersPanel ();
	}

	public void GotoShopAtMovesPanel()
	{
		cameFromCharacterSelectionPanelToShop = true;
		characterSHows.inst.GotoShopAtMovesPanel ();
	}
	public void GotoShopAtMovesPanelSpecialMoves()
	{
		cameFromCharacterSelectionPanelToShop = true;
		characterSHows.inst.GotoShopAtMovesPanelSpecialMove ();
	}

	public void SelectSpecialMove(int index)
	{
		if (PlayerPrefs.GetInt ("Special Move Bought" + (index)) == 0) {
			GoToPreviousScreen ();
			GotoShopAtMovesPanelSpecialMoves ();
			return;
		}

		// Checking if Special Move exist in current pool then we will  delete it from pool
		if (specialMovesContainer.Contains (index)) {

			warningTxt.SetActive (false);
			specialMovesContainer.Remove (index);
			specialMovesBtns [index - 1].transform.GetChild (0).gameObject.SetActive (false);
			PlayerPrefs.DeleteKey ("Special Move" + index);
			PlayerPrefs.Save ();

		} else {
			if (specialMovesContainer.Count >= 3) {
				warningTxt.SetActive (true);
				return;
			}
			specialMovesContainer.Add (index);
			specialMovesBtns [index - 1].transform.GetChild (0).gameObject.SetActive (true);
			ShowMove (index);
			//Getting the index of current special Move in Array and add it in preference to add it in pool

			PlayerPrefs.SetInt ("Special Move" +index, index);
			PlayerPrefs.Save ();

		}


	}
	public void SelectPowerMove(int index)
	{
		if (PlayerPrefs.GetInt ("Power Move" + (index)) == 0) {
			GoToPreviousScreen ();
			GotoShopAtMovesPanel ();
			return;
		}
		// Checking if Special Move exist in current pool then we will  delete it from pool
		if (powerMovesContainer.Contains (index)) {

			ShowPower (index);

		} 

	}
	public void openStoryModeProgressScreen()
	{
		characterSHows.inst.SetToStoryTransform();
		storyModeProgressionScreen.SetActive (true);
		TrainingAndVersusModeCharacterSelection.SetActive (false);
		storyModeCharacterSelection.SetActive (false);

	}
	public void openStoryModeCharacterSelectionScreen()
	{
		characterSHows.inst.SetToNormalTransform();
		MainMenuScreen.isStoryModeCharacterSelection = true;
		characterSHows.inst.showImg ();
		storyModeProgressionScreen.SetActive (false);
		TrainingAndVersusModeCharacterSelection.SetActive (false);
		storyModeCharacterSelection.SetActive (true);
	}
	public void checkForCharacter(int characterIndex){
		
			if (UFE.gameMode == GameMode.StoryMode) {
			if (PlayerPrefs.GetInt ("Characters Bought" + (characterIndex + 1), 0) == 1) {
				charIndex = (characterIndex);
				if (characterIndex != PlayerPrefs.GetInt ("selectedTekkenIndex", 2)) { 
					if (PlayerPrefs.HasKey ("defeatedoppo")) {
						print ("inside");
						popDia.SetActive (true);
						return;
					} else {
						base.TryDeselectCharacter ();
						base.TrySelectCharacter ((characterIndex), 1);
						charScript.showChar1Ftn ((characterIndex));
					
					}
				} else {
					base.TryDeselectCharacter ();
					base.TrySelectCharacter ((characterIndex), 1);
					charScript.showChar1Ftn ((characterIndex));
		
				}
			} else {
			   
				base.GoToPreviousScreen ();
				GotoShopAtPlayersPanel ();
			}
			} else {
			if (UFE.config.player1Character != null && UFE.config.player2Character == null && DefaultCharacterSelectionScreen.isMultiplayerSelected)
			{
				return;
			}
			
			base.TryDeselectCharacter ();
				base.TrySelectCharacter ((characterIndex));
				//charScript.showChar1Ftn ((characterIndex));

			}

		
	}
	public  CanvasGroup [] groupSwitchOffForTrainingNonStatic;
	public static CanvasGroup [] groupSwitchOffForTraining;
	public void ShowMove(int indexOfMove)
	{
		switch(indexOfMove)
		{
		case 1:
			characterSHows.inst.ShowMove ("Special Move 1");
			break;
		case 2:
			characterSHows.inst.ShowMove ("Special Move 2");
			break;
		case 3:
			characterSHows.inst.ShowMove ("Special Move 3");
			break;
		case 4:
			characterSHows.inst.ShowMove ("Special Move 4");
			break;
		case 5:
			characterSHows.inst.ShowMove ("Special Move 5");
			break;
		case 6:
			characterSHows.inst.ShowMove ("Special Move 6");
			break;

		}
	}
	public void ShowPower(int indexOfMove)
	{
		switch(indexOfMove)
		{
		case 1:
			characterSHows.inst.ShowMove ("PowerShot 1");
			break;
		case 2:
			characterSHows.inst.ShowMove ("PowerShot 2");
			break;


		}
	}
	int charIndex;
	public void yesFtn(){
		popDia.SetActive (false);
		PlayerPrefs.DeleteKey ("firstTime");
		PlayerPrefs.DeleteKey ("oppoIndex");
		PlayerPrefs.DeleteKey ("currentBattle");
		PlayerPrefs.DeleteKey ("defeatedoppo");
		PlayerPrefs.DeleteKey ("levels");
		for (int i = 0; i < 15; i++) {
			PlayerPrefs.DeleteKey ("StoryModeStarsOfLevel" + i);
			if(PlayerPrefs.HasKey("level"+i+"Unlocked"))
				PlayerPrefs.DeleteKey ("level"+i+"Unlocked");
			PlayerPrefs.Save ();
		}
		PlayerPrefs.Save ();
		UFE.once = true;
		storyModeProgressionScreen.GetComponent<StoryModeProgression> ().Reset();
			base.TryDeselectCharacter ();
			base.TrySelectCharacter (charIndex , 1);
			charScript.showChar1Ftn (charIndex);
			
		
	}

	public void noFtn(){
		popDia.SetActive (false);
	}


	public virtual void MoveCursorDown(){
		if (Network.peerType == NetworkPeerType.Disconnected){
			// If it's a local game, update the corresponding character immediately...
			if (UFE.config.player1Character == null){
				this.MoveCursorDown(1);
			}else if (UFE.config.player2Character == null && UFE.gameMode != GameMode.StoryMode){
				this.MoveCursorDown(2);
			}
		}else{
			// If it's an online game, find out if the local player is Player1 or Player2...
			// And only update the selection for the local player...
			int localPlayer = UFE.GetLocalPlayer();
			if (localPlayer == 1 && UFE.config.player1Character == null){
				this.MoveCursorDown(1);
			}else if (localPlayer == 2 && UFE.config.player2Character == null){
				this.MoveCursorDown(2);
			}
		}
	}

	public virtual void MoveCursorDown(int player){
		// The player can only move the cursor if he hasn't selected a character yet
		if (player == 1 && UFE.config.player1Character == null || player == 2 && UFE.config.player2Character == null){
			if (this.charactersPerRow > 0 && this.numberOfRows > 0){
				// Retrieve the index, row and column of the character
				int currentIndex = this.GetHoverIndex(player);
				int currentRow = currentIndex / this.charactersPerRow;
				int currentColumn = currentIndex % this.charactersPerRow;
				
				// Move the cursor to the left
				currentRow = (currentRow + 1) % this.numberOfRows;
				
				// Finally, update the position of the cursor
				this.MoveCursor(player, currentRow * this.charactersPerRow + currentColumn);
			}
		}
	}

	public virtual void MoveCursorLeft(){
		if (Network.peerType == NetworkPeerType.Disconnected){
			// If it's a local game, update the corresponding character immediately...
			if (UFE.config.player1Character == null){
				this.MoveCursorLeft(1);
			}else if (UFE.config.player2Character == null && UFE.gameMode != GameMode.StoryMode){
				this.MoveCursorLeft(2);
			}
		}else{
			// If it's an online game, find out if the local player is Player1 or Player2...
			// And only update the selection for the local player...
			int localPlayer = UFE.GetLocalPlayer();
			if (localPlayer == 1 && UFE.config.player1Character == null){
				this.MoveCursorLeft(1);
			}else if (localPlayer == 2 && UFE.config.player2Character == null){
				this.MoveCursorLeft(2);
			}
		}
	}

	public virtual void MoveCursorLeft(int player){
		// The player can only move the cursor if he hasn't selected a character yet
		if (player == 1 && UFE.config.player1Character == null || player == 2 && UFE.config.player2Character == null){
			if (this.charactersPerRow > 0){
				// Retrieve the index, row and column of the character
				int currentIndex = this.GetHoverIndex(player);
				int currentRow = currentIndex / this.charactersPerRow;
				int currentColumn = currentIndex % this.charactersPerRow;

				// Move the cursor to the left
				currentColumn = (currentColumn + this.charactersPerRow - 1) % this.charactersPerRow;

				// Finally, update the position of the cursor
				this.MoveCursor(player, currentRow * this.charactersPerRow + currentColumn);
			}
		}
	}

	public virtual void MoveCursorRight(){
		if (Network.peerType == NetworkPeerType.Disconnected){
			// If it's a local game, update the corresponding character immediately...
			if (UFE.config.player1Character == null){
				this.MoveCursorRight(1);
			}else if (UFE.config.player2Character == null && UFE.gameMode != GameMode.StoryMode){
				this.MoveCursorRight(2);
			}
		}else{
			// If it's an online game, find out if the local player is Player1 or Player2...
			// And only update the selection for the local player...
			int localPlayer = UFE.GetLocalPlayer();
			if (localPlayer == 1 && UFE.config.player1Character == null){
				this.MoveCursorRight(1);
			}else if (localPlayer == 2 && UFE.config.player2Character == null){
				this.MoveCursorRight(2);
			}
		}
	}

	public virtual void MoveCursorRight(int player){
		// The player can only move the cursor if he hasn't selected a character yet
		if (player == 1 && UFE.config.player1Character == null || player == 2 && UFE.config.player2Character == null){
			if (this.charactersPerRow > 0){
				// Retrieve the index, row and column of the character
				int currentIndex = this.GetHoverIndex(player);
				int currentRow = currentIndex / this.charactersPerRow;
				int currentColumn = currentIndex % this.charactersPerRow;
				
				// Move the cursor to the left
				currentColumn = (currentColumn + 1) % this.charactersPerRow;
				
				// Finally, update the position of the cursor
				this.MoveCursor(player, currentRow * this.charactersPerRow + currentColumn);
			}
		}
	}

	public virtual void MoveCursorUp(){
		if (Network.peerType == NetworkPeerType.Disconnected){
			// If it's a local game, update the corresponding character immediately...
			if (UFE.config.player1Character == null){
				this.MoveCursorUp(1);
			}else if (UFE.config.player2Character == null && UFE.gameMode != GameMode.StoryMode){
				this.MoveCursorUp(2);
			}
		}else{
			// If it's an online game, find out if the local player is Player1 or Player2...
			// And only update the selection for the local player...
			int localPlayer = UFE.GetLocalPlayer();
			if (localPlayer == 1 && UFE.config.player1Character == null){
				this.MoveCursorUp(1);
			}else if (localPlayer == 2 && UFE.config.player2Character == null){
				this.MoveCursorUp(2);
			}
		}
	}

	public virtual void MoveCursorUp(int player){
		// The player can only move the cursor if he hasn't selected a character yet
		if (player == 1 && UFE.config.player1Character == null || player == 2 && UFE.config.player2Character == null){
			if (this.charactersPerRow > 0 && this.numberOfRows > 0){
				// Retrieve the index, row and column of the character
				int currentIndex = this.GetHoverIndex(player);
				int currentRow = currentIndex / this.charactersPerRow;
				int currentColumn = currentIndex % this.charactersPerRow;
				
				// Move the cursor to the left
				currentRow = (currentRow + this.numberOfRows - 1) % this.numberOfRows;
				
				// Finally, update the position of the cursor
				this.MoveCursor(player, currentRow * this.charactersPerRow + currentColumn);
			}
		}
	}
	#endregion

	#region public override methods
	public override void DoFixedUpdate(){
		base.DoFixedUpdate();

		AbstractInputController p1InputController = UFE.GetPlayer1Controller();
		AbstractInputController p2InputController = UFE.GetPlayer2Controller();

		// Retrieve the values of the horizontal and vertical axis
		bool p1AxisDown = 
			p1InputController.GetButtonDown(p1InputController.horizontalAxis) ||
			p1InputController.GetButtonDown(p1InputController.verticalAxis);

		bool p2AxisDown = 
			p2InputController.GetButtonDown(p2InputController.horizontalAxis) ||
			p2InputController.GetButtonDown(p2InputController.verticalAxis);

		// Process the player input...
		if (UFE.gameMode != GameMode.StoryMode && !UFE.GetCPU(2)){
			// If we are in "Story Mode" or in "Player vs Player (versus mode)", the controller assigned to 
			// the first player will be used always for selecting the character assigned to that player. If 
			// that player has already selected a character, he can't move the cursor unless he deselects
			// the character first.
			if (p1AxisDown){
				this.MoveCursor(p1InputController, 1);
			}
			
			if (p1InputController.GetButtonDown(UFE.config.inputOptions.confirmButton)){
				this.TrySelectCharacter(this.p1HoverIndex, 1);
			}else if (p1InputController.GetButtonDown(UFE.config.inputOptions.cancelButton)){
				this.TryDeselectCharacter(1);
			}

			// The controller assigned to the second player only can be used for selecting the character assigned to
			// the second player in "Player vs Player (versus mode)". In other game modes, the character assigned to
			// the second player will be chosen randomly (Story Mode) or will be selected by Player 1 (Player vs CPU).
			if (p2AxisDown){
				this.MoveCursor(p2InputController, 2);
			}
			
			if (p2InputController.GetButtonDown(UFE.config.inputOptions.confirmButton)){
				this.TrySelectCharacter(this.p2HoverIndex, 2);
			}else if (p2InputController.GetButtonDown(UFE.config.inputOptions.cancelButton)){
				this.TryDeselectCharacter(2);
			}
		}else{
			// However, the character assigned to the second player will be chosen by the first player in other
			// game modes (for example: Player vs CPU).
			if (p1AxisDown){
				this.MoveCursor(p1InputController);
			}
			
			if (p1InputController.GetButtonDown(UFE.config.inputOptions.confirmButton)){
				this.TrySelectCharacter();
			}else if (p1InputController.GetButtonDown(UFE.config.inputOptions.cancelButton)){
				this.TryDeselectCharacter();
			}
		}

		if (Input.GetKeyUp (KeyCode.Escape)) {
			GoToPreviousScreen ();
		}

	}

	public override void SetHoverIndex (int player, int characterIndex){
		if (!this.closing){
			base.SetHoverIndex (player, characterIndex);
			if (UFE.config.player1Character != null)
			{
				this.hudPlayer2.gameObject.SetActive(true);
				for (int i = 0; i < characters.Length; i++)
				{
					characters[i].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			if (UFE.config.player1Character != null) {
				this.hudPlayer2.gameObject.SetActive (true);
			}
			CharacterInfo[] selectableCharacters = this.GetSelectableCharacters();
			if (characterIndex >= 0 && characterIndex < selectableCharacters.Length && characterIndex < this.characters.Length){
				CharacterInfo character = selectableCharacters[characterIndex];

				// First, update the big portrait
				if (player == 1){
					if (this.namePlayer1 != null) this.namePlayer1.text = character.characterName;
					if (this.portraitPlayer1 != null){
						this.portraitPlayer1.sprite = Sprite.Create(
							character.profilePictureBig,
							new Rect(0f, 0f, character.profilePictureBig.width, character.profilePictureBig.height),
							new Vector2(0.5f * character.profilePictureBig.width, 0.5f * character.profilePictureBig.height)
						);
					}

				}else if (player == 2){
					if (this.namePlayer2 != null) this.namePlayer2.text = character.characterName;
					if (this.portraitPlayer2 != null){
						this.portraitPlayer2.sprite = Sprite.Create(
							character.profilePictureBig,
							new Rect(0f, 0f, character.profilePictureBig.width, character.profilePictureBig.height),
							new Vector2(0.5f * character.profilePictureBig.width, 0.5f * character.profilePictureBig.height)
						);
					}
				}

				// Then, update the cursor position
				if (this.hudPlayer1 != null){
					

						this.hudPlayer1.transform.position = this.characters[this.p1HoverIndex].transform.position;

				}

				if (this.hudPlayer2 != null){
			
						this.hudPlayer2.transform.position = this.characters[this.p2HoverIndex].transform.position;

				}

				if (this.hudBothPlayers != null){
					RectTransform rt = this.hudBothPlayers.transform as RectTransform;
					if (rt != null){
						rt.anchoredPosition = this.characters[this.p2HoverIndex].rectTransform.anchoredPosition;
					}else{
						this.hudBothPlayers.transform.position = this.characters[this.p2HoverIndex].transform.position;
					}
				}
			}

			this.UpdateHud();
		}
	}

	public override void OnCharacterSelectionAllowed (int characterIndex, int player){
		base.OnCharacterSelectionAllowed (characterIndex, player);
		this.UpdateHud();
	}

	public override void OnShow (){
		// Set the portraits of the characters
		if (this.characters != null){
			// First, update the portraits of the characters until we run out of characters or portrait slots....
			CharacterInfo[] selectableCharacters = this.GetSelectableCharacters();
			for (int i = 0; i < selectableCharacters.Length && i < this.characters.Length; ++i){
				this.characters[i].gameObject.SetActive(true);


//				this.characters[i].sprite = Sprite.Create(
//					selectableCharacters[i].profilePictureSmall,
//					new Rect(0f, 0f, selectableCharacters[i].profilePictureSmall.width, selectableCharacters[i].profilePictureSmall.height),
//					new Vector2(0.5f * selectableCharacters[i].profilePictureSmall.width, 0.5f * selectableCharacters[i].profilePictureSmall.height)
//				);
			}

			// If there are more slots than characters, fill the remaining slots with the "No Character" sprite...
			// If the "No Character" sprite is undefined, hide the image instead.
			for (int i = selectableCharacters.Length; i < this.characters.Length; ++i){
				if (this.noCharacterSprite != null){
					this.characters[i].gameObject.SetActive(true);
					this.characters[i].sprite = this.noCharacterSprite;
				}else{
					this.characters[i].gameObject.SetActive(false);
				}
			}
		}

		base.OnShow();

		if (this.music != null){
			UFE.DelayLocalAction(delegate(){UFE.PlayMusic(this.music);}, this.delayBeforePlayingMusic);
		}

		if (this.stopPreviousSoundEffectsOnLoad){
			UFE.StopSounds();
		}
		
		if (this.onLoadSound != null){
			UFE.DelayLocalAction(delegate(){UFE.PlaySound(this.onLoadSound);}, this.delayBeforePlayingMusic);
		}

		if (UFE.gameMode == GameMode.StoryMode){
			if (this.namePlayer2 != null){
				this.namePlayer2.text = "???";
			}

			if (this.portraitPlayer2 != null){
				this.portraitPlayer2.gameObject.SetActive(false);
			}

			this.UpdateHud();
		}else{
			this.SetHoverIndex(2, Mathf.Min(this.GetHoverIndex(2), this.charactersPerRow - 1));

			if (this.portraitPlayer2 != null){
				this.portraitPlayer2.gameObject.SetActive(true);
			}
			countryName.gameObject.SetActive(false);
			flagParent.SetActive(false);
			if (isMultiplayerSelected)
			{

				//portraitPlayer2.gameObject.SetActive(false);
				namePlayer2.gameObject.SetActive(false);
				backContainer.SetActive(false);
				player2Txt.SetActive(false);
				characterSHows.inst.hidePlayer2Models();

			}
			else
			{
				//portraitPlayer2.gameObject.SetActive(true);
				namePlayer2.gameObject.SetActive(true);
				backContainer.SetActive(true);
				player2Txt.SetActive(true);
				
			}
		}
	}
	#endregion

	#region protected instance methods
	protected virtual void UpdateHud(){
		if (UFE.gameMode == GameMode.StoryMode){
			if (this.hudPlayer1 != null){
				this.hudPlayer1.SetBool("IsHidden", false);
				this.hudPlayer1.SetBool("IsSelected", UFE.config.player1Character != null);
			}
			
			if (this.hudPlayer2 != null){
				this.hudPlayer2.SetBool("IsHidden", true);
				this.hudPlayer2.SetBool("IsSelected", UFE.config.player2Character != null);
			}
			
			if (this.hudBothPlayers != null){
				this.hudBothPlayers.SetBool("IsHidden", true);
				this.hudBothPlayers.SetBool("IsSelected", UFE.config.player1Character != null && UFE.config.player2Character != null);
			}
		}else{
			if (this.hudPlayer1 != null){
				//this.hudPlayer1.SetBool("IsHidden", this.p1HoverIndex == this.p2HoverIndex);
				this.hudPlayer1.SetBool("IsSelected", UFE.config.player1Character != null);
			}
			
			if (this.hudPlayer2 != null){
			//	this.hudPlayer2.SetBool("IsHidden", this.p1HoverIndex == this.p2HoverIndex);
				this.hudPlayer2.SetBool("IsSelected", UFE.config.player2Character != null);
			}

			if (this.hudBothPlayers != null){
				this.hudBothPlayers.SetBool("IsHidden", this.p1HoverIndex != this.p2HoverIndex);

				this.hudBothPlayers.SetBool(
					"IsSelected", 
					UFE.config.player1Character != null && UFE.config.player2Character != null
				);
			}
		}
	}

	protected virtual void MoveCursor(int player, int characterIndex){
		int previousIndex = this.GetHoverIndex(player);
		this.SetHoverIndex(player, characterIndex);
		int newIndex = this.GetHoverIndex(player);
		if (previousIndex != newIndex && this.moveCursorSound != null) UFE.PlaySound(this.moveCursorSound);
	}

	protected virtual void MoveCursor(AbstractInputController controller){
		if (Network.peerType == NetworkPeerType.Disconnected){
			// If it's a local game, update the corresponding character immediately...
			if (UFE.config.player1Character == null){
				this.MoveCursor(controller, 1);
			}else if (UFE.config.player2Character == null && UFE.gameMode != GameMode.StoryMode){
				this.MoveCursor(controller, 2);
			}
		}else{
			// If it's an online game, find out if the local player is Player1 or Player2...
			// And only update the selection for the local player...
			int localPlayer = UFE.GetLocalPlayer();
			if (localPlayer == 1 && UFE.config.player1Character == null){
				this.MoveCursor(controller, 1);
			}else if (localPlayer == 2 && UFE.config.player2Character == null){
				this.MoveCursor(controller, 2);
			}
		}
	}

	protected virtual void MoveCursor(AbstractInputController controller, int player){
		float horizontalAxis = controller.GetAxisRaw(controller.horizontalAxis);
		float verticalAxis = controller.GetAxisRaw(controller.verticalAxis);

		if (horizontalAxis > 0)			this.MoveCursorRight(player);
		else if (horizontalAxis < 0)	this.MoveCursorLeft(player);
		
		if (verticalAxis > 0)			this.MoveCursorUp(player);
		else if (verticalAxis < 0)		this.MoveCursorDown(player);
	}
	#endregion
}
