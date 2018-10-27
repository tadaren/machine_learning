using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyQLProblems;

namespace QL{
    class Program{
        static void Main(string[] args){
            double alpha = 0.7;     // 学習率
            double ganma = 0.8;     // 割引率

            State[] states = new State[10];     // 状態数
            for(var i = 0; i < states.Length; i++){
                states[i] = new State();
            }

            for(var i = 0; i < 1000; i++){
                MyQLProblems.Problem1.initState();  // 状態の初期化
                int count = 1;  // 行動数
                while(true){
                    int s = MyQLProblems.Problem1.getState();   // 現在の状態

                    int action = states[s].selectAction();      // 行動を決める
                    double r = MyQLProblems.Problem1.doAction(action);      // 行動し報酬を得る
                    Console.WriteLine(r);
                    
                    int ss = MyQLProblems.Problem1.getState();      // 次の状態を取得する
                    states[s].update(action, alpha, ganma, r, states[ss]);  // 状態の配列を更新する

                    // ゴールにたどり着いたら終了
                    if(MyQLProblems.Problem1.getStateIsGoal()){
                        // Console.WriteLine(count+" "+r);     // 行動数と報酬を出力
                        Console.WriteLine();
                        break;
                    }
                    count++;
                }
            }

            //最終エピソード
            MyQLProblems.Problem1.initState();

            while(true){
                int s = MyQLProblems.Problem1.getState();
 
                int action = states[s].selectAction();
                double r = MyQLProblems.Problem1.doAction(action);
                int ss = MyQLProblems.Problem1.getState();
                // Console.WriteLine(r+" "+action+" "+ss);
                states[s].update(action, alpha, ganma, r, states[ss]);
                
                if(MyQLProblems.Problem1.getStateIsGoal()){
                    // Console.WriteLine("GOAL!!!");
                    break;
                }
            }
            // Console.WriteLine();
            // foreach(var e in states){
            //     Console.WriteLine(e);
            // }
        }
    }
    
    // 状態を表すクラス
    class State{
        double[] actions = new double[3];
        static Random rand = new Random();

        double epsilon = 0.99;      // εグリーディ法

        public int selectAction(){
            epsilon -= 0.001;
            double a = rand.NextDouble();
            if(a < epsilon){
                return rand.Next(3);
            }
            int index = 0;
            double max = 0;
            for(var i = 0; i < 3; i++){
                if(max < actions[i]){
                    max = actions[i];
                    index = i;
                }
            }
            return index;
        }

        public void update(int action, double alpha, double ganma, double r, State nextState){
            // Console.WriteLine("Updated");
            // Console.WriteLine(action+" "+alpha+" "+ganma+" "+r);
            actions[action] = (1-alpha)*actions[action] + alpha*(r + ganma*nextState.max());
        }

        double max(){
            double max = 0;
            foreach (var e in actions){
                if(max < e){
                    max = e;
                }
            }
            return max;
        }

        public override string ToString(){
            return string.Join(" ", actions);
        }
    }
}

