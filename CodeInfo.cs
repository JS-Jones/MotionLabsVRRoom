/* README - Motion Labs VR Room
 * Update: 8 / 1 / 2024
 * Author: Joshua Jones(jjo108 @u.rochester.edu)
 * Game Credits: Made by URMC Motion Labs: Mila Paymukhina(UR'24), Joshua Jones (UR'25)

 *------------------------------------------------------------------------------------------------------------------------------
 * SUMMARY
 * ------------------------------------------------------------------------------------------------------------------------------

 *The VR Room is an immersive environment that promotoes gamification in the physical rehabilitation process. By incorporating
 * Motion Capture Cameras for hand tracking and Force Plate Treadmills for walking tasks, we were able to create an adaptive game
 * that challenges subjects through a series of task. Feedback from physcians, therapists, and researchers mention how this could 
 * be used as an early recovery tool for athletes in both upper and lower extremities (Most athletes don't avoid pressure after 
 * an injury, hence being an appealing alternative to still maintain reaction time or motion). This could also be a great tool to 
 * test proprioceptive dysfunction (when a patient thinks they have full extension, but really doesn't). The game so far only 
 * considers a catching task for baseball, but can be expanded to different sports like lacrosse, football, etc. The game itself 
 * is not a form of Virtual Reality (more like mixed reality), but the goal is to eventually translate the game into those forms.
 * 
 * ------------------------------------------------------------------------------------------------------------------------------
 * GAME MECHANICS / GETTING STARTED
 * ------------------------------------------------------------------------------------------------------------------------------

 * Game Options:
 * - Hand Choice: Both, Left, Right - determines which hand tracking is active
 * - Trial Duration: Total Time Trial in Seconds (Has a lower limit based on throwing speed, target count, time between throw)
 * - Walking Speed: How fast the bertec treadmill is moving in Meters/Seconds
 * - Throwing Speed: Slow, Medium, Fast
 * - Gamemode Type: Fixed Postions(Targets will appear), Free Moving(Operator can control baseball, see below)
 * - Target Count: 24, 38, 52 - Number of baseball that will be thrown and potential locations
 * - Show Markers: True, False - Whether a target shows where the baseball is heading (easier to localize)
 * - Repeat Markers: False, True - Determines whether the baseball can repeat a location thrown, or go to each target once
 * - Time Between Throws: Auto calculated number that considers throwing speed, target count, and time between throw
 * - Left to Right Weighting: Ratio of left to right targets that appear
 * - Timer Type: The type of clock that appears
  
 * Nexus (Hand Tracking) 
 * - Make sure that the Vicon Nexus switch (Cameras) is turned on and open the Nexus App.Make sure that the Subjects are set and
 * moving (left/right hand tracking). If it isn't, subjects can be found in this location: 
 * C:/ Users / Public / Documents / Nexus Unity Testing/Pilot/HandPrototype/New Session
 
 * Bertec (Treadmill) Integration:
 * - Make sure the Bertec (treadmill) switch is turned on and open the Bertec App.Click on the 'Enable' button in the Emergency
 * Stop switch once it starts blinking. In The Bertec app, Go to Settings and change units to 'Metric'. Then enable 'Remote 
 * Control' at the bottom right of the app. When starting or stopping the treadmill, a safety pop up screen will appear which you
 * need to just press 'confirm'. The treadmill will start moving afterwards
  
 * Calibration:
 * - This is a process that occurs right after the operator chooses the conditions and used to adjust the radius of the targets,
 * height of the camera, and position of baseball throws.The sample targets that show after stretching out hands can be dragged
 * around for further customization.
  
 * Warmup:
 * - For 20 seconds, baseballs will be thrown, but no catch or miss will be counted towards the final results.This is reflected in
 * the clock and the Message at the top of the screen. This is followed by the acual trial session duration as seen in Game options
 
 * Pausing:
 * - At any time, the operator can click the 'space bar' button to pause the game.
 
 * Sound:
 * - There is envrionment noises in the background which can be heard by adjusting the volume dial in the display panel IRL.
 * Sounds also play when ever the baseball is caught (with a slight color change also). Sound is a great way to induce a 
 * distractor for patients and also clarify if the baseball was caught.

 * Free Moving: BROKEN AT THE MOMENT
 * - Operator can contol the baseball using the arrow keys.Use the 'q' and 'w' keys to move camera up and down. Press 'Enter' to
 * release the baseball to be caught.
 
 * Results:
 * - The report depicts 2 graphs and information from the trial split between left and right sides. Range of Motion Graph depicts
 * the highest angle from the center of the subject on each side. The Positional Graph shows where the targets were thrown. Both 
 * show caught and missed targets. Accuracy, Reaction Time, Angular Velocity (Degrees/Second), Walking Speed, Average Miss Angle. 
 * A new game with new settings can be selected, or to just replay with the same settings. The results are cleared each round. 
 
 * ------------------------------------------------------------------------------------------------------------------------------
 * CODING
 * ------------------------------------------------------------------------------------------------------------------------------
 * Core codes has ***
 
 * MenuController: ***
 * - Handles detecting when a new option is selected in the menu and then passing that information to the playerprefs unity func
 * - When the play button is selected, the main scene is then loaded into the scene.
 
 * SliderStep:
 * - Code to handle how the slider moves for the node weighting and adjusting the UI corresppondingly. Rounded to .1 step.
 
 * Calibration: ***
 * - First code to run when entering the main Scene. It has 6 phases:
        // -1 = "Calibrating, Press Enter to Continue"
        // 0 = "reach out your hands as straight as possible"
        // 1 = "confirm nodes" --> Intializes height and radius. adjusts the camera height and stores the center values for angle
                calculation.
        // 2 = "stretch out your hands as high as possible" --> Removes nodes from previous method
        // 3 = "confirm nodes" --> Intializes maxHeightLeftHand and Right hand 
        // 4 = "confirm calculated targets" --> removes nodes from previous method and displays all potential targets color
                coded based on difficulty.A range of motion intensity circle is show for easy, medium, and hard to help the 
                therapist better adjust the nodes to their liking.
        // 5 = "Click Enter to begin" --> removes Displayed targets
        // 6 --> begin game
 * - It also sets a playerprefs value for moving the nodes together or seperately
 * - This is where most of the target size, position, and making the game playable by everyone (regardless of e.g.height) should
 *   take place

 * General Note: Node Organization
 * - All Nodes are stored in the parent object called "Nodes". This is seperated by easy medium and hard which is based on user
 *   selection for target count. Each of these has a set of GameObjects each with a left and right node. The left and right nodes
 *   are colored different which are to correspond to the colors on the subjects glove. This form of organization helps with the
 *   dragging function of the nodes during calibration and also just pairing it up with its partner on the other side. 
 

 * DragObject:
 * - Remains in each of the target nodes and allows it to be dragged around the screen with its pair during calibration.
 
 * BaseballMove: ***
 * - The main crutch of the whole game. Handles throwing the ball and recording data to report. This is where most of the baseball
 *   interactions take place such as hitting the wall in the back or the glove. The weighting for the ball locations and how the
 *   node organization is implement are also located here. This is where different gamemodes are determined and processed. Gloves
 *   change color when baseball is caught and sound also comes from this code. Show markers are also determined here.
 * - Records:
        leftAngles 
        missedLeftAngles 
        rightAngles 
        missedRightAngles 
        hitPositions 
        missHitPositions 
        leftCatches
        leftThrows
        rightCatches
        rightThrows
        totalThrows
 
 * AdjustHeight:
 * - Allows for adjustment of camera up and down using Q and W. Mainly used for Free Moving Gamemode. Used in Main Camera 
 *   GameObject
 
 * AutoDeactivator:
 * - Sometimes the baseball target doesn't disappear even if another baseball is thrown. As a backup, each of the targets has this
 *   code to deactivate it after 5 seconds of being set active.
 
 * ReactionTime: 
 * - Located in each of the baseball targets and activated when the target is enabled. This causes the start time for the reaction
 *   time to be recorded. After the target is touched by the glove, then the reaction time ends by passing it to the baseballmove
 *   method and adding it to the list in reactiontimecontroller codes. Angular velocity is calculated by finding the starting 
 *   angle and subtracting it by the ending angle of the glove. Limits are set in place to avoid crazy values calculated.
 
 * ReactionTimeController:
 * - Located in the SpeedCode GameObject. Provides the methods of AddTime and AddAV for the ReactionTime code above. 
 * - Records:
        reactionTimesLeft
        reactionTimesRight
        angularVelocityLeft
        angularVelocityRight

 * SpeedAdjuster:
 * - This code changes how fast the background is moving. Ideally, this information should also match with the speed of the
 *   treadmill, but its challenging because of the security conditions to check whenever changing the treadmill speed. there is
 *   a GUI in the Canvas with the "SpeedValue", "Plus", and "Minus" used to change the background speed.
 
 * DestroySection:
 * - The core part of this code may want to be changed. This Destroys one of the ground/forest settings once it reaches the brick
 *   wall in the back. Instead, it might be worthwile to move the object to the start instead of destroying and recreating.

 * EndlessSceneGeneration:
 * - This code goes with the DestorySection code. This checks if a gameobject has the section tag and if it reaches a certain point
 *   then create a new section randomly based on the 3 different sections available.
 
 * EnvironmentNoise:
 * - This is where the environment noises come from. Change the audiosource to adjust the sounds being played.
 
 * FlashingColor:
 * - Solely to Flash the main message text when transitioning from warmup session to the actual test. Activated by Timer code.
 
 * Timer: ***
 * - The core game timing mechancis. Allows you to start, stop, and flash colors on the main message. this is where the buffer 
 *   phase and test phase are declared and referenced from in other codes (to determine if speed/angles should be recorded). code
 *   to also restasrt the timer is also located here, referenced in the QuitGame Code
 
 * Move:
 * - Code to change how fast the baseball is moving when thrown (S to increease, press A to decrease). Mainly used in free mvoing
 *   gamemode.
 
 * PauseMenu:
 * - Brings up the Pause Panel if the spacebar button is clicked at anytime. May want to add more regulations to this code 
 *   throughout other codes.
 
 * StartController:
 * - Using the PlayerPrefs of LeftActive and RightActive form GameMenu, the left and right baseball glove is set active or not.

 * ScaleChange:
 * - No real application. Used in scaling glove maybe.
 
 * QuitGame:
 * - Applied in the the Report GUi and gives the option to go back to the main options screen to change the parameters or to 
 *   replay the round with the same settings. If the replay is selected, all the values are cleared out to reset the reults.
 
 * ReportCode: ***
 * - The results takes the values from the reactionTime and BaseballMove to plot the 2 graphs and change the ui text of the
 *   report. The average/max are calculate for most and reported for the left, right, and total sides. A template circle dot is
 *   created so that whenver plotting occurs, that image is displayed on teh graphs.
 
 * Vicon Hand Tracking Codes: ***
 * - Subjects for Nexus found in C:/Users/Public/Documents/Nexus Unity Testing/Pilot/HandPrototype/New Session
 * ^ use this in case the subjects are reset in Vicon for whatever reason. the hands shoudl pop in again
 * - Reach out to Vicon for any complex help on this. 
 * - Code located in ViconDataStreamPrefab gameobject and also in each of the player gloves called RBScript
    * // Input data is in Vicon co-ordinate space; z-up, x-forward, rhs.
        // We need it in Unity space, y-up, z-forward lhs
        //           Vicon Unity
        // forward    x     z
        // up         z     y
        // right     -y     x
        // See https://gamedev.stackexchange.com/questions/157946/converting-a-quaternion-in-a-right-to-left-handed-coordinate-system

 
 * Bertec Treadmill Codes: *** ENSURE THAT REMOTE CONTROL IS SET ACTIVE ON BERTEC
 * - PythonTrigger: contains a starter, stopper and an emergency brake whenever space is clicked to pause screen. Passes info
 *   to the RunPythonScript.
 * - RunPythonScript: After the python trigger calls this code, it intiallize it as a stop or move and passes it to scriptv2.py.
 *   contains locations of the python and scriptv2
 * - The folllowing are located in the folder: MotionLabsGameV4 --> PythonUnity 
     * - scriptv2.py: First connect to the remote control and start the connection whcih will lead to the popup whcih you have to
     *   press confrim. So keep looping until this is  clicked. then moving at acceleration of .1, change to the speed passed in.
     * - python_client_demo.py: This is a demo provided by Bertec on how to remotely connect the system and get components.
     * - BertecRemoteControl.py: DO NOT CHANGE ANYTHING HERE AT ANY TIME UNLESS YOU KNOW WHAT YOU ARE DOING. This is how the code
     *   speaks to the treadmill and passes information accordingly. 
     *   BACKUPS TO THE ABOVE: 
         * Right click on the treadmill control program's desktop icon
         * Select open fil location
         * scroll until you find the remotecontrolexample folder
         * open the folder and the python folder
 * - TO DO: Aim to avoid the secuirty confrimation everysingle time. Figure out a way to avoid having to remotely connect and
 *   start connection every time. Ideally, do this once and then just keep changing the speed as seen in the demo.



 */
