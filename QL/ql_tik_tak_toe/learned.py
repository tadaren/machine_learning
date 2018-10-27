from main import Game, select_action
import sys

states = {}

with open(sys.argv[1], "r") as f:
    line = f.readline()
    while line:
        data = line[:-1].split("#")
        states[eval(data[0])] = eval(data[1])
        line = f.readline()

game = Game()

while 1:
    state = game.get_state()
    if not state in states.keys():
        states[state] = {}
        for i in game.get_actions():
            states[state][i] = 0.0
    action = max(states[state], key=states[state].get)
    # print(states[state])
    game.do_action(action, 1)
    next_state = game.get_state()

    if game.is_end() != -1:
        game.print()
        print(f"game end {game.is_end()}")
        break

    actions = game.get_actions()
    game.print()
    if actions:
        print(actions)
        game.do_action(int(input()), 2)

    result = game.is_end()

    if result != -1:
        game.print()
        print(f"game end {result}")
        break

