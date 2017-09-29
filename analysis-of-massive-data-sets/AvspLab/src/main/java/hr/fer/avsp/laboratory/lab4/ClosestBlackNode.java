package hr.fer.avsp.laboratory.lab4;

import java.io.*;
import java.text.DecimalFormat;
import java.util.*;


public class ClosestBlackNode {

    public static void main(String[] args) throws Exception {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

        String[] line = br.readLine().split(" ");

        int n = Integer.parseInt(line[0]);
        int e = Integer.parseInt(line[1]);

        boolean[] isBlack = new boolean[n];
        int[] closestBlackIndex = new int[n];
        int[] closestBlackDistance = new int[n];

        for (int i = 0; i < n; i++) {
            int t = Integer.parseInt(br.readLine());
            if (t == 1) {
                isBlack[i] = true;
            } else {
                isBlack[i] = false;
            }
            closestBlackIndex[i] = -1;
            closestBlackDistance[i] = -1;

        }

        Map<Integer, List<Integer>> sourceDestinations = new HashMap<>();

        for (int i = 0; i < n; i++) {
            ArrayList<Integer> list = new ArrayList<>();
            sourceDestinations.put(i, list);
        }

        for (int i = 0; i < e; i++) {
            line = br.readLine().split(" ");
            int s = Integer.parseInt(line[0]);
            int d = Integer.parseInt(line[1]);

            sourceDestinations.get(s).add(d);
            sourceDestinations.get(d).add(s);
        }

        Set<Integer> visitedNodes = new HashSet<>();
        Queue<Integer> openedNodes = new LinkedList<>();

        //setting blacks and add to queue
        for (int i = 0; i < n; i++) {
            if (isBlack[i]) {
                openedNodes.add(i);
                closestBlackIndex[i] = i;
                closestBlackDistance[i] = 0;
            }
        }

        while (!openedNodes.isEmpty()) {
            int currentNode = openedNodes.poll();
            visitedNodes.add(currentNode);

            for (int i : sourceDestinations.get(currentNode)) {
                if (closestBlackDistance[i] == -1) {
                    closestBlackDistance[i] = closestBlackDistance[currentNode] + 1;
                }
                if (closestBlackIndex[i] == -1) {
                    closestBlackIndex[i] = closestBlackIndex[currentNode];
                }
                if (!visitedNodes.contains(i)) {
                    openedNodes.add(i);
                }
            }
        }

        for (int i = 0; i < n; i++) {
            String out;
            if (closestBlackDistance[i] > 10) {
                out = "-1 -1";
            } else {
                out = closestBlackIndex[i] + " " + closestBlackDistance[i];
            }
            System.out.println(out);
        }

        br.close();
    }

}
