import BertecRemoteControl
from time import sleep

# This is a demo script to use as an example of how to start a connection to the Bertec
# Treadmill Sof6tware, and send commands to operate your hardware.

# Set up RemoteControl object, then use the start_connection method to set up the 
# network parameters and then call init_connect on the server
remote = BertecRemoteControl.RemoteControl()
res = remote.start_connection()
print(res)

# Check to see if we got any initial resposne from the server. If a connection could not be 
# made, None will be return. If the RPC init_connect request failed, the code will return the 
# corresponding error code (and not 1)
if (res is not None and res['code'] == 1):
    print("The following commands are supported:\n")
    print("1. run_treadmill\n2. run_incline")
    print("3. is_treadmill_moving\n4. is_incline_moving\n5. is_client_authenticated")
    print("6. get_force_data\n\nInput 0 to exit program")

    command = 1
    res = ' '

    while (command):
        command = input("Which command do you wish to use: ")
        if (command == '1'):
            print("Using run_treadmill")
            params = remote.get_run_treadmill_user_input()
            res = remote.run_treadmill(params['leftVel'], params['leftAccel'], params['leftDecel'], params['rightVel'], params['rightAccel'], params['rightDecel'])
        elif (command == '2'):
            print("Using run_incline")
            params = remote.get_run_incline_user_input()
            res = remote.run_incline(params['inclineAngle'])     
        elif (command == '3'):
            print("Using is_treadmill_moving")
            res = remote.is_treadmill_moving()
        elif (command == '4'):
            print("Using is_incline_moving")
            res = remote.is_incline_moving()
        elif (command == '5'):
            print("Using is_client_authenticated")
            res = remote.is_client_authenticated()
        elif (command == '6'):
            print("Using get_force_data")
            res = remote.get_force_data()
        else:
            print("Exiting program")
            remote.stop_connection()
            command = 0
            break

        if (command != '6'):
            print("Result code: " + str(res['code']) + " - Message: " + res['message'])
        else:
            print(str(res))

remote.stop_connection()

