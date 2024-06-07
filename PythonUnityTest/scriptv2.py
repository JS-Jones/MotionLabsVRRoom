import BertecRemoteControl
import sys
from time import sleep

global remote
remote = BertecRemoteControl.RemoteControl()
global res
res = remote.start_connection()

def main(args, res):

    while True:
        if remote.is_client_authenticated()['result'] == "True":
    
            if (res is not None and res['code'] == 1):
                command = args[0] if len(args) > 0 else None

                if command == "Move":
                    if len(args) < 2:
                        print("No value provided for Move command")
                    else:
                        value = float(args[1])

                        res = remote.run_treadmill(value, 1, 1, value, 1, 1)
                        print(f"Move command executed with value {value}, response: {res}")
                        
                elif command == "Stop":
                    res = remote.run_treadmill(0, 1, 1, 0, 1, 1)
                    print("Stop command executed, response:", res)
                    
                else:
                    print(f"Unknown command: {command}")
                    

                remote.stop_connection()
                break

if __name__ == "__main__":
    #main(sys.argv[1:])
    main(sys.argv[1:], res)
