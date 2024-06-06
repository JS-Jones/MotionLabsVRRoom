import sys

def main(args):
    if len(args) < 1:
        print("No command provided")
        return

    command = args[0]

    if command == "print_string":
        if len(args) < 2:
            print("Python: No string provided to print")
        else:
            print("Python: The string is:", args[1])
    elif command == "increase":
        print("Python: Increase")
    elif command == "decrease":
        print("Python: Decrease")
    else:
        print("Python: Unknown command:", command)

if __name__ == "__main__":
    main(sys.argv[1:])
