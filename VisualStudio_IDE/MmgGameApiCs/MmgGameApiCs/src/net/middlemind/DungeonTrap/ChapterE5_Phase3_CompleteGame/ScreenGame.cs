﻿using System;
using Microsoft.Xna.Framework;
using net.middlemind.MmgGameApiCs.MmgBase;
using net.middlemind.MmgGameApiCs.MmgCore;
using static net.middlemind.MmgGameApiCs.MmgCore.GamePanel;

namespace net.middlemind.DungeonTrap.ChapterE5_Phase3_CompleteGame
{
    /// <summary>
    /// A game screen object, ScreenGame, that extends the MmgGameScreen base
    /// class. This class is for testing new UI widgets, etc.
    ///
    /// @author Victor G.Brusca
    /// 03/15/2020
    /// </summary>
    public class ScreenGame : Screen
    {
        /// <summary>
        /// An enumeration that tracks which number is visible during game start countdown and in-game countdown.
        /// </summary>
        private enum NumberState
        {
            NONE,
            NUMBER_1,
            NUMBER_2,
            NUMBER_3
        };

        /// <summary>
        /// An enumeration that tracks which state this Screen is currently rendering.
        /// Allows the Screen to support different views like in-game, countdown, game over, game start, etc.
        /// </summary>
        private enum State
        {
            NONE,
            SHOW_GAME,
            SHOW_COUNT_DOWN,
            SHOW_COUNT_DOWN_IN_GAME,
            SHOW_GAME_OVER,
            SHOW_GAME_EXIT
        };

        /// <summary>
        /// The type of game to run.
        /// </summary>
        private GameType gameType = GameType.GAME_ONE_PLAYER;

        /// <summary>
        /// The previous game state. 
        /// </summary>
        private State statePrev = State.NONE;

        /// <summary>
        /// The current game state.
        /// </summary>
        private new State state = State.NONE;

        /// <summary>
        /// The current state of the count down number.
        /// </summary>
        private NumberState numberState = NumberState.NONE;

        /// <summary>
        /// The display time of the number in milliseconds.
        /// </summary>
        private long timeNumberMs = 0L;

        /// <summary>
        /// The total display time of the number in milliseconds.
        /// </summary>
        private long timeNumberDisplayMs = 1000;

        /// <summary>
        /// A number display temporary value.
        /// </summary>
        private long timeTmpMs = 0L;

        /// <summary>
        /// The background of the game.
        /// </summary>
        private MmgBmp bground;

        /// <summary>
        /// The count down number 1.
        /// </summary>
        private MmgBmp number1;

        /// <summary>
        /// The count down number 2.
        /// </summary>
        private MmgBmp number2;

        /// <summary>
        /// The count down number 3.
        /// </summary>
        private MmgBmp number3;

        /// <summary>
        /// A bool indicating if the level's time is up.
        /// </summary>
        private bool scoreTimeUp = false;

        /// <summary>
        /// The player one score.
        /// </summary>
        private int scorePlayerOne = 0;

        /// <summary>
        /// The player two score.
        /// </summary>
        private int scorePlayerTwo = 0;

        /// <summary>
        /// A text image for the exit string.
        /// </summary>
        private MmgFont exit;

        /// <summary>
        /// A background for the text exit image.
        /// </summary>
        private MmgBmp exitBground;

        /// <summary>
        /// A random number generator.
        /// </summary>
        private Random rand;

        /// <summary>
        /// The current position of the screen.
        /// </summary>
        private MmgVector2 screenPos;

        /// <summary>
        /// The source of the popup background image. 
        /// </summary>
        private MmgBmp bgroundPopupSrc;

        /// <summary>
        /// The popup background image. 
        /// </summary>
        private Mmg9Slice bgroundPopup;

        /// <summary>
        /// A text image for the ok string. 
        /// </summary>
        private MmgFont txtOk;

        /// <summary>
        /// A text image for the cancel string.
        /// </summary>
        private MmgFont txtCancel;

        /// <summary>
        /// A text image for the game's goal string. 
        /// </summary>
        private MmgFont txtGoal;

        /// <summary>
        /// A text image for the game's player one directions.
        /// </summary>
        private MmgFont txtDirecP1;

        /// <summary>
        /// A text image for the game's player two directions. 
        /// </summary>
        private MmgFont txtDirecP2;

        /// <summary>
        /// A text image for the game over text player one. 
        /// </summary>
        private MmgFont txtGameOver1;

        /// <summary>
        /// A text image for the game over text player two. 
        /// </summary>
        private MmgFont txtGameOver2;

        /// <summary>
        /// A text image for the game over time ran out message. 
        /// </summary>
        private MmgFont txtGameOver3;

        /// <summary>
        /// A padding value used in UI positioning.
        /// </summary>
        private int padding = MmgHelper.ScaleValue(4);

        /// <summary>
        /// The total width of the popup window.
        /// </summary>
        private int popupTotalWidth = MmgHelper.ScaleValue(300);

        /// <summary>
        /// The total height of the popup window.
        /// </summary>
        private int popupTotalHeight = MmgHelper.ScaleValue(120);

        /// <summary>
        /// The player one character.
        /// </summary>
        private MdtCharInterPlayer player1;

        /// <summary>
        /// The player two character.
        /// </summary>
        private MdtCharInterPlayer player2;

        /// <summary>
        /// A private bool value used in internal class methods.
        /// </summary>
        private bool lret;

        /// <summary>
        /// The top of the game.
        /// </summary>
        public static int GAME_TOP = MmgScreenData.GetGameTop();

        /// <summary>
        /// The top of the game's board.
        /// </summary>
        public static int BOARD_TOP = GAME_TOP + MmgHelper.ScaleValue(106);

        /// <summary>
        /// The bottom of the game. 
        /// </summary>
        public static int GAME_BOTTOM = MmgScreenData.GetGameBottom();

        /// <summary>
        /// The bottom of the game's board.
        /// </summary>
        public static int BOARD_BOTTOM = GAME_BOTTOM - MmgHelper.ScaleValue(56);

        /// <summary>
        /// The left of the game.
        /// </summary>
        public static int GAME_LEFT = MmgScreenData.GetGameLeft();

        /// <summary>
        /// The left of the game's board.
        /// </summary>
        public static int BOARD_LEFT = GAME_LEFT + MmgHelper.ScaleValue(20);

        /// <summary>
        /// The right of the game.
        /// </summary>
        public static int GAME_RIGHT = MmgScreenData.GetGameRight();

        /// <summary>
        /// The right of the game's board.
        /// </summary>
        public static int BOARD_RIGHT = GAME_RIGHT - MmgHelper.ScaleValue(132);

        /// <summary>
        /// The game's width.
        /// </summary>
        public static int GAME_WIDTH = GAME_RIGHT - GAME_LEFT;

        /// <summary>
        /// The game board's width.
        /// </summary>
        public static int BOARD_WIDTH = BOARD_RIGHT - BOARD_LEFT;

        /// <summary>
        /// The game's height.
        /// </summary>
        public static int GAME_HEIGHT = GAME_BOTTOM - GAME_TOP;

        /// <summary>
        /// The game board's height.
        /// </summary>
        public static int BOARD_HEIGHT = BOARD_BOTTOM - BOARD_TOP;

        /// <summary>
        /// The number of enemy waves in this game.
        /// </summary>
        public static int WAVE_COUNT = 12;

        /// <summary>
        /// The speed in milliseconds of the player character's frames when moving.
        /// </summary>
        public int frameMsPerFrameMoving = 100;

        /// <summary>
        /// The speed in milliseconds of the player character's frames when not moving.
        /// </summary>
        public int frameMsPerFrameNotMoving = 300;

        /// <summary>
        /// A bool indicating if the player snaps to the front direction.
        /// </summary>
        public bool playerSnapToFront = false;

        /// <summary>
        /// A UI health bar for the player one health.
        /// </summary>
        public MdtUiHealthBar player1HealthBar = null;

        /// <summary>
        /// A UI health bar for the player two health.
        /// </summary>
        public MdtUiHealthBar player2HealthBar = null;

        /// <summary>
        /// An MmgBmp images used to represent the source of a sprite matrix.
        /// </summary>
        private MmgBmp spriteMatrixSrc;

        /// <summary>
        /// An MmgSpriteMatrix instance used to hold sprite matrix data.
        /// </summary>
        private MmgSpriteMatrix spriteMatrix;

        /// <summary>
        /// An MmgSprite instance used to hold sprite data.
        /// </summary>
        private MmgSprite sprite;

        /// <summary>
        /// A private field used to hold a Color value.
        /// </summary>
        private Color c;

        /// <summary>
        /// A text representation of player 1.
        /// </summary>
        public MmgFont txtPlayer1 = null;

        /// <summary>
        /// A text representation of player 2.
        /// </summary>
        public MmgFont txtPlayer2 = null;

        /// <summary>
        /// A text representation of player 2's score.
        /// </summary>
        public MmgFont txtPlayer1Score = null;

        /// <summary>
        /// A text representation of player 2's score.
        /// </summary>
        public MmgFont txtPlayer2Score = null;

        /// <summary>
        /// A text element for the player 1 HUD section.
        /// </summary>
        public MmgFont txtPlayer1Section = null;

        /// <summary>
        /// A text element for the player 2 HUD section.
        /// </summary>
        public MmgFont txtPlayer2Section = null;

        /// <summary>
        /// A text HUD UI element for player 1 weapons.
        /// </summary>
        public MmgFont txtPlayer1Weapon = null;

        /// <summary>
        /// A text HUD UI element for player 2 weapons.
        /// </summary>
        public MmgFont txtPlayer2Weapon = null;

        /// <summary>
        /// A text HUD UI element for player 1 modifications.
        /// </summary>
        public MmgFont txtPlayer1Mod = null;

        /// <summary>
        /// A text HUD UI element for player 2 modifications.
        /// </summary>
        public MmgFont txtPlayer2Mod = null;

        /// <summary>
        /// A text representation of the player 1 modifier time.
        /// </summary>
        public MmgFont txtPlayer1ModTime = null;

        /// <summary>
        /// A text representation of the player 2 modifier time.
        /// </summary>
        public MmgFont txtPlayer2ModTime = null;

        /// <summary>
        /// A text representation of the current level time.
        /// </summary>
        public MmgFont txtLevelTime = null;

        /// <summary>
        /// A text representation of the current level.
        /// </summary>
        public MmgFont txtLevel = null;

        /// <summary>
        /// The game logo to use on the main menu and game board.
        /// </summary>
        public MmgBmp gameLogo = null;

        /// <summary>
        /// An array of enemy waves. 
        /// </summary>
        private MdtEnemyWave[] waves;

        /// <summary>
        /// The current enemy wave.
        /// </summary>
        private MdtEnemyWave wavesCurrent;

        /// <summary>
        /// The first background torch.
        /// </summary>
        private MdtObjTorch torch1;

        /// <summary>
        /// The second background torch.
        /// </summary>
        private MdtObjTorch torch2;

        /// <summary>
        /// The third background torch.
        /// </summary>
        private MdtObjTorch torch3;

        /// <summary>
        /// The fourth background torch.
        /// </summary>
        private MdtObjTorch torch4;

        /// <summary>
        /// The current enemy wave index.
        /// </summary>
        private int wavesCurrentIdx;

        /// <summary>
        /// Source ID for player 1 input.
        /// </summary>
        public static int SRC_PLAYER_1 = GameSettings.SRC_KEYBOARD;

        /// <summary>
        /// Source ID for player 2 input.
        /// </summary>
        public static int SRC_PLAYER_2 = 255;

        /// <summary>
        /// Player 1 HUD weapon image.
        /// </summary>
        public MmgBmp player1WeaponBmp;

        /// <summary>
        /// Player 1 HUD modifier image.
        /// </summary>
        public MmgBmp player1ModBmp;

        /// <summary>
        /// Player 2 HUD weapon image.
        /// </summary>
        public MmgBmp player2WeaponBmp;

        /// <summary>
        /// Player 2 HUD modifier image.
        /// </summary>
        public MmgBmp player2ModBmp;

        /// <summary>
        /// Generic door lock full.
        /// </summary>
        public MmgBmp doorLockFull;

        /// <summary>
        /// Generic door open full.
        /// </summary>
        public MmgBmp doorOpenFull;

        /// <summary>
        /// Generic door lock icon.
        /// </summary>
        public MmgBmp doorLockIcon;

        /// <summary>
        /// Door locked icon for the top left door.
        /// </summary>
        public MmgBmp doorTopLeftLocked;

        /// <summary>
        /// Door opened icon for the top left door.
        /// </summary>
        public MmgBmp doorTopLeftOpened;

        /// <summary>
        /// Lock icon for the left hand door.
        /// </summary>
        public MmgBmp doorLeftLockIcon;

        /// <summary>
        /// Door locked icon for the top right door. 
        /// </summary>
        public MmgBmp doorTopRightLocked;

        /// <summary>
        /// Door opened icon for the top right door. 
        /// </summary>
        public MmgBmp doorTopRightOpened;

        /// <summary>
        /// Lock icon for the right hand door.
        /// </summary>
        public MmgBmp doorRightLockIcon;

        /// <summary>
        /// Lock icon for the bottom left hand door.
        /// </summary>
        public MmgBmp doorBotLeftLockIcon;

        /// <summary>
        /// Lock icon for the bottom right hand door.
        /// </summary>
        public MmgBmp doorBotRightLockIcon;

        /// <summary>
        /// The random placement rectangle for the left hand side of the board.
        /// </summary>
        public MmgRect randoLeft = null;

        /// <summary>
        /// The random placement rectangle for the right hand side of the board. 
        /// </summary>
        public MmgRect randoRight = null;

        /// <summary>
        /// A background image used to frame the count down numbers. 
        /// </summary>
        public Mmg9Slice numberBground = null;

        /// <summary>
        /// A container that is used to hold object game objects. 
        /// </summary>
        public MmgContainer gameObjects = null;

        /// <summary>
        /// A container that is used to hold item game objects.
        /// </summary>
        public MmgContainer gameItems = null;

        /// <summary>
        /// A container that is used to hold enemy game objects.
        /// </summary>
        public MmgContainer gameEnemies = null;

        /// <summary>
        /// The image animation frames needed to display the demon enemy.
        /// </summary>
        private MmgSprite enemyDemonFrames = null;

        /// <summary>
        /// The image animation frames needed to display the banshee enemy. 
        /// </summary>
        private MmgSprite enemyBansheeFrames = null;

        /// <summary>
        /// The image animation frames needed to display the warlock enemy.
        /// </summary>
        private MmgSprite enemyWarlockFrames = null;

        /// <summary>
        /// The number of player characters that are still alive.
        /// </summary>
        private int playersAliveCount = 0;

        /// <summary>
        /// A bool indicating random level numbers are active.
        /// </summary>
        private bool randomWaves = false;

        /// <summary>
        /// A sound to play when an enemy character is struck with a weapon.
        /// </summary>
        private MmgSound sound1 = null;

        /// <summary>
        /// Constructor, sets the game state associated with this screen, and sets
        /// the owner GamePanel instance.
        /// </summary>
        /// <param name="State">The game state of this game screen.</param>
        /// <param name="Owner">The owner of this game screen.</param>
        public ScreenGame(GameStates State, GamePanel Owner)
            : base(State, Owner)
        {
            pause = false;
            ready = false;
            owner = Owner;
            rand = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        /// <summary>
        /// Loads all the resources needed to display this game screen and support all Screen states.
        /// </summary>
        public override void LoadResources()
        {
            pause = true;
            rand = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            classConfig = MmgHelper.ReadClassConfigFile(GameSettings.CLASS_CONFIG_DIR + GameSettings.NAME + "/screen_game.txt");

            SetHeight(MmgScreenData.GetGameHeight());
            SetWidth(MmgScreenData.GetGameWidth());
            SetPosition(MmgScreenData.GetPosition());
            screenPos = GetPosition();

            String key = "";
            MmgBmp lval = null;
            String file = "";
            int tmp = 0;
            int tmpW = 0;
            int tmpH = 0;
            MmgBmp[] frames = null;
            MmgObj obj = null;
            MdtItemPotionYellow pTmp = null;

            scorePlayerOne = 0;
            scorePlayerTwo = 0;
            scoreTimeUp = false;

            //Load game board config
            file = MmgHelper.ContainsKeyString("bmpGameBoard", "dt_level1.png", classConfig);
            bground = MmgHelper.GetBasicCachedBmp(file);
            if (bground != null)
            {
                MmgHelper.CenterHorAndVert(bground);
                bground = MmgHelper.ContainsKeyMmgBmpScaleAndPosition("gameBoard", bground, classConfig, bground.GetPosition());
                AddObj(bground);
            }

            //Load door lock source
            doorLockFull = MmgHelper.GetBasicCachedBmp("door_locked.png");
            doorOpenFull = MmgHelper.GetBasicCachedBmp("door_opened.png");
            doorLockIcon = MmgHelper.GetBasicCachedBmp("door_lock_icon.png");

            //Load door locks
            int adj = 90;
            doorTopLeftLocked = doorLockFull.CloneTyped();
            doorTopLeftLocked.SetPosition(new MmgVector2(MmgHelper.ScaleValue(146), MmgHelper.ScaleValue(adj + 43)));
            AddObj(doorTopLeftLocked);

            doorTopLeftOpened = doorOpenFull.CloneTyped();
            doorTopLeftOpened.SetPosition(new MmgVector2(MmgHelper.ScaleValue(146), MmgHelper.ScaleValue(adj + 43)));
            AddObj(doorTopLeftOpened);

            doorTopRightLocked = doorLockFull.CloneTyped();
            doorTopRightLocked.SetPosition(new MmgVector2(MmgHelper.ScaleValue(466), MmgHelper.ScaleValue(adj + 43)));
            AddObj(doorTopRightLocked);

            doorTopRightOpened = doorOpenFull.CloneTyped();
            doorTopRightOpened.SetPosition(new MmgVector2(MmgHelper.ScaleValue(466), MmgHelper.ScaleValue(adj + 43)));
            AddObj(doorTopRightOpened);

            doorLeftLockIcon = doorLockIcon.CloneTyped();
            doorLeftLockIcon.SetPosition(new MmgVector2(MmgHelper.ScaleValue(18 + doorLockIcon.GetWidth() / 2), MmgHelper.ScaleValue(adj + 206 + doorLockIcon.GetHeight() / 2)));
            AddObj(doorLeftLockIcon);

            doorRightLockIcon = doorLockIcon.CloneTyped();
            doorRightLockIcon.SetPosition(new MmgVector2(MmgHelper.ScaleValue(690 + doorLockIcon.GetWidth() / 2), MmgHelper.ScaleValue(adj + 206 + doorLockIcon.GetHeight() / 2)));
            AddObj(doorRightLockIcon);

            doorBotLeftLockIcon = doorLockIcon.CloneTyped();
            doorBotLeftLockIcon.SetPosition(new MmgVector2(MmgHelper.ScaleValue(154 + doorLockIcon.GetWidth() / 2), MmgHelper.ScaleValue(adj + 318 + doorLockIcon.GetHeight() / 2)));
            AddObj(doorBotLeftLockIcon);

            doorBotRightLockIcon = doorLockIcon.CloneTyped();
            doorBotRightLockIcon.SetPosition(new MmgVector2(MmgHelper.ScaleValue(474 + doorLockIcon.GetWidth() / 2), MmgHelper.ScaleValue(adj + 318 + doorLockIcon.GetHeight() / 2)));
            AddObj(doorBotRightLockIcon);

            //Load torches
            MdtObjTorch torch = new MdtObjTorch();
            torch.SetPosition(MmgHelper.ScaleValue(65) - torch.GetWidth() / 2, MmgScreenData.GetGameTop() + MmgHelper.ScaleValue(50));
            torch.isBurning = true;
            torch1 = torch;
            AddObj(torch1);

            torch = new MdtObjTorch();
            torch.SetPosition(MmgHelper.ScaleValue(290) - torch.GetWidth() / 2, MmgScreenData.GetGameTop() + MmgHelper.ScaleValue(50));
            torch.isBurning = true;
            torch2 = torch;
            AddObj(torch2);

            torch = new MdtObjTorch();
            torch.SetPosition(MmgHelper.ScaleValue(380) - torch.GetWidth() / 2, MmgScreenData.GetGameTop() + MmgHelper.ScaleValue(50));
            torch.isBurning = true;
            torch3 = torch;
            AddObj(torch3);

            torch = new MdtObjTorch();
            torch.SetPosition(MmgHelper.ScaleValue(610) - torch.GetWidth() / 2, MmgScreenData.GetGameTop() + MmgHelper.ScaleValue(50));
            torch.isBurning = true;
            torch4 = torch;
            AddObj(torch4);

            //Load random item drop rectangles
            randoLeft = new MmgRect(BOARD_LEFT + MmgHelper.ScaleValue(96), BOARD_TOP + MmgHelper.ScaleValue(64), BOARD_BOTTOM - MmgHelper.ScaleValue(64), BOARD_LEFT + BOARD_WIDTH / 2 - MmgHelper.ScaleValue(32));
            randoRight = new MmgRect(BOARD_LEFT + BOARD_WIDTH / 2 + MmgHelper.ScaleValue(64), BOARD_TOP + MmgHelper.ScaleValue(64), BOARD_BOTTOM - MmgHelper.ScaleValue(64), BOARD_RIGHT - MmgHelper.ScaleValue(96));

            //These containers ensure these items are visible below the player        
            //Load game items container
            gameItems = new MmgContainer();
            gameItems.SetX(BOARD_LEFT);
            gameItems.SetY(BOARD_TOP);
            gameItems.SetWidth(BOARD_WIDTH);
            gameItems.SetHeight(BOARD_HEIGHT);
            AddObj(gameItems);

            //Load game objects container
            gameObjects = new MmgContainer();
            gameObjects.SetX(BOARD_LEFT);
            gameObjects.SetY(BOARD_TOP);
            gameObjects.SetWidth(BOARD_WIDTH);
            gameObjects.SetHeight(BOARD_HEIGHT);
            AddObj(gameObjects);

            //Load game objects container
            gameEnemies = new MmgContainer();
            gameEnemies.SetX(BOARD_LEFT);
            gameEnemies.SetY(BOARD_TOP);
            gameEnemies.SetWidth(BOARD_WIDTH);
            gameEnemies.SetHeight(BOARD_HEIGHT);
            AddObj(gameEnemies);

            //Load player movement speeds
            tmp = MmgHelper.ContainsKeyInt("bmpPlayerFrameMsPerFrameMoving", 250, classConfig);
            frameMsPerFrameMoving = tmp;
            tmp = MmgHelper.ContainsKeyInt("bmpPlayerFrameMsPerFrameNotMoving", 500, classConfig);
            frameMsPerFrameNotMoving = tmp;

            //Load player1 frames
            tmp = MmgHelper.ContainsKeyInt("bmpPlayerFrameCount", 16, classConfig);
            frames = new MmgBmp[tmp];
            for (int i = 0; i < frames.Length; i++)
            {
                file = MmgHelper.ContainsKeyString("bmpPlayer1Frame" + (i + 1), "soldier_frame_" + (i + 1) + ".png", classConfig);
                frames[i] = MmgHelper.GetBasicCachedBmp(file);

                if (frames[i] != null)
                {
                    frames[i] = MmgHelper.ContainsKeyMmgBmpScaleAndPosition("bmpPlayer1Frame" + (i + 1), frames[i], classConfig, MmgVector2.GetOriginVec());
                }
                else
                {
                    MmgHelper.wr("ScreenGame: Error loading player1 frame " + i + ".");
                }
            }

            obj = new MmgObj(frames[0].GetWidth(), frames[0].GetHeight());
            MmgHelper.CenterHorAndVert(obj);
            obj.SetX(obj.GetX() - (GAME_WIDTH - BOARD_WIDTH) / 2 + obj.GetWidth());
            obj.SetY(obj.GetY() - frames[0].GetHeight() - MmgHelper.ScaleValue(20));

            player1 = new MdtCharInterPlayer(new MmgSprite(frames), 0, 3, 12, 15, 4, 7, 8, 11, this, MdtPlayerType.PLAYER_1);
            player1.SetMmgColor(null);
            player1.SetDir(MmgDir.DIR_FRONT);
            player1.SetIsVisible(true);
            player1.SetPosition(obj.GetPosition().Clone());
            player1.SetPosition(new MmgVector2(player1.GetX(), player1.GetY() + MmgHelper.ScaleValue(10)));

            tmp = MmgHelper.ContainsKeyInt("bmpPlayerFrameMsPerFrame", 250, classConfig);
            player1.subj.SetMsPerFrame(tmp);
            player1.speed = ScreenGame.GetSpeedPerFrame(120);
            player1WeaponBmp = player1.weaponCurrent.subjRight;
            AddObj(player1);

            //Load player2 frames
            tmp = MmgHelper.ContainsKeyInt("bmpPlayerFrameCount", 16, classConfig);
            frames = new MmgBmp[tmp];
            for (int i = 0; i < frames.Length; i++)
            {
                file = MmgHelper.ContainsKeyString("bmpPlayer2Frame" + (i + 1), "soldier_frame_" + (i + 1) + "_2p.png", classConfig);
                frames[i] = MmgHelper.GetBasicCachedBmp(file);

                if (frames[i] != null)
                {
                    frames[i] = MmgHelper.ContainsKeyMmgBmpScaleAndPosition("bmpPlayer2Frame" + (i + 1), frames[i], classConfig, MmgVector2.GetOriginVec());
                }
                else
                {
                    MmgHelper.wr("ScreenGame: Error loading player2 frame " + i + ".");
                }
            }

            obj = new MmgObj(frames[0].GetWidth(), frames[0].GetHeight());
            MmgHelper.CenterHorAndVert(obj);
            obj.SetX(obj.GetX() - (GAME_WIDTH - BOARD_WIDTH) / 2 + obj.GetWidth());
            obj.SetY(obj.GetY() + frames[0].GetHeight() + MmgHelper.ScaleValue(20));

            player2 = new MdtCharInterPlayer(new MmgSprite(frames), 0, 3, 12, 15, 4, 7, 8, 11, this, MdtPlayerType.PLAYER_2);
            player2.SetMmgColor(null);
            player2.SetDir(MmgDir.DIR_FRONT);
            player2.SetIsVisible(true);
            player2.SetPosition(obj.GetPosition().Clone());

            tmp = MmgHelper.ContainsKeyInt("bmpPlayerFrameMsPerFrame", 250, classConfig);
            player2.subj.SetMsPerFrame(tmp);
            player2.speed = ScreenGame.GetSpeedPerFrame(120);
            player2WeaponBmp = player2.weaponCurrent.subjRight;
            AddObj(player2);

            //Load enemy demon frames
            spriteMatrixSrc = MmgHelper.GetBasicCachedBmp("enemy_demon_spritematrix_w_shadow.png");
            spriteMatrixSrc = MmgBmpScaler.ScaleMmgBmp(spriteMatrixSrc, 1.5f, true);

            MmgHelper.CenterHor(spriteMatrixSrc);
            spriteMatrixSrc.SetPosition(new MmgVector2(25, MmgHelper.ScaleValue(160)));
            spriteMatrix = new MmgSpriteMatrix(spriteMatrixSrc.CloneTyped(), 48, 51, 4, 3);
            MmgObj tmpObj = new MmgObj();
            tmpObj.SetHeight(68);
            tmpObj.SetWidth(64);
            MmgHelper.CenterHorAndVert(tmpObj);

            MmgVector2 tmpPos = tmpObj.GetPosition().Clone();
            tmpPos.SetY(tmpPos.GetY() + MmgHelper.ScaleValue(15));
            tmpPos.SetX(MmgScreenData.GetGameWidth() - 64 - 25);

            MmgBmp[] tEnm = new MmgBmp[16];
            //Enemy Front
            tEnm[0] = spriteMatrix.GetFrame(0);
            tEnm[1] = spriteMatrix.GetFrame(1);
            tEnm[2] = spriteMatrix.GetFrame(2);
            tEnm[3] = spriteMatrix.GetFrame(1);

            //Enemy Left
            tEnm[4] = spriteMatrix.GetFrame(3);
            tEnm[5] = spriteMatrix.GetFrame(4);
            tEnm[6] = spriteMatrix.GetFrame(5);
            tEnm[7] = spriteMatrix.GetFrame(4);

            //Enemy Right
            tEnm[8] = spriteMatrix.GetFrame(6);
            tEnm[9] = spriteMatrix.GetFrame(7);
            tEnm[10] = spriteMatrix.GetFrame(8);
            tEnm[11] = spriteMatrix.GetFrame(7);

            //Enemy Back
            tEnm[12] = spriteMatrix.GetFrame(9);
            tEnm[13] = spriteMatrix.GetFrame(10);
            tEnm[14] = spriteMatrix.GetFrame(11);
            tEnm[15] = spriteMatrix.GetFrame(10);

            enemyDemonFrames = new MmgSprite(tEnm, tmpPos);
            enemyDemonFrames.SetMsPerFrame(tmp);

            //Load enemy banshee frames
            spriteMatrixSrc = MmgHelper.GetBasicCachedBmp("enemy_banshee_spritematrix_w_shadow.png");
            spriteMatrixSrc = MmgBmpScaler.ScaleMmgBmp(spriteMatrixSrc, 1.5f, true);

            MmgHelper.CenterHor(spriteMatrixSrc);
            spriteMatrixSrc.SetPosition(new MmgVector2(25, MmgHelper.ScaleValue(160)));
            spriteMatrix = new MmgSpriteMatrix(spriteMatrixSrc.CloneTyped(), 48, 51, 4, 3);
            tmpObj = new MmgObj();
            tmpObj.SetHeight(68);
            tmpObj.SetWidth(64);
            MmgHelper.CenterHorAndVert(tmpObj);

            tmpPos = tmpObj.GetPosition().Clone();
            tmpPos.SetY(tmpPos.GetY() + MmgHelper.ScaleValue(15));
            tmpPos.SetX(MmgScreenData.GetGameWidth() - 64 - 25);

            tEnm = new MmgBmp[16];
            //Enemy Front
            tEnm[0] = spriteMatrix.GetFrame(0);
            tEnm[1] = spriteMatrix.GetFrame(1);
            tEnm[2] = spriteMatrix.GetFrame(2);
            tEnm[3] = spriteMatrix.GetFrame(1);

            //Enemy Left
            tEnm[4] = spriteMatrix.GetFrame(3);
            tEnm[5] = spriteMatrix.GetFrame(4);
            tEnm[6] = spriteMatrix.GetFrame(5);
            tEnm[7] = spriteMatrix.GetFrame(4);

            //Enemy Right
            tEnm[8] = spriteMatrix.GetFrame(6);
            tEnm[9] = spriteMatrix.GetFrame(7);
            tEnm[10] = spriteMatrix.GetFrame(8);
            tEnm[11] = spriteMatrix.GetFrame(7);

            //Enemy Back
            tEnm[12] = spriteMatrix.GetFrame(9);
            tEnm[13] = spriteMatrix.GetFrame(10);
            tEnm[14] = spriteMatrix.GetFrame(11);
            tEnm[15] = spriteMatrix.GetFrame(10);

            enemyBansheeFrames = new MmgSprite(tEnm, tmpPos);
            enemyBansheeFrames.SetMsPerFrame(tmp);

            //Load enemy warlock frames
            spriteMatrixSrc = MmgHelper.GetBasicCachedBmp("enemy_warlock_spritematrix_w_shadow.png");
            spriteMatrixSrc = MmgBmpScaler.ScaleMmgBmp(spriteMatrixSrc, 1.5f, true);

            MmgHelper.CenterHor(spriteMatrixSrc);
            spriteMatrixSrc.SetPosition(new MmgVector2(25, MmgHelper.ScaleValue(160)));
            spriteMatrix = new MmgSpriteMatrix(spriteMatrixSrc.CloneTyped(), 48, 51, 4, 3);
            tmpObj = new MmgObj();
            tmpObj.SetHeight(68);
            tmpObj.SetWidth(64);
            MmgHelper.CenterHorAndVert(tmpObj);

            tmpPos = tmpObj.GetPosition().Clone();
            tmpPos.SetY(tmpPos.GetY() + MmgHelper.ScaleValue(15));
            tmpPos.SetX(MmgScreenData.GetGameWidth() - 64 - 25);

            tEnm = new MmgBmp[16];
            //Enemy Front
            tEnm[0] = spriteMatrix.GetFrame(0);
            tEnm[1] = spriteMatrix.GetFrame(1);
            tEnm[2] = spriteMatrix.GetFrame(2);
            tEnm[3] = spriteMatrix.GetFrame(1);

            //Enemy Left
            tEnm[4] = spriteMatrix.GetFrame(3);
            tEnm[5] = spriteMatrix.GetFrame(4);
            tEnm[6] = spriteMatrix.GetFrame(5);
            tEnm[7] = spriteMatrix.GetFrame(4);

            //Enemy Right
            tEnm[8] = spriteMatrix.GetFrame(6);
            tEnm[9] = spriteMatrix.GetFrame(7);
            tEnm[10] = spriteMatrix.GetFrame(8);
            tEnm[11] = spriteMatrix.GetFrame(7);

            //Enemy Back
            tEnm[12] = spriteMatrix.GetFrame(9);
            tEnm[13] = spriteMatrix.GetFrame(10);
            tEnm[14] = spriteMatrix.GetFrame(11);
            tEnm[15] = spriteMatrix.GetFrame(10);

            enemyWarlockFrames = new MmgSprite(tEnm, tmpPos);
            enemyWarlockFrames.SetMsPerFrame(tmp);

            //Load string exit config
            key = "strExitText";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Press B to Exit";
            }
            exit = MmgFontData.CreateDefaultBoldMmgFontLg();
            exit.SetText(file);
            exit.SetMmgColor(MmgColor.GetRed());
            exit.SetPosition((BOARD_WIDTH - exit.GetWidth()) / 2 + MmgHelper.ScaleValue(22), screenPos.GetY() + exit.GetHeight() + MmgHelper.ScaleValue(5));

            //C# Adjustment
            exit.SetY(exit.GetY() - MmgHelper.ScaleValue(5));
            exit = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("exitText", exit, classConfig, exit.GetPosition().Clone());

            //Load exit text background config
            key = "exitTextBgroundWidth";
            if (classConfig.ContainsKey(key))
            {
                tmpW = MmgHelper.ScaleValue((int)classConfig[key].number);
            }
            else
            {
                tmpW = exit.GetWidth() + (padding * 2);
            }

            key = "exitTextBgroundHeight";
            if (classConfig.ContainsKey(key))
            {
                tmpH = MmgHelper.ScaleValue((int)classConfig[key].number);
            }
            else
            {
                tmpH = exit.GetHeight() + (padding * 2);
            }

            exitBground = MmgHelper.CreateFilledBmp(tmpW, tmpH, MmgColor.GetBlack());
            exitBground.SetPosition(exit.GetX() - padding, exit.GetY() - exit.GetHeight());
            exitBground = (MmgBmp)MmgHelper.ContainsKeyMmgObjPosition("exitTextBground", exitBground, classConfig, exitBground.GetPosition().Clone());
            AddObj(exitBground);
            AddObj(exit);

            txtPlayer1 = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer1.SetText("Player1:");
            txtPlayer1.SetPosition(new MmgVector2(MmgHelper.ScaleValue(30), GAME_BOTTOM - MmgHelper.ScaleValue(8)));

            //C# Adjustment
            txtPlayer1.SetY(txtPlayer1.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer1);

            player1HealthBar = new MdtUiHealthBar(MdtPlayerType.PLAYER_1, this, MmgColor.GetRed());
            player1HealthBar.SetPosition(new MmgVector2(txtPlayer1.GetX() + txtPlayer1.GetWidth() + MmgHelper.ScaleValue(5), GAME_BOTTOM - player1HealthBar.GetHeight() - MmgHelper.ScaleValue(5)));
            AddObj(player1HealthBar);

            txtPlayer1Score = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer1Score.SetText("000000");
            txtPlayer1Score.SetPosition(new MmgVector2(player1HealthBar.GetX() + player1HealthBar.GetWidth() + MmgHelper.ScaleValue(5), GAME_BOTTOM - MmgHelper.ScaleValue(8)));
            AddObj(txtPlayer1Score);

            txtPlayer2 = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer2.SetText("Player2:");
            txtPlayer2.SetPosition(new MmgVector2(BOARD_RIGHT - MmgHelper.ScaleValue(250) - txtPlayer2.GetWidth(), GAME_BOTTOM - MmgHelper.ScaleValue(8)));

            //C# Adjustment
            txtPlayer2.SetY(txtPlayer2.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer2);

            player2HealthBar = new MdtUiHealthBar(MdtPlayerType.PLAYER_2, this, MmgColor.GetBlue());
            player2HealthBar.SetPosition(new MmgVector2(txtPlayer2.GetX() + txtPlayer2.GetWidth() + MmgHelper.ScaleValue(15), GAME_BOTTOM - player2HealthBar.GetHeight() - MmgHelper.ScaleValue(5)));
            AddObj(player2HealthBar);

            txtPlayer2Score = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer2Score.SetText("000000");
            txtPlayer2Score.SetPosition(new MmgVector2(player2HealthBar.GetX() + player2HealthBar.GetWidth() + MmgHelper.ScaleValue(5), GAME_BOTTOM - MmgHelper.ScaleValue(8)));
            AddObj(txtPlayer2Score);

            gameLogo = MmgHelper.GetBasicCachedBmp("mdt_game_title.png");
            gameLogo = MmgBmpScaler.ScaleMmgBmp(gameLogo, 0.28, true);
            gameLogo.SetPosition(new MmgVector2(GAME_RIGHT - MmgHelper.ScaleValue(115), GetY() + MmgHelper.ScaleValue(20)));
            AddObj(gameLogo);

            txtLevel = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtLevel.SetText("Level: 00");
            txtLevel.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), gameLogo.GetY() + gameLogo.GetHeight() + MmgHelper.ScaleValue(20)));

            //C# Adjustment
            txtLevel.SetY(txtLevel.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtLevel);

            txtLevelTime = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtLevelTime.SetText("Time: 000");
            txtLevelTime.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtLevel.GetY() + txtLevel.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            txtLevelTime.SetY(txtLevelTime.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtLevelTime);

            txtPlayer1Section = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer1Section.SetText("- Player1 -");
            txtPlayer1Section.SetPosition(new MmgVector2(GAME_RIGHT - txtPlayer1Section.GetWidth() - MmgHelper.ScaleValue(16), txtLevelTime.GetY() + txtLevelTime.GetHeight() + MmgHelper.ScaleValue(20)));

            //C# Adjustment
            txtPlayer1Section.SetY(txtPlayer1Section.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer1Section);

            txtPlayer1Weapon = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer1Weapon.SetText("W:");
            txtPlayer1Weapon.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtPlayer1Section.GetY() + txtPlayer1Section.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            //txtPlayer1Weapon.SetY(txtPlayer1Weapon.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer1Weapon);

            player1WeaponBmp.SetPosition(txtPlayer1Weapon.GetPosition().Clone());
            player1WeaponBmp.SetPosition(player1WeaponBmp.GetPosition().GetX() + txtPlayer1Weapon.GetWidth() + MmgHelper.ScaleValue(5), player1WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));

            txtPlayer1Mod = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer1Mod.SetText("M:");
            txtPlayer1Mod.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtPlayer1Weapon.GetY() + txtPlayer1Weapon.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            //txtPlayer1Mod.SetY(txtPlayer1Mod.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer1Mod);

            txtPlayer1ModTime = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer1ModTime.SetText("MT:");
            txtPlayer1ModTime.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtPlayer1Mod.GetY() + txtPlayer1Mod.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            txtPlayer1ModTime.SetY(txtPlayer1ModTime.GetY() + MmgHelper.ScaleValue(3));
            AddObj(txtPlayer1ModTime);

            txtPlayer2Section = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer2Section.SetText("- Player2 -");
            txtPlayer2Section.SetPosition(new MmgVector2(GAME_RIGHT - txtPlayer2Section.GetWidth() - MmgHelper.ScaleValue(16), txtPlayer1ModTime.GetY() + txtPlayer1ModTime.GetHeight() + MmgHelper.ScaleValue(20)));

            //C# Adjustment
            txtPlayer2Section.SetY(txtPlayer2Section.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer2Section);

            txtPlayer2Weapon = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer2Weapon.SetText("W:");
            txtPlayer2Weapon.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtPlayer2Section.GetY() + txtPlayer2Section.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            txtPlayer2Weapon.SetY(txtPlayer2Weapon.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer2Weapon);

            player2WeaponBmp.SetPosition(txtPlayer2Weapon.GetPosition().Clone());
            player2WeaponBmp.SetPosition(player2WeaponBmp.GetPosition().GetX() + txtPlayer2Weapon.GetWidth() + MmgHelper.ScaleValue(5), player2WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));

            txtPlayer2Mod = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer2Mod.SetText("M:");
            txtPlayer2Mod.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtPlayer2Weapon.GetY() + txtPlayer2Weapon.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            txtPlayer2Mod.SetY(txtPlayer2Mod.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer2Mod);

            txtPlayer2ModTime = MmgFontData.CreateDefaultBoldMmgFontSm();
            txtPlayer2ModTime.SetText("MT:");
            txtPlayer2ModTime.SetPosition(new MmgVector2(BOARD_RIGHT + MmgHelper.ScaleValue(20), txtPlayer2Mod.GetY() + txtPlayer2Mod.GetHeight() + MmgHelper.ScaleValue(10)));

            //C# Adjustment
            txtPlayer2ModTime.SetY(txtPlayer2ModTime.GetY() - MmgHelper.ScaleValue(3));
            AddObj(txtPlayer2ModTime);

            //Load number one config
            key = "bmpNumberOne";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "num_1_lrg.png";
            }
            lval = MmgHelper.GetBasicCachedBmp(file);

            MmgBmp tlval = MmgHelper.GetBasicCachedBmp("popup_window_base.png");
            numberBground = new Mmg9Slice(MmgHelper.ScaleValue(16), tlval, lval.GetWidth() + MmgHelper.ScaleValue(12), lval.GetHeight() + MmgHelper.ScaleValue(12));
            MmgHelper.CenterHorAndVert(numberBground);
            AddObj(numberBground);

            number1 = lval;
            if (number1 != null)
            {
                MmgHelper.CenterHorAndVert(number1);
                pos = number1.GetPosition().Clone();
                number1 = MmgHelper.ContainsKeyMmgBmpScaleAndPosition("numberOne", number1, classConfig, pos);
                number1.SetIsVisible(false);
                AddObj(number1);
            }

            //Load number two config
            key = "bmpNumberTwo";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "num_2_lrg.png";
            }
            lval = MmgHelper.GetBasicCachedBmp(file);
            number2 = lval;
            if (number2 != null)
            {
                MmgHelper.CenterHorAndVert(number2);
                pos = number2.GetPosition().Clone();
                number2 = MmgHelper.ContainsKeyMmgBmpScaleAndPosition("numberTwo", number2, classConfig, pos);
                number2.SetIsVisible(false);
                AddObj(number2);
            }

            //Load number three config
            key = "bmpNumberThree";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "num_3_lrg.png";
            }
            lval = MmgHelper.GetBasicCachedBmp(file);
            number3 = lval;
            if (number3 != null)
            {
                MmgHelper.CenterHorAndVert(number3);
                pos = number3.GetPosition().Clone();
                number3 = MmgHelper.ContainsKeyMmgBmpScaleAndPosition("numberThree", number3, classConfig, pos);
                number3.SetIsVisible(false);
                AddObj(number3);
            }

            //Load string game win config
            key = "strGoalText";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Goal: Survive for as long as you can!";
            }
            txtGoal = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtGoal.SetText(file);
            txtGoal.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtGoal);
            txtGoal.SetY(number1.GetY() - txtGoal.GetHeight() + MmgHelper.ScaleValue(5));
            pos = txtGoal.GetPosition().Clone();
            txtGoal = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("goalText", txtGoal, classConfig, pos);
            txtGoal.SetIsVisible(false);
            AddObj(txtGoal);

            //Load string player 1 direction config
            key = "strPlayer1DirectionText";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Player 1: Red visor, D-PAD to move, '.' to attack, '/' to exit.";
            }
            txtDirecP1 = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtDirecP1.SetText(file);
            txtDirecP1.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtDirecP1);
            txtDirecP1.SetY(number1.GetY() + number1.GetHeight() + txtDirecP1.GetHeight() + MmgHelper.ScaleValue(10));
            pos = txtDirecP1.GetPosition().Clone();
            txtDirecP1 = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("player1DirectionText", txtDirecP1, classConfig, pos);
            txtDirecP1.SetIsVisible(false);
            AddObj(txtDirecP1);

            //Load string player 2 direction config
            key = "strPlayer2DirectionText";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Player 2: Green visor, 's', 'x', 'z', 'c' to move, 'f' to attack, 'v' to exit.";
            }
            txtDirecP2 = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtDirecP2.SetText(file);
            txtDirecP2.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtDirecP2);
            txtDirecP2.SetY(txtDirecP1.GetY() + txtDirecP1.GetHeight() + MmgHelper.ScaleValue(10));
            pos = txtDirecP2.GetPosition().Clone();
            txtDirecP2 = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("player2DirectionText", txtDirecP2, classConfig, pos);
            txtDirecP2.SetIsVisible(false);
            AddObj(txtDirecP2);

            //Load game over player 1 config
            key = "strTextGameOver1";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Player 1 has won the game! Press ('a', '.') or ('b', '/') to exit.";
            }
            txtGameOver1 = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtGameOver1.SetText(file);
            txtGameOver1.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtGameOver1);
            pos = txtGameOver1.GetPosition().Clone();
            txtGameOver1 = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("textGameOver1", txtGameOver1, classConfig, pos);
            txtGameOver1.SetIsVisible(false);
            AddObj(txtGameOver1);

            //Load game over player 2 config
            key = "strTextGameOver2";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Player 2 has won the game! Press ('a', 'f') or ('b', 'v') to exit.";
            }
            txtGameOver2 = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtGameOver2.SetText(file);
            txtGameOver2.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtGameOver2);
            pos = txtGameOver2.GetPosition().Clone();
            txtGameOver2 = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("textGameOver2", txtGameOver2, classConfig, pos);
            txtGameOver2.SetIsVisible(false);
            AddObj(txtGameOver2);

            //Load game over player 3 config
            key = "strTextGameOver3";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Game over! You ran out of time! Press ('a', 'f') or ('b', 'v') to exit.";
            }
            txtGameOver3 = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtGameOver3.SetText(file);
            txtGameOver3.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtGameOver3);
            pos = txtGameOver3.GetPosition().Clone();
            txtGameOver3 = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("textGameOver3", txtGameOver3, classConfig, pos);
            txtGameOver3.SetIsVisible(false);
            AddObj(txtGameOver3);

            //Load popup base
            key = "bmpPopupWindowBase";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "popup_window_base.png";
            }
            lval = MmgHelper.GetBasicCachedBmp(file);
            bgroundPopupSrc = lval;
            if (bgroundPopupSrc != null)
            {
                popupTotalWidth = MmgHelper.ScaleValue(300);
                popupTotalHeight = MmgHelper.ScaleValue(120);

                bgroundPopup = new Mmg9Slice(16, bgroundPopupSrc, popupTotalWidth, popupTotalHeight);
                bgroundPopup.SetPosition(MmgVector2.GetOriginVec());

                key = "popupWindowBaseWidth";
                if (classConfig.ContainsKey(key))
                {
                    tmpW = MmgHelper.ScaleValue((int)classConfig[key].number);
                }
                else
                {
                    tmpW = popupTotalWidth;
                }
                bgroundPopup.SetWidth(tmpW);

                key = "popupWindowBaseHeight";
                if (classConfig.ContainsKey(key))
                {
                    tmpH = MmgHelper.ScaleValue((int)classConfig[key].number);
                }
                else
                {
                    tmpH = popupTotalHeight;
                }
                bgroundPopup.SetHeight(tmpH);

                MmgHelper.CenterHorAndVert(bgroundPopup);
                pos = bgroundPopup.GetPosition().Clone();
                bgroundPopup = (Mmg9Slice)MmgHelper.ContainsKeyMmgObjPosition("popupWindowBase", bgroundPopup, classConfig, pos);

                AddObj(bgroundPopup);
                bgroundPopup.SetIsVisible(false);
            }

            //Load popup window text exit
            key = "strPopupWindowTextExit";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Exit Game ('A', 'F', '.')";
            }
            txtOk = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtOk.SetText(file);
            txtOk.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtOk);
            txtOk.SetY(bgroundPopup.GetY() + txtOk.GetHeight() + MmgHelper.ScaleValue(20));
            pos = txtOk.GetPosition().Clone();
            txtOk = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("popupWindowTextExit", txtOk, classConfig, pos);
            txtOk.SetIsVisible(false);
            AddObj(txtOk);

            //Load popup window text cancel
            key = "strPopupWindowTextCancel";
            if (classConfig.ContainsKey(key))
            {
                file = classConfig[key].str;
            }
            else
            {
                file = "Cancel Exit ('B', 'V', '/')";
            }
            txtCancel = MmgFontData.CreateDefaultBoldMmgFontLg();
            txtCancel.SetText(file);
            txtCancel.SetMmgColor(MmgColor.GetWhite());
            MmgHelper.CenterHorAndVert(txtCancel);
            txtCancel.SetY(txtOk.GetY() + txtOk.GetHeight() + MmgHelper.ScaleValue(20));
            pos = txtCancel.GetPosition().Clone();
            txtCancel = (MmgFont)MmgHelper.ContainsKeyMmgObjPosition("popupWindowTextCancel", txtCancel, classConfig, pos);
            txtCancel.SetIsVisible(false);
            AddObj(txtCancel);

            MdtWeaponType[] wps = null;
            MdtItemType[] itms = null;
            MdtDoorType[] drs = null;
            int wLen = WAVE_COUNT;
            int cnt = 0;
            waves = new MdtEnemyWave[WAVE_COUNT];

            MmgHelper.wr("Configure enemy waves (" + wLen + ")...");
            for (int i = 0; i < wLen; i++)
            {
                if (i == 0)
                {
                    wps = new MdtWeaponType[] { MdtWeaponType.SWORD, MdtWeaponType.SPEAR };
                    itms = new MdtItemType[] { MdtItemType.COIN_BAG, MdtItemType.POTION_GREEN, MdtItemType.POTION_YELLOW, MdtItemType.POTION_RED };
                    drs = new MdtDoorType[] { MdtDoorType.TOP_LEFT, MdtDoorType.RIGHT };
                }
                else if (i >= 3 && i < 6)
                {
                    wps = new MdtWeaponType[] { MdtWeaponType.SWORD, MdtWeaponType.SPEAR, MdtWeaponType.AXE };
                    itms = new MdtItemType[] { MdtItemType.COIN_BAG, MdtItemType.POTION_GREEN, MdtItemType.POTION_YELLOW, MdtItemType.CHEST };
                    drs = new MdtDoorType[] { MdtDoorType.TOP_LEFT, MdtDoorType.RIGHT, MdtDoorType.LEFT };
                }
                else if (i >= 6 && i < 9)
                {
                    wps = new MdtWeaponType[] { MdtWeaponType.SWORD, MdtWeaponType.SPEAR, MdtWeaponType.AXE };
                    itms = new MdtItemType[] { MdtItemType.COIN_BAG, MdtItemType.POTION_GREEN, MdtItemType.POTION_YELLOW, MdtItemType.CHEST };
                    drs = new MdtDoorType[] { MdtDoorType.TOP_LEFT, MdtDoorType.TOP_RIGHT, MdtDoorType.RIGHT, MdtDoorType.LEFT };
                }
                else if (i >= 9 && i < 12)
                {
                    wps = new MdtWeaponType[] { MdtWeaponType.SWORD, MdtWeaponType.SPEAR, MdtWeaponType.AXE, MdtWeaponType.WAND };
                    itms = new MdtItemType[] { MdtItemType.COIN_BAG, MdtItemType.POTION_GREEN, MdtItemType.POTION_YELLOW, MdtItemType.CHEST, MdtItemType.BOMB };
                    drs = new MdtDoorType[] { MdtDoorType.TOP_LEFT, MdtDoorType.TOP_RIGHT, MdtDoorType.RIGHT, MdtDoorType.LEFT, MdtDoorType.BOTTOM_LEFT, MdtDoorType.BOTTOM_RIGHT };
                }
                cnt = ((i + 1) * 10);
                waves[i] = new MdtEnemyWave(i, (((i + 2) * 30) * 1000), 0, 0, ((((i + 1) * 30) * 1000) / cnt), ((int)(cnt * 0.10) + 1), ((int)(cnt * 0.20) + 1), cnt, wps, itms, drs, ((i % 3) + 3), ((i % 3) + 6), ((i % 3) + 3), ((i % 3) + 6));
                MmgHelper.wr("\n" + waves[i]);
            }
            wavesCurrentIdx = 0;

            sound1 = MmgHelper.GetBasicCachedSound("jump1.wav");
            SetState(State.SHOW_COUNT_DOWN);
            ready = true;
            pause = false;
        }

        /// <summary>
        /// Sets the current game type.
        /// </summary>
        /// <param name="gt">The specified game type.</param>
        public void SetGameType(GameType gt)
        {
            gameType = gt;
        }

        /// <summary>
        /// Gets the current game type.
        /// </summary>
        /// <returns>The specified game type.</returns>
        public GameType GetGameType()
        {
            return gameType;
        }

        /// <summary>
        /// Converts the given speed to a uniform speed per frame so that the game movement will
        /// be the same even if the game runs at different frame rates.
        /// </summary>
        /// <param name="speed">The target speed to convert to a speed per frame.</param>
        /// <returns>A converted speed that represents the speed per frame of the given input speed.</returns>
        public static int GetSpeedPerFrame(int speed)
        {
            return (int)(speed / (DungeonTrap.FPS - 4));
        }

        /// <summary>
        /// Clears objects from the screens object collection and the items, objects, and enemies containers.
        /// </summary>
        private void UpdateClearObjects()
        {
            MmgContainer c = GetObjects();
            int len = c.GetCount();
            MmgObj obj = null;
            for (int i = 0; i < len; i++)
            {
                obj = c.GetChildAt(i);
                if (obj is MdtItemChest || obj is MdtCharInterWarlock || obj is MdtCharInterBanshee || obj is MdtCharInterDemon || obj is MdtWeapon || obj is MdtObjPushTableSmall || obj is MdtObjPushTableLarge || obj is MdtObjPushBarrel || obj is MdtItemBomb || obj is MdtItemCoinBag || obj is MdtItemPotionGreen || obj is MdtItemPotionRed || obj is MdtItemPotionYellow)
                {
                    c.RemoveAt(i);
                    len--;
                    i--;
                }
            }
            gameObjects.Clear();
            gameItems.Clear();
            gameEnemies.Clear();
        }

        /*
        private void PrintObjects()
        {
            MmgContainer c = GetObjects();
            int len = c.GetCount();
            MmgObj obj = null;
            for (int i = 0; i < len; i++)
            {
                obj = c.GetChildAt(i);
                MmgHelper.wr("UpdateClearObjects: Found: " + obj.GetType().Name + ", " + obj.GetType().Namespace);
            }
        }
        */

        /// <summary>
        /// Unloads resources needed to display this game screen.
        /// </summary>
        public override void UnloadResources()
        {
            pause = true;

            SetBackground(null);
            state = State.NONE;
            statePrev = State.NONE;
            UpdateClearObjects();

            bground = null;
            doorLockFull = null;
            doorOpenFull = null;
            doorLockIcon = null;
            doorTopLeftLocked = null;
            doorTopLeftOpened = null;
            doorTopRightLocked = null;
            doorTopRightOpened = null;
            doorLeftLockIcon = null;
            doorRightLockIcon = null;
            doorBotLeftLockIcon = null;
            doorBotRightLockIcon = null;
            torch1 = null;
            torch2 = null;
            torch3 = null;
            torch4 = null;
            randoLeft = null;
            randoRight = null;
            gameItems = null;
            gameObjects = null;
            gameEnemies = null;
            player1 = null;
            player1WeaponBmp = null;
            player2 = null;
            player2WeaponBmp = null;
            spriteMatrixSrc = null;
            enemyDemonFrames = null;
            spriteMatrix = null;
            enemyBansheeFrames = null;
            enemyWarlockFrames = null;
            exit = null;
            classConfig = null;
            exitBground = null;
            txtPlayer1 = null;
            txtPlayer1Score = null;
            txtPlayer2 = null;
            txtPlayer2Score = null;
            gameLogo = null;
            txtLevel = null;
            txtLevelTime = null;
            txtPlayer1Section = null;
            txtPlayer1Weapon = null;
            txtPlayer1Mod = null;
            txtPlayer1ModTime = null;
            txtPlayer2Section = null;
            txtPlayer2Weapon = null;
            txtPlayer2Mod = null;
            txtPlayer2ModTime = null;
            numberBground = null;
            number1 = null;
            number2 = null;
            number3 = null;
            txtGoal = null;
            pos = null;
            txtDirecP1 = null;
            txtDirecP2 = null;
            txtGameOver1 = null;
            txtGameOver2 = null;
            txtGameOver3 = null;
            bgroundPopupSrc = null;
            txtOk = null;
            txtCancel = null;
            waves = null;
            wavesCurrent = null;

            ClearObjs();
            base.UnloadResources();

            ready = false;
        }

        /// <summary>
        /// The MmgUpdate method used to call the update method of the child objects.
        /// </summary>
        /// <param name="updateTick">The update tick number.</param>
        /// <param name="currentTimeMs">The current time in the game in milliseconds.</param>
        /// <param name="msSinceLastFrame">The number of milliseconds between the last frame and this frame.</param>
        /// <returns>A bool indicating if any work was done this game frame.</returns>
        public override bool MmgUpdate(int updateTick, long currentTimeMs, long msSinceLastFrame)
        {
            lret = base.MmgUpdate(updateTick, currentTimeMs, msSinceLastFrame);
            if (pause == false && isVisible == true)
            {
                if (state == State.SHOW_GAME)
                {
                    if (wavesCurrent != null)
                    {
                        UpdateCurrentEnemyWave(msSinceLastFrame);
                    }

                    if (player1 != null)
                    {
                        if (player1.isPushStart)
                        {
                            player1.pushingCurrentMs += msSinceLastFrame;
                            if (player1.pushingCurrentMs >= player1.pushingLengthMs)
                            {
                                player1.isPushStart = false;
                                player1.isPushing = true;
                            }
                        }
                    }

                    if (player2 != null)
                    {
                        if (player2.isPushStart)
                        {
                            player2.pushingCurrentMs += msSinceLastFrame;
                            if (player2.pushingCurrentMs >= player2.pushingLengthMs)
                            {
                                player2.isPushStart = false;
                                player2.isPushing = true;
                            }
                        }
                    }

                    if (playersAliveCount <= 0)
                    {
                        SetState(State.SHOW_GAME_OVER);
                    }
                }
                lret = true;
            }
            return lret;
        }

        /// <summary>
        /// The main drawing routine.
        /// </summary>
        /// <param name="p">An MmgPen object to use for drawing this game screen.</param>
        public override void MmgDraw(MmgPen p)
        {
            if (pause == false && isVisible == true)
            {
                base.GetObjects().MmgDraw(p);
            }
        }

        /// <summary>
        /// A method to handle A button click events from the MainFrame class.
        /// </summary>
        /// <param name="src">The player source of the input.</param>
        /// <returns>A bool indicating if this Screen has handled the A click event.</returns>
        public override bool ProcessAClick(int src)
        {
            if (pause || !isVisible)
            {
                return false;
            }

            if (state == State.SHOW_GAME)
            {
                if (gameType == GameType.GAME_ONE_PLAYER || gameType == GameType.GAME_TWO_PLAYER)
                {
                    if (src == ScreenGame.SRC_PLAYER_1)
                    {
                        if (!player1.isAttacking)
                        {
                            if (player1.weaponCurrent.attackType == MdtWeaponAttackType.THROWING && player1.weaponCurrent.weaponType == MdtWeaponType.AXE)
                            {
                                player1.weaponCurrent = player1.weaponCurrent.Clone();
                                player1.weaponCurrent.SetPosition(GetX() + GetWidth() / 2, GetY() + GetHeight() / 2);
                                player1.weaponCurrent.current = null;
                                player1.weaponCurrent.throwingPath = MdtWeaponPathType.NONE;
                                player1.weaponCurrent.screen = this;
                                AddObj(player1.weaponCurrent);
                            }

                            player1.weaponCurrent.animTimeMsCurrent = 0;
                            player1.weaponCurrent.animPrctComplete = 0.0d;
                            player1.isAttacking = true;
                            player1.weaponCurrent.active = true;
                        }
                    }
                    else if (src == ScreenGame.SRC_PLAYER_2)
                    {
                        if (!player2.isAttacking)
                        {
                            if (player2.weaponCurrent.attackType == MdtWeaponAttackType.THROWING && player2.weaponCurrent.weaponType == MdtWeaponType.AXE)
                            {
                                player2.weaponCurrent = player2.weaponCurrent.Clone();
                                player2.weaponCurrent.SetPosition(GetX() + GetWidth() / 2, GetY() + GetHeight() / 2);
                                player2.weaponCurrent.current = null;
                                player2.weaponCurrent.throwingPath = MdtWeaponPathType.NONE;
                                player2.weaponCurrent.screen = this;
                                AddObj(player2.weaponCurrent);
                            }

                            player2.weaponCurrent.animTimeMsCurrent = 0;
                            player2.weaponCurrent.animPrctComplete = 0.0d;
                            player2.isAttacking = true;
                            player2.weaponCurrent.active = true;
                        }
                    }
                }
                return true;
            }
            else if (state == State.SHOW_GAME_EXIT)
            {
                owner.SwitchGameState(GameStates.MAIN_MENU);
                return true;
            }
            else if (state == State.SHOW_GAME_OVER)
            {
                owner.SwitchGameState(GameStates.MAIN_MENU);
                return true;
            }
            return false;
        }

        /// <summary>
        /// A method to handle B button click events from the MainFrame class.
        /// </summary>
        /// <param name="src">The player source of the input.</param>
        /// <returns>A bool indicating if this Screen has handled the B click event.</returns>
        public override bool ProcessBClick(int src)
        {
            if (pause || !isVisible)
            {
                return false;
            }

            if (state == State.SHOW_GAME_OVER)
            {
                owner.SwitchGameState(GameStates.MAIN_MENU);
                return true;
            }
            else
            {
                if (state != State.SHOW_GAME_EXIT)
                {
                    SetState(State.SHOW_GAME_EXIT);
                    return true;
                }
                else
                {
                    SetState(statePrev);
                    return true;
                }
            }
        }

        /// <summary>
        /// A method to handle debug click events from the MainFrame class, the D key on the keyboard.
        /// You can use this method to turn on different debugging helpers.
        /// </summary>
        public override void ProcessDebugClick()
        {
            randomWaves = !randomWaves;
            MmgHelper.wr("RandomWaves: " + randomWaves);
        }

        /// <summary>
        /// A method to handle key press events from the MainFrame class.
        /// </summary>
        /// <param name="c">The character of the key that was pressed on the keyboard.</param>
        /// <param name="code">An alternate key code.</param>
        /// <returns>A bool indicating if the key press event was handled by this Screen.</returns>
        public override bool ProcessKeyPress(char c, int code)
        {
            if (pause || !isVisible)
            {
                return false;
            }

            if (state == State.SHOW_GAME)
            {
                if (gameType == GameType.GAME_TWO_PLAYER)
                {
                    bool found = false;

                    if (c == 'x' || c == 'X')
                    {
                        //down
                        if (player2.GetDir() != MmgDir.DIR_FRONT)
                        {
                            player2.SetDir(MmgDir.DIR_FRONT);
                        }
                        found = true;
                    }
                    else if (c == 's' || c == 'S')
                    {
                        //up
                        if (player2.GetDir() != MmgDir.DIR_BACK)
                        {
                            player2.SetDir(MmgDir.DIR_BACK);
                        }
                        found = true;
                    }
                    else if (c == 'z' || c == 'Z')
                    {
                        //left
                        if (player2.GetDir() != MmgDir.DIR_LEFT)
                        {
                            player2.SetDir(MmgDir.DIR_LEFT);
                        }
                        found = true;
                    }
                    else if (c == 'c' || c == 'C')
                    {
                        //right
                        if (player2.GetDir() != MmgDir.DIR_RIGHT)
                        {
                            player2.SetDir(MmgDir.DIR_RIGHT);
                        }
                        found = true;
                    }

                    if (found)
                    {
                        player2.isMoving = true;
                        player2.subj.SetMsPerFrame(frameMsPerFrameMoving);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// A method to handle key release events from the MainFrame class.
        /// </summary>
        /// <param name="c">The character of the key that was released on the keyboard.</param>
        /// <param name="code">An alternate key code.</param>
        /// <returns>A bool indicating if the key release event was handled by this Screen.</returns>
        public override bool ProcessKeyRelease(char c, int code)
        {
            if (pause || !isVisible)
            {
                return false;
            }

            if (state == State.SHOW_GAME_EXIT)
            {
                if (gameType == GameType.GAME_TWO_PLAYER)
                {
                    if (c == 'v' || c == 'V')
                    {
                        ProcessBClick(SRC_PLAYER_2);
                    }
                    else if (c == 'f' || c == 'F')
                    {
                        ProcessAClick(SRC_PLAYER_2);
                    }
                }

                if (c == '/' || c == '?')
                {
                    ProcessBClick(SRC_PLAYER_1);
                }
                else if (c == '.' || c == '>')
                {
                    ProcessAClick(SRC_PLAYER_1);
                }
            }
            else if (state == State.SHOW_GAME)
            {
                if (gameType == GameType.GAME_TWO_PLAYER)
                {
                    bool found = true;
                    if (c == 'x' || c == 'X')
                    {
                        //down
                        found = true;
                    }
                    else if (c == 's' || c == 'S')
                    {
                        //up
                        found = true;
                    }
                    else if (c == 'z' || c == 'Z')
                    {
                        //left
                        found = true;
                    }
                    else if (c == 'c' || c == 'C')
                    {
                        //right
                        found = true;
                    }
                    else if (c == 'f' || c == 'F')
                    {
                        ProcessAClick(SRC_PLAYER_2);
                    }
                    else if (c == 'v' || c == 'V')
                    {
                        ProcessBClick(SRC_PLAYER_2);
                    }

                    if (found)
                    {
                        if (playerSnapToFront == true)
                        {
                            player2.SetDir(MmgDir.DIR_FRONT);
                        }

                        player2.isMoving = false;
                        player2.isPushStart = false;
                        player2.isPushing = false;
                        player2.subj.SetMsPerFrame(frameMsPerFrameNotMoving);
                    }
                }

                if (c == '.' || c == '>')
                {
                    ProcessAClick(SRC_PLAYER_1);
                }
                else if (c == '/' || c == '?')
                {
                    ProcessBClick(SRC_PLAYER_1);
                }
            }
            return false;
        }

        /// <summary>
        /// A method to handle dpad press events from the MainFrame class.
        /// </summary>
        /// <param name="dir">The dpad code, UP, DOWN, LEFT, RIGHT of the direction that was pressed on the keyboard.</param>
        /// <returns>A bool indicating if the dpad press was handled by this Screen.</returns>
        public override bool ProcessDpadPress(int dir)
        {
            if (pause || !isVisible)
            {
                return false;
            }

            if (state == State.SHOW_GAME)
            {
                if (gameType == GameType.GAME_ONE_PLAYER || gameType == GameType.GAME_TWO_PLAYER)
                {
                    bool found = false;

                    if (dir == GameSettings.DOWN_KEYBOARD)
                    {
                        if (player1.GetDir() != MmgDir.DIR_FRONT)
                        {
                            player1.SetDir(MmgDir.DIR_FRONT);
                        }
                        found = true;

                    }
                    else if (dir == GameSettings.UP_KEYBOARD)
                    {
                        if (player1.GetDir() != MmgDir.DIR_BACK)
                        {
                            player1.SetDir(MmgDir.DIR_BACK);
                        }
                        found = true;

                    }
                    else if (dir == GameSettings.LEFT_KEYBOARD)
                    {
                        if (player1.GetDir() != MmgDir.DIR_LEFT)
                        {
                            player1.SetDir(MmgDir.DIR_LEFT);
                        }
                        found = true;

                    }
                    else if (dir == GameSettings.RIGHT_KEYBOARD)
                    {
                        if (player1.GetDir() != MmgDir.DIR_RIGHT)
                        {
                            player1.SetDir(MmgDir.DIR_RIGHT);
                        }
                        found = true;
                    }

                    if (found)
                    {
                        player1.isMoving = true;
                        player1.subj.SetMsPerFrame(frameMsPerFrameMoving);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// A method to handle dpad release events from the MainFrame class.
        /// </summary>
        /// <param name="dir">The dpad code, UP, DOWN, LEFT, RIGHT of the direction that was released on the keyboard.</param>
        /// <returns>A bool indicating if the dpad release was handled by this Screen.</returns>
        public override bool ProcessDpadRelease(int dir)
        {
            if (pause || !isVisible)
            {
                return false;
            }

            if (state == State.SHOW_GAME)
            {
                if (gameType == GameType.GAME_ONE_PLAYER || gameType == GameType.GAME_TWO_PLAYER)
                {
                    if (playerSnapToFront == true)
                    {
                        player1.SetDir(MmgDir.DIR_FRONT);
                    }

                    player1.isMoving = false;
                    player1.isPushStart = false;
                    player1.isPushing = false;
                    player1.subj.SetMsPerFrame(frameMsPerFrameNotMoving);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the Screen's current state. The state is used to prepare what MmgObj's are visible for the given state.
        /// </summary>
        /// <param name="inS">The desired State to set the Screen to.</param>
        private void SetState(State inS)
        {
            switch (statePrev)
            {
                case State.NONE:
                    break;

                case State.SHOW_GAME:
                    break;

                case State.SHOW_COUNT_DOWN:
                    break;

                case State.SHOW_COUNT_DOWN_IN_GAME:
                    break;

                case State.SHOW_GAME_OVER:
                    break;

                case State.SHOW_GAME_EXIT:
                    break;
            }

            statePrev = state;
            state = inS;

            switch (state)
            {
                case State.NONE:
                    UpdateHideAllDoors();

                    numberBground.SetIsVisible(false);

                    player1.SetIsVisible(false);
                    player2.SetIsVisible(false);

                    torch1.SetIsVisible(false);
                    torch2.SetIsVisible(false);
                    torch3.SetIsVisible(false);
                    torch4.SetIsVisible(false);

                    txtLevel.SetIsVisible(false);
                    txtLevelTime.SetIsVisible(false);
                    player1HealthBar.SetIsVisible(false);
                    player2HealthBar.SetIsVisible(false);

                    exit.SetIsVisible(false);
                    exitBground.SetIsVisible(false);
                    gameLogo.SetIsVisible(false);

                    bground.SetIsVisible(false);
                    number1.SetIsVisible(false);
                    number2.SetIsVisible(false);
                    number3.SetIsVisible(false);
                    txtGoal.SetIsVisible(false);
                    txtDirecP1.SetIsVisible(false);
                    txtDirecP2.SetIsVisible(false);

                    txtPlayer1.SetIsVisible(false);
                    txtPlayer1Mod.SetIsVisible(false);
                    txtPlayer1ModTime.SetIsVisible(false);
                    txtPlayer1Score.SetIsVisible(false);
                    txtPlayer1Section.SetIsVisible(false);
                    txtPlayer1Weapon.SetIsVisible(false);

                    txtPlayer2.SetIsVisible(false);
                    txtPlayer2Mod.SetIsVisible(false);
                    txtPlayer2ModTime.SetIsVisible(false);
                    txtPlayer2Score.SetIsVisible(false);
                    txtPlayer2Section.SetIsVisible(false);
                    txtPlayer2Weapon.SetIsVisible(false);

                    txtGameOver1.SetIsVisible(false);
                    txtGameOver2.SetIsVisible(false);
                    txtGameOver3.SetIsVisible(false);

                    bgroundPopup.SetIsVisible(false);
                    txtOk.SetIsVisible(false);
                    txtCancel.SetIsVisible(false);

                    pause = false;
                    isDirty = false;
                    break;
                case State.SHOW_GAME_OVER:
                    UpdateHideAllDoors();
                    numberBground.SetIsVisible(false);
                    player1.SetIsVisible(false);
                    player2.SetIsVisible(false);

                    torch1.SetIsVisible(false);
                    torch2.SetIsVisible(false);
                    torch3.SetIsVisible(false);
                    torch4.SetIsVisible(false);

                    txtLevel.SetIsVisible(false);
                    txtLevelTime.SetIsVisible(false);
                    player1HealthBar.SetIsVisible(false);
                    player2HealthBar.SetIsVisible(false);

                    exit.SetIsVisible(false);
                    exitBground.SetIsVisible(false);
                    gameLogo.SetIsVisible(false);

                    bground.SetIsVisible(false);
                    number1.SetIsVisible(false);
                    number2.SetIsVisible(false);
                    number3.SetIsVisible(false);
                    txtGoal.SetIsVisible(false);
                    txtDirecP1.SetIsVisible(false);
                    txtDirecP2.SetIsVisible(false);

                    txtPlayer1.SetIsVisible(false);
                    txtPlayer1Mod.SetIsVisible(false);
                    txtPlayer1ModTime.SetIsVisible(false);
                    txtPlayer1Score.SetIsVisible(false);
                    txtPlayer1Section.SetIsVisible(false);
                    txtPlayer1Weapon.SetIsVisible(false);

                    if (player1WeaponBmp != null)
                    {
                        player1WeaponBmp.SetIsVisible(false);
                    }

                    if (player1ModBmp != null)
                    {
                        player1ModBmp.SetIsVisible(false);
                    }

                    txtPlayer2.SetIsVisible(false);
                    txtPlayer2Mod.SetIsVisible(false);
                    txtPlayer2ModTime.SetIsVisible(false);
                    txtPlayer2Score.SetIsVisible(false);
                    txtPlayer2Section.SetIsVisible(false);
                    txtPlayer2Weapon.SetIsVisible(false);

                    if (player2WeaponBmp != null)
                    {
                        player2WeaponBmp.SetIsVisible(false);
                    }

                    if (player2ModBmp != null)
                    {
                        player2ModBmp.SetIsVisible(false);
                    }

                    bgroundPopup.SetIsVisible(false);
                    txtOk.SetIsVisible(false);
                    txtCancel.SetIsVisible(false);

                    UpdateClearObjects();

                    txtGameOver1.SetIsVisible(false);
                    txtGameOver2.SetIsVisible(false);
                    txtGameOver3.SetIsVisible(false);

                    if (scoreTimeUp)
                    {
                        txtGameOver3.SetIsVisible(true);
                    }
                    else
                    {
                        if (gameType == GameType.GAME_TWO_PLAYER)
                        {
                            if (scorePlayerOne >= scorePlayerTwo)
                            {
                                txtGameOver1.SetIsVisible(true);
                            }
                            else
                            {
                                txtGameOver2.SetIsVisible(true);
                            }
                        }
                        else
                        {
                            txtGameOver1.SetIsVisible(true);
                        }
                    }

                    numberState = NumberState.NONE;
                    pause = false;
                    isDirty = true;
                    break;
                case State.SHOW_GAME:
                    numberBground.SetIsVisible(false);

                    torch1.SetIsVisible(true);
                    torch2.SetIsVisible(true);
                    torch3.SetIsVisible(true);
                    torch4.SetIsVisible(true);

                    txtLevel.SetIsVisible(true);
                    txtLevelTime.SetIsVisible(true);
                    exit.SetIsVisible(true);
                    exitBground.SetIsVisible(true);
                    gameLogo.SetIsVisible(true);

                    if (statePrev != State.SHOW_GAME_EXIT)
                    {
                        timeNumberMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

                    bground.SetIsVisible(true);
                    number1.SetIsVisible(false);
                    number2.SetIsVisible(false);
                    number3.SetIsVisible(false);
                    txtGoal.SetIsVisible(false);
                    txtDirecP1.SetIsVisible(false);
                    txtDirecP2.SetIsVisible(false);

                    if (gameType == GameType.GAME_ONE_PLAYER || gameType == GameType.GAME_TWO_PLAYER)
                    {
                        UpdatePlayerWeapon(player1.playerType, player1.weaponCurrent.weaponType);
                        txtPlayer1.SetIsVisible(true);
                        txtPlayer1Mod.SetIsVisible(true);
                        txtPlayer1ModTime.SetIsVisible(true);
                        txtPlayer1Score.SetIsVisible(true);
                        txtPlayer1Section.SetIsVisible(true);
                        txtPlayer1Weapon.SetIsVisible(true);

                        player1HealthBar.SetIsVisible(true);
                        player1.SetIsVisible(true);
                        player1.isMoving = false;
                        player1.isAttacking = false;
                        player1.isPushStart = false;
                        player1.isPushing = false;
                    }

                    if (gameType == GameType.GAME_TWO_PLAYER)
                    {
                        UpdatePlayerWeapon(player2.playerType, player2.weaponCurrent.weaponType);
                        txtPlayer2.SetIsVisible(true);
                        txtPlayer2Mod.SetIsVisible(true);
                        txtPlayer2ModTime.SetIsVisible(true);
                        txtPlayer2Score.SetIsVisible(true);
                        txtPlayer2Section.SetIsVisible(true);
                        txtPlayer2Weapon.SetIsVisible(true);

                        player2HealthBar.SetIsVisible(true);
                        player2.SetIsVisible(true);
                        player2.isMoving = false;
                        player2.isAttacking = false;
                        player2.isPushStart = false;
                        player2.isPushing = false;
                    }

                    bgroundPopup.SetIsVisible(false);
                    txtOk.SetIsVisible(false);
                    txtCancel.SetIsVisible(false);
                    txtGameOver1.SetIsVisible(false);
                    txtGameOver2.SetIsVisible(false);
                    txtGameOver3.SetIsVisible(false);

                    if (statePrev != State.SHOW_GAME_EXIT)
                    {
                        UpdateStartEnemyWave(wavesCurrentIdx);
                        UpdateResetPlayers();
                    }
                    else
                    {
                        wavesCurrent.timeStartMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

                    pause = false;
                    isDirty = true;
                    break;
                case State.SHOW_COUNT_DOWN_IN_GAME:
                    UpdateClearObjects();
                    numberBground.SetIsVisible(true);

                    player1.SetIsVisible(false);
                    player2.SetIsVisible(false);

                    torch1.SetIsVisible(true);
                    torch2.SetIsVisible(true);
                    torch3.SetIsVisible(true);
                    torch4.SetIsVisible(true);

                    txtLevel.SetIsVisible(true);
                    txtLevelTime.SetIsVisible(true);
                    player1HealthBar.SetIsVisible(false);
                    player2HealthBar.SetIsVisible(false);

                    txtPlayer1Score.SetIsVisible(false);
                    txtPlayer2Score.SetIsVisible(false);

                    exit.SetIsVisible(true);
                    exitBground.SetIsVisible(true);
                    gameLogo.SetIsVisible(true);
                    bground.SetIsVisible(true);

                    if (statePrev != State.SHOW_GAME_EXIT)
                    {
                        number1.SetIsVisible(false);
                        number2.SetIsVisible(false);
                        number3.SetIsVisible(false);
                    }

                    txtGoal.SetIsVisible(false);
                    txtDirecP1.SetIsVisible(false);
                    txtDirecP2.SetIsVisible(false);

                    bgroundPopup.SetIsVisible(false);
                    txtOk.SetIsVisible(false);
                    txtCancel.SetIsVisible(false);
                    txtGameOver1.SetIsVisible(false);
                    txtGameOver2.SetIsVisible(false);
                    txtGameOver3.SetIsVisible(false);

                    txtPlayer1.SetIsVisible(false);
                    txtPlayer2.SetIsVisible(false);

                    if (statePrev != State.SHOW_GAME_EXIT)
                    {
                        numberState = NumberState.NONE;
                    }
                    else
                    {
                        timeNumberMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

                    pause = false;
                    isDirty = true;
                    break;
                case State.SHOW_COUNT_DOWN:
                    UpdateClearObjects();
                    numberBground.SetIsVisible(false);

                    UpdateHideAllDoors();

                    player1.SetIsVisible(false);
                    player2.SetIsVisible(false);

                    torch1.SetIsVisible(false);
                    torch2.SetIsVisible(false);
                    torch3.SetIsVisible(false);
                    torch4.SetIsVisible(false);

                    txtLevel.SetIsVisible(false);
                    txtLevelTime.SetIsVisible(false);
                    player1HealthBar.SetIsVisible(false);
                    player2HealthBar.SetIsVisible(false);

                    exit.SetIsVisible(false);
                    exitBground.SetIsVisible(false);
                    gameLogo.SetIsVisible(false);
                    bground.SetIsVisible(false);

                    if (statePrev != State.SHOW_GAME_EXIT)
                    {
                        number1.SetIsVisible(false);
                        number2.SetIsVisible(false);
                        number3.SetIsVisible(false);
                        txtGoal.SetIsVisible(false);
                        txtDirecP1.SetIsVisible(false);
                        txtDirecP2.SetIsVisible(false);
                    }

                    txtPlayer1.SetIsVisible(false);
                    txtPlayer1Mod.SetIsVisible(false);
                    txtPlayer1ModTime.SetIsVisible(false);
                    txtPlayer1Score.SetIsVisible(false);
                    txtPlayer1Section.SetIsVisible(false);
                    txtPlayer1Weapon.SetIsVisible(false);

                    txtPlayer2.SetIsVisible(false);
                    txtPlayer2Mod.SetIsVisible(false);
                    txtPlayer2ModTime.SetIsVisible(false);
                    txtPlayer2Score.SetIsVisible(false);
                    txtPlayer2Section.SetIsVisible(false);
                    txtPlayer2Weapon.SetIsVisible(false);

                    bgroundPopup.SetIsVisible(false);
                    txtOk.SetIsVisible(false);
                    txtCancel.SetIsVisible(false);
                    txtGameOver1.SetIsVisible(false);
                    txtGameOver2.SetIsVisible(false);
                    txtGameOver3.SetIsVisible(false);

                    if (statePrev != State.SHOW_GAME_EXIT)
                    {
                        numberState = NumberState.NONE;
                    }
                    else
                    {
                        timeNumberMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

                    pause = false;
                    isDirty = true;
                    break;
                case State.SHOW_GAME_EXIT:
                    numberBground.SetIsVisible(false);
                    bgroundPopup.SetIsVisible(true);
                    txtOk.SetIsVisible(true);
                    txtCancel.SetIsVisible(true);
                    isDirty = true;
                    break;
            }
        }

        /// <summary>
        /// Updates player2's score, left hand paddle.
        /// </summary>
        /// <param name="score">The score to set for player two.</param>
        private void SetScoreLeftText(int score)
        {
            string tmp = score + "";
            while (tmp.Length < 6)
            {
                tmp = "0" + tmp;
            }
            txtPlayer1Score.SetText(tmp);
        }

        /// <summary>
        /// Updates player1's score, right hand paddle.
        /// </summary>
        /// <param name="score">The score to set for player one.</param>
        private void SetScoreRightText(int score)
        {
            string tmp = score + "";
            while (tmp.Length < 6)
            {
                tmp = "0" + tmp;
            }
            txtPlayer2Score.SetText(tmp);
        }

        /*
         * The DrawScreen method gets called by the MmgUpdate method if the Screen is not paused and is responsible for drawing the current screen state.
         */
        public override void DrawScreen()
        {
            pause = true;
            switch (state)
            {
                case State.NONE:
                    break;
                case State.SHOW_GAME_EXIT:
                    break;
                case State.SHOW_COUNT_DOWN_IN_GAME:
                case State.SHOW_COUNT_DOWN:
                    switch (numberState)
                    {
                        case NumberState.NONE:
                            timeNumberMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            numberState = NumberState.NUMBER_3;
                            number1.SetIsVisible(false);
                            number2.SetIsVisible(false);
                            number3.SetIsVisible(true);
                            if (state == State.SHOW_COUNT_DOWN)
                            {
                                txtGoal.SetIsVisible(true);
                                txtDirecP1.SetIsVisible(true);
                                if (gameType == GameType.GAME_TWO_PLAYER)
                                {
                                    txtDirecP2.SetIsVisible(true);
                                }
                                else
                                {
                                    txtDirecP2.SetIsVisible(false);
                                }
                            }
                            else
                            {
                                txtGoal.SetIsVisible(false);
                                txtDirecP1.SetIsVisible(false);
                                txtDirecP2.SetIsVisible(false);
                            }
                            break;
                        case NumberState.NUMBER_1:
                            timeTmpMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            if (timeTmpMs - timeNumberMs >= timeNumberDisplayMs)
                            {
                                timeNumberMs = timeTmpMs;
                                numberState = NumberState.NONE;
                                number1.SetIsVisible(false);
                                number2.SetIsVisible(false);
                                number3.SetIsVisible(false);
                                txtDirecP1.SetIsVisible(false);
                                txtDirecP2.SetIsVisible(false);
                                SetState(State.SHOW_GAME);
                            }
                            break;
                        case NumberState.NUMBER_2:
                            timeTmpMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            if (timeTmpMs - timeNumberMs >= timeNumberDisplayMs)
                            {
                                timeNumberMs = timeTmpMs;
                                numberState = NumberState.NUMBER_1;
                                number1.SetIsVisible(true);
                                number2.SetIsVisible(false);
                                number3.SetIsVisible(false);
                                txtDirecP1.SetIsVisible(true);
                                if (state == State.SHOW_COUNT_DOWN)
                                {
                                    txtGoal.SetIsVisible(true);
                                    if (gameType == GameType.GAME_TWO_PLAYER)
                                    {
                                        txtDirecP2.SetIsVisible(true);
                                    }
                                    else
                                    {
                                        txtDirecP2.SetIsVisible(false);
                                    }
                                }
                                else
                                {
                                    txtGoal.SetIsVisible(false);
                                    txtDirecP1.SetIsVisible(false);
                                    txtDirecP2.SetIsVisible(false);
                                }
                            }
                            break;
                        case NumberState.NUMBER_3:
                            timeTmpMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            if (timeTmpMs - timeNumberMs >= timeNumberDisplayMs)
                            {
                                timeNumberMs = timeTmpMs;
                                numberState = NumberState.NUMBER_2;
                                number1.SetIsVisible(false);
                                number2.SetIsVisible(true);
                                number3.SetIsVisible(false);
                                if (state == State.SHOW_COUNT_DOWN)
                                {
                                    txtGoal.SetIsVisible(true);
                                    if (gameType == GameType.GAME_TWO_PLAYER)
                                    {
                                        txtDirecP2.SetIsVisible(true);
                                    }
                                    else
                                    {
                                        txtDirecP2.SetIsVisible(false);
                                    }
                                }
                                else
                                {
                                    txtGoal.SetIsVisible(false);
                                    txtDirecP1.SetIsVisible(false);
                                    txtDirecP2.SetIsVisible(false);
                                }
                            }
                            break;
                    }
                    break;
                case State.SHOW_GAME:
                    if (wavesCurrent != null)
                    {
                        if (wavesCurrent.actEnemyCount >= wavesCurrent.enemyCount && gameEnemies.GetCount() == 0)
                        {
                            wavesCurrentIdx++;
                            if (wavesCurrentIdx < waves.Length)
                            {
                                if (randomWaves)
                                {
                                    wavesCurrentIdx = MmgHelper.GetRandomIntRange(0, waves.Length);
                                }
                                SetState(State.SHOW_COUNT_DOWN_IN_GAME);
                            }
                            else
                            {
                                SetState(State.SHOW_GAME_OVER);
                            }
                        }
                        else if ((wavesCurrent.timeTotalMs - wavesCurrent.timeCurrentMs) <= 0)
                        {
                            int tmpt = (int)(wavesCurrent.timeTotalMs - wavesCurrent.timeCurrentMs) / 1000;
                            if (tmpt < 0)
                            {
                                tmpt *= -1;
                                if (tmpt >= 15 && gameEnemies.GetCount() > 0)
                                {
                                    scoreTimeUp = true;
                                    SetState(State.SHOW_GAME_OVER);
                                }
                                else if (tmpt >= 0 && gameEnemies.GetCount() == 0)
                                {
                                    tmpt *= 100;
                                    if (gameType == GameType.GAME_TWO_PLAYER)
                                    {
                                        tmpt = tmpt / 2;
                                        scorePlayerOne -= tmpt;
                                        if (scorePlayerOne < 0)
                                        {
                                            scorePlayerOne = 0;
                                        }
                                        SetScoreLeftText(scorePlayerOne);

                                        scorePlayerTwo -= tmpt;
                                        if (scorePlayerTwo < 0)
                                        {
                                            scorePlayerTwo = 0;
                                        }
                                        SetScoreRightText(scorePlayerTwo);
                                    }
                                    else
                                    {
                                        scorePlayerOne -= tmpt;
                                        if (scorePlayerOne < 0)
                                        {
                                            scorePlayerOne = 0;
                                        }
                                        SetScoreLeftText(scorePlayerOne);
                                    }

                                    if (gameEnemies.GetCount() == 0)
                                    {
                                        wavesCurrentIdx++;
                                        if (randomWaves)
                                        {
                                            wavesCurrentIdx = MmgHelper.GetRandomIntRange(0, waves.Length);
                                        }
                                        SetState(State.SHOW_COUNT_DOWN_IN_GAME);
                                    }
                                }
                            }
                        }
                        txtLevelTime.SetText("Time: " + FormatTime(wavesCurrent.timeTotalMs - wavesCurrent.timeCurrentMs));
                    }

                    if (player1 != null && player1.mod != MdtPlayerModType.NONE)
                    {
                        if (player1.mod == MdtPlayerModType.INVINCIBLE)
                        {
                            txtPlayer1Mod.SetText("M: Invinc");
                            txtPlayer1ModTime.SetText("MT: " + FormatMod((player1.modTimingInvTotal - player1.modTimingInv)));
                        }
                        else if (player1.mod == MdtPlayerModType.DOUBLE_POINTS)
                        {
                            txtPlayer1Mod.SetText("M: DblPts");
                            txtPlayer1ModTime.SetText("MT: " + FormatMod((player1.modTimingDpTotal - player1.modTimingDp)));
                        }
                        else if (player1.mod == MdtPlayerModType.FULL_HEALTH)
                        {
                            txtPlayer1Mod.SetText("M: FullHlth");
                            txtPlayer1ModTime.SetText("MT: " + FormatMod((player1.modTimingFullHealthTotal - player1.modTimingFullHealth)));
                        }
                        else
                        {
                            txtPlayer1Mod.SetText("M: -");
                            txtPlayer1ModTime.SetText("MT: 0000");
                        }
                    }
                    else
                    {
                        txtPlayer1Mod.SetText("M: -");
                        txtPlayer1ModTime.SetText("MT: 0000");
                    }

                    if (player2 != null && player2.mod != MdtPlayerModType.NONE)
                    {
                        if (player2.mod == MdtPlayerModType.INVINCIBLE)
                        {
                            txtPlayer2Mod.SetText("M: Invinc");
                            txtPlayer2ModTime.SetText("MT: " + FormatMod((player2.modTimingInvTotal - player2.modTimingInv)));
                        }
                        else if (player2.mod == MdtPlayerModType.DOUBLE_POINTS)
                        {
                            txtPlayer2Mod.SetText("M: DblPts");
                            txtPlayer2ModTime.SetText("MT: " + FormatMod((player2.modTimingDpTotal - player2.modTimingDp)));
                        }
                        else if (player2.mod == MdtPlayerModType.FULL_HEALTH)
                        {
                            txtPlayer2Mod.SetText("M: FullHlth");
                            txtPlayer2ModTime.SetText("MT: " + FormatMod((player2.modTimingFullHealthTotal - player2.modTimingFullHealth)));
                        }
                        else
                        {
                            txtPlayer2Mod.SetText("M: -");
                            txtPlayer2ModTime.SetText("MT: 0000");
                        }
                    }
                    else
                    {
                        txtPlayer2Mod.SetText("M: -");
                        txtPlayer2ModTime.SetText("MT: 0000");
                    }
                    break;
            }
            pause = false;
        }

        /// <summary>
        /// Formats the level number based on the default length of two.
        /// </summary>
        /// <param name="idx">The level number to format.</param>
        /// <returns>A formatted level number.</returns>
        private string FormatLevel(int idx)
        {
            string ret = (idx + 1) + "";
            while (ret.Length < 2)
            {
                ret = "0" + ret;
            }
            return ret;
        }

        /// <summary>
        /// Formats the modifier time based on the milliseconds specified.
        /// </summary>
        /// <param name="ms">The milliseconds specified.</param>
        /// <returns>A formatted time.</returns>
        private string FormatMod(long ms)
        {
            string ret = ms + "";
            while (ret.Length < 4)
            {
                ret = "0" + ret;
            }
            return ret;
        }

        /// <summary>
        /// Formats the current time based on the milliseconds and the default length of three.
        /// </summary>
        /// <param name="ms">The milliseconds to format.</param>
        /// <returns>A formatted time.</returns>
        private string FormatTime(long ms)
        {
            return FormatTime(ms, 3);
        }

        /// <summary>
        /// Formats the current time based on the milliseconds and the length specified.
        /// </summary>
        /// <param name="ms">The milliseconds to format.</param>
        /// <param name="l">The desired number length.</param>
        /// <returns>A formatted time.</returns>
        private string FormatTime(long ms, int l)
        {
            int mul = 1;
            if (ms < 0)
            {
                mul = -1;
            }
            ms *= mul;
            String ret = (ms / 1000) + "";
            while (ret.Length < l)
            {
                ret = "0" + ret;
            }

            if (mul == -1)
            {
                ret = "-" + ret;
            }
            return ret;
        }

        /// <summary>
        /// Clears the modifier image for the given player.
        /// </summary>
        /// <param name="p">The player to clear modifier image for.</param>
        public void UpdateClearPlayerMod(MdtPlayerType p)
        {
            if (p == MdtPlayerType.PLAYER_1)
            {
                RemoveObj(player1ModBmp);
            }
            else
            {
                RemoveObj(player2ModBmp);
            }
        }

        /// <summary>
        /// Updates the modifier HUD UI for the given player and modifier type.
        /// </summary>
        /// <param name="p">The player for which to apply the modifier HUD UI change.</param>
        /// <param name="m">The modifier type to apply to the specified player's HUD UI.</param>
        private void UpdatePlayerMod(MdtPlayerType p, MdtPlayerModType m)
        {
            if (p == MdtPlayerType.PLAYER_1)
            {
                RemoveObj(player1ModBmp);

                if (m == MdtPlayerModType.INVINCIBLE)
                {
                    MdtItemPotionYellow p3 = new MdtItemPotionYellow();
                    player1ModBmp = p3.GetSubj();
                }
                else if (m == MdtPlayerModType.FULL_HEALTH)
                {
                    MdtItemPotionGreen p2 = new MdtItemPotionGreen();
                    player1ModBmp = p2.GetSubj();
                }
                else if (m == MdtPlayerModType.DOUBLE_POINTS)
                {
                    MdtItemPotionRed p1 = new MdtItemPotionRed();
                    player1ModBmp = p1.GetSubj();
                }

                if (player1ModBmp != null)
                {
                    player1ModBmp.SetPosition(txtPlayer1Mod.GetPosition().Clone());
                    player1ModBmp.SetPosition(player1ModBmp.GetPosition().GetX() + txtPlayer1Mod.GetWidth() + MmgHelper.ScaleValue(5), player1ModBmp.GetPosition().GetY() - MmgHelper.ScaleValue(24));
                    player1ModBmp.SetIsVisible(false);
                    AddObj(player1ModBmp);
                }
            }
            else if (p == MdtPlayerType.PLAYER_2)
            {
                RemoveObj(player2ModBmp);

                if (m == MdtPlayerModType.INVINCIBLE)
                {
                    MdtItemPotionYellow p3 = new MdtItemPotionYellow();
                    player2ModBmp = p3.GetSubj();
                }
                else if (m == MdtPlayerModType.FULL_HEALTH)
                {
                    MdtItemPotionGreen p2 = new MdtItemPotionGreen();
                    player2ModBmp = p2.GetSubj();
                }
                else if (m == MdtPlayerModType.DOUBLE_POINTS)
                {
                    MdtItemPotionRed p1 = new MdtItemPotionRed();
                    player2ModBmp = p1.GetSubj();
                }

                if (player2ModBmp != null)
                {
                    player2ModBmp.SetPosition(txtPlayer2Mod.GetPosition().Clone());
                    player2ModBmp.SetPosition(player2ModBmp.GetPosition().GetX() + txtPlayer2Mod.GetWidth() + MmgHelper.ScaleValue(5), player2ModBmp.GetPosition().GetY() - MmgHelper.ScaleValue(24));
                    player2ModBmp.SetIsVisible(false);
                    AddObj(player2ModBmp);
                }
            }
        }

        /// <summary>
        /// Updates and configures the player's weapon.
        /// </summary>
        /// <param name="p">The player to update the weapon for.</param>
        /// <param name="w">The weapon type to set the weapon to.</param>
        public void UpdatePlayerWeapon(MdtPlayerType p, MdtWeaponType w)
        {
            if (p == MdtPlayerType.PLAYER_1)
            {
                RemoveObj(player1WeaponBmp);
                if (w == MdtWeaponType.SPEAR)
                {
                    player1WeaponBmp = player1.weaponCurrent.subjRight.CloneTyped();
                    player1WeaponBmp.SetPosition(txtPlayer1Weapon.GetPosition().Clone());
                    player1WeaponBmp.SetPosition(player1WeaponBmp.GetPosition().GetX() + txtPlayer1Weapon.GetWidth() + MmgHelper.ScaleValue(5), player1WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));
                }
                else if (w == MdtWeaponType.SWORD)
                {
                    player1WeaponBmp = player1.weaponCurrent.subjRight.CloneTyped();
                    player1WeaponBmp.SetPosition(txtPlayer1Weapon.GetPosition().Clone());
                    player1WeaponBmp.SetPosition(player1WeaponBmp.GetPosition().GetX() + txtPlayer1Weapon.GetWidth() + MmgHelper.ScaleValue(5), player1WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));
                }
                else if (w == MdtWeaponType.AXE)
                {
                    player1WeaponBmp = player1.weaponCurrent.subjRight.CloneTyped();
                    player1WeaponBmp.SetPosition(txtPlayer1Weapon.GetPosition().Clone());
                    player1WeaponBmp.SetPosition(player1WeaponBmp.GetPosition().GetX() + txtPlayer1Weapon.GetWidth() + MmgHelper.ScaleValue(5), player1WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));
                }

                if (player1WeaponBmp != null)
                {
                    AddObj(player1WeaponBmp);
                }
            }
            else if (p == MdtPlayerType.PLAYER_2)
            {
                RemoveObj(player2WeaponBmp);
                if (w == MdtWeaponType.SPEAR)
                {
                    player2WeaponBmp = player2.weaponCurrent.subjRight.CloneTyped();
                    player2WeaponBmp.SetPosition(txtPlayer2Weapon.GetPosition().Clone());
                    player2WeaponBmp.SetPosition(player2WeaponBmp.GetPosition().GetX() + txtPlayer2Weapon.GetWidth() + MmgHelper.ScaleValue(5), player2WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));
                }
                else if (w == MdtWeaponType.SWORD)
                {
                    player2WeaponBmp = player2.weaponCurrent.subjRight.CloneTyped();
                    player2WeaponBmp.SetPosition(txtPlayer2Weapon.GetPosition().Clone());
                    player2WeaponBmp.SetPosition(player2WeaponBmp.GetPosition().GetX() + txtPlayer2Weapon.GetWidth() + MmgHelper.ScaleValue(5), player2WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));
                }
                else if (w == MdtWeaponType.AXE)
                {
                    player2WeaponBmp = player2.weaponCurrent.subjRight.CloneTyped();
                    player2WeaponBmp.SetPosition(txtPlayer2Weapon.GetPosition().Clone());
                    player2WeaponBmp.SetPosition(player2WeaponBmp.GetPosition().GetX() + txtPlayer2Weapon.GetWidth() + MmgHelper.ScaleValue(5), player2WeaponBmp.GetPosition().GetY() - MmgHelper.ScaleValue(12));
                }

                if (player2WeaponBmp != null)
                {
                    AddObj(player2WeaponBmp);
                }
            }
        }

        /// <summary>
        /// Removes the specified object from the game screen.
        /// </summary>
        /// <param name="obj">The object to remove from this game screen.</param>
        public override void RemoveObj(MmgObj obj)
        {
            if (obj is MdtBase)
            {
                UpdateRemoveObj((MdtBase)obj, MdtPlayerType.NONE);
            }
            else
            {
                base.RemoveObj(obj);
            }
        }

        /// <summary>
        /// Updates the board by removing the specified object from the base set of objects or the items, objects, or enemies collection.
        /// </summary>
        /// <param name="obj">The object to remove from the game.</param>
        /// <param name="p">The player associated with removing this object.</param>
        public void UpdateRemoveObj(MdtBase obj, MdtPlayerType p)
        {
            if (obj.GetMdtType() == MdtObjType.OBJECT)
            {
                gameObjects.Remove(obj);
                if (obj.GetMdtSubType() == MdtObjSubType.OBJECT_BARREL || obj.GetMdtSubType() == MdtObjSubType.OBJECT_TABLE_1 || obj.GetMdtSubType() == MdtObjSubType.OBJECT_TABLE_2)
                {
                    UpdateAddPoints(obj.GetPosition(), MdtPointsType.PTS_100, p);
                }
            }
            else if (obj.GetMdtType() == MdtObjType.ITEM)
            {
                gameItems.Remove(obj);
            }
            else if (obj.GetMdtType() == MdtObjType.ENEMY)
            {
                gameEnemies.Remove(obj);
            }
            else if (obj.GetMdtType() == MdtObjType.WEAPON)
            {
                base.RemoveObj(obj);
            }
            else if (obj.GetMdtType() == MdtObjType.PLAYER)
            {
                if (obj.GetMdtSubType() == MdtObjSubType.PLAYER_1)
                {
                    playersAliveCount--;
                }
                else if (obj.GetMdtSubType() == MdtObjSubType.PLAYER_2)
                {
                    playersAliveCount--;
                }
                base.RemoveObj(obj);
            }
        }

        /// <summary>
        /// Adds points to the board and the specified player's points.
        /// </summary>
        /// <param name="pos">The position to use to generate the floating points.</param>
        /// <param name="pts">The points image to display on the board.</param>
        /// <param name="p">The player associated with the new points.</param>
        public void UpdateAddPoints(MmgVector2 pos, MdtPointsType pts, MdtPlayerType p)
        {
            MmgBmp ptsBmp = null;
            MdtUiPoints ptsUi = null;
            MmgVector2 posC = null;

            if (p == MdtPlayerType.PLAYER_1)
            {
                if (pts == MdtPointsType.PTS_100)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_red_100.png");
                    scorePlayerOne += 100;
                    if (player1.hasDoublePoints)
                    {
                        scorePlayerOne += 100;
                    }
                    SetScoreLeftText(scorePlayerOne);
                }
                else if (pts == MdtPointsType.PTS_250)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_red_250.png");
                    scorePlayerOne += 250;
                    if (player1.hasDoublePoints)
                    {
                        scorePlayerOne += 250;
                    }
                    SetScoreLeftText(scorePlayerOne);
                }
                else if (pts == MdtPointsType.PTS_500)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_red_500.png");
                    scorePlayerOne += 500;
                    if (player1.hasDoublePoints)
                    {
                        scorePlayerOne += 500;
                    }
                    SetScoreLeftText(scorePlayerOne);
                }
                else if (pts == MdtPointsType.PTS_1000)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_red_1000.png");
                    scorePlayerOne += 1000;
                    if (player1.hasDoublePoints)
                    {
                        scorePlayerOne += 1000;
                    }
                    SetScoreLeftText(scorePlayerOne);
                }

                ptsUi = new MdtUiPoints(ptsBmp, p, this, pos);
                AddObj(ptsUi);

                if (player1.hasDoublePoints)
                {
                    posC = pos.Clone();
                    posC.SetY(posC.GetY() + ptsBmp.GetHeight());
                    ptsUi = new MdtUiPoints(ptsBmp.CloneTyped(), p, this, posC);
                    AddObj(ptsUi);
                }
            }
            else
            {
                if (pts == MdtPointsType.PTS_100)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_blue_100.png");
                    scorePlayerTwo += 100;
                    if (player2.hasDoublePoints)
                    {
                        scorePlayerTwo += 100;
                    }
                    SetScoreRightText(scorePlayerTwo);
                }
                else if (pts == MdtPointsType.PTS_250)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_blue_250.png");
                    scorePlayerTwo += 250;
                    if (player2.hasDoublePoints)
                    {
                        scorePlayerTwo += 250;
                    }
                    SetScoreRightText(scorePlayerTwo);
                }
                else if (pts == MdtPointsType.PTS_500)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_blue_500.png");
                    scorePlayerTwo += 500;
                    if (player2.hasDoublePoints)
                    {
                        scorePlayerTwo += 500;
                    }
                    SetScoreRightText(scorePlayerTwo);
                }
                else if (pts == MdtPointsType.PTS_1000)
                {
                    ptsBmp = MmgHelper.GetBasicCachedBmp("pts_blue_1000.png");
                    scorePlayerTwo += 1000;
                    if (player2.hasDoublePoints)
                    {
                        scorePlayerTwo += 1000;
                    }
                    SetScoreRightText(scorePlayerTwo);
                }

                ptsUi = new MdtUiPoints(ptsBmp, p, this, pos);
                AddObj(ptsUi);

                if (player2.hasDoublePoints)
                {
                    posC = pos.Clone();
                    posC.SetY(posC.GetY() + ptsBmp.GetHeight());
                    ptsUi = new MdtUiPoints(ptsBmp.CloneTyped(), p, this, posC);
                    AddObj(ptsUi);
                }
            }
        }

        /// <summary>
        /// Updates the board and hides all the doors.
        /// </summary>
        private void UpdateHideAllDoors()
        {
            doorTopLeftLocked.SetIsVisible(false);
            doorTopLeftOpened.SetIsVisible(false);

            doorTopRightLocked.SetIsVisible(false);
            doorTopRightOpened.SetIsVisible(false);

            doorLeftLockIcon.SetIsVisible(false);
            doorRightLockIcon.SetIsVisible(false);
            doorBotLeftLockIcon.SetIsVisible(false);
            doorBotRightLockIcon.SetIsVisible(false);
        }

        /// <summary>
        /// Updates the board and locks all the doors.
        /// </summary>
        private void UpdateLockAllDoors()
        {
            doorTopLeftLocked.SetIsVisible(true);
            doorTopLeftOpened.SetIsVisible(false);

            doorTopRightLocked.SetIsVisible(true);
            doorTopRightOpened.SetIsVisible(false);

            doorLeftLockIcon.SetIsVisible(true);
            doorRightLockIcon.SetIsVisible(true);
            doorBotLeftLockIcon.SetIsVisible(true);
            doorBotRightLockIcon.SetIsVisible(true);
        }

        /// <summary>
        /// Updates the board and unlocks the specified door.
        /// </summary>
        /// <param name="d">The door specified to unlock.</param>
        private void UpdateUnlockDoor(MdtDoorType d)
        {
            if (d == MdtDoorType.TOP_LEFT)
            {
                doorTopLeftLocked.SetIsVisible(false);
                doorTopLeftOpened.SetIsVisible(true);
            }
            else if (d == MdtDoorType.TOP_RIGHT)
            {
                doorTopRightLocked.SetIsVisible(false);
                doorTopRightOpened.SetIsVisible(true);
            }
            else if (d == MdtDoorType.LEFT)
            {
                doorLeftLockIcon.SetIsVisible(false);
            }
            else if (d == MdtDoorType.RIGHT)
            {
                doorRightLockIcon.SetIsVisible(false);
            }
            else if (d == MdtDoorType.BOTTOM_LEFT)
            {
                doorBotLeftLockIcon.SetIsVisible(false);
            }
            else if (d == MdtDoorType.BOTTOM_RIGHT)
            {
                doorBotRightLockIcon.SetIsVisible(false);
            }
        }

        /// <summary>
        /// Can an object move to the target MmgRect ignoring the specified object.
        /// </summary>
        /// <param name="r">The target MmgRect the object intends to move to.</param>
        /// <param name="iO">The target to ignore.</param>
        /// <returns>The object that collided with the target.</returns>
        public MdtBase CanMove(MmgRect r, MdtBase iO)
        {
            int len = GetObjects().GetCount();
            MmgObj o = null;
            MdtBase b = null;
            MmgContainer c = null;

            for (int i = 0; i < len; i++)
            {
                o = GetObjects().GetChildAt(i);
                if (o is MmgContainer)
                {
                    c = (MmgContainer)o;
                    int len2 = c.GetCount();
                    for (int j = 0; j < len2; j++)
                    {
                        o = c.GetChildAt(j);
                        if (o is MdtBase && ((MdtBase)o).isVisible)
                        {
                            b = ((MdtBase)o);

                            if (b != null && iO != null && b.Equals(iO) == false && MmgHelper.RectCollision(r, b.GetRect()))
                            {
                                return b;
                            }
                            else if (b != null && iO == null && MmgHelper.RectCollision(r, b.GetRect()))
                            {
                                return b;
                            }
                        }
                    }
                }
                else if (o is MdtBase && ((MdtBase)o).isVisible)
                {
                    b = ((MdtBase)o);
                    if (b != null && iO != null && b.Equals(iO) == false && MmgHelper.RectCollision(r, b.GetRect()))
                    {
                        return b;
                    }
                    else if (b != null && iO == null && MmgHelper.RectCollision(r, b.GetRect()))
                    {
                        return b;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Can an object move to the target MmgRect ignoring the specified object.
        /// </summary>
        /// <param name="r">The target MmgRect the object intends to move to.</param>
        /// <returns>The object that collided with the target.</returns>
        public MdtBase CanMove(MmgRect r)
        {
            return CanMove(r, null);
        }

        /// <summary>
        /// Processes the collision of two game objects.
        /// </summary>
        /// <param name="o1">An object involved in the collision.</param>
        /// <param name="o2">An object involved in the collision.</param>
        public void UpdateProcessCollision(MdtBase o1, MdtBase o2)
        {
            bool found = false;
            bool hasObjO1 = false;
            bool hasObjO2 = false;
            MdtObjPushBarrel t = null;
            MdtObjPushTableSmall st = null;
            MdtObjPushTableLarge lt = null;

            if (o1 is MdtObjPushBarrel || o1 is MdtObjPushTableSmall || o1 is MdtObjPushTableLarge)
            {
                hasObjO1 = true;
            }
            else if (o2 is MdtObjPushBarrel || o2 is MdtObjPushTableSmall || o2 is MdtObjPushTableLarge)
            {
                hasObjO2 = true;
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasObjO1 || hasObjO2)
                {
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasObjO1 || hasObjO2)
                {
                    found = true;
                }
            }

            if (found)
            {
                if (!player1.isPushStart && !player1.isPushing)
                {
                    player1.isPushStart = true;
                    player1.pushingCurrentMs = 0;
                    player1.pushingStartMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
                else if (player1.isPushStart)
                {

                }
                else if (player1.isPushing)
                {
                    player1.isPushing = false;

                    if (hasObjO1)
                    {
                        if (o1 is MdtObjPushBarrel)
                        {
                            t = (MdtObjPushBarrel)o1;
                            if (t.isBeingPushed == false)
                            {
                                UpdateAddPoints(o1.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_1);
                            }
                            t.isBeingPushed = true;
                            t.pushDir = player1.dir;
                            t.pushedBy = MdtPlayerType.PLAYER_1;
                        }
                        else if (o1 is MdtObjPushTableSmall)
                        {
                            st = (MdtObjPushTableSmall)o1;
                            if (st.isBeingPushed == false)
                            {
                                UpdateAddPoints(o1.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_1);
                            }
                            st.isBeingPushed = true;
                            st.pushDir = player1.dir;
                            st.pushedBy = MdtPlayerType.PLAYER_1;
                        }
                        else if (o1 is MdtObjPushTableLarge)
                        {
                            lt = (MdtObjPushTableLarge)o1;
                            if (lt.isBeingPushed == false)
                            {
                                UpdateAddPoints(o1.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_1);
                            }
                            lt.isBeingPushed = true;
                            lt.pushDir = player1.dir;
                            lt.pushedBy = MdtPlayerType.PLAYER_1;
                        }
                    }
                    else
                    {
                        if (o2 is MdtObjPushBarrel)
                        {
                            t = (MdtObjPushBarrel)o2;
                            if (t.isBeingPushed == false)
                            {
                                UpdateAddPoints(o2.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_1);
                            }
                            t.isBeingPushed = true;
                            t.pushDir = player1.dir;
                            t.pushedBy = MdtPlayerType.PLAYER_1;
                        }
                        else if (o2 is MdtObjPushTableSmall)
                        {
                            st = (MdtObjPushTableSmall)o2;
                            if (st.isBeingPushed == false)
                            {
                                UpdateAddPoints(o2.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_1);
                            }
                            st.isBeingPushed = true;
                            st.pushDir = player1.dir;
                            st.pushedBy = MdtPlayerType.PLAYER_1;
                        }
                        else if (o2 is MdtObjPushTableLarge)
                        {
                            lt = (MdtObjPushTableLarge)o2;
                            if (lt.isBeingPushed == false)
                            {
                                UpdateAddPoints(o2.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_1);
                            }
                            lt.isBeingPushed = true;
                            lt.pushDir = player1.dir;
                            lt.pushedBy = MdtPlayerType.PLAYER_1;
                        }
                    }
                }
            }
            else
            {
                player1.isPushStart = false;
                player1.isPushing = false;
                player1.pushingCurrentMs = 0;
                player1.pushingStartMs = 0;
            }

            if (found)
            {
                return;
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasObjO1 || hasObjO2)
                {
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasObjO1 || hasObjO2)
                {
                    found = true;
                }
            }

            if (found)
            {
                if (!player2.isPushStart && !player2.isPushing)
                {
                    player2.isPushStart = true;
                    player2.pushingCurrentMs = 0;
                    player2.pushingStartMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
                else if (player2.isPushStart)
                {

                }
                else if (player2.isPushing)
                {
                    player2.isPushing = false;

                    if (hasObjO1)
                    {
                        if (o1 is MdtObjPushBarrel)
                        {
                            t = (MdtObjPushBarrel)o1;
                            t.isBeingPushed = true;
                            t.pushDir = player2.dir;
                            t.pushedBy = MdtPlayerType.PLAYER_2;
                            UpdateAddPoints(o1.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_2);
                        }
                        else if (o1 is MdtObjPushTableSmall)
                        {
                            st = (MdtObjPushTableSmall)o1;
                            st.isBeingPushed = true;
                            st.pushDir = player2.dir;
                            st.pushedBy = MdtPlayerType.PLAYER_2;
                            UpdateAddPoints(o1.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_2);
                        }
                        else if (o1 is MdtObjPushTableLarge)
                        {
                            lt = (MdtObjPushTableLarge)o1;
                            lt.isBeingPushed = true;
                            lt.pushDir = player2.dir;
                            lt.pushedBy = MdtPlayerType.PLAYER_2;
                            UpdateAddPoints(o1.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_2);
                        }
                    }
                    else
                    {
                        if (o2 is MdtObjPushBarrel)
                        {
                            t = (MdtObjPushBarrel)o2;
                            t.isBeingPushed = true;
                            t.pushDir = player2.dir;
                            t.pushedBy = MdtPlayerType.PLAYER_2;
                            UpdateAddPoints(o2.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_2);
                        }
                        else if (o2 is MdtObjPushTableSmall)
                        {
                            st = (MdtObjPushTableSmall)o2;
                            st.isBeingPushed = true;
                            st.pushDir = player2.dir;
                            st.pushedBy = MdtPlayerType.PLAYER_2;
                            UpdateAddPoints(o2.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_2);
                        }
                        else if (o2 is MdtObjPushTableLarge)
                        {
                            lt = (MdtObjPushTableLarge)o2;
                            lt.isBeingPushed = true;
                            lt.pushDir = player2.dir;
                            lt.pushedBy = MdtPlayerType.PLAYER_2;
                            UpdateAddPoints(o2.GetPosition().Clone(), MdtPointsType.PTS_100, MdtPlayerType.PLAYER_2);
                        }
                    }
                }
            }
            else
            {
                player2.isPushStart = false;
                player2.isPushing = false;
                player2.pushingCurrentMs = 0;
                player2.pushingStartMs = 0;
            }

            if (found)
            {
                return;
            }

            bool hasItemPotionYellowO1 = false;
            bool hasItemPotionYellowO2 = false;
            if (o1 is MdtItemPotionYellow)
            {
                hasItemPotionYellowO1 = true;
                RemoveObj(o1);
            }
            else if (o2 is MdtItemPotionYellow)
            {
                hasItemPotionYellowO2 = true;
                RemoveObj(o2);
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasItemPotionYellowO1 || hasItemPotionYellowO2)
                {
                    player1.SetMod(MdtPlayerModType.INVINCIBLE);
                    player1.modTimingInv = 0;
                    player1.SetHasInvincibility(true);
                    UpdatePlayerMod(player1.playerType, MdtPlayerModType.INVINCIBLE);
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemPotionYellow)o2).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasItemPotionYellowO1 || hasItemPotionYellowO2)
                {
                    player1.SetMod(MdtPlayerModType.INVINCIBLE);
                    player1.modTimingInv = 0;
                    player1.SetHasInvincibility(true);
                    UpdatePlayerMod(player1.playerType, MdtPlayerModType.INVINCIBLE);
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemPotionYellow)o1).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasItemPotionYellowO1 || hasItemPotionYellowO2)
                {
                    player2.SetMod(MdtPlayerModType.INVINCIBLE);
                    player2.modTimingInv = 0;
                    player2.SetHasInvincibility(true);
                    UpdatePlayerMod(player2.playerType, MdtPlayerModType.INVINCIBLE);
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemPotionYellow)o2).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasItemPotionYellowO1 || hasItemPotionYellowO2)
                {
                    player2.SetMod(MdtPlayerModType.INVINCIBLE);
                    player2.modTimingInv = 0;
                    player2.SetHasInvincibility(true);
                    UpdatePlayerMod(player2.playerType, MdtPlayerModType.INVINCIBLE);
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemPotionYellow)o1).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }

            if (found)
            {
                return;
            }

            bool hasItemPotionRedO1 = false;
            bool hasItemPotionRedO2 = false;
            if (o1 is MdtItemPotionRed)
            {
                hasItemPotionRedO1 = true;
                RemoveObj(o1);
            }
            else if (o2 is MdtItemPotionRed)
            {
                hasItemPotionRedO2 = true;
                RemoveObj(o2);
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasItemPotionRedO1 || hasItemPotionRedO2)
                {
                    player1.SetMod(MdtPlayerModType.DOUBLE_POINTS);
                    player1.modTimingDp = 0;
                    player1.SetHasDoublePoints(true);
                    UpdatePlayerMod(player1.playerType, MdtPlayerModType.DOUBLE_POINTS);
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemPotionRed)o2).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasItemPotionRedO1 || hasItemPotionRedO2)
                {
                    player1.SetMod(MdtPlayerModType.DOUBLE_POINTS);
                    player1.modTimingDp = 0;
                    player1.SetHasDoublePoints(true);
                    UpdatePlayerMod(player1.playerType, MdtPlayerModType.DOUBLE_POINTS);
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemPotionRed)o1).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasItemPotionRedO1 || hasItemPotionRedO2)
                {
                    player2.SetMod(MdtPlayerModType.DOUBLE_POINTS);
                    player2.modTimingDp = 0;
                    player2.SetHasDoublePoints(true);
                    UpdatePlayerMod(player2.playerType, MdtPlayerModType.DOUBLE_POINTS);
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemPotionRed)o2).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasItemPotionRedO1 || hasItemPotionRedO2)
                {
                    player2.SetMod(MdtPlayerModType.DOUBLE_POINTS);
                    player2.modTimingDp = 0;
                    player2.SetHasDoublePoints(true);
                    UpdatePlayerMod(player2.playerType, MdtPlayerModType.DOUBLE_POINTS);
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemPotionRed)o1).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }

            if (found)
            {
                return;
            }

            bool hasItemPotionGreenO1 = false;
            bool hasItemPotionGreenO2 = false;
            if (o1 is MdtItemPotionGreen)
            {
                hasItemPotionGreenO1 = true;
                RemoveObj(o1);
            }
            else if (o2 is MdtItemPotionGreen)
            {
                hasItemPotionGreenO2 = true;
                RemoveObj(o2);
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasItemPotionGreenO1 || hasItemPotionGreenO2)
                {
                    player1.SetMod(MdtPlayerModType.FULL_HEALTH);
                    player1.modTimingFullHealth = 0;
                    player1.SetHasFullHealth(true);
                    player1.SetHealthToMax();
                    player1HealthBar.RestoreAllHealth();
                    UpdatePlayerMod(player1.playerType, MdtPlayerModType.FULL_HEALTH);
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemPotionGreen)o2).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasItemPotionGreenO1 || hasItemPotionGreenO2)
                {
                    player1.SetMod(MdtPlayerModType.FULL_HEALTH);
                    player1.modTimingFullHealth = 0;
                    player1.SetHasFullHealth(true);
                    player1.SetHealthToMax();
                    player1HealthBar.RestoreAllHealth();
                    UpdatePlayerMod(player1.playerType, MdtPlayerModType.FULL_HEALTH);
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemPotionGreen)o1).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasItemPotionGreenO1 || hasItemPotionGreenO2)
                {
                    player2.SetMod(MdtPlayerModType.FULL_HEALTH);
                    player2.modTimingFullHealth = 0;
                    player2.SetHasFullHealth(true);
                    player2.SetHealthToMax();
                    player2HealthBar.RestoreAllHealth();
                    UpdatePlayerMod(player2.playerType, MdtPlayerModType.FULL_HEALTH);
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemPotionGreen)o2).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasItemPotionGreenO1 || hasItemPotionGreenO2)
                {
                    player2.SetMod(MdtPlayerModType.FULL_HEALTH);
                    player2.modTimingFullHealth = 0;
                    player2.SetHasFullHealth(true);
                    player2.SetHealthToMax();
                    player2HealthBar.RestoreAllHealth();
                    UpdatePlayerMod(player2.playerType, MdtPlayerModType.FULL_HEALTH);
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemPotionGreen)o1).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }

            if (found)
            {
                return;
            }

            //Check for coin bag collision
            bool hasCoinBagO1 = false;
            bool hasCoinBagO2 = false;
            if (o1 is MdtItemCoinBag)
            {
                hasCoinBagO1 = true;
                RemoveObj(o1);
            }
            else if (o2 is MdtItemCoinBag)
            {
                hasCoinBagO2 = true;
                RemoveObj(o2);
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasCoinBagO1 || hasCoinBagO2)
                {
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemCoinBag)o2).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasCoinBagO1 || hasCoinBagO2)
                {
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemCoinBag)o1).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasCoinBagO1 || hasCoinBagO2)
                {
                    UpdateAddPoints(o2.GetPosition().Clone(), ((MdtItemCoinBag)o2).points, MdtPlayerType.PLAYER_1);
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasCoinBagO1 || hasCoinBagO2)
                {
                    UpdateAddPoints(o1.GetPosition().Clone(), ((MdtItemCoinBag)o1).points, MdtPlayerType.PLAYER_2);
                    found = true;
                }
            }

            if (found)
            {
                return;
            }

            //Check for enemy collision
            bool hasEnemyO1 = false;
            bool hasEnemyO2 = false;
            if (o1 is MdtCharInter && ((MdtCharInter)o1).playerType == MdtPlayerType.ENEMY)
            {
                hasEnemyO1 = true;
            }
            else if (o2 is MdtCharInter && ((MdtCharInter)o2).playerType == MdtPlayerType.ENEMY)
            {
                hasEnemyO2 = true;
            }

            MdtCharInter ecO1;
            MdtCharInter ecO2;

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasEnemyO1 || hasEnemyO2)
                {
                    if (player1.isBouncing == false)
                    {
                        ecO2 = (MdtCharInter)o2;
                        if (!player1.hasFullHealth && !player1.hasInvincibility)
                        {
                            player1HealthBar.TakeDamage(1);
                            player1.TakeDamage(1, ecO2.playerType);
                        }

                        if (!player1.isBroken)
                        {
                            player1.Bounce(ecO2.GetPosition(), ecO2.GetWidth() / 2, ecO2.GetHeight() / 2, GetOppositeDir(player1.dir), ecO2.playerType);
                        }
                    }
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                if (hasEnemyO1 || hasEnemyO2)
                {
                    if (player1.isBouncing == false)
                    {
                        ecO1 = (MdtCharInter)o1;
                        if (!player1.hasFullHealth && !player1.hasInvincibility)
                        {
                            player1HealthBar.TakeDamage(1);
                            player1.TakeDamage(1, ecO1.playerType);
                        }

                        if (!player1.isBroken)
                        {
                            player1.Bounce(ecO1.GetPosition(), ecO1.GetWidth() / 2, ecO1.GetHeight() / 2, GetOppositeDir(player1.dir), ecO1.playerType);
                        }
                    }
                    found = true;
                }
            }

            if (o1.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasEnemyO1 || hasEnemyO2)
                {
                    if (player2.isBouncing == false)
                    {
                        ecO2 = (MdtCharInter)o2;
                        if (!player2.hasFullHealth && !player2.hasInvincibility)
                        {
                            player2HealthBar.TakeDamage(1);
                            player2.TakeDamage(1, ecO2.playerType);
                        }

                        if (!player2.isBroken)
                        {
                            player2.Bounce(ecO2.GetPosition(), ecO2.GetWidth() / 2, ecO2.GetHeight() / 2, GetOppositeDir(player2.dir), ecO2.playerType);
                        }
                    }
                    found = true;
                }
            }
            else if (o2.GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                if (hasEnemyO1 || hasEnemyO2)
                {
                    if (player2.isBouncing == false)
                    {
                        ecO1 = (MdtCharInter)o1;
                        if (!player2.hasFullHealth && !player2.hasInvincibility)
                        {
                            player2HealthBar.TakeDamage(1);
                            player2.TakeDamage(1, ecO1.playerType);
                        }

                        if (!player2.isBroken)
                        {
                            player2.Bounce(ecO1.GetPosition(), ecO1.GetWidth() / 2, ecO1.GetHeight() / 2, GetOppositeDir(player2.dir), ecO1.playerType);
                        }
                    }
                    found = true;
                }
            }

            if (found)
            {
                return;
            }
        }

        /// <summary>
        /// Gets the direction that is opposite the direction provided.
        /// </summary>
        /// <param name="d">The direction to find an opposite direction for.</param>
        /// <returns>The direction opposite the direction provided.</returns>
        public int GetOppositeDir(int d)
        {
            if (d == MmgDir.DIR_BACK)
            {
                return MmgDir.DIR_FRONT;
            }
            else if (d == MmgDir.DIR_FRONT)
            {
                return MmgDir.DIR_BACK;
            }
            else if (d == MmgDir.DIR_LEFT)
            {
                return MmgDir.DIR_RIGHT;
            }
            else if (d == MmgDir.DIR_RIGHT)
            {
                return MmgDir.DIR_LEFT;
            }
            else
            {
                return MmgDir.DIR_NONE;
            }
        }

        /// <summary>
        /// Process weapon collisions from a weapon projectile.
        /// </summary>
        /// <param name="o1">The object hit by the weapon projectile.</param>
        /// <param name="o2">The object that is the weapon projectile.</param>
        /// <param name="weapon">The MmgRect that describes the weapon.</param>
        public void UpdateProcessWeaponCollision(MdtBase o1, MdtWeapon o2, MmgRect weapon)
        {
            MdtPlayerType tp = MdtPlayerType.NONE;
            if (o2.GetHolder().GetMdtSubType() == MdtObjSubType.PLAYER_1)
            {
                tp = MdtPlayerType.PLAYER_1;
            }
            else if (o2.GetHolder().GetMdtSubType() == MdtObjSubType.PLAYER_2)
            {
                tp = MdtPlayerType.PLAYER_2;
            }
            else if (o2.GetHolder().GetMdtSubType() == MdtObjSubType.ENEMY_BANSHEE || o2.GetHolder().GetMdtSubType() == MdtObjSubType.ENEMY_DEMON || o2.GetHolder().GetMdtSubType() == MdtObjSubType.ENEMY_WARLOCK)
            {
                tp = MdtPlayerType.ENEMY;
            }

            if (o1 is MdtCharInter && ((MdtCharInter)o1).GetPlayerType() == MdtPlayerType.ENEMY && o2.GetHolder().GetMdtType() != MdtObjType.ENEMY) {
                MdtCharInter mci = (MdtCharInter)o1;
                if (!mci.isBouncing && tp != MdtPlayerType.NONE)
                {
                    UpdateAddPoints(weapon.GetPosition(), MdtPointsType.PTS_100, tp);
                    mci.Bounce(weapon.GetPosition(), weapon.GetWidth() / 2, weapon.GetHeight() / 2, o2.GetHolder().GetDir(), tp);

                    if (sound1 != null)
                    {
                        sound1.Play();
                    }
                }
            } else if (o1 is MdtCharInter && ((MdtCharInter)o1).GetPlayerType() != MdtPlayerType.ENEMY && o2.GetHolder().GetMdtType() == MdtObjType.ENEMY) {
                MdtCharInter mci = (MdtCharInter)o1;
                if (!mci.isBouncing && tp != MdtPlayerType.NONE)
                {
                    UpdateAddPoints(weapon.GetPosition(), MdtPointsType.PTS_100, tp);
                    mci.Bounce(weapon.GetPosition(), weapon.GetWidth() / 2, weapon.GetHeight() / 2, o2.GetHolder().GetDir(), tp);

                    if (sound1 != null)
                    {
                        sound1.Play();
                    }
                }
            }
        }

        /// <summary>
        /// Process weapon collisions from a non-projectile weapon. 
        /// </summary>
        /// <param name="o1">The target of the weapon.</param>
        /// <param name="o2">The holder of the weapon.</param>
        /// <param name="weapon">The MmgRect of the weapon.</param>
        public void UpdateProcessWeaponCollision(MdtBase o1, MdtCharInter o2, MmgRect weapon)
        {
            if (o1 is MdtCharInter && ((MdtCharInter)o1).GetPlayerType() == MdtPlayerType.ENEMY && o2.GetPlayerType() != MdtPlayerType.ENEMY) {
                MdtCharInter mci = (MdtCharInter)o1;
                if (!mci.isBouncing)
                {
                    UpdateAddPoints(weapon.GetPosition(), MdtPointsType.PTS_100, o2.GetPlayerType());
                    mci.Bounce(weapon.GetPosition(), weapon.GetWidth() / 2, weapon.GetHeight() / 2, o2.GetDir(), o2.GetPlayerType());

                    if (sound1 != null)
                    {
                        sound1.Play();
                    }
                }
            } else if (o1 is MdtCharInter && ((MdtCharInter)o1).GetPlayerType() != MdtPlayerType.ENEMY && o2.GetPlayerType() == MdtPlayerType.ENEMY) {
                MdtCharInter mci = (MdtCharInter)o1;
                if (!mci.isBouncing)
                {
                    UpdateAddPoints(weapon.GetPosition(), MdtPointsType.PTS_100, o2.GetPlayerType());
                    mci.Bounce(weapon.GetPosition(), weapon.GetWidth() / 2, weapon.GetHeight() / 2, o2.GetDir(), o2.GetPlayerType());

                    if (sound1 != null)
                    {
                        sound1.Play();
                    }
                }
            }
        }

        /// <summary>
        /// Generates a random item from a list based on the current enemy wave.
        /// </summary>
        /// <param name="x">The X coordinate to place the item.</param>
        /// <param name="y">The Y coordinate to place the item.</param>
        /// <returns>A generated item.</returns>
        public MdtBase UpdateGenerateMdtItem(int x, int y)
        {
            if (wavesCurrent != null)
            {
                MdtItem itm = UpdateGenerateMdtItem(x, y, wavesCurrent.activeItems);
                if (itm != null)
                {
                    itm.SetScreen(this);
                    itm.SetPosition(x, y);
                    gameItems.Add(itm);
                }
                return itm;
            }
            return null;
        }

        /// <summary>
        /// Generate a random item from a list of item types.
        /// </summary>
        /// <param name="x">The X coordinate to place the item.</param>
        /// <param name="y">The Y coordinate to place the item.</param>
        /// <param name="items">The available items to choose from.</param>
        /// <returns>A generated item.</returns>
        public MdtItem UpdateGenerateMdtItem(int x, int y, MdtItemType[] items)
        {
            int idx = MmgHelper.GetRandomInt(items.Length);
            MdtItemType t = items[idx];
            MdtItem itm = null;

            if (t == MdtItemType.BOMB)
            {
                MdtItemBomb bomb = new MdtItemBomb();
                itm = bomb;
            }
            else if (t == MdtItemType.COIN_BAG)
            {
                MdtItemCoinBag coins = new MdtItemCoinBag();
                itm = coins;
            }
            else if (t == MdtItemType.POTION_GREEN)
            {
                MdtItemPotionGreen potion1 = new MdtItemPotionGreen();
                itm = potion1;
            }
            else if (t == MdtItemType.POTION_RED)
            {
                MdtItemPotionRed potion2 = new MdtItemPotionRed();
                itm = potion2;
            }
            else if (t == MdtItemType.POTION_YELLOW)
            {
                MdtItemPotionYellow potion3 = new MdtItemPotionYellow();
                itm = potion3;
            }
            else
            {
                MdtItemPotionGreen potion1 = new MdtItemPotionGreen();
                itm = potion1;
            }
            return itm;
        }

        /// <summary>
        /// Generate a random push-able object.
        /// </summary>
        /// <param name="x">The X coordinate to place the object.</param>
        /// <param name="y">The Y coordinate to place the object.</param>
        /// <returns>A generated object.</returns>
        public MdtObjPush UpdateGenerateMdtObj(int x, int y)
        {
            int idx = MmgHelper.GetRandomInt(3);
            MdtObjPush objPush = null;

            if (idx == 0)
            {
                objPush = new MdtObjPushBarrel(this);
            }
            else if (idx == 1)
            {
                objPush = new MdtObjPushTableSmall(this);
            }
            else if (idx == 2)
            {
                objPush = new MdtObjPushTableLarge(this);
            }
            else
            {
                objPush = new MdtObjPushBarrel(this);
            }
            return objPush;
        }

        /// <summary>
        /// Generates the random objects available at the start of a new level.
        /// </summary>
        /// <param name="cnt">The number of entries to generate.</param>
        /// <param name="items">The types of items available during this level generation.</param>
        private void UpdateGenerateObjects(int cnt, MdtItemType[] items)
        {
            int tmp = 0;
            int tmpl = 0;
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;

            MdtBase obj = null;
            MdtItemType t;
            MmgRect r = null;
            MdtBase coll = null;

            for (int i = 0; i < cnt; i++)
            {
                tmp = MmgHelper.GetRandomInt(11) % 2;
                tmpl = MmgHelper.GetRandomInt(11) % 2;

                if (tmpl == 0)
                {
                    x = MmgHelper.GetRandomIntRange(randoLeft.GetLeft(), randoLeft.GetRight());
                    y = MmgHelper.GetRandomIntRange(randoLeft.GetTop(), randoLeft.GetBottom());
                }
                else
                {
                    x = MmgHelper.GetRandomIntRange(randoRight.GetLeft(), randoRight.GetRight());
                    y = MmgHelper.GetRandomIntRange(randoRight.GetTop(), randoRight.GetBottom());
                }

                if (tmp == 0)
                {
                    MdtItem itm = UpdateGenerateMdtItem(x, y, items);

                    if (itm != null)
                    {
                        itm.SetScreen(this);
                        itm.SetPosition(x, y);
                        w = itm.GetWidth();
                        h = itm.GetHeight();
                        pos = itm.GetPosition();
                        obj = itm;
                        gameItems.Add(itm);
                    }
                }
                else
                {
                    MdtObjPush objPush = UpdateGenerateMdtObj(x, y);

                    if (objPush != null)
                    {
                        objPush.SetScreen(this);
                        objPush.SetPosition(x, y);
                        w = objPush.GetWidth();
                        h = objPush.GetHeight();
                        pos = objPush.GetPosition();
                        obj = objPush;
                        gameObjects.Add(objPush);
                    }
                }

                if (obj != null)
                {
                    x -= MmgHelper.ScaleValue(32);
                    y -= MmgHelper.ScaleValue(32);
                    w += MmgHelper.ScaleValue(32);
                    h += MmgHelper.ScaleValue(32);
                    r = new MmgRect(x, y, y + h, x + w);
                    coll = CanMove(r);

                    while (coll != null)
                    {
                        if (tmpl == 0)
                        {
                            x = MmgHelper.GetRandomIntRange(randoLeft.GetLeft(), randoLeft.GetRight());
                            y = MmgHelper.GetRandomIntRange(randoLeft.GetTop(), randoLeft.GetBottom());
                        }
                        else
                        {
                            x = MmgHelper.GetRandomIntRange(randoRight.GetLeft(), randoRight.GetRight());
                            y = MmgHelper.GetRandomIntRange(randoRight.GetTop(), randoRight.GetBottom());
                        }

                        if (rand.Next(11) % 2 == 0)
                        {
                            x -= MmgHelper.ScaleValue(32);
                        }
                        else
                        {
                            x += MmgHelper.ScaleValue(32);
                        }

                        if (rand.Next(11) % 2 == 0)
                        {
                            y -= MmgHelper.ScaleValue(32);
                        }
                        else
                        {
                            y += MmgHelper.ScaleValue(32);
                        }
                        r = new MmgRect(x, y, y + h, x + w);
                        coll = CanMove(r);
                    }

                    obj.SetPosition(x, y);
                }
            }
        }

        /// <summary>
        /// Resets the players for the next wave of enemies.
        /// </summary>
        private void UpdateResetPlayers()
        {
            if (gameType == GameType.GAME_ONE_PLAYER || gameType == GameType.GAME_TWO_PLAYER)
            {
                MmgObj obj = new MmgObj(player1.GetWidth(), player1.GetHeight());
                MmgHelper.CenterHorAndVert(obj);
                obj.SetX(obj.GetX() - (GAME_WIDTH - BOARD_WIDTH) / 2 + obj.GetWidth());
                obj.SetY(obj.GetY() - player1.GetHeight() - MmgHelper.ScaleValue(10));

                player1.isMoving = false;
                player1.isAttacking = false;
                player1.isPushStart = false;
                player1.isPushing = false;
                player1.SetPosition(obj.GetPosition().Clone());
                player1HealthBar.RestoreAllHealth();
                player1.SetHealthToMax();
                playersAliveCount = 1;
            }

            if (gameType == GameType.GAME_TWO_PLAYER)
            {
                MmgObj obj = new MmgObj(player2.GetWidth(), player2.GetHeight());
                MmgHelper.CenterHorAndVert(obj);
                obj.SetX(obj.GetX() - (GAME_WIDTH - BOARD_WIDTH) / 2 + obj.GetWidth());
                obj.SetY(obj.GetY() + player2.GetHeight() + MmgHelper.ScaleValue(20));

                player2.isMoving = false;
                player2.isAttacking = false;
                player2.isPushStart = false;
                player2.isPushing = false;
                player2.SetPosition(obj.GetPosition().Clone());
                player2HealthBar.RestoreAllHealth();
                player2.SetHealthToMax();
                playersAliveCount = 2;
            }
        }

        /// <summary>
        /// Gets the position of the player1 character.
        /// </summary>
        /// <returns>The position of the player1 character.</returns>
        public MmgVector2 GetPlayer1Pos()
        {
            if (player1 != null)
            {
                return player1.GetPosition();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a boolean indicating if the player1 character is broken.
        /// </summary>
        /// <returns>A boolean indicating if the player1 character is broken.</returns>
        public bool GetPlayer1Broken()
        {
            if (player1 != null)
            {
                return player1.GetIsBroken();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the position of the player2 character.
        /// </summary>
        /// <returns>The position of the player2 character.</returns>
        public MmgVector2 GetPlayer2Pos()
        {
            if (player2 != null)
            {
                return player2.GetPosition();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a boolean indicating if the player2 character is broken.
        /// </summary>
        /// <returns>A boolean indicating if the player2 character is broken.</returns>
        public bool GetPlayer2Broken()
        {
            if (player2 != null)
            {
                return player2.GetIsBroken();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Updates the current enemy wave with the number of milliseconds since the last frame.
        /// </summary>
        /// <param name="msSinceLastFrame"></param>
        private void UpdateCurrentEnemyWave(long msSinceLastFrame)
        {
            if (wavesCurrent != null)
            {
                wavesCurrent.timeIntervalMs += msSinceLastFrame;
                wavesCurrent.timeCurrentMs += msSinceLastFrame;

                if (wavesCurrent.timeIntervalMs >= wavesCurrent.intervalBetweenEnemiesMs && wavesCurrent.actEnemyCount < wavesCurrent.enemyCount)
                {
                    wavesCurrent.timeIntervalMs = 0;
                    int len = wavesCurrent.actAtOneTime;
                    int dLen = wavesCurrent.activeDoors.Length;
                    int eLen = 8;
                    int dIdx = 0;
                    int eIdx = 0;
                    MdtBase coll = null;
                    MdtCharInter emn = null;
                    MdtDoorType door;
                    int adjW = enemyDemonFrames.GetWidth() + MmgHelper.ScaleValue(12);
                    int adjH = enemyDemonFrames.GetHeight() + MmgHelper.ScaleValue(12);
                    bool found = false;

                    for (int i = 0; i < len; i++)
                    {
                        dIdx = MmgHelper.GetRandomIntRange(0, dLen);
                        found = false;
                        door = wavesCurrent.activeDoors[dIdx];

                        eIdx = MmgHelper.GetRandomIntRange(0, eLen);
                        if (eIdx == 7)
                        {
                            emn = new MdtCharInterBanshee(enemyBansheeFrames.CloneTyped(), 0, 3, 12, 15, 4, 7, 8, 11, this);
                        }
                        else if (eIdx % 2 == 0 || eIdx == 3)
                        {
                            emn = new MdtCharInterDemon(enemyDemonFrames.CloneTyped(), 0, 3, 12, 15, 4, 7, 8, 11, this);
                        }
                        else if (eIdx % 2 == 1 || eIdx == 5)
                        {
                            emn = new MdtCharInterWarlock(enemyWarlockFrames.CloneTyped(), 0, 3, 12, 15, 4, 7, 8, 11, this);
                        }
                        else
                        {
                            emn = new MdtCharInterBanshee(enemyBansheeFrames.CloneTyped(), 0, 3, 12, 15, 4, 7, 8, 11, this);
                        }

                        if (door == MdtDoorType.TOP_LEFT)
                        {
                            emn.SetDir(MmgDir.DIR_FRONT);
                            emn.SetPosition(doorTopLeftOpened.GetPosition().Clone());
                            emn.SetY(emn.GetY() + adjH);

                            coll = CanMove(emn.GetRect(), emn);
                            if (coll == null)
                            {
                                found = true;
                            }
                            else
                            {
                                emn.SetX(emn.GetX() + adjW);
                                coll = CanMove(emn.GetRect(), emn);
                                if (coll == null)
                                {
                                    found = true;
                                }
                                else
                                {
                                    emn.SetX(emn.GetX() - adjW - adjW);
                                    coll = CanMove(emn.GetRect(), emn);
                                    if (coll == null)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        emn.SetX(emn.GetX() + adjW);
                                        emn.SetY(emn.GetY() + adjH);
                                        coll = CanMove(emn.GetRect(), emn);
                                        if (coll == null)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (door == MdtDoorType.TOP_RIGHT)
                        {
                            emn.SetDir(MmgDir.DIR_FRONT);
                            emn.SetPosition(doorTopRightOpened.GetPosition().Clone());
                            emn.SetY(emn.GetY() + adjH);

                            coll = CanMove(emn.GetRect(), emn);
                            if (coll == null)
                            {
                                found = true;
                            }
                            else
                            {
                                emn.SetX(emn.GetX() + adjW);
                                coll = CanMove(emn.GetRect(), emn);
                                if (coll == null)
                                {
                                    found = true;
                                }
                                else
                                {
                                    emn.SetX(emn.GetX() - adjW - adjW);
                                    coll = CanMove(emn.GetRect(), emn);
                                    if (coll == null)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        emn.SetX(emn.GetX() + adjW);
                                        emn.SetY(emn.GetY() + adjH);
                                        coll = CanMove(emn.GetRect(), emn);
                                        if (coll == null)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (door == MdtDoorType.LEFT)
                        {
                            emn.SetDir(MmgDir.DIR_RIGHT);
                            emn.SetPosition(doorLeftLockIcon.GetPosition().Clone());
                            emn.SetX(emn.GetX() + adjW);

                            coll = CanMove(emn.GetRect(), emn);
                            if (coll == null)
                            {
                                found = true;
                            }
                            else
                            {
                                emn.SetY(emn.GetY() + adjH);
                                coll = CanMove(emn.GetRect(), emn);
                                if (coll == null)
                                {
                                    found = true;
                                }
                                else
                                {
                                    emn.SetY(emn.GetY() - adjH - adjH);
                                    coll = CanMove(emn.GetRect(), emn);
                                    if (coll == null)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        emn.SetX(emn.GetX() - adjW);
                                        emn.SetY(emn.GetY() - adjH);
                                        coll = CanMove(emn.GetRect(), emn);
                                        if (coll == null)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (door == MdtDoorType.BOTTOM_LEFT)
                        {
                            emn.SetDir(MmgDir.DIR_BACK);
                            emn.SetPosition(doorBotLeftLockIcon.GetPosition().Clone());
                            emn.SetY(emn.GetY() - adjH);

                            coll = CanMove(emn.GetRect(), emn);
                            if (coll == null)
                            {
                                found = true;
                            }
                            else
                            {
                                emn.SetX(emn.GetX() + adjW);
                                coll = CanMove(emn.GetRect(), emn);
                                if (coll == null)
                                {
                                    found = true;
                                }
                                else
                                {
                                    emn.SetX(emn.GetX() - adjW - adjW);
                                    coll = CanMove(emn.GetRect(), emn);
                                    if (coll == null)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        emn.SetX(emn.GetX() - adjW);
                                        emn.SetY(emn.GetY() - adjH);
                                        coll = CanMove(emn.GetRect(), emn);
                                        if (coll == null)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (door == MdtDoorType.BOTTOM_RIGHT)
                        {
                            emn.SetDir(MmgDir.DIR_BACK);
                            emn.SetPosition(doorBotRightLockIcon.GetPosition().Clone());
                            emn.SetY(emn.GetY() - adjH);

                            coll = CanMove(emn.GetRect(), emn);
                            if (coll == null)
                            {
                                found = true;
                            }
                            else
                            {
                                emn.SetX(emn.GetX() + adjW);
                                coll = CanMove(emn.GetRect(), emn);
                                if (coll == null)
                                {
                                    found = true;
                                }
                                else
                                {
                                    emn.SetX(emn.GetX() - adjW - adjW);
                                    coll = CanMove(emn.GetRect(), emn);
                                    if (coll == null)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        emn.SetX(emn.GetX() - adjW);
                                        emn.SetY(emn.GetY() - adjH);
                                        coll = CanMove(emn.GetRect(), emn);
                                        if (coll == null)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (door == MdtDoorType.RIGHT)
                        {
                            emn.SetDir(MmgDir.DIR_LEFT);
                            emn.SetPosition(doorRightLockIcon.GetPosition().Clone());
                            emn.SetX(emn.GetX() - adjW);

                            coll = CanMove(emn.GetRect(), emn);
                            if (coll == null)
                            {
                                found = true;
                            }
                            else
                            {
                                emn.SetY(emn.GetY() + adjH);
                                coll = CanMove(emn.GetRect(), emn);
                                if (coll == null)
                                {
                                    found = true;
                                }
                                else
                                {
                                    emn.SetY(emn.GetY() - adjH - adjH);
                                    coll = CanMove(emn.GetRect(), emn);
                                    if (coll == null)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        emn.SetX(emn.GetX() - adjW);
                                        emn.SetY(emn.GetY() - adjH);
                                        coll = CanMove(emn.GetRect(), emn);
                                        if (coll == null)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (found)
                        {
                            wavesCurrent.actEnemyCount++;
                            gameEnemies.Add(emn);
                            emn.SetMotor(MdtEnemyMotorType.MOVE_Y_THEN_X);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Starts a new enemy wave.
        /// </summary>
        /// <param name="waveIdx">The enemy wave to start.</param>
        private void UpdateStartEnemyWave(int waveIdx)
        {
            wavesCurrentIdx = waveIdx;
            wavesCurrent = waves[wavesCurrentIdx];
            wavesCurrent.timeStartMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            wavesCurrent.timeCurrentMs = 0;
            wavesCurrent.timeIntervalMs = 0;
            wavesCurrent.actEnemyCount = 0;
            txtLevel.SetText("Level: " + FormatLevel(wavesCurrentIdx));

            UpdateLockAllDoors();
            if (wavesCurrent.activeDoors != null)
            {
                int i = 0;
                int len = wavesCurrent.activeDoors.Length;
                for (; i < len; i++)
                {
                    UpdateUnlockDoor(wavesCurrent.activeDoors[i]);
                }
            }

            UpdateClearObjects();
            UpdateGenerateObjects(wavesCurrent.actObjCount, wavesCurrent.activeItems);
        }

        /// <summary>
        /// A placeholder method for adding randomly generated chests to the board.
        /// </summary>
        /// <param name="cnt">The number of chests to generate.</param>
        /// <param name="items">The items available to find in the chests.</param>
        /// <param name="weapons">The weapons available to find in the chests.</param>
        private void UpdateGenerateChests(int cnt, MdtItemType[] items, MdtWeaponType[] weapons)
        {
            for (int i = 0; i < cnt; i++)
            {
                int tmp = MmgHelper.GetRandomInt(11);
                int idx = 0;
                //TODO: Finish this method.

                if (tmp % 2 == 0)
                {
                    //items
                    idx = MmgHelper.GetRandomInt(items.Length);

                }
                else
                {
                    //weapons
                    idx = MmgHelper.GetRandomInt(weapons.Length);

                }
            }
        }


    }
}