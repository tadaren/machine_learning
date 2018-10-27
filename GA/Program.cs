using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGAEnvironments;

namespace GA{
    class Program{
        static void Main(string[] args){
            // int seed = 1000;
            int l = 10;
            int N = 5;
            int G = 10000;
            double mutationRate = 0.01;
            Kotai[] hoge = new Kotai[N];

            // Random rand = new Random(seed);
            Random rand = new Random();
            for (var i = 0; i < hoge.Length; i++){
                hoge[i] = new Kotai(l, rand);
            }

            for (var i = 0; i < G; i++){
                for (var j = 0; j < N; j++){
                    double adaption = hoge[j].fitness();
                }
                foreach(var e in hoge){
                    Console.WriteLine(e);
                }
                Console.WriteLine();
                Array.Sort(hoge);
                if(hoge[hoge.Length-1].Tekiodo == 100.0){
                    Console.WriteLine(i+1);
                    break;
                }
                // foreach (var e in hoge){
                //     Console.WriteLine(e);
                // }
                Kotai maxAdaptionKotai = hoge[hoge.Length-1];
                Kotai[] tmp = new Kotai[hoge.Length-1];
                Array.Copy(hoge, tmp, tmp.Length);

                int choicedIndex = choice(tmp, rand);
                Kotai[] children1 = Kotai.cross(maxAdaptionKotai, tmp[choicedIndex], rand);

                // foreach (var e in tmp)
                // {
                //     Console.WriteLine(e);
                // }
                // Console.WriteLine();
                Kotai[] tmp2 = new Kotai[tmp.Length-1];
                Array.Copy(tmp, tmp2, choicedIndex);
                Array.Copy(tmp, choicedIndex +1, tmp2, choicedIndex, tmp.Length-choicedIndex-1);
                // foreach (var e in tmp2)
                // {
                //     Console.WriteLine(e);
                // }
                // Console.WriteLine();
                choicedIndex = choice(tmp2, rand);
                Kotai[] children2 = Kotai.cross(maxAdaptionKotai, tmp2[choicedIndex], rand);
                Array.Copy(children1, hoge, children1.Length);
                Array.Copy(children2, 0, hoge, children1.Length, children2.Length);
                hoge[N-1] = maxAdaptionKotai;

                // 突然変異
                if(mutationRate <= rand.NextDouble()){
                    int kotaiIndex = rand.Next(N);
                    int geneIndex = rand.Next(l);
                    if(hoge[kotaiIndex].Gene[geneIndex] == '0'){
                        hoge[kotaiIndex].Gene[geneIndex] = '1';
                    }else{
                        hoge[kotaiIndex].Gene[geneIndex] = '0';
                    }

                }

            }
        }
        public static int choice(Kotai[] array, Random rand){
            double sum = 0;
            foreach (var e in array)
            {
                sum += e.Tekiodo;
            }
            double[] rate = new double[array.Length];
            for (var j = 0; j < array.Length; j++)
            {
                rate[j] = array[j].Tekiodo / sum;
            }
            int a = rand.Next(1);
            int b = 0;
            for (var s = 0.0; s < 1; b++)
            {
                s += rate[b];
                if (s > a)
                {
                    break;
                }
            }
            return b;
        }
    }



    class Kotai : IComparable{
        public char[] Gene;
        public double Tekiodo;

        public Kotai(int length, Random rand){
            Gene = new char[length];
            for (var i = 0; i < length; i++){
                Gene[i] = (char)(rand.Next(2) + '0');
            }
        }
        public Kotai(int length, char[] a, char[] b){
            Gene = new char[length];
            a.CopyTo(Gene, 0);
            b.CopyTo(Gene, a.Length);
        }

        public double fitness(){
            Tekiodo = MyGAEnvironments.Environment3.fitness(new String(Gene));
            return Tekiodo;
        }

        public int CompareTo(object obj){
            //nullより大きい
            if (obj == null){
                return 1;
            }

            if (this.GetType() != obj.GetType()){
                throw new ArgumentException("別の型とは比較できません。", "obj");
            }
            
            return this.Tekiodo.CompareTo(((Kotai)obj).Tekiodo);
        }

        public override string ToString(){
            return new String(Gene) + " " + Tekiodo;
        }

        public static Kotai[] cross(Kotai a, Kotai b, Random rand){
			int div = rand.Next(1, a.Gene.Length-1);
            char[] c = new char[div];
            char[] d = new char[a.Gene.Length-div];

            Kotai[] children = new Kotai[2];

            Array.Copy(a.Gene, c, div);
            Array.Copy(b.Gene, 0, d, 0, b.Gene.Length - div); 
            children[0] = new Kotai(a.Gene.Length, c, d);

            Array.Copy(a.Gene, 0, d, 0, a.Gene.Length - div);
            Array.Copy(b.Gene, c, div);

            children[1] = new Kotai(a.Gene.Length, c, d);

            return children;
        }
    }
}
