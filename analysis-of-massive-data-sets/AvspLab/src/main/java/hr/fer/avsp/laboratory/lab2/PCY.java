package hr.fer.avsp.laboratory.lab2;

import java.io.*;
import java.security.NoSuchAlgorithmException;
import java.util.*;
//import java.util.stream.Collectors;

import javafx.util.Pair;


public class PCY {

    public static void main(String[] args) throws NoSuchAlgorithmException, IOException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

        //number of buckets
        int N = Integer.parseInt(br.readLine());

        //support
        double s = Double.parseDouble(br.readLine());
        int threshold = (int) (s * N);

        //number of buckets available to hash function
        int b = Integer.parseInt(br.readLine());

        List<int[]> buckets = new ArrayList<>();

        HashMap<Integer, Integer> numberOfItems = new HashMap<>();

        //read buckets and first passage
        for (int i = 0; i < N; i++) {
            String line = br.readLine();
            int[] bucket = stringArrayToIntArray(line.split(" "));
            buckets.add(bucket);

            for (int j = 0; j < bucket.length; j++) {
                if (numberOfItems.containsKey(bucket[j])) {
                    int val = numberOfItems.get(bucket[j]);
                    numberOfItems.put(bucket[j], val + 1);
                } else {
                    numberOfItems.put(bucket[j], 1);
                }
            }
        }

        int A = 0;
        for (int i : numberOfItems.values()) {
            if (i >= threshold) {
                A++;
            }
        }

        //second passage
        int[] compartment = new int[b];

        for (int[] bucket : buckets) {
            for (int i = 0; i < bucket.length; i++) {
                for (int j = i + 1; j < bucket.length; j++) {
                    if ((numberOfItems.get(bucket[i]) >= threshold) && (numberOfItems.get(bucket[j]) >= threshold)) {
                        int k = (bucket[i] * numberOfItems.size() + bucket[j]) % b;
                        compartment[k] += 1;
                    }
                }
            }
        }

        //third passage - coloring pairs
        //map - key of pairs [i, j], number of occurrences
        HashMap<Pair<Integer, Integer>, Integer> pairs = new HashMap<>();
        for (int[] bucket : buckets) {
            for (int i = 0; i < bucket.length; i++) {
                for (int j = i + 1; j < bucket.length; j++) {
                    if ((numberOfItems.get(bucket[i]) >= threshold) && (numberOfItems.get(bucket[j]) >= threshold)) {
                        int k = (bucket[i] * numberOfItems.size() + bucket[j]) % b;
                        if (compartment[k] >= threshold) {
                            Pair<Integer, Integer> p = new Pair<>(bucket[i], bucket[j]);
                            if (pairs.containsKey(p)) {
                                pairs.put(p, pairs.get(p) + 1);
                            } else {
                                pairs.put(p, 1);
                            }
                        }
                    }
                }
            }
        }

        ArrayList<Integer> sorted = new ArrayList<>(pairs.values());
        Collections.sort(sorted);
        Collections.reverse(sorted);

        System.out.println(A * (A - 1) / 2);
        System.out.println(pairs.size());

        for (int i = 0; i < sorted.size(); i++) {
            if (sorted.get(i) >= threshold) {
                System.out.println(sorted.get(i));
            }
        }

        br.close();
    }

    private static int[] stringArrayToIntArray(String[] stringArray) {
        int[] ia = new int[stringArray.length];

        for (int i = 0; i < stringArray.length; i++) {
            ia[i] = Integer.parseInt(stringArray[i]);
        }
        return ia;
    }
}
