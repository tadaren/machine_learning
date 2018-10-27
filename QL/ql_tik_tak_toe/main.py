import random
import sys

class Game:
    board = [
            0,0,0,
            0,0,0,
            0,0,0
            ]

    def __init__(self):
        self.reset()

    def print(self):
        print(f"{translate_number_mark(self.board[0])}|{translate_number_mark(self.board[1])}|{translate_number_mark(self.board[2])}")
        print("-+-+-")
        print(f"{translate_number_mark(self.board[3])}|{translate_number_mark(self.board[4])}|{translate_number_mark(self.board[5])}")
        print("-+-+-")
        print(f"{translate_number_mark(self.board[6])}|{translate_number_mark(self.board[7])}|{translate_number_mark(self.board[8])}")

    def reset(self):
        self.board = [
            0,0,0,
            0,0,0,
            0,0,0
            ]
    
    def do_action(self, action, is_first):
        if self.board[action] != 0:
            return
        self.board[action] = is_first


    # win First = 1, win Second = 2, not end game = -1, draw = 0
    def is_end(self):
        for i in range(3):
            if self.board[3*i] == self.board[3*i+1] == self.board[3*i+2] == 1:
                return 1
            elif  self.board[3*i] == self.board[3*i+1] == self.board[3*i+2] == 2:
                return 2
        for i in range(3):
            if self.board[i] == self.board[i+3] == self.board[i+6] == 1:
                return 1
            elif self.board[i] == self.board[i+3] == self.board[i+6] == 2:
                return 2

        if self.board[0] == self.board[4] == self.board[8] == 1:
            return 1
        elif self.board[0] == self.board[4] == self.board[8] == 2:
            return 2
        if self.board[2] == self.board[4] == self.board[6] == 1:
            return 1
        elif self.board[2] == self.board[4] == self.board[6] == 2:
            return 2
        
        if sum(self.board) >= 13:
            return 0

        return -1


    def get_state(self):
        return tuple(self.board)

    def get_actions(self):
        return tuple([i for i,e in enumerate(self.board) if e == 0])


def translate_number_mark(num):
    if num == 0:
        return ' '
    elif num == 1:
        return '○'
    elif num == 2:
        return '×'

epsilon = 0.99

def select_action(actions):
    action_list = list(actions.keys())
    if epsilon > random.random():
        return random.choice(action_list)
    return max(actions, key=actions.get)

def select_action_random(actions, board):
    pattern = ((0,2,2),(2,0,2),(2,2,0))
    for e in pattern:
        for i in range(3):
            if board[3*i] == e[0] and board[3*i+1] == e[1] and board[3*i+2] == e[2]:
                return 3*i+e.index(0)
        for i in range(3):
            if board[i] == e[0] and board[i+3] == e[1] and board[i+6] == e[2]:
                return e.index(0)*3+i
        if board[0] == e[0] and board[4] == e[1] and board[8] == e[2]:
            a = (0,4,8)
            return a[e.index(0)]
        elif board[2] == e[0] and board[4] == e[1] and board[6] == e[2]:
            a = (2,4,6)
            return a[e.index(0)]
    pattern = ((0,1,1),(1,0,1),(1,1,0))
    do_list = []
    for e in pattern:
        for i in range(3):
            if board[3*i] == e[0] and board[3*i+1] == e[1] and board[3*i+2] == e[2]:
                do_list.append(3*i+e.index(0))
        for i in range(3):
            if board[i] == e[0] and board[i+3] == e[1] and board[i+6] == e[2]:
                do_list.append(e.index(0)*3+i)
        if board[0] == e[0] and board[4] == e[1] and board[8] == e[2]:
            a = (0,4,8)
            do_list.append(a[e.index(0)])
        elif board[2] == e[0] and board[4] == e[1] and board[6] == e[2]:
            a = (2,4,6)
            do_list.append(a[e.index(0)])
    if do_list:
        return random.choice(do_list)
    return random.choice(actions)

if __name__ == "__main__":
    alpha = 0.7
    ganma = 0.9

    states = {}

    game = Game()

    wc = 0
    lc = 0
    dc = 0

    for i in range(100000):
        game.reset()

        while 1:
            state = game.get_state()
            if not state in states.keys():
                states[state] = {}
                for i in game.get_actions():
                    states[state][i] = 0.0
        
            action = select_action(states[state])
            game.do_action(action, 1)

            actions = game.get_actions()
            if actions:
                game.do_action(select_action_random(actions, game.board), 2)

            next_state = game.get_state()

            result = game.is_end()
        
            r = 0
            if result == 1:
                r = 1
                wc += 1
            elif result == 2:
                r = -1
                lc += 1
            
            qp = states[state][action]
            nsq = states.get(next_state, {0:0}).values()
            states[state][action] = (1-alpha)*qp + alpha*(r+ganma*max(nsq))
            
            epsilon -= 1/100000

            if result != -1:
                # print(f"game end {result} {wc} {lc}")
                break


    game.reset()

    with open(sys.argv[1], "w") as f:
        for key in states:
            f.write(f"{key}#{states[key]}\n")

