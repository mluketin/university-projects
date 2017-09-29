package hr.fer.avsp.laboratory.lab4;

import java.io.*;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


public class NodeRank {

    public static void main(String[] args) throws Exception {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        String[] line = br.readLine().split(" ");

        int n = Integer.parseInt(line[0]);
        double beta = Double.parseDouble(line[1]);

        HashMap<Integer, List<Integer>> inputLinksMap = new HashMap<>();
        int[] numberOfOutLinks = new int[n];

        for(int i = 0; i < n; i++){
            line = br.readLine().split(" ");
            numberOfOutLinks[i] = line.length;

            for(int j = 0; j < numberOfOutLinks[i]; j++){
                int nodeJ = Integer.parseInt(line[j]);

                if(inputLinksMap.containsKey(nodeJ)){
                    inputLinksMap.get(nodeJ).add(i); //for j-th node add i-th in list
                } else {
                    List<Integer> newList = new ArrayList<>();
                    newList.add(i);
                    inputLinksMap.put(nodeJ, newList);
                }
            }
        }

        double[][] rank = new double[101][n];

        //0-th iteration => init all nodes to have rank 1/n
        for(int i=0; i<n; i++){
            rank[0][i] = 1.0/n;
        }

        int t_max = 100;
        for(int k = 0; k < t_max; k++){
            double S = 0;

            //foreach node that exists in map we calculate rank in k-th iteration
            for(int i : inputLinksMap.keySet()){
                double sum = 0;
                for(int j: inputLinksMap.get(i)){
                    sum += beta * (rank[k][j] / numberOfOutLinks[j]);
                }
                rank[k+1][i] = sum;
                S += rank[k+1][i];
            }
            for (int i = 0; i < n; i++){
                rank[k+1][i] += (1-S)/n;
            }
        }

        DecimalFormat df2 = new DecimalFormat( "0.0000000000");
        int q = Integer.parseInt(br.readLine());
        for(int i = 0; i < q; i++){
            line = br.readLine().split(" ");

            int index = Integer.parseInt(line[0]);
            int t = Integer.parseInt(line[1]);
            String out = df2.format(rank[t][index]).replace(',', '.');
            System.out.println(out);
        }

        br.close();
    }
}
